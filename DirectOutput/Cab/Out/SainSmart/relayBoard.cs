using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DirectOutput.Cab.Out.FTDIChip;


namespace DirectOutput.Cab.Out.SainSmart
{
   
    /// <summary>
    /// SainSmart 5V USB Relay Board control
    /// 
    /// - Make sure FTDI D2XX drivers are installed: http://www.ftdichip.com/Drivers/D2XX.htm
    /// - Reference the .NET Wrapper FTD2XX_NET v1.0.14.0: http://www.ftdichip.com/Support/SoftwareExamples/CodeExamples/CSharp.htm
    /// 
    /// Author: Anthony Marshall, Feburary 2013
    /// 
    /// </summary>

    public enum Relaystate
    {
        ON,
        OFF
    }

    public enum Relaynum : int
    {
        ONE=0,
        TWO=1,
        THREE=2,
        FOUR=3
    }

    class relayBoard
    {
        public relayBoard()
        {
            connect();
        }

        private static readonly Lazy<relayBoard> lazy = new Lazy<relayBoard>(() => new relayBoard());

        public static relayBoard Instance { get { return lazy.Value; } }

        private byte[] startup = { 0x00 };
        private uint bytesToSend = 1;

        private UInt32 ftdiDeviceCount = 0;
        private FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_OK;
       
        FTDI myFtdiDevice = new FTDI();

        public int[] SainSmartIOGetIdList()
        {
            var instancelist = new List<int>();

            for (int i = 0; i < ftdiDeviceCount; i++)
                instancelist.Add(i+1);

            return instancelist.ToArray();
        }


        /// <summary>
        /// Find the FDTI chip, connect, set baud to 9600, set sync bit-bang mode
        /// </summary>
        /// <returns></returns>
        public bool connect()
        {
            // Determine the number of FTDI devices connected to the machine
            ftStatus = myFtdiDevice.GetNumberOfDevices(ref ftdiDeviceCount);
            // Check status
            if (ftStatus == FTDI.FT_STATUS.FT_OK)
            {
    //            // Console.WriteLine("Number of FTDI devices: " + ftdiDeviceCount.ToString());
      //          // Console.WriteLine("");
            }
            else
            {
        //        // Console.WriteLine("Failed to get number of devices (error " + ftStatus.ToString() + ")");
                return false;
            }

            if (ftdiDeviceCount == 0)
            {              
        //        // Console.WriteLine("Relay board not found, please try again");
                return false;
            }
            else
            {
                // Allocate storage for device info list
                FTDI.FT_DEVICE_INFO_NODE[] ftdiDeviceList = new FTDI.FT_DEVICE_INFO_NODE[ftdiDeviceCount];

                // Populate our device list
                ftStatus = myFtdiDevice.GetDeviceList(ftdiDeviceList);

                // TODO: break this out into its own function
                // so desired adapter can be selected. 
                // Open first device in our list by serial number
                ftStatus = myFtdiDevice.OpenBySerialNumber(ftdiDeviceList[0].SerialNumber);

                if (ftStatus != FTDI.FT_STATUS.FT_OK)
                {
                    // Console.WriteLine("Error connecting to relay board");
                    return false;
                }

                // Set Baud rate to 9600
                ftStatus = myFtdiDevice.SetBaudRate(9600);

                // Set FT245RL to synchronous bit-bang mode, used on sainsmart relay board
                myFtdiDevice.SetBitMode(0xFF, FTDI.FT_BIT_MODES.FT_BIT_MODE_SYNC_BITBANG);
                // Switch off all the relays
                myFtdiDevice.Write(startup, 1, ref bytesToSend);

                return true;
            }
        }

        /// <summary>
        /// Activate/De-activate a specific relay
        /// </summary>
        /// <param name="Rnum"></param>
        /// <param name="state"></param>
        public void RelaySwitch(Relaynum Rnum, Relaystate state)
        {
            uint numBytes = 1;
            int relay = 0x00;
            byte[] Out = { 0x00 };
            byte pins = 0x00;
            byte output = 0x00;

            // Find which relays are ON/OFF
            myFtdiDevice.GetPinStates(ref pins);

            if ((int)Rnum > 7)
                throw new Exception("Relay number must be between 0 and 7");

            relay = 1 << (int)Rnum;

            switch (state)
            {
                case Relaystate.ON:
                    output = (byte)(pins | relay);
                    break;
                case Relaystate.OFF:
                    output = (byte)(pins & ~(relay));
                    break;
            }

            Out[0] = output;
            myFtdiDevice.Write(Out, 1, ref numBytes);    
        }

        public void RelayAll(byte Output)
        {
            uint numBytes = 0 ;
            byte[] Out = { Output };


            var res = myFtdiDevice.Write(Out, 1, ref numBytes);
            if (numBytes != 1)
            {
                // Rapid calls seem to kill the output. 
                myFtdiDevice.Close();
                connect();
            }
             //   throw new Exception(String.Format("FTDI failed write, status = {0}", (int)res));
        
            //myFtdiDevice.            
        }
    }
}
