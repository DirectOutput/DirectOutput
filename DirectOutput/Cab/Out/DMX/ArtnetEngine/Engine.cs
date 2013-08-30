using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Timers;


namespace DirectOutput.Cab.Out.DMX.ArtnetEngine
{
    /// <summary>
    /// Artnet Engine used for DMX output.<br />
    /// The code of this class is a stripped down and modified version of the Engine class of eDMX.Net hosted on http://edmx.codeplex.com/.
    /// </summary>
   public class Engine
   {

       #region "Public"

       /// <summary>
       /// Initializes a new instance of the <see cref="Engine"/> class.<br/>
       /// \note The BroadcastAddress is a optional parameter. It is however highly recommended to specifiy the IP address of your Art-Net node. This avoids unnecessary traffic on your network.
       /// </summary>
       /// <param name="Name">The name for the Art-Net engine (optional).</param>
       /// <param name="BroadcastAddress">The broadcast address of the ArtNet node to be addressed (optional).</param>
       public Engine(string Name="", string BroadcastAddress="")
       {
           ArtNetHeader = new byte[] { 0x41, 0x72, 0x74, 0x2d, 0x4e, 0x65, 0x74, 0 };
           //ArtNetAddress = new byte[] { 0x7f, 0, 0, 1, Convert.ToByte(this.LoByte(0x1936)), Convert.ToByte(this.HiByte(0x1936)) };
           ArtNetName = "Art-Net Node";

           if (Name.IsNullOrWhiteSpace())
           {
               ArtNetName = "Art-Net Node";
               //ArtNetNodeReport = "LiteCore"; 
           }
           else
           {
               ArtNetName = Name;
               //ArtNetNodeReport = Name; 
           }


           if (BroadcastAddress.IsNullOrWhiteSpace())
           {
               this.BroadcastAddress = "255.255.255.255";
           }
           else
           {
               this.BroadcastAddress = BroadcastAddress;
           }

           UdpServer = new UdpClient(0x1936); 
           UdpServer.EnableBroadcast = true;
           UdpServer.Client.SendTimeout = 100;
           UdpServer.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

           
           //updateTimer = new System.Timers.Timer();
           //updateTimer.Interval = 400;
           //updateTimer.Elapsed += new ElapsedEventHandler(timer_Elapsed);

           //Status = RunStatus.Stopped;          
       }

       //public void Start()
       //{
       //    //updateTimer.Enabled = true;
       //    //updateTimer.Start();
       //    Status = RunStatus.Running;
       //}

       //public void Pause()
       //{
       //    //updateTimer.Enabled = false;
       //    //updateTimer.Stop();
       //    Status = RunStatus.Paused;
       //}

       /// <summary>
       /// Sends DMX data to a Art-Net node.
       /// </summary>
       /// <param name="Universe">The DMX universe to be addressed.</param>
       /// <param name="Data">The DMX data to be sent.</param>
       /// <param name="DataLength">Length of the data.</param>
       public void SendDMX(short Universe, byte[] Data, int DataLength)
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

           broadcast(packet);
       }
       
       #endregion 
              
       //---------------------------------------------------------------------------------------------------------------------------------------------

       #region "Private"

       #region Commented Receive routines
       //private void sendArtPoll()
       //{
       //    byte[] packet = new byte[14];

       //    Buffer.BlockCopy(this.artNetHeader, 0, packet, 0, this.artNetHeader.Length);

       //    packet[8] = Convert.ToByte(this.LoByte(0x2000));
       //    packet[9] = Convert.ToByte(this.HiByte(0x2000));
       //    packet[10] = 0;      //ProtVerHi
       //    packet[11] = 14;     //ProtVerLo
       //    packet[12] = 0;      //TalkToMe
       //    packet[13] = 0;      //Priority
       //    broadcast(packet);
       //}

       //private void sendArtPollReply()
       //{
       //    byte[] header = new byte[14];

       //    byte num;
       //    byte[] packet = new byte[240];
       //    Buffer.BlockCopy(this.artNetHeader, 0, packet, 0, this.artNetHeader.Length);
       //    packet[8] = Convert.ToByte(this.LoByte(0x2100));
       //    packet[9] = Convert.ToByte(this.HiByte(0x2100));
       //    Buffer.BlockCopy(this.artNetAddress, 0, packet, 10, this.artNetAddress.Length);
       //    packet[0x10] = 1;
       //    packet[0x11] = 1;
       //    packet[0x12] = 0;
       //    packet[0x13] = 0;
       //    packet[20] = Convert.ToByte(this.HiByte(0xff));
       //    packet[0x15] = Convert.ToByte(this.LoByte(0xff));
       //    packet[0x16] = 0;
       //    packet[0x17] = 0;
       //    packet[0x18] = 0;
       //    packet[0x19] = 0;
       //    byte num2 = (byte)((0x1a + this.artNetShortName.Length) - 1);
       //    for (num = 0x1a; num <= num2; num = (byte)(num + 1))
       //    {
       //        packet[num] = (byte)Asc(new string(this.artNetShortName.ToCharArray(num - 0x1a, 1)));
       //    }
       //    byte num3 = (byte)((0x2c + this.artNetLongName.Length) - 1);
       //    for (num = 0x2c; num <= num3; num = (byte)(num + 1))
       //    {
       //        packet[num] = (byte) Asc(new string(this.artNetLongName.ToCharArray(num - 0x2c, 1)));
       //    }
       //    byte num4 = (byte)((0x6c + this.artNetNodeReport.Length) - 1);
       //    for (num = 0x6c; num <= num4; num = (byte)(num + 1))
       //    {

       //        packet[num] = (byte) Asc(new string(this.artNetNodeReport.ToCharArray(num - 0x6c, 1)));
       //    }
       //    packet[0xac] = 0;
       //    packet[0xad] = 4;
       //    packet[0xae] = 0;
       //    packet[0xaf] = 0;
       //    packet[0xb0] = 0;
       //    packet[0xb1] = 0;
       //    packet[0xb2] = 0;
       //    packet[0xb3] = 0;
       //    packet[180] = 0;
       //    packet[0xb5] = 0;
       //    packet[0xb6] = 0;
       //    packet[0xb7] = 0;
       //    packet[0xb8] = 0;
       //    packet[0xb9] = 0;
       //    packet[0xba] = 0;
       //    packet[0xbb] = 0;
       //    packet[0xbc] = 0;
       //    packet[0xbd] = 0;
       //    packet[190] = 0;
       //    packet[0xbf] = 0;
       //    packet[0xc0] = 0;
       //    packet[0xc1] = 0;
       //    packet[0xc2] = 0;
       //    packet[0xc3] = 0;
       //    packet[0xc4] = 0;
       //    num = 0xc5;
       //    do
       //    {
       //        packet[num] = 0;
       //        num = (byte)(num + 1);
       //    }
       //    while (num <= 0xc7);
       //    packet[200] = 0;
       //    packet[0xc9] = 0;
       //    packet[0xca] = 0;
       //    packet[0xcb] = 0;
       //    packet[0xcc] = 0;
       //    packet[0xcd] = 0;
       //    packet[0xce] = 0;
       //    num = 0xcf;
       //    do
       //    {
       //        packet[num] = num;
       //        num = (byte)(num + 1);
       //    }
       //    while (num <= 0xef);
       //    packet[0xef] = 0xff;

       //    broadcast(packet);

       //}

       //private void recieve()
       //{

       //    IPEndPoint RemoteServer = new IPEndPoint(IPAddress.Any, 0);
       //    for (; ; )
       //    {
       //        try
       //        {
       //            byte[] RecPacket = udpServer.Receive(ref RemoteServer);
       //            string test = Encoding.ASCII.GetString(RecPacket);

       //            switch (((OpCodesEnum)(RecPacket[this.artNetHeader.Length + 1] << (8 + RecPacket[this.artNetHeader.Length]))))
       //            {
       //                case OpCodesEnum.OpPoll:
       //                    this.onPoll(RecPacket);
       //                    break;

       //                case OpCodesEnum.OpPollReply:
       //                    this.onPollReply(RecPacket, RemoteServer);
       //                    break;

       //                case OpCodesEnum.OpDmx:
       //                    this.onDmxRecieved(RecPacket, RemoteServer);
       //                    break;
       //            }

       //        }
       //        catch 
       //        {
       //            //Error(ex, new EventArgs()); 

       //            //Nothing has happened so we shall do nout.
       //        }
       //    }
       //}

       //private void onDmxRecieved(byte[] Data, IPEndPoint RemoteServer)
       //{
       //    DmxInPacket Dmx = new DmxInPacket();

       //    //Set Universe number.
       //    Dmx.Universe = Convert.ToInt16(Data[14]);
       //    Buffer.BlockCopy(Data, 18, Dmx.DmxData, 0, Dmx.DmxData.Length);

       //    ArtNetDetected = true;

       //    lastPacketRecieved = Dmx;


       //    RecievingDMX(Dmx, new EventArgs());

       //}

       //private void onPollReply(byte[] RecPacket, IPEndPoint RemoteServer)
       //{
       //    throw new NotImplementedException();
       //}

       //private void onPoll(byte[] RecPacket)
       //{
       //    sendArtPollReply();
       //}

       //private void timer_Elapsed(object sender, ElapsedEventArgs e)
       //{
       //    sendArtPoll();
       //    recieve();

       //} 
       #endregion

       private void broadcast(byte[] data)
       {


               UdpServer.Send(data, data.Length, this.BroadcastAddress, 0x1936);
         

       }


       
       #endregion

       //---------------------------------------------------------------------------------------------------------------------------------------------

       #region "Work Around"

       //Work around for lack of Asc in C# vs VB.
           //static short Asc(string String)
           //{
           //    return Encoding.Default.GetBytes(String)[0];
           //}
       
           //static string Chr(int CharCode)
           //{
           //    if (CharCode > 255)
           //        throw new ArgumentOutOfRangeException("CharCode", CharCode, "CharCode must be between 0 and 255.");
           //    return Encoding.Default.GetString(new[] { (byte)CharCode });
           //}

           private object LoByte(int wParam)
           {
               return (wParam & 0xffL);
           }

           private object HiByte(int wParam)
           {
               return ((wParam / 0x100) & 0xffL);
           }

           //private short MakeInt16(byte lsb, byte msb)
           //{
           //    short num2 = msb;
           //    num2 = (short)(num2 << 8);
           //    return (short)(num2 + lsb);
           //}

#endregion

       //---------------------------------------------------------------------------------------------------------------------------------------------

       //Events
       //public event EventHandler RecievingDMX;
       //private event EventHandler Error;

       //---------------------------------------------------------------------------------------------------------------------------------------------

       //Fields
           /// <summary>
           /// Gets or sets the broadcast address for the ArtNet node.
           /// </summary>
           /// <value>
           /// The broadcast address.
           /// </value>
       public string BroadcastAddress { get; set; }
       private UdpClient UdpServer;
       //private System.Timers.Timer updateTimer;
       //private byte[] ArtNetAddress;
       private string ArtNetName;
       private byte[] ArtNetHeader;
       //private string ArtNetNodeReport;       
       //private RunStatus Status;
       //private bool ArtNetDetected = false;
       //private DmxInPacket lastPacketRecieved;
       //private enum RunStatus
       //{
       //   Running,
       //   Paused,
       //   Stopped
       //}

    }
}
