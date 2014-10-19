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
                    try
                    {
                        DOFType = Type.GetTypeFromProgID("DirectOutput.ComObject", true);

                    }
                    catch (Exception E)
                    {

                        throw new Exception("Could not find the DirectOutput.ComObject. Please check if the DirectOutputComObject is registered.", E);
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
                Finish();
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


    }
}
