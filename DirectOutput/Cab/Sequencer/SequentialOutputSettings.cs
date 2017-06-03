using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using DirectOutput.General.Generic;
using DirectOutput.Cab.Out;
using System;

namespace DirectOutput.Cab.Schedules {
    public class SequentialOutputSettings : NamedItemList<SequentialOutputSetting>, IXmlSerializable {

        //thread safe singleton...though not sure how safe it is, setter has been opened up to allow cabinet to parse xml into it
        public static SequentialOutputSettings Instance { get; set; }
        private SequentialOutputSettings() { }
        static SequentialOutputSettings() { Instance = new SequentialOutputSettings(); }

        //list of items that we've processed and has had an active setting, idea being to avoid lookups if found in list
        private static List<SequentialOutputDevice> CacheList = new List<SequentialOutputDevice>();

        /// <summary>
        /// Serializes the SequentialOutputSetting objects in this list to Xml.<br/>
        /// WriteXml is part of the IXmlSerializable interface.
        /// </summary>
        public void WriteXml(XmlWriter writer) {

            XmlSerializerNamespaces Namespaces = new XmlSerializerNamespaces();
            Namespaces.Add(string.Empty, string.Empty);
            foreach (SequentialOutputSetting C in this) {
                XmlSerializer serializer = new XmlSerializer(typeof(SequentialOutputSetting));
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
                if (reader.LocalName == typeof(SequentialOutputSetting).Name) {
                    //Log.Write("SequentialOutputSettings.ReadXml...found settings...reader.LocalName="+reader.LocalName);

                    XmlSerializer serializer = new XmlSerializer(typeof(SequentialOutputSetting));
                    SequentialOutputSetting C = (SequentialOutputSetting)serializer.Deserialize(reader);

                    /*Log.Write("SequentialOutputSettings.ReadXml...name=" + C.Name);
                    Log.Write("SequentialOutputSettings.ReadXml...enabled=" + C.Enabled);
                    Log.Write("SequentialOutputSettings.ReadXml...name=" + C.SequentialOutputDeviceList[0].Name);
                    Log.Write("SequentialOutputSettings.ReadXml...ConfigPostfixID=" + C.SequentialOutputDeviceList[0].ConfigPostfixID);
                    Log.Write("SequentialOutputSettings.ReadXml...Outputs=" + C.SequentialOutputDeviceList[0].Outputs);
                    Log.Write("SequentialOutputSettings.ReadXml...OutputMaxTime=" + C.SequentialOutputDeviceList[0].OutputMaxTime);*/

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
        /// <param name="startingdeviceIndex">Specifies start index of device ID (1st UIO=27).</param>
        /// <param name="currentdeviceIndex">Specifies active index of device ID (for UIO this is zero-based, making the 1st UIO #27).</param>
        public SequentialOutputDevice getactiveSequentialDevice(IOutput currentOutput, int startingdeviceIndex, int currentdeviceIndex) {
            SequentialOutputDevice foundactiveDevice = null;
            SequentialOutputSetting foundactiveSequential = null;

            foreach (SequentialOutputSetting sequentialSetting in this) {

                //first check if enabled
                if (sequentialSetting.Enabled == true) {

                    //Log.Write("SequentialOutputSettings.activeSequentialDevice, enabled: " + sequentialSetting.Name+", checking output range...");

                    //find active sequence setting by checking if input output number / port matches first / primary index in outputs
                    foreach (SequentialOutputDevice sequentialOutputDevice in sequentialSetting.SequentialOutputDeviceList) {
                        if ((currentdeviceIndex + startingdeviceIndex) == sequentialOutputDevice.ConfigPostfixID && sequentialOutputDevice.OutputList[0] == currentOutput.Number) {
                            //Log.Write("SequentialOutputSettings.SequentialOutputDevice... " + sequentialSetting.Name + " is active at channel #" + currentOutput.Number + " on device " + sequentialOutputDevice.Name);
                            foundactiveDevice = sequentialOutputDevice;
                            foundactiveSequential = sequentialSetting;
                            break;
                        }
                    }

               
                }
                
            }

            return foundactiveDevice;
        }


        /// <summary>
        /// Checks if a SequentialOutputSetting is active, and returns a newly mirrored Output object with modified Value. If not found, returns same input back.
        /// This doesn't affect the output value of that next output, but will act as a forward output instead.
        /// </summary>
        /// <param name="currentOutput">Output / port of device.</param>
        /// <param name="startingdeviceIndex">Specifies start index of device ID (1st UIO=27).</param>
        /// <param name="currentdeviceIndex">Specifies active index of device ID (for UIO this is zero-based, making the 1st UIO #27).</param>
        public IOutput getnextOutput(IOutput currentOutput, int startingdeviceIndex, int currentdeviceIndex) {
            long currenttimeMilliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long currenttimeDelta = 0;
            bool foundsequenceOutput = false;
            SequentialOutputDevice activeSequentialDevice = null;

            //create a new dummy output with no event mirroring input arg to avoid triggering a recursive OnOutputValueChanged (modifying Output directly would retrigger this method)
            Output newOutput = new Output();
            newOutput.Value = currentOutput.Value;
            newOutput.Name = currentOutput.Name;
            newOutput.Number = currentOutput.Number;

            activeSequentialDevice = getactiveSequentialDevice(newOutput, startingdeviceIndex, currentdeviceIndex);

            //calculate delta, and reset timestamp for next run
            if (activeSequentialDevice != null) {
                currenttimeDelta = currenttimeMilliseconds - activeSequentialDevice.OutputTimestamp;
                activeSequentialDevice.OutputTimestamp = currenttimeMilliseconds;
            }
            
            //check for next output, ignore if active output value 0 / turning off to avoid turning off the wrong output
            if (activeSequentialDevice != null) {
                foundsequenceOutput = true;
            }

            //Log.Write("SequentialOutputSettings.getnextOutput... name=" + newOutput.Name + ", number=" + newOutput.Number + ", currentdeviceIndex=" + currentdeviceIndex);

            //if we found a hit, and length of outputs isn't zero, try to check timestamp and offset to next output index if within retrigger time delta
            if (foundsequenceOutput == true) {
                if (activeSequentialDevice.OutputList.Count > 1) {

                    //check if below retrigger delta
                    if (currenttimeDelta <= activeSequentialDevice.OutputMaxTime) {

                        //if not zero, increase to next
                        //if zero, reuse last index to ensure retrigger gets shut down
                        if (currentOutput.Value >0) {
                            newOutput.Number = activeSequentialDevice.getsetnextoutputValue();
                        } else {
                            newOutput.Number = activeSequentialDevice.OutputList[activeSequentialDevice.OutputIndex];
                        }
                        
                        Log.Write("SequentialOutputSettings.getnextOutput... retrigger! currenttimeDelta=" + currenttimeDelta + ", name = " + newOutput.Name + ", old number=" + currentOutput.Number + ", new number=" + newOutput.Number + ", currentdeviceIndex=" + currentdeviceIndex + ", value=" + newOutput.Value);
                    } else {
                        activeSequentialDevice.OutputIndex = 0;
                    }
                }
                return newOutput;
            } else {
                return currentOutput;
            }

        }

    }
}
