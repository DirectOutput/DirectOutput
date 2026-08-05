using System;
using System.Diagnostics;
using System.Net.Sockets;
using DirectOutput;
using DirectOutput.Cab.Out;

namespace DirectOutput.Cab.Out.AdressableLedStrip
{
    public class FastUdpController : OutputControllerCompleteBase
    {
        // --- DEFAULT SETTINGS ---
        public string IpAddress { get; set; } = "192.168.4.1";
        public int Port { get; set; } = 6454;

        // --- HARDWARE HANDBRAKE ---
		// true: Dynamically engages the brake after 14KB (Ideal for W5500 chips)
		// false: Fires unrestricted (Ideal for Wi-Fi or native Ethernet PHYs)
        public bool SetW5500 { get; set; } = true;

        // --- DOF XML PROPERTIES ---
        public int NumberOfLedsStrip1 { get; set; } = 0;
        public int NumberOfLedsStrip2 { get; set; } = 0;
        public int NumberOfLedsStrip3 { get; set; } = 0;
        public int NumberOfLedsStrip4 { get; set; } = 0;
        public int NumberOfLedsStrip5 { get; set; } = 0;
        public int NumberOfLedsStrip6 { get; set; } = 0;
        public int NumberOfLedsStrip7 { get; set; } = 0;
        public int NumberOfLedsStrip8 { get; set; } = 0;
        public int NumberOfLedsStrip9 { get; set; } = 0;
        public int NumberOfLedsStrip10 { get; set; } = 0;
        public int NumberOfLedsStrip11 { get; set; } = 0;
        public int NumberOfLedsStrip12 { get; set; } = 0;
        public int NumberOfLedsStrip13 { get; set; } = 0;
        public int NumberOfLedsStrip14 { get; set; } = 0;
        public int NumberOfLedsStrip15 { get; set; } = 0;
        public int NumberOfLedsStrip16 { get; set; } = 0;

        private UdpClient _udpClient;
        private byte[] _ledBuffer;
        private int _headerSize = 33;
        private int _maxStrips = 16;
        private int _totalLeds = 0;
        private int _longestStripLeds = 0;

        // --- CHUNKING VARIABLES ---
        private byte _frameId = 0;
        private const int MAX_PAYLOAD_SIZE = 1400;

        protected override int GetNumberOfConfiguredOutputs()
        {
            int[] strips = new int[] {
                NumberOfLedsStrip1, NumberOfLedsStrip2, NumberOfLedsStrip3, NumberOfLedsStrip4,
                NumberOfLedsStrip5, NumberOfLedsStrip6, NumberOfLedsStrip7, NumberOfLedsStrip8,
                NumberOfLedsStrip9, NumberOfLedsStrip10, NumberOfLedsStrip11, NumberOfLedsStrip12,
                NumberOfLedsStrip13, NumberOfLedsStrip14, NumberOfLedsStrip15, NumberOfLedsStrip16
            };

            _totalLeds = 0;
            _longestStripLeds = 0;

            for (int i = 0; i < _maxStrips; i++)
            {
                _totalLeds += strips[i];
                if (strips[i] > _longestStripLeds) { _longestStripLeds = strips[i]; }
            }

            return _totalLeds * 3;
        }

        protected override bool VerifySettings()
        {
            return true;
        }

        protected override void ConnectToController()
        {
            GetNumberOfConfiguredOutputs();

            int[] strips = new int[] {
                NumberOfLedsStrip1, NumberOfLedsStrip2, NumberOfLedsStrip3, NumberOfLedsStrip4,
                NumberOfLedsStrip5, NumberOfLedsStrip6, NumberOfLedsStrip7, NumberOfLedsStrip8,
                NumberOfLedsStrip9, NumberOfLedsStrip10, NumberOfLedsStrip11, NumberOfLedsStrip12,
                NumberOfLedsStrip13, NumberOfLedsStrip14, NumberOfLedsStrip15, NumberOfLedsStrip16
            };

            _ledBuffer = new byte[_headerSize + (_totalLeds * 3)];
            _ledBuffer[0] = (byte)_maxStrips;

            for (int i = 0; i < _maxStrips; i++)
            {
                _ledBuffer[1 + (i * 2)] = (byte)(strips[i] >> 8);
                _ledBuffer[2 + (i * 2)] = (byte)(strips[i] & 0xFF);
            }

            _udpClient = new UdpClient();
        }

        protected override void DisconnectFromController()
        {
            if (_udpClient != null)
            {
                _udpClient.Close();
                _udpClient = null;
            }
        }

        protected override void UpdateOutputs(byte[] OutputValues)
        {
            if (OutputValues.Length == _totalLeds * 3)
            {
                // 1. Complete the large array in one go (copy data behind the 33-byte header)
                Buffer.BlockCopy(OutputValues, 0, _ledBuffer, _headerSize, OutputValues.Length);

                // 2. Calculate how many snacks we need (including header)
                int totalChunks = (int)Math.Ceiling((double)_ledBuffer.Length / MAX_PAYLOAD_SIZE);
                if (totalChunks == 0) totalChunks = 1;

                Stopwatch sw = new Stopwatch();
                int burstSize = 10;
                double msPerChunk = 0.4;
                int chunksInCurrentBurst = 0;

                // 3. Pack and send snacks
                for (int i = 0; i < totalChunks; i++)
                {
                    int offset = i * MAX_PAYLOAD_SIZE;
                    int payloadSize = Math.Min(MAX_PAYLOAD_SIZE, _ledBuffer.Length - offset);

                    byte[] chunk = new byte[3 + payloadSize];
                    chunk[0] = _frameId;
                    chunk[1] = (byte)i;
                    chunk[2] = (byte)totalChunks;

                    Buffer.BlockCopy(_ledBuffer, offset, chunk, 3, payloadSize);

                    _udpClient.Send(chunk, chunk.Length, IpAddress, Port);

                    // --- THE DYNAMIC W5500 CHUNK HANDBRAKE ---
                    if (SetW5500)
                    {
                        chunksInCurrentBurst++;

                        if (chunksInCurrentBurst == burstSize && (i + 1) < totalChunks)
                        {
                            long waitTicks = (long)(chunksInCurrentBurst * msPerChunk * Stopwatch.Frequency / 1000.0);
                            sw.Restart();
                            while (sw.ElapsedTicks < waitTicks) { }

                            chunksInCurrentBurst = 0; // Reset counter
                        }
                    }
                }

                _frameId++;

                // --- 4. CALCULATE END-OF-FRAME WAIT TIME---
                // Basis: Time for the latching of the LEDs (0.02ms per LED on the longest strip)
                double totalWaitTimeMs = _longestStripLeds * 0.02;

                // Add remaining time for W5500 if activated and leftovers remain
                if (SetW5500 && totalChunks > burstSize && chunksInCurrentBurst > 0)
                {
                    totalWaitTimeMs += (chunksInCurrentBurst * msPerChunk);
                }

                // 5. A single, highly efficient wait block for the rest of the frame
                if (totalWaitTimeMs > 0)
                {
                    long waitTicksEnd = (long)(totalWaitTimeMs * Stopwatch.Frequency / 1000.0);
                    sw.Restart();
                    while (sw.ElapsedTicks < waitTicksEnd) { }
                }
            }
        }
    }
}