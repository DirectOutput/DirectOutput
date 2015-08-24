using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using DirectOutput.Cab.Out.FTDIChip;

namespace DirectOutput.Cab.Out.AdressableLedStrip
{
    /// <summary>
    /// This class handles all communication with Direct StripControllers.
    /// </summary>
    public class DirectStripControllerApi
    {
        private static readonly string[] ControllerNameBase = { "WS2811 Strip Controller", "Direct Strip Controller" };


        /// <summary>
        /// Sends a clear data buffer command to the Ledstrip controller
        /// </summary>
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

        /// <summary>
        /// Sends a display data command to the ledstrip controller.
        /// </summary>
        /// <param name="Length">The length of the data to be displayed.</param>
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

        /// <summary>
        /// Sends the specified data to the ledstrip controller and displays the data.
        /// </summary>
        /// <param name="Data">The data.</param>
        public void SetAndDisplayData(byte[] Data)
        {
            lock (FT245RLocker)
            {
                SetData(Data);
                DisplayData(Data.Length);
            }
        }



        /// <summary>
        /// Sends the specified data to the ledstripn controller.
        /// </summary>
        /// <param name="Data">The data.</param>
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
                        FT245R.Write(Data, Data.Length, ref Dummy);
                    }
                    catch { Close(); }
                }
            }
        }


        /// <summary>
        /// Packs the specified data, sends the data to the controller and displays the data.
        /// </summary>
        /// <param name="Data">The data.</param>
        public void SetAndDisplayPackedData(byte[] Data)
        {
            lock (FT245RLocker)
            {
                SetPackedData(Data);
                DisplayData(Data.Length);
            }
        }

        /// <summary>
        /// Packs the specified data using a IFF like compression and send the data to the ledstrip controller.
        /// </summary>
        /// <param name="Data">The data.</param>
        public void SetPackedData(byte[] Data)
        {

            int DataPos = 0;

            //Calc number of bytes to pack. Truncate data to a length which is divideable by 3.
            int DataLength = Data.Length;
            if (DataLength % 3 != 0)
            {
                DataLength -= (DataLength % 3);
            }

            //Create array for packed data (I hope the calc für the max size of the packed data is correct)
            byte[] Packed = new byte[(int)(DataLength + ((double)DataLength / 127) + 2)];
            int PackedPos = 3;

            int BlockStartPos = 0;
            byte BlockSize = 0;

            while (DataPos < DataLength)
            {

                BlockStartPos = PackedPos++;               //Store startpos of block 
                Packed[PackedPos++] = Data[DataPos++];      //Get first 3 bytes
                Packed[PackedPos++] = Data[DataPos++];
                Packed[PackedPos++] = Data[DataPos++];
                BlockSize = 1;

                if (DataPos >= DataLength)
                {
                    //reached the end of the data
                    Packed[BlockStartPos] = BlockSize; //Set the size of the block (1 triplet)
                }
                else
                {
                    //not yet the end of the data

                    //Check if the next bytes are the same as the first 3 bytes
                    if (Packed[BlockStartPos + 1] != Data[DataPos] || Packed[BlockStartPos + 2] != Data[DataPos + 1] || Packed[BlockStartPos + 3] != Data[DataPos + 2])
                    {
                        //Data is different, build block of org data

                        while (BlockSize < 127 && DataPos < DataLength && ((DataPos >= DataLength - 3) || (Data[DataPos] != Data[DataPos + 3] || Data[DataPos + 1] != Data[DataPos + 4] || Data[DataPos + 2] != Data[DataPos + 5])))
                        {
                            Packed[PackedPos++] = Data[DataPos++];      //Get next 3 bytes
                            Packed[PackedPos++] = Data[DataPos++];
                            Packed[PackedPos++] = Data[DataPos++];
                            BlockSize++;
                        }
                        Packed[BlockStartPos] = BlockSize; //Set the number of triplets for the block

                    }
                    else
                    {
                        //Data is the same, build block of packed data
                        while (BlockSize < 127 && DataPos < DataLength && Packed[BlockStartPos + 1] == Data[DataPos] && Packed[BlockStartPos + 2] == Data[DataPos + 1] && Packed[BlockStartPos + 3] == Data[DataPos + 2])
                        {
                            DataPos += 3;
                            BlockSize++;
                        }
                        Packed[BlockStartPos] = (byte)(BlockSize + 128); //Set the number of repetions for the block, add 128 to mark the packed data

                    }
                }

            }

            Packed[PackedPos++] = 0; //Add 0 byte to mark the end of the data

            byte[] Header = { (byte)'P', (byte)(PackedPos / 256), (byte)(PackedPos & 255) };
            lock (FT245RLocker)
            {
                if (FT245R != null)
                {
                    try
                    {
                        uint Dummy = 0;
                        FT245R.Write(Header, 3, ref Dummy);
                        FT245R.Write(Data, Data.Length, ref Dummy);
                    }
                    catch { Close(); }
                }
            }


        }





        /// <summary>
        /// Initializes a new instance of the <see cref="DirectStripControllerApi"/> class.
        /// </summary>
        public DirectStripControllerApi() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectStripControllerApi"/> class and opens a connection to the specified controller.
        /// </summary>
        /// <param name="ControllerNumber">The controller number.</param>
        public DirectStripControllerApi(int ControllerNumber)
        {
            Open(ControllerNumber);

        }

        private int _ControllerNumber;

        /// <summary>
        /// Gets the controller number.
        /// </summary>
        /// <value>
        /// The controller number.
        /// </value>
        public int ControllerNumber
        {
            get { return _ControllerNumber; }
            private set { _ControllerNumber = value; }
        }


        /// <summary>
        /// Gets a value indicating whether the ledstrip controller is present.
        /// </summary>
        /// <value>
        ///   <c>true</c> if device is present; otherwise, <c>false</c>.
        /// </value>
        public bool DeviceIsPresent
        {
            get
            {
                uint Dummy = 0;
                return FT245R != null && FT245R.GetRxBytesAvailable(ref Dummy) == FTDI.FT_STATUS.FT_OK;
            }
        }




        FTDI FT245R;
        private object FT245RLocker = new object();

        /// <summary>
        /// Opens a connection to the ledstrip controller with the specified controller number.
        /// </summary>
        /// <param name="ControllerNumber">The controller number.</param>
        public void Open(int ControllerNumber)
        {
            lock (FT245RLocker)
            {
                Close();

                bool OK = false;

                string Desc = "";
                this.ControllerNumber = ControllerNumber;

                FT245R = new FTDI();
                FTDI.FT_STATUS FTStatus;


                //Get number of devices
                uint DeviceCnt = 0;
                FTStatus = FT245R.GetNumberOfDevices(ref DeviceCnt);

                if (FTStatus == FTDI.FT_STATUS.FT_OK && DeviceCnt > 0)
                {
                    FTDI.FT_DEVICE_INFO_NODE[] Devices = new FTDI.FT_DEVICE_INFO_NODE[DeviceCnt];

                    //for (uint i = 0; i < DeviceCnt; i++)
                    //{
                    //    FTStatus = FT245R.OpenByIndex(i);
                    // //   Log.Write("Open {0}: Result: {1}".Build(i, FTStatus.ToString()));
                    //    if (FT245R.IsOpen)
                    //    {
                    //        string D = "";
                    //        FT245R.GetDescription(out D);
                    //        Log.Write("Desc: {0}".Build(D));
                    //        try
                    //        {
                    //            FTStatus = FT245R.Close();
                    //            Log.Write("Close {i}: Result: {1}".Build(i, FTStatus.ToString()));
                    //        }
                    //        catch { }
                    //    }

                    //}
                    //Log.Write("All listed");

                    FTStatus = FT245R.GetDeviceList(Devices);
                    if (FTStatus == FTDI.FT_STATUS.FT_OK)
                    {


                        foreach (FTDI.FT_DEVICE_INFO_NODE DI in Devices)
                        {
                            if (DI != null && DI.Type == FTDI.FT_DEVICE.FT_DEVICE_232R)
                            {
                                if (ControllerNameBase.Any(N => DI.Description == N.Trim() + " " + ControllerNumber))
                                {

                                    Desc = DI.Description;
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
                                        else
                                        {
                                            Log.Exception("Purge failed for WS2811StripController {0}. Error: {1}".Build(ControllerNumber, FTStatus.ToString()));
                                        }
                                    }
                                    else
                                    {
                                        Log.Exception("Open failed for WS2811StripController {0}. Error: {1}".Build(ControllerNumber, FTStatus.ToString()));
                                    }
                                }
                            }
                        }
                    }
                    else
                    {

                        Log.Exception("Could not fetch devicelist for WS2811StripControllers. Error: {0}".Build(FTStatus.ToString()));
                    }
                }


                if (!OK)
                {
                    if (!Desc.IsNullOrWhiteSpace())
                    {
                        Log.Warning("{0} detected, but could not open connection.".Build(Desc));
                    }
                    else
                    {
                        Log.Warning("Direct Strip Controller with number {0} not found.".Build(ControllerNumber));
                    }
                    Close();

                }
                else
                {
                    Log.Write("{0} detected and connection opend.".Build(Desc));
                }
            }
        }


        private int ErrorCorrectionCnt = 0;

        void FT245R_CharReceived(object sender, EventArgs e)
        {
            uint CharsToRead = 0;

            FT245R.GetRxBytesAvailable(ref CharsToRead);

            if (CharsToRead > 0)
            {
                uint BytesRead = 0;
                byte[] Response = new Byte[CharsToRead];
                FT245R.Read(Response, CharsToRead, ref BytesRead);

                bool OK = true;
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
                        ErrorCorrectionCnt++;
                        if (ErrorCorrectionCnt <= 30)
                        {
                            Log.Warning("Received unexpected answer from Direct Strip Controller with number {0}. Will try to fix problem.".Build(ControllerNumber));
                            if (ErrorCorrectionCnt == 30)
                            {
                                Log.Write("No further warnings will be recorded for unexpected answers from this unit.");
                            }
                        }
                        FT245R.CharReceived -= new EventHandler<EventArgs>(FT245R_CharReceived);
                        uint Dummy = 0;
                        for (int i = 0; i < 2000; i++)
                        {
                            FT245R.Write(new byte[10], 10, ref Dummy);
                            Thread.Sleep(10);
                            CharsToRead = 0;
                            FT245R.GetRxBytesAvailable(ref CharsToRead);
                            if (CharsToRead > 0)
                            {
                                Thread.Sleep(10);
                                CharsToRead = 0;
                                FT245R.GetRxBytesAvailable(ref CharsToRead);
                                Response = new Byte[CharsToRead];
                                BytesRead = 0;
                                FT245R.Read(Response, CharsToRead, ref BytesRead);
                                break;
                            }


                        }
                        FT245R.CharReceived += new EventHandler<EventArgs>(FT245R_CharReceived);

                    }


                }
            }
        }


        /// <summary>
        /// Closes a currently open connection to a ledstrip controller.
        /// </summary>
        public void Close()
        {
            lock (FT245RLocker)
            {
                if (FT245R != null)
                {
                    if (FT245R.IsOpen)
                    {
                        FT245R.Close();

                    }
                    try
                    {
                        FT245R.CharReceived -= new EventHandler<EventArgs>(FT245R_CharReceived);
                    }
                    catch { }

                    FT245R = null;
                }
            }
        }



        /// <summary>
        /// Gets a list of available ledstrip controller numbers.
        /// </summary>
        /// <returns></returns>
        public static List<int> GetAvailableControllerNumbers()
        {
            List<int> L = new List<int>();

            FTDI FTD2xxWrapper = new FTDI();
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
                            string B = ControllerNameBase.FirstOrDefault(N => DI.Description.StartsWith(N.Trim() + " "));
                            int ControllerNr = 0;
                            if (B != null && int.TryParse(DI.Description.Substring(B.Length), out ControllerNr))
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
