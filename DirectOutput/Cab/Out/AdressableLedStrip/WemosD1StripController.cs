using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DirectOutput.Cab.Out.AdressableLedStrip
{
    /// <summary>
    /// The WemosD1MPStripController is a Teensy equivalent board called Wemos D1 Mini Pro (also known as ESP8266), it's cheaper than the Teensy and can support the same amount of Ledstrip and leds per strip.
    /// 
    /// \image html wemos-d1-mini-pro.jpg
    /// 
    /// The Wemos D1 Mini Pro firmware made by aetios50, peskopat and yoyofr can be found there <a target="_blank" href="https://github.com/aetios50/PincabLedStrip">aetios50 Github page</a>
    /// 
    /// You can also find a tutorial (in french for now) explaining the flashing process for the Wemos D1 Mini Pro using these firmware <a target="_blank" href="https://shop.arnoz.com/laboratoire/2019/10/29/flasher-unewemos-d1-mini-pro-pour-lutiliser-dans-son-pincab/">Arnoz' lab Wemos D1 flashing tutorial</a>
    /// 
    /// There is a great online tool to setup easily both Teensy and Wemos D1 based ledstrips (an english version is also available) <a target="_blank" href="https://shop.arnoz.com/laboratoire/2020/09/17/cacabinet-generator/">Arnoz' cacabinet generator</a>
    /// 
    /// </summary>
    public class WemosD1MPStripController : TeensyStripController
    {
        private bool _SendPerLedstripLength = false;

        /// <summary>
        /// Set if the controller will send per ledstrip length commands during the handshake.
        /// </summary>
        /// <value>
        /// true if the commands are sent
        /// </value>
        public bool SendPerLedstripLength
        {
            get { return _SendPerLedstripLength; }
            set {
                _SendPerLedstripLength = value;
            }
        }

        private bool _UseCompression = false;

        /// <summary>
        /// Use a simple colorbased RLE compression on each ledstrip data when sent
        /// </summary>
        /// <value>
        /// true, use the compression when it worth it
        /// </value>
        public bool UseCompression
        {
            get { return _UseCompression; }
            set { _UseCompression = value; }
        }

        private bool _TestOnConnect = false;

        /// <summary>
        /// Ask the Wemos to make a simple RGB led test when connecting
        /// </summary>
        /// <value>
        /// true, will ask for the test at connection stage
        /// </value>
        public bool TestOnConnect
        {
            get { return _TestOnConnect; }
            set { _TestOnConnect = value; }
        }

        protected override void SetupController()
        {
            byte[] ReceiveData = null;
            int BytesRead = -1;
            byte[] CommandData = null;

            base.SetupController();

            //Send number of leds per leds strips 
            if (SendPerLedstripLength) {
                for (var numled = 0; numled < NumberOfLedsPerStrip.Length; ++numled) {
                    int nbleds = NumberOfLedsPerStrip[numled];
                    if (nbleds > 0) {
                        CommandData = new byte[5] { (byte)'Z', (byte)numled, (byte)(NumberOfLedsPerStrip.Length - 1), (byte)(nbleds >> 8), (byte)(nbleds & 255) };
                        Log.Write($"Resize ledstrip {numled} to {nbleds} leds.");
                        ComPort.Write(CommandData, 0, 5);
                        ReceiveData = new byte[1];
                        BytesRead = -1;
                        try {
                            BytesRead = ReadPortWait(ReceiveData, 0, 1);
                        } catch (Exception E) {
                            throw new Exception($"Expected 1 bytes after setting the number of leds for ledstrip {numled} , but the read operation resulted in a exception. Will not send data to the controller.", E);
                        }

                        if (BytesRead != 1 || ReceiveData[0] != (byte)'A') {
                            throw new Exception($"Expected a Ack (A) after setting the number of leds for ledstrip {numled}, but received no answer or a unexpected answer ({(char)ReceiveData[0]}). Will not send data to the controller.");
                        }
                    }
                }
            }

            if (TestOnConnect) {
                CommandData = new byte[1] { (byte)'T' };
                Log.Write($"Send a test request to the controller");
                ComPort.Write(CommandData, 0, 1);

                //Temporary wait before asking the Ack
                Thread.Sleep(2000);

                ReceiveData = new byte[1];
                BytesRead = -1;
                try {
                    BytesRead = ReadPortWait(ReceiveData, 0, 1);
                } catch (Exception E) {
                    throw new Exception($"Expected 1 bytes after requesting a test sequence, but the read operation resulted in a exception. Will not send data to the controller.", E);
                }

                if (BytesRead != 1 || ReceiveData[0] != (byte)'A') {
                    throw new Exception($"Expected a Ack (A) after requesting a test sequence, but received no answer or a unexpected answer ({(char)ReceiveData[0]}). Will not send data to the controller.");
                }

                TestOnConnect = false;
            }

        }

        protected List<byte> CompressedData = new List<byte>();
        protected List<byte> UncompressedData = new List<byte>();

        protected override void SendLedstripData(byte[] OutputValues, int TargetPosition)
        {
            if (UseCompression) {
                //Try a simple color based RLE compression
                CompressedData.Clear();
                UncompressedData.Clear();
                UncompressedData.AddRange(OutputValues);

                while (UncompressedData.Count > 0) {
                    if (UncompressedData.Count == 3) {
                        CompressedData.Add(1);
                        CompressedData.Add(UncompressedData[0]);
                        CompressedData.Add(UncompressedData[1]);
                        CompressedData.Add(UncompressedData[2]);
                        UncompressedData.RemoveRange(0, 3);
                    } else {
                        byte r = UncompressedData[0];
                        byte g = UncompressedData[1];
                        byte b = UncompressedData[2];
                        UncompressedData.RemoveRange(0, 3);
                        int value = (r << 16) | (g << 8) | b;
                        int cnt = 1;
                        while (UncompressedData.Count > 0 && ((UncompressedData[0] << 16) | (UncompressedData[1] << 8) | UncompressedData[2]) == value && cnt < byte.MaxValue-1) {
                            UncompressedData.RemoveRange(0, 3);
                            cnt++;
                        }
                        CompressedData.Add((byte)cnt);
                        CompressedData.Add(r);
                        CompressedData.Add(g);
                        CompressedData.Add(b);
                    }

                }

                if (CompressedData.Count < OutputValues.Length) {
                    var nbData = CompressedData.Count / 4;
                    var nbLeds = OutputValues.Length / 3;
                    byte[] CommandData = new byte[7] {  (byte)'Q',
                                                    (byte)(TargetPosition >> 8), (byte)(TargetPosition & 255),
                                                    (byte)(nbData >> 8), (byte)(nbData & 255),
                                                    (byte)(nbLeds >> 8), (byte)(nbLeds & 255)
                                                    };
                    ComPort.Write(CommandData, 0, 7);
                    ComPort.Write(CompressedData.ToArray(), 0, CompressedData.Count);
                } else {
                    base.SendLedstripData(OutputValues, TargetPosition);
                }
            } else {
                base.SendLedstripData(OutputValues, TargetPosition);
            }
        }
    }
}
