using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.IO;

namespace PinballX
{
    public class DOFManager
    {


        private delegate string GetVersionDelegate();
      //  private delegate string GetNameDelegate();
      //  private delegate string GetDllPathDelegate();
        private delegate void FinishDelegate();
        private delegate void UpdateTableElementDelegate(string TableElementTypeChar, int Number, int Value);
        private delegate void UpdateNamedTableElementDelegate(string TableElementName, int Value);
        private delegate void InitDelegate(string HostingApplicationName, string TableFileName, string GameName);
        private delegate void ShowFrontendDelegate();

        Type DOFType;
        object DOF;
        private GetVersionDelegate GetVersionDel;
  //      private GetNameDelegate GetNameDel;
    //    private GetDllPathDelegate GetDllPathDel;
        private FinishDelegate FinishDel;
        private UpdateTableElementDelegate UpdateTableElementDel;
        private UpdateNamedTableElementDelegate UpdateNamedTableElementDel;
        private InitDelegate InitDel;
        private ShowFrontendDelegate ShowFrontendDel;

        private Config Config = null;

        Assembly DOFAssembly = null;


        public void Load()
        {
            FileInfo PluginAssemblyFileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);
            string ConfigFilename = PluginAssemblyFileInfo.FullName.Substring(0, PluginAssemblyFileInfo.FullName.Length - PluginAssemblyFileInfo.Extension.Length - 1) + ".xml";


            Config = Config.GetConfigFromXmlFile(ConfigFilename);


            DOFAssembly = Assembly.LoadFrom(Config.DirectOutputPath);

            DOFType = Type.GetTypeFromProgID("DirectOutput.DirectOutputHandler");

            MessageBox.Show(DOFType.FullName);


            //try
            //{
            GetVersionDel = (GetVersionDelegate)Delegate.CreateDelegate(typeof(GetVersionDelegate), DOFType, "GetVersion");
      //      GetNameDel = (GetNameDelegate)Delegate.CreateDelegate(typeof(GetNameDelegate), DOFType, "GetName");
//            GetDllPathDel = (GetDllPathDelegate)Delegate.CreateDelegate(typeof(GetDllPathDelegate), DOFType, "GetDllPath");
            FinishDel = (FinishDelegate)Delegate.CreateDelegate(typeof(FinishDelegate), DOFType, "Finish");
            UpdateTableElementDel = (UpdateTableElementDelegate)Delegate.CreateDelegate(typeof(UpdateTableElementDelegate), DOFType, "UpdateTableElement");
            UpdateNamedTableElementDel = (UpdateNamedTableElementDelegate)Delegate.CreateDelegate(typeof(UpdateNamedTableElementDelegate), DOFType, "UpdateNamedTableElement");
            InitDel = (InitDelegate)Delegate.CreateDelegate(typeof(InitDelegate), DOFType, "Init");
            ShowFrontendDel = (ShowFrontendDelegate)Delegate.CreateDelegate(typeof(ShowFrontendDelegate), DOFType, "ShowFrontend");

            //}
            //catch (Exception E)
            //{
            //    UnlinkDOF();
            //    throw new Exception("Could not create delegates for the functions of the DirectOutput.ComObject. " + E.Message);
            //}
        }


        public bool IsInitialized { get; private set; }

        public string GetVersion()
        {
            if (GetVersionDel != null)
            {
                return GetVersionDel();
            }
            return "";
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
            if (FinishDel != null && IsInitialized)
            {
                FinishDel();
                IsInitialized = false;
            }
        }
        public void UpdateTableElement(string TableElementTypeChar, int Number, int Value)
        {
            if (UpdateTableElementDel != null && IsInitialized)
            {
                UpdateTableElementDel(TableElementTypeChar, Number, Value);
            }
        }
        public void UpdateNamedTableElement(string TableElementName, int Value)
        {
            if (UpdateNamedTableElementDel != null & IsInitialized)
            {
                UpdateNamedTableElementDel(TableElementName, Value);
            }
        }
        public void Init()
        {
            if (InitDel != null && !IsInitialized)
            {
                InitDel("PinballX", "", "PinballX");
                IsInitialized = true;
            }

        }
        public void ShowFrontend()
        {
            if (ShowFrontendDel != null)
            {
                ShowFrontendDel();
            }
        }

    }
}
