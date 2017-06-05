using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using DirectOutput.General.Generic;
using DirectOutput.Cab.Out;
using System;

namespace DirectOutput.Cab.Overrides {
    public class TableOverrideSettings : NamedItemList<TableOverrideSetting>, IXmlSerializable {

        //thread safe singleton...though not sure how safe it is, setter has been opened up to allow cabinet to parse xml into it
        public static TableOverrideSettings Instance { get; set; }
        private TableOverrideSettings() { }
        static TableOverrideSettings() { Instance = new TableOverrideSettings(); }

        /// <summary>
        /// Active table name.
        /// </summary>
        public string activetableName = "";

        /// <summary>
        /// Active ROM name.
        /// </summary>
        public string activeromName = "";

        /// <summary>
        /// Serializes the TableOverrideSetting objects in this list to Xml.<br/>
        /// WriteXml is part of the IXmlSerializable interface.
        /// </summary>
        public void WriteXml(XmlWriter writer) {

            XmlSerializerNamespaces Namespaces = new XmlSerializerNamespaces();
            Namespaces.Add(string.Empty, string.Empty);
            foreach (TableOverrideSetting C in this) {
                XmlSerializer serializer = new XmlSerializer(typeof(TableOverrideSetting));
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
                if (reader.LocalName == typeof(TableOverrideSetting).Name) {
                    //Log.Write("SequentialOutputSettings.ReadXml...found settings...reader.LocalName="+reader.LocalName);

                    XmlSerializer serializer = new XmlSerializer(typeof(TableOverrideSetting));
                    TableOverrideSetting C = (TableOverrideSetting)serializer.Deserialize(reader);

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
        /// Checks which overrides to activate or disable depending on roms or tables.
        /// Should be called once at init.
        /// Will override with wildcards "*" if found. Checks using wildcards are forced lowercase to avoid casing errors.
        /// </summary>
        public void activateOverrides() {
            string currentItem = "";
            string currentitemwithoutWildcard = "";

            foreach (TableOverrideSetting currenttableOverrideSetting in this) {

                //first check by string
                if (currenttableOverrideSetting.RomList.Contains(activeromName) == true || currenttableOverrideSetting.TableList.Contains(activetableName) == true) {
                    currenttableOverrideSetting.activeSetting = true;
                } else {
                    currenttableOverrideSetting.activeSetting = false;
                }

                //double check roms for wildcards (afm* for instance), and override if they match
                for (int i = 0; i < currenttableOverrideSetting.RomList.Count; i++) {
                    currentItem = currenttableOverrideSetting.RomList[i].ToLower();
                    if (currentItem.IndexOf("*") != -1) {
                        currentitemwithoutWildcard = currentItem.Substring(0, currentItem.IndexOf("*"));
                        if (activeromName.ToLower().IndexOf(currentitemwithoutWildcard) == 0) {
                            currenttableOverrideSetting.activeSetting = true;
                        }
                    }
                }

                //double check tables for wildcards (attack* for instance), and override if they match
                for (int i = 0; i < currenttableOverrideSetting.TableList.Count; i++) {
                    currentItem = currenttableOverrideSetting.TableList[i].ToLower();
                    if (currentItem.IndexOf("*") != -1) {
                        currentitemwithoutWildcard = currentItem.Substring(0, currentItem.IndexOf("*"));
                        if (activetableName.ToLower().IndexOf(currentitemwithoutWildcard) == 0) {
                            currenttableOverrideSetting.activeSetting = true;
                        }
                    }
                }

                Log.Write("TableOverrideSettings.activateOverrides... activeSetting[" + currenttableOverrideSetting.Name + "]=" + currenttableOverrideSetting.activeSetting);
            }
        }

        /// <summary>
        /// Checks if a TableOverrideSetting is active and enabled. Then among list of affected outputs, and of correct device ID.
        /// </summary>
        /// <param name="currentOutput">Output / port of device.</param>
        /// <param name="recalculateoutputValue">If true, recalculates output value directly.</param>
        /// <param name="startingdeviceIndex">Specifies start index of device ID (1st UIO=27).</param>
        /// <param name="currentdeviceIndex">Specifies active index of device ID (for UIO this is zero-based, making the 1st UIO #27).</param>
        public TableOverrideSettingDevice getactiveDevice(IOutput currentOutput, bool recalculateoutputValue, int startingdeviceIndex, int currentdeviceIndex) {
            TableOverrideSettingDevice foundactiveDevice = null;
            TableOverrideSetting foundactiveSequential = null;

            foreach (TableOverrideSetting tableOverrideSetting in this) {

                //first check if enabled and active
                if (tableOverrideSetting.Enabled == true && tableOverrideSetting.activeSetting == true) {

                    //Log.Write("TableOverrideSettings.getactiveSequentialDevice, enabled: " + tableOverrideSetting.Name + ", checking output range...");

                    //find active sequence setting by checking if input output number / port matches first / primary index in outputs
                    foreach (TableOverrideSettingDevice tableoverridesettingDevice in tableOverrideSetting.TableOverrideSettingDeviceList) {
                        if ((currentdeviceIndex + startingdeviceIndex) == tableoverridesettingDevice.ConfigPostfixID && tableoverridesettingDevice.OutputList.Contains(currentOutput.Number) == true) {
                            //Log.Write("TableOverrideSettings.TableOverrideSettingDevice... " + tableOverrideSetting.Name + " is active at channel #" + currentOutput.Number + " on device " + tableoverridesettingDevice.Name);
                            foundactiveDevice = tableoverridesettingDevice;
                            foundactiveSequential = tableOverrideSetting;
                            break;
                        }
                    }
                }
            }

            //recalculate output value based on strength?
            if (foundactiveDevice != null && recalculateoutputValue == true) {
                double strengthFactor = foundactiveDevice.OutputPercent / 100f;
                byte newValue = Convert.ToByte(currentOutput.Value * strengthFactor);

                /*if (CacheList.Contains(foundactiveDevice) == false) {
                    CacheList.Add(foundactiveDevice);

                    if (currentOutput.Value != 0) {
                        Log.Write("ScheduledSettings.GetActiveSchedule: found active schedule: " + foundactiveSchedule.Name + " [" + foundactiveSchedule.ClockStart + "-" + foundactiveSchedule.ClockEnd + "] at channel #" + currentOutput.Number + " on device config " + foundactiveDevice.Name + ", applying strength multiplier: " + strengthFactor + ", old value=" + currentOutput.Value + ", new value=" + newValue);
                    }
                }*/

                if (foundactiveDevice.OutputPercent == 0 && currentOutput.Value != 0) {
                    currentOutput.Value = 0;
                } else if (foundactiveDevice.OutputPercent != 100) {
                    currentOutput.Value = newValue;
                }
            }


            return foundactiveDevice;
        }

        /// <summary>
        /// Checks if a TableOverrideSetting is active, and returns a newly mirrored Output object with modified Value. If not found, returns same input back.
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
                TableOverrideSettingDevice activeDevice = getactiveDevice(newOutput, true, startingdeviceIndex, currentdeviceIndex);

                if (activeDevice != null) {
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
