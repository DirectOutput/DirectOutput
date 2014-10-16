using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Windows.Forms;

namespace PinballX
{
    public class DOFManager
    {


        private delegate string GetVersionDelegate();
        private delegate string GetNameDelegate();
        private delegate string GetDllPathDelegate();
        private delegate void FinishDelegate();
        private delegate void UpdateTableElementDelegate(string TableElementTypeChar, int Number, int Value);
        private delegate void UpdateNamedTableElementDelegate(string TableElementName, int Value);
        private delegate void InitDelegate(string HostingApplicationName, string TableFileName, string GameName);
        private delegate void ShowFrontendDelegate();

        Type DOFType;
        object DOF;
        private GetVersionDelegate GetVersionDel;
        private GetNameDelegate GetNameDel;
        private GetDllPathDelegate GetDllPathDel;
        private FinishDelegate FinishDel;
        private UpdateTableElementDelegate UpdateTableElementDel;
        private UpdateNamedTableElementDelegate UpdateNamedTableElementDel;
        private InitDelegate InitDel;
        private ShowFrontendDelegate ShowFrontendDel;


        public bool Linked
        {
            get { return DOF != null; }
        }

        public void LinkDOF()
        {

            try
            {
                DOFType = Type.GetTypeFromProgID("DirectOutput.ComObject");

            }
            catch (Exception E)
            {
                UnlinkDOF();
                throw new Exception("Could not find the DirectOutput.ComObject. Please check if the DirectOutputComObject is registered.", E);
            }

            MessageBox.Show(DOFType.FullName);

            try
            {
                DOF = Activator.CreateInstance(DOFType);

            }
            catch (Exception E)
            {
                UnlinkDOF();
                throw new Exception("Could not create a instance of the DirectOutput framework. " + E.Message);
            }

            string A = DOF.GetType().ToString();
            foreach (MethodInfo Mi in DOF.GetType().GetMethods(BindingFlags.Instance |BindingFlags.NonPublic |BindingFlags.Public))
            {
                try
                {
                    A = A + Mi.Name + "\n";
                }
                catch { }
            }

            MessageBox.Show(A);


            try
            {
                GetVersionDel = (GetVersionDelegate)Delegate.CreateDelegate(typeof(GetVersionDelegate), DOF, "GetVersion");
                GetNameDel = (GetNameDelegate)Delegate.CreateDelegate(typeof(GetNameDelegate), DOF, "GetName");
                GetDllPathDel = (GetDllPathDelegate)Delegate.CreateDelegate(typeof(GetDllPathDelegate), DOF, "GetDllPath");
                FinishDel = (FinishDelegate)Delegate.CreateDelegate(typeof(FinishDelegate), DOF, "Finish");
                UpdateTableElementDel = (UpdateTableElementDelegate)Delegate.CreateDelegate(typeof(UpdateTableElementDelegate), DOF, "UpdateTableElement");
                UpdateNamedTableElementDel = (UpdateNamedTableElementDelegate)Delegate.CreateDelegate(typeof(UpdateNamedTableElementDelegate), DOF, "UpdateNamedTableElement");
                InitDel = (InitDelegate)Delegate.CreateDelegate(typeof(InitDelegate), DOF, "Init");
                ShowFrontendDel = (ShowFrontendDelegate)Delegate.CreateDelegate(typeof(ShowFrontendDelegate), DOF, "ShowFrontend");

            }
            catch (Exception E)
            {
                UnlinkDOF();
                throw new Exception("Could not create delegates for the functions of the DirectOutput.ComObject. " + E.Message);
            }
        }


        public void UnlinkDOF()
        {
            GetVersionDel = null;
            GetNameDel = null;
            GetDllPathDel = null;
            FinishDel = null;
            UpdateTableElementDel = null;
            UpdateNamedTableElementDel = null;
            InitDel = null;
            ShowFrontendDel = null;
            DOF = null;
            DOFType = null;
        }

        public string GetVersion()
        {
            if (GetVersionDel != null)
            {
                return GetVersionDel();
            }
            return "";
        }

        public string GetName()
        {
            if (GetNameDel != null)
            {
                return GetNameDel();
            }
            return "";
        }
        public string GetDllPath()
        {
            if (GetDllPathDel != null)
            {
                return GetDllPathDel();
            }
            return "";
        }
        public void Finish()
        {
            if (FinishDel != null)
            {
                FinishDel();
            }
        }
        public void UpdateTableElement(string TableElementTypeChar, int Number, int Value)
        {
            if (UpdateTableElementDel != null)
            {
                UpdateTableElementDel(TableElementTypeChar, Number, Value);
            }
        }
        public void UpdateNamedTableElement(string TableElementName, int Value)
        {
            if (UpdateNamedTableElementDel != null)
            {
                UpdateNamedTableElementDel(TableElementName, Value);
            }
        }
        public void Init(string HostingApplicationName, string TableFileName, string GameName)
        {
            if (InitDel != null)
            {
                Init(HostingApplicationName, TableFileName, GameName);
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
