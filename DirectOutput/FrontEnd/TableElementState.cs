using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DirectOutput.Table;
using System.Collections.Concurrent;
using System.Threading;

namespace DirectOutput.FrontEnd
{
    public partial class TableElementState : Form
    {
        private TableElementState(Table.Table Table)
        {
            InitializeComponent();

            InitFilter();
            this.Table = Table;
        }

        private Table.Table _Table;
        public Table.Table Table
        {
            get { return _Table; }
            set
            {
                if (value != _Table)
                {
                    if (_Table != null)
                    {
                        _Table.TableElements.TableElementValueChanged -= new DirectOutput.Table.TableElementList.TableElementValueChangedEventHandler(TableElements_TableElementValueChanged);
                    }
                    _Table = value;
                    if (_Table != null)
                    {
                        _Table.TableElements.TableElementValueChanged += new DirectOutput.Table.TableElementList.TableElementValueChangedEventHandler(TableElements_TableElementValueChanged);
                    }
                }
            }
        }

        void TableElements_TableElementValueChanged(object sender, Table.TableElementValueChangedEventArgs e)
        {
            lock (UpdateQueueLocker)
            {
                UpdateQueue.Enqueue(e.TableElement);
            }
            lock (UpdateThreadLocker)
            {
                Monitor.Pulse(UpdateThreadLocker);
            }
        }



        private object UpdateLock = new object();
        private void UpdateValue(TableElement TableElement)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<TableElement>(UpdateValue), new object[] { TableElement });
                return;
            }
            lock (UpdateLock)
            {
                if (RowLookupDict.ContainsKey(TableElement))
                {
                    States.Rows[RowLookupDict[TableElement]].Cells[3].Value = TableElement.Value;
                }
                else
                {

                    //if ((((KeyValuePair<TableElementTypeEnum, string>)Filter.SelectedValue).Key == 0) || ((KeyValuePair<TableElementTypeEnum, string>)Filter.SelectedValue).Key == TableElement.TableElementType)
                    {
                        PopulateStates();
                    }
                }
            }
        }

        private Dictionary<TableElement, int> RowLookupDict = new Dictionary<TableElement, int>();

        private void PopulateStates()
        {
            lock (UpdateLock)
            {
                States.Rows.Clear();
                RowLookupDict = new Dictionary<TableElement, int>();

                if (Table != null)
                {
                    //bool FilterEnabled = (((KeyValuePair<TableElementTypeEnum, string>)Filter.SelectedValue).Key == 0);

                    foreach (TableElement TE in Table.TableElements)
                    {
                        //  if (!FilterEnabled || (TableElementTypeEnum)Filter.SelectedValue == TE.TableElementType)
                        {
                            int Index = States.Rows.Add(((TableElementTypeEnum)TE.TableElementType).ToString(), TE.Number, TE.Name, TE.Value);
                            RowLookupDict.Add(TE, Index);
                        }
                    }
                }
            }
        }




        private void InitFilter()
        {
            Dictionary<TableElementTypeEnum, string> F = new Dictionary<TableElementTypeEnum, string>();
            F.Add(0, "All table elements");

            foreach (TableElementTypeEnum TET in Enum.GetValues(typeof(TableElementTypeEnum)))
            {
                F.Add(TET, ((TableElementTypeEnum)TET).ToString());
            }


            Filter.DataSource = new BindingSource(F, null);
            Filter.DisplayMember = "Value";
            Filter.ValueMember = "Key";
            Filter.Refresh();

        }

        private void Filter_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateStates();
        }

        ///TODO: Check if this is better done in dispose.
        private void TableElementState_FormClosed(object sender, FormClosedEventArgs e)
        {
            Table = null;
            TerminateUpdateThread();
        }


        #region Static methods

        private static TableElementState TableElementStateInstance = null;
        public static void Open(Table.Table Table)
        {
            if (TableElementStateInstance != null)
            {
                if (TableElementStateInstance.InvokeRequired)
                {
                    TableElementStateInstance.Invoke(new Action<Table.Table>(TableElementState.Open), new object[] { Table });
                }
                TableElementStateInstance.Focus();
            };
            StartUpdateThread(Table);

        }


        #region Update thread
        private static void StartUpdateThread(Table.Table Table)
        {
            KeepUpdateThreadAlive = true;
            UpdateThread = new Thread(() => UpdateThreadDoIt(Table));
            UpdateThread.Name = "TableElementState Form Update Thread";
            UpdateThread.SetApartmentState(ApartmentState.STA);

            UpdateThread.Start();

        }

        private static void TerminateUpdateThread()
        {
            if (UpdateThread != null)
            {
                KeepUpdateThreadAlive = false;
                lock (UpdateThreadLocker)
                {
                    Monitor.Pulse(UpdateThreadLocker);
                }
                if (!UpdateThread.Join(1000))
                {
                    UpdateThread.Abort();
                }
                if (TableElementStateInstance != null)
                {
                    TableElementStateInstance.Close();
                    TableElementStateInstance = null;
                }
                UpdateThread = null;
            }
        }

        private static object UpdateThreadLocker = new object();
        private static object UpdateQueueLocker = new object();
        private static ConcurrentQueue<TableElement> UpdateQueue = new ConcurrentQueue<TableElement>();

        private static Thread UpdateThread;
        private static bool KeepUpdateThreadAlive = false;
        private static void UpdateThreadDoIt(Table.Table Table)
        {
            TableElementStateInstance = new TableElementState(Table);
            TableElementStateInstance.Show();

            while (KeepUpdateThreadAlive)
            {
                if (UpdateQueue.Count == 0 && KeepUpdateThreadAlive)
                {
                    lock (UpdateThreadLocker)
                    {
                        while (UpdateQueue.Count == 0 && KeepUpdateThreadAlive)
                        {
                            Monitor.Wait(UpdateThreadLocker, 50);  // Lock is released while we’re waiting
                        }
                    }
                }

                try
                {
                    TableElement TE = null;
                    lock (UpdateQueueLocker)
                    {
                        UpdateQueue.TryDequeue(out TE);
                    }

                    if (TE != null)
                    {
                        TableElementStateInstance.UpdateValue(TE);
                    }

                    //TableElementStateInstance.Refresh();
                }
                catch
                {
                    Console.Write("x");
                }

            }
            TableElementStateInstance = null;
        }
        #endregion


        #endregion



    }
}
