using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Timers;


namespace DirectOutput.Cab.Out.DMX.ArtnetEngine
{
    /// <summary>
    /// Artnet Engine used for DMX output.<br />
    /// The code of this class is based on the Engine class of eDMX.Net hosted on http://edmx.codeplex.com/.
    /// </summary>
    public class Engine  
    {

        private static readonly Lazy<Engine> lazy = new Lazy<Engine>(() => new Engine());

        /// <summary>
        /// Gets a singleton instance of the ArtNet engine.
        /// </summary>
        /// <value>
        /// The instance of the ArtNet engine.
        /// </value>
        public static Engine Instance { get { return lazy.Value; } }





        #region "Public"

        /// <summary>
        /// Initializes a new instance of the <see cref="Engine"/> class.<br/>
        /// \note The BroadcastAddress is a optional parameter. It is however highly recommended to specifiy the IP address of your Art-Net node. This avoids unnecessary traffic on your network.
        /// </summary>
        private Engine()
        {
            ArtNetHeader = new byte[] { 0x41, 0x72, 0x74, 0x2d, 0x4e, 0x65, 0x74, 0 };
            //ArtNetAddress = new byte[] { 0x7f, 0, 0, 1, Convert.ToByte(this.LoByte(0x1936)), Convert.ToByte(this.HiByte(0x1936)) };

            lock (UdpServerLocker)
            {
                try
                {
                    //Log.Write("Init artnet engine");
                    UdpServer = new UdpClient();
                    UdpServer.ExclusiveAddressUse = false;
                    
                    UdpServer.EnableBroadcast = true;
                    UdpServer.Client.SendTimeout = 100;
                    UdpServer.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
              
                }
                catch (Exception E)
                {
                    Log.Exception("Could not initialize UdpServer for ArtNet (Port: 6454).", E);
                    UdpServer = null;
                }
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Engine"/> class.
        /// </summary>
        ~Engine()
        {
            lock (UdpServerLocker)
            {
                if (UdpServer != null)
                {
                    try
                    {
                        UdpServer.Close();
                    }
                    catch { }
                    UdpServer = null;
                    //Log.Write("drop artnet engine");
                }
            }
        }


        int SendExceptionCnt = 0;
        /// <summary>
        /// Sends DMX data to a Art-Net node.
        /// </summary>
        /// <param name="BroadcastAdress">The broadcast adress for the transmission.</param>
        /// <param name="Universe">The DMX universe to be addressed.</param>
        /// <param name="Data">The DMX data to be sent.</param>
        /// <param name="DataLength">Length of the data.</param>
        public void SendDMX(string BroadcastAdress, short Universe, byte[] Data, int DataLength)
        {
            if (UdpServer != null)
            {
                byte[] packet = new byte[(0x11 + DataLength) + 1];
                Buffer.BlockCopy(this.ArtNetHeader, 0, packet, 0, this.ArtNetHeader.Length);
                packet[8] = Convert.ToByte(this.LoByte(0x5000));
                packet[9] = Convert.ToByte(this.HiByte(0x5000));
                packet[10] = 0;          //ProtVerHi
                packet[11] = 14;         //ProtVerLo
                packet[12] = 0;          //Sequence
                packet[13] = 0;          //Physical
                packet[14] = Convert.ToByte(this.LoByte(Universe));
                packet[15] = Convert.ToByte(this.HiByte(Universe));
                packet[0x10] = Convert.ToByte(this.HiByte(DataLength));
                packet[0x11] = Convert.ToByte(this.LoByte(DataLength));

                try
                {
                    Buffer.BlockCopy(Data, 0, packet, 0x12, DataLength);
                }
                catch (Exception exception1)
                {
                    Log.Exception("A exception occured in the ArtNet Engine class.", exception1);
                    //Error(exception1, new EventArgs());
                }

                lock (UdpServerLocker)
                {
                    if (UdpServer != null)
                    {
                        try
                        {

                            UdpServer.Send(packet, packet.Length, BroadcastAdress, 0x1936);
                            SendExceptionCnt = 0;
                        }
                        catch (Exception E)
                        {
                            SendExceptionCnt++;
                            if (SendExceptionCnt > 10)
                            {
                                try
                                {
                                    Log.Exception("More than 10 consuecutive transmissions of ArtNet data have failed. ArtNet enigine will be disabled for the session.", E);
                                }
                                catch { }

                                if (UdpServer != null)
                                {
                                    try
                                    {
                                        UdpServer.Close();
                                    }
                                    catch { }
                                    UdpServer = null;
                                }
                            }
                            else
                            {
                                Log.Exception("A exception has occured when sending ArtNet data.", E);
                            }

                        }

                    }
                }
            }
        }

        #endregion




        #region "Work Around"


        private object LoByte(int wParam)
        {
            return (wParam & 0xffL);
        }

        private object HiByte(int wParam)
        {
            return ((wParam / 0x100) & 0xffL);
        }



        #endregion



        private object UdpServerLocker = new object();
        private UdpClient UdpServer;


        private byte[] ArtNetHeader;


    }
}
