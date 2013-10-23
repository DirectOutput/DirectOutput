using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace DirectOutput.Cab.Out.WS2811StripController
{
    public class WS2811StripControllerApi
    {
        const string ControllerNameBase = "WS2811 Strip Controller ";


        public void ClearData()
        {
            lock (FT245RLocker)
            {
                if (FT245R != null)
                {
                    try
                    {
                        uint Dummy = 0;
                        FT245R.Write(new byte[] { (byte)'C' }, 1, ref Dummy);
                    }
                    catch { Close(); }
                }
            }
        }

        public void DisplayData(int Length)
        {
            lock (FT245RLocker)
            {
                if (FT245R != null)
                {
                    try
                    {
                        uint Dummy = 0;
                        FT245R.Write(new byte[] { (byte)'O', (byte)(Length / 256), (byte)(Length & 255) }, 3, ref Dummy);
                    }
                    catch { Close(); }
                }
            }
        }

        public void SetAndDisplayData(byte[] Data)
        {
            lock (FT245RLocker)
            {
                SetData(Data);
                DisplayData(Data.Length);
            }
        }
       

        public void SetData(byte[] Data)
        {
            byte[] Header = { (byte)'R', (byte)(Data.Length / 256), (byte)(Data.Length & 255) };
            lock (FT245RLocker)
            {
                if (FT245R != null)
                {
                    try
                    {
                        uint Dummy = 0;
                        FT245R.Write(Header, 3, ref Dummy);
                        FT245R.Write(Data, Data.Length,ref Dummy);
                        if (Dummy != Data.Length)
                        {
                            Console.WriteLine("Stop");

                        }
                    }
                    catch { Close(); }
                }
            }
        }

        public WS2811StripControllerApi() { }

        public WS2811StripControllerApi(int ControllerNumber)
        {
            Open(ControllerNumber);

        }

        private int _ControllerNumber;

        public int ControllerNumber
        {
            get { return _ControllerNumber; }
            private set { _ControllerNumber = value; }
        }


        public bool DeviceIsPresent
        {
            get
            {
                uint Dummy=0;
                return FT245R != null && FT245R.GetRxBytesAvailable(ref Dummy) == FTDI.FT_STATUS.FT_OK;
            }
        }




        FTDI FT245R;
        private object FT245RLocker=new object();

        public void Open(int ControllerNumber)
        {
            lock (FT245RLocker)
            {
                Close();

                bool OK = false;
                
                this.ControllerNumber = ControllerNumber;

                FT245R = new FTDI();
                FTDI.FT_STATUS FTStatus;

                //Get number of devices
                uint DeviceCnt = 0;
                FTStatus = FT245R.GetNumberOfDevices(ref DeviceCnt);

                if (FTStatus == FTDI.FT_STATUS.FT_OK && DeviceCnt > 0)
                {
                    FTDI.FT_DEVICE_INFO_NODE[] Devices = new FTDI.FT_DEVICE_INFO_NODE[DeviceCnt];

                    FTStatus = FT245R.GetDeviceList(Devices);
                    if (FTStatus == FTDI.FT_STATUS.FT_OK)
                    {
                        foreach (FTDI.FT_DEVICE_INFO_NODE DI in Devices)
                        {
                            if (DI != null && DI.Type == FTDI.FT_DEVICE.FT_DEVICE_232R)
                            {
                                if (DI.Description == ControllerNameBase + ControllerNumber)
                                {

                                    FT245R.CharReceived += new EventHandler<EventArgs>(FT245R_CharReceived);

                                    FTStatus = FT245R.OpenByLocation(DI.LocId);
                                    if (FTStatus == FTDI.FT_STATUS.FT_OK)
                                    {
                                        FTStatus = FT245R.Purge(FTDI.FT_PURGE.FT_PURGE_RX + FTDI.FT_PURGE.FT_PURGE_TX);
                                        if (FTStatus == FTDI.FT_STATUS.FT_OK)
                                        {
                                            OK = true;
                                            break;
                                        }
                                    }
                                   
                                }
                            }
                        }
                    }
                }

                if (!OK)
                {
                    Close();
                }
            }
        }

        void FT245R_CharReceived(object sender, EventArgs e)
        {
            uint CharsToRead = 0;

            FT245R.GetRxBytesAvailable(ref CharsToRead);

            if (CharsToRead > 0)
            {
                uint BytesRead = 0;
                byte[] Response = new Byte[CharsToRead];
                FT245R.Read(Response, CharsToRead, ref BytesRead);

                bool OK=true;
                for (int i = 0; i < BytesRead; i++)
                {
                    if (Response[i] == 0x4e)
                    {
                        OK = false;
                        break;
                    }
                }
                if (!OK)
                {
                    lock (FT245RLocker)
                    {
                        uint Dummy = 0;
                        for (int i = 0; i < 2000; i++)
                        {
                            FT245R.Write(new byte[10], 10, ref Dummy);
                            CharsToRead = 0;
                            FT245R.GetRxBytesAvailable(ref CharsToRead);
                            if (CharsToRead > 0)
                            {
                                Thread.Sleep(100);
                                CharsToRead = 0;
                                FT245R.GetRxBytesAvailable(ref CharsToRead);
                                Response = new Byte[CharsToRead];
                                BytesRead = 0;
                                FT245R.Read(Response, CharsToRead, ref BytesRead);
                                break;
                            }


                        }
                    }


                }
            }
        }


        public void Close()
        {
            lock (FT245RLocker)
            {
                if (FT245R != null)
                {
                    if (FT245R.IsOpen)
                    {
                        FT245R.Close();
                        FT245R.CharReceived += new EventHandler<EventArgs>(FT245R_CharReceived);
                    }
                    FT245R = null;
                }
            }
        }



        public static List<int> GetAvailableControllerNumbers()
        {
            List<int> L = new List<int>();

            FTDI  FTD2xxWrapper = new FTDI();
            FTDI.FT_STATUS FTStatus;

            //Get number of devices
            uint DeviceCnt = 0;
            FTStatus = FTD2xxWrapper.GetNumberOfDevices(ref DeviceCnt);

            if (FTStatus == FTDI.FT_STATUS.FT_OK && DeviceCnt > 0)
            {
                FTDI.FT_DEVICE_INFO_NODE[] Devices = new FTDI.FT_DEVICE_INFO_NODE[DeviceCnt];

                FTStatus = FTD2xxWrapper.GetDeviceList(Devices);
                if (FTStatus == FTDI.FT_STATUS.FT_OK)
                {
                    foreach (FTDI.FT_DEVICE_INFO_NODE DI in Devices)
                    {
                        if (DI != null && DI.Type == FTDI.FT_DEVICE.FT_DEVICE_232R)
                        {
                            int ControllerNr=0;
                            if (DI.Description.StartsWith(ControllerNameBase) && int.TryParse(DI.Description.Substring(ControllerNameBase.Length),out ControllerNr)) 
                            {
                                if (ControllerNr > 0)
                                {
                                    L.Add(ControllerNr);

                                }
                            }
                        }
                    }
                }
            }
            return L;
        }



    }
}
