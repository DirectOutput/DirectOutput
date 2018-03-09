using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace PinballX
{
    public class DOFManager
    {


        Type DOFType;
        object DOF;
        object DOFLocker = new object();

        public void Load()
        {

            lock (DOFLocker)
            {
                if (DOF == null)
                {
                    String DofComObjProgID = "DirectOutput.ComObject";
                    Guid DofComObjGuid = new Guid("A23BFDBC-9A8A-46C0-8672-60F23D54FFB6");
                    try
                    {
                        // try getting the DOF COM object by name
                        DOFType = Type.GetTypeFromProgID(DofComObjProgID, true);
                    }
                    catch (Exception E1)
                    {
                        // That didn't work - try getting it by GUID.  Type.GetTypeFromProgID()
                        // is known to fail on some systems for reasons that aren't entirely
                        // clear, and one workaround seems to be to load the type by GUID 
                        // instead.
                        try
                        {                            
                            DOFType = Type.GetTypeFromCLSID(DofComObjGuid);
                        }
                        catch (Exception E2)
                        {
                            throw new Exception("The type data for the DOF COM object wasn't found. "
                                + "Please check that DirectOutputComObject.dll has been registered. "
                                + "Type.GetTypeFromProgID(" + DofComObjProgID + ") failed with error: "
                                + E1 + "; and then on retry, Type.GetTypeFromCLSID(" + DofComObjGuid
                                + ") failed with error: " + E2);
                        }
                    }

                    try
                    {
                        DOF = Activator.CreateInstance(DOFType);
                    }
                    catch (Exception E)
                    {
                        throw new Exception("Could not create a instance of the DirectOutput framework. " + E.Message);
                    }

                }
            }
        }


        public void Unload()
        {
            lock (DOFLocker)
            {

                if (DOF != null)
                {
                    // Marshal.ReleaseComObject(DOF);
                    DOF = null;
                    DOFType = null;
                }
            }
        }



        public bool IsInitialized { get; private set; }

        public string GetVersion()
        {
            lock (DOFLocker)
            {
                if (DOF != null)
                {
                    return (string)DOFType.InvokeMember("GetVersion", BindingFlags.InvokeMethod, null, DOF, null);
                }
                return "";
            }
        }

        public string GetDllPath()
        {
            lock (DOFLocker)
            {
                if (DOF != null)
                {
                    return (string)DOFType.InvokeMember("GetDllPath", BindingFlags.InvokeMethod, null, DOF, null);
                }
                return "";
            }
        }

        //public string GetName()
        //{
        //    if (GetNameDel != null)
        //    {
        //        return GetNameDel();
        //    }
        //    return "";
        //}
        //public string GetDllPath()
        //{
        //    if (GetDllPathDel != null)
        //    {
        //        return GetDllPathDel();
        //    }
        //    return "";
        //}
        public void Finish()
        {
            lock (DOFLocker)
            {
                if (DOF != null && IsInitialized)
                {
                    DOFType.InvokeMember("Finish", BindingFlags.InvokeMethod, null, DOF, null);
                    IsInitialized = false;
                }
                Unload();
            }
        }
        public void UpdateTableElement(string TableElementTypeChar, int Number, int Value)
        {
            lock (DOFLocker)
            {
                if (DOF != null && IsInitialized)
                {
                    object[] Args = new object[] { TableElementTypeChar, Number, Value };
                    DOFType.InvokeMember("UpdateTableElement", BindingFlags.InvokeMethod, null, DOF, Args);

                }
            }
        }
        public void UpdateNamedTableElement(string TableElementName, int Value)
        {
            lock (DOFLocker)
            {
                if (DOF != null & IsInitialized)
                {
                    object[] Args = new object[] { TableElementName, Value };
                    DOFType.InvokeMember("UpdateNamedTableElement", BindingFlags.InvokeMethod, null, DOF, Args);
                }
            }
        }

        public void SignalNamedTableElement(string TableElementName)
        {
            lock (DOFLocker)
            {
                if (DOF != null & IsInitialized)
                {
                    object[] Args = new object[] { TableElementName, 1 };
                    DOFType.InvokeMember("UpdateNamedTableElement", BindingFlags.InvokeMethod, null, DOF, Args);
                    Args = new object[] { TableElementName, 0 };
                    DOFType.InvokeMember("UpdateNamedTableElement", BindingFlags.InvokeMethod, null, DOF, Args);
                }
            }
        }

        public void Init()
        {
            lock (DOFLocker)
            {
                Load();
                if (DOF != null && !IsInitialized)
                {

                    object[] Args = new object[] { "PinballX", "", "PinballX" };
                    DOFType.InvokeMember("Init", BindingFlags.InvokeMethod, null, DOF, Args);

                    IsInitialized = true;
                }
            }
        }



        public string[] GetConfiguredTableElmentDescriptors()
        {
            lock (DOFLocker)
            {
                Load();
                if (DOF != null && IsInitialized)
                {

                    return (string[])DOFType.InvokeMember("GetConfiguredTableElmentDescriptors", BindingFlags.InvokeMethod, null, DOF, null);
                }
                return new string[0];
            }
        }


        public string GetTableMappingFilename()
        {
            lock (DOFLocker)
            {
                Load();
                if (DOF != null && IsInitialized)
                {
                    return (string)DOFType.InvokeMember("TableMappingFileName", BindingFlags.InvokeMethod, null, DOF, null);
                }
                return null;
            }

        }

    }
}
