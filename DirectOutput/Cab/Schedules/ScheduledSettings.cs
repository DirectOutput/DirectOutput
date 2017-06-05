using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using DirectOutput.General.Generic;
using DirectOutput.Cab.Schedules;
using DirectOutput.Cab.Out;
using System.Globalization;

namespace DirectOutput.Cab.Schedules {
    public class ScheduledSettings : NamedItemList<ScheduledSetting>, IXmlSerializable {

        //https://msdn.microsoft.com/en-us/library/ff650316.aspx
        /*private static volatile ScheduledSettings instance;
        private static object syncRoot = new Object();

        private ScheduledSettings() { }
        public static ScheduledSettings Instance {
            get {
                if (instance == null) {
                    lock (syncRoot) {
                        if (instance == null) {
                            instance = new ScheduledSettings();
                            Log.Write("First init");
                        }
                    }
                }
                return instance;
            }
        }*/

        //thread safe singleton...though not sure how safe it is, setter has been opened up to allow cabinet to parse xml into it
        public static ScheduledSettings Instance { get; set; }
        private ScheduledSettings() { }
        static ScheduledSettings() { Instance = new ScheduledSettings(); }

        //list of items that we've processed and has had an active setting, idea being to avoid lookups if found in list
        private static List<ScheduledSettingDevice> CacheList = new List<ScheduledSettingDevice>();

        /// <summary>
        /// Serializes the ScheduleSetting objects in this list to Xml.<br/>
        /// WriteXml is part of the IXmlSerializable interface.
        /// </summary>
        public void WriteXml(XmlWriter writer) {

            XmlSerializerNamespaces Namespaces = new XmlSerializerNamespaces();
            Namespaces.Add(string.Empty, string.Empty);
            foreach (ScheduledSetting C in this) {
                XmlSerializer serializer = new XmlSerializer(typeof(ScheduledSetting));
                serializer.Serialize(writer, C, Namespaces);
            }
        }

        /// <summary>
        /// Deserializes the v objects in the XmlReader.<br/>
        /// ReadXml is part of the IXmlSerializable interface.
        /// </summary>
        public void ReadXml(XmlReader reader) {
            if (reader.IsEmptyElement) {
                reader.ReadStartElement();
                return;
            }
            reader.Read();
            //Log.Write("ScheduledSettings.ReadXml...2...localname="+reader.LocalName);
            while (reader.NodeType != System.Xml.XmlNodeType.EndElement) {
                if (reader.LocalName == typeof(ScheduledSetting).Name) {
                    //Log.Write("ScheduledSettings.ReadXml...found settings...reader.LocalName="+reader.LocalName);

                    XmlSerializer serializer = new XmlSerializer(typeof(ScheduledSetting));
                    ScheduledSetting C = (ScheduledSetting)serializer.Deserialize(reader);

                    /*Log.Write("ScheduledSettings.ReadXml...3...name="+C.Name);
                    Log.Write("ScheduledSettings.ReadXml...3...start=" + C.ClockStart);
                    Log.Write("ScheduledSettings.ReadXml...3...name=" + C.ScheduledSettingDeviceList);
                    Log.Write("ScheduledSettings.ReadXml...3...name=" + C.ScheduledSettingDeviceList.Count);
                    Log.Write("ScheduledSettings.ReadXml...3...name=" + C.ScheduledSettingDeviceList[0].Name);
                    Log.Write("ScheduledSettings.ReadXml...3...ConfigPostfixID=" + C.ScheduledSettingDeviceList[0].ConfigPostfixID);
                    Log.Write("ScheduledSettings.ReadXml...3...OutputStrength=" + C.ScheduledSettingDeviceList[0].OutputPercent);
                    Log.Write("ScheduledSettings.ReadXml...3...Outputs=" + C.ScheduledSettingDeviceList[0].Outputs);
                    Log.Write("ScheduledSettings.ReadXml...3...OutputList has 51=" + C.ScheduledSettingDeviceList[0].OutputList.Contains(51));*/

                    if (!Contains(C.Name)) {
                        Add(C);
                    }
                } else {
                    reader.Skip();
                }
            }
            reader.ReadEndElement();
        }

        /// <summary>
        /// Method is required by the IXmlSerializable interface
        /// </summary>
        /// <returns>Returns always null</returns>
        public System.Xml.Schema.XmlSchema GetSchema() { return (null); }


        /// <summary>
        /// Checks if a ScheduledSetting is active; enabled, within time region, among list of affected outputs, and of correct device ID.
        /// When the time calculations here are done, make sure and update the ScheduledSetting directly so we won't need to do this every event.
        /// There's an interesting issue here when a range passes midnight (2300-0300 for instance) that might need to be handled better, idea being schedules should be able to run all day long in real time without reboots.
        /// </summary>
        /// <param name="currentOutput">Output / port of device.</param>
        /// <param name="recalculateoutputValue">If true, recalculates output value directly.</param>
        /// <param name="startingdeviceIndex">Specifies start index of device ID (1st UIO=27).</param>
        /// <param name="currentdeviceIndex">Specifies active index of device ID (for UIO this is zero-based, making the 1st UIO #27).</param>
        public ScheduledSettingDevice GetActiveSchedule(IOutput currentOutput, bool recalculateoutputValue, int startingdeviceIndex, int currentdeviceIndex) {
            ScheduledSettingDevice foundactiveDevice = null;
            ScheduledSetting foundactiveSchedule = null;


            //possible to add result into cache for faster processing next time?


            foreach (ScheduledSetting scheduledSetting in this) {

                //first check if enabled
                if (scheduledSetting.Enabled == true) {

                    //Log.Write("ScheduledSettings.GetActiveSchedule, enabled: "+scheduledSetting.Name+", checking time range...");

                    //next check if within time range using military time (1730 etc)
                    string clockStart = scheduledSetting.ClockStart;
                    string clockEnd = scheduledSetting.ClockEnd;

                    //convert military string time to actual time of current day
                    //it's important this is done in real time, not at dof boot as it probably wouldn't become active again if a pincab was online more than a day...which is why we need to recalculate using real time
                    DateTime datetimeStart = DateTime.ParseExact(clockStart, "HHmm", CultureInfo.InvariantCulture);
                    DateTime datetimeEnd = DateTime.ParseExact(clockEnd, "HHmm", CultureInfo.InvariantCulture);
                    long currenttimeMilliseconds = DateTime.Now.Ticks;

                    //check if we're passing midnight, and if we are add a day to clockend...now, this gets pretty interesting, a real spinning clock helped programming this thing :)
                    //will this work again on next midnight?

                    //scenario; time is 1200, range is 2200-1400... since we still haven't passed end clock, do not advance one day ahead for end time since we're assuming clockstart is in the past (last day).. instead set start time one day back and assume we've passed midnight
                    //scenario; time is 1200, range is 1000-1400... since we're within time range and within the same day, don't advance either start or end
                    if (int.Parse(clockEnd) < int.Parse(clockStart) && currenttimeMilliseconds < datetimeEnd.Ticks) {
                        datetimeStart = datetimeStart.AddDays(-1);
                    } else if (int.Parse(clockEnd) < int.Parse(clockStart)) {
                        datetimeEnd = datetimeEnd.AddDays(1);
                    }

                    //Log.Write("ScheduledSettings.GetActiveSchedule, current time="+DateTime.Now+", start-end: " + datetimeStart.ToString()+" - "+datetimeEnd.ToString());
                    //Log.Write("ScheduledSettings.GetActiveSchedule, currenttimeMilliseconds="+ currenttimeMilliseconds);
                    //Log.Write("ScheduledSettings.GetActiveSchedule, start-end: " + datetimeStart.Ticks+" - "+datetimeEnd.Ticks);

                    //check if within time region, and if we are check if output port and device id is in list
                    if (currenttimeMilliseconds >= datetimeStart.Ticks && currenttimeMilliseconds <= datetimeEnd.Ticks) {
                        //Log.Write("ScheduledSettings.GetActiveSchedule, within time range: "+scheduledSetting.ClockStart+"-"+scheduledSetting.ClockEnd+", checking device ids and output list...");

                        foreach (ScheduledSettingDevice scheduledsettingDevice in scheduledSetting.ScheduledSettingDeviceList) {
                            if ((currentdeviceIndex + startingdeviceIndex) == scheduledsettingDevice.ConfigPostfixID && scheduledsettingDevice.OutputList.Contains(currentOutput.Number)) {
                                //Log.Write("ScheduledSettings.GetActiveSchedule... " + scheduledSetting.Name + " is active [" + clockStart + "-" + clockEnd + "] at channel #" + currentOutput.Number + " on device " + scheduledsettingDevice.Name);
                                foundactiveDevice = scheduledsettingDevice;
                                foundactiveSchedule = scheduledSetting;
                                break;
                            }
                        }

                    }

                }

            }

            //recalculate output value based on strength?
            if (foundactiveDevice != null && recalculateoutputValue == true) {
                double strengthFactor = foundactiveDevice.OutputPercent / 100f;
                byte newValue = Convert.ToByte(currentOutput.Value * strengthFactor);

                if (CacheList.Contains(foundactiveDevice) == false) {
                    CacheList.Add(foundactiveDevice);

                    if (currentOutput.Value != 0) {
                        Log.Write("ScheduledSettings.GetActiveSchedule: found active schedule: " + foundactiveSchedule.Name + " [" + foundactiveSchedule.ClockStart + "-" + foundactiveSchedule.ClockEnd + "] at channel #" + currentOutput.Number + " on device config " + foundactiveDevice.Name + ", applying strength multiplier: " + strengthFactor + ", old value=" + currentOutput.Value + ", new value=" + newValue);
                    }
                }

                if (foundactiveDevice.OutputPercent == 0 && currentOutput.Value != 0) {
                    currentOutput.Value = 0;
                } else if (foundactiveDevice.OutputPercent != 100) {
                    currentOutput.Value = newValue;
                }



            }

            return foundactiveDevice;
        }


        /// <summary>
        /// Checks if a ScheduledSetting is active, and returns a newly mirrored Output object with modified Value. If not found, returns same input back.
        /// There's an interesting issue here when a range passes midnight (2300-0300 for instance) that might need to be handled better, idea being schedules should be able to run all day long in real time without reboots.
        /// NOTE: this will change sending output if there is an active schedule.
        /// If value of input is 0, ignore changing and return input unchanged.
        /// </summary>
        /// <param name="currentOutput">Output / port of device.</param>
        /// <param name="startingdeviceIndex">Specifies start index of device ID (1st UIO=27).</param>
        /// <param name="currentdeviceIndex">Specifies active index of device ID (for UIO this is zero-based, making the 1st UIO #27).</param>
        public IOutput getnewrecalculatedOutput(IOutput currentOutput, int startingdeviceIndex, int currentdeviceIndex) {

            if (currentOutput.Value != 0) {
                //create a new dummy output with no event mirroring input arg to avoid triggering a recursive OnOutputValueChanged (modifying Output directly would retrigger this method)
                Output newOutput = new Output();
                newOutput.Value = currentOutput.Value;
                newOutput.Name = currentOutput.Name;
                newOutput.Number = currentOutput.Number;

                //Log.Write("ScheduledSettings.getnewrecalculatedOutput..name=" + newOutput.Name + ", number=" + newOutput.Number + ", currentdeviceIndex=" + currentdeviceIndex);
                ScheduledSettingDevice ActiveScheduleDevice = GetActiveSchedule(newOutput, true, startingdeviceIndex, currentdeviceIndex);

                if (ActiveScheduleDevice != null) {
                    return newOutput;
                } else {
                    return currentOutput;
                }
            } else {
                return currentOutput;
            }
            
            
        }
    }
}
