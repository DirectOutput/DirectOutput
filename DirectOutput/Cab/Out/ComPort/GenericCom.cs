using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Text.RegularExpressions;

namespace DirectOutput.Cab.Out.ComPort
{
    //public class GenericCom : OutputControllerFlexCompleteBase
    //{

    //    /// <summary>
    //    /// Gets or sets the COM port for the controller.
    //    /// </summary>
    //    /// <value>
    //    /// The COM port for the controller.
    //    /// </value>
    //    public string ComPort { get; set; }


    //    private int _BaudRate = 115200;

    //    /// <summary>
    //    /// Gets or sets the baudrate for the com port.
    //    /// </summary>
    //    /// <value>
    //    /// The baudrate.
    //    /// </value>
    //    public int Baudrate
    //    {
    //        get { return _BaudRate; }
    //        set { _BaudRate = value; }
    //    }


    //    private Parity _Parity = Parity.None;

    //    /// <summary>
    //    /// Gets or sets the parity for the com port.
    //    /// </summary>
    //    /// <value>
    //    /// The parity.
    //    /// </value>
    //    public Parity Parity
    //    {
    //        get { return _Parity; }
    //        set { _Parity = value; }
    //    }

    //    private int _DataBits = 0;

    //    /// <summary>
    //    /// Gets or sets the data bits for the com port.
    //    /// </summary>
    //    /// <value>
    //    /// The data bits.
    //    /// </value>
    //    public int DataBits
    //    {
    //        get { return _DataBits; }
    //        set { _DataBits = value; }
    //    }

    //    private StopBits _StopBits = StopBits.One;

    //    /// <summary>
    //    /// Gets or sets the stop bits for the com port.
    //    /// </summary>
    //    /// <value>
    //    /// The stop bits.
    //    /// </value>
    //    public StopBits StopBits
    //    {
    //        get { return _StopBits; }
    //        set { _StopBits = value; }
    //    }


    //    /// <summary>
    //    /// Gets or sets the open connection expression.
    //    /// The result of this expression is sent to the com port after the connection to the port has been established.
    //    /// </summary>
    //    /// <value>
    //    /// The open connection expression.
    //    /// </value>
    //    public string OpenConnectionExpression { get; set; }

    //    /// <summary>
    //    /// Gets or sets the close connection expression.
    //    /// </summary>
    //    /// <value>
    //    /// The close connection expression.
    //    /// </value>
    //    public string CloseConnectionExpression { get; set; }

    //    public string UpdateStartExpression { get; set; }

    //    public string UpdateEndExpression { get; set; }

    //    public string UpdateOutputExpression { get; set; }

    //    private GenericComExpression OpenConnection = null;
    //    private GenericComExpression CloseConnection = null;
    //    private GenericComExpression UpdateStart = null;
    //    private GenericComExpression UpdateEnd = null;
    //    private GenericComExpression UpdateOutput = null;


    //    private SerialPort Port = null;
    //    private object PortLocker = new object();

    //    ExpressionContext Context = null;



    //    /// <summary>
    //    /// Initializes the output controller and starts the updater thread.
    //    /// </summary>
    //    /// <param name="Cabinet">The cabinet object which is using the output controller instance.</param>
    //    public override void Init(Cabinet Cabinet)
    //    {
    //        base.Init(Cabinet);

    //        SetExpressionContext();


    //        if (!OpenConnectionExpression.IsNullOrWhiteSpace())
    //        {
    //            OpenConnection = ParseExpression(Context, OpenConnectionExpression);
    //        }
    //        if (!CloseConnectionExpression.IsNullOrWhiteSpace())
    //        {
    //            CloseConnection = ParseExpression(Context, CloseConnectionExpression);
    //        }
    //        if (!UpdateStartExpression.IsNullOrWhiteSpace())
    //        {
    //            UpdateStart = ParseExpression(Context, UpdateStartExpression);
    //        }
    //        if (!UpdateEndExpression.IsNullOrWhiteSpace())
    //        {
    //            UpdateEnd = ParseExpression(Context, UpdateEndExpression);
    //        }
    //        if (!UpdateOutputExpression.IsNullOrWhiteSpace())
    //        {
    //            UpdateOutput = ParseExpression(Context, UpdateOutputExpression);
    //        }



    //    }

    //    /// <summary>
    //    /// Finishes the output controller and stops the updater thread.
    //    /// </summary>
    //    public override void Finish()
    //    {
    //        OpenConnection = null;
    //        CloseConnection = null;
    //        UpdateStart = null;
    //        CloseConnection = null;
    //        UpdateEnd = null;
    //        UpdateOutput = null;
    //        Context = null;
    //        base.Finish();
    //    }
       


    //    protected override bool VerifySettings()
    //    {
    //        if (ComPort.IsNullOrWhiteSpace())
    //        {
    //            Log.Warning("ComPort is not set for {0} {1}.".Build(this.GetType().Name, Name));
    //            return false;
    //        }

    //        if (!SerialPort.GetPortNames().Any(x => x.Equals(ComPort, StringComparison.InvariantCultureIgnoreCase)))
    //        {
    //            Log.Warning("ComPort {2} is defined for {0} {1}, but does not exist.".Build(this.GetType().Name, Name, ComPort));
    //            return false;
    //        };

    //        if (OpenConnectionExpression.IsNullOrWhiteSpace() && CloseConnectionExpression.IsNullOrWhiteSpace() && UpdateStartExpression.IsNullOrWhiteSpace() && UpdateEndExpression.IsNullOrWhiteSpace() && UpdateOutputExpression.IsNullOrWhiteSpace())
    //        {
    //            Log.Warning("No communication data is defined for {0} {1} on port {2}.".Build(this.GetType().Name, Name, ComPort));
    //            return false;
    //        }


    //        return true;
    //    }


    //    private void SetExpressionContext()
    //    {
    //        Context = new ExpressionContext();
    //        Context.Options.ParseCulture = System.Globalization.CultureInfo.InvariantCulture;
    //        Context.Imports.AddType(typeof(Math));

    //        for (int i = 1; i <= NumberOfOutputs; i++)
    //        {
    //            Context.Variables["V" + i] = 0;
    //        }
    //        Context.Variables["CV"] = 0;
    //        Context.Variables["CN"] = 0;


    //    }

    //    private void UpdateExpressionContextValues(byte[] Values)
    //    {
    //        for (int i = 1; i <= NumberOfOutputs; i++)
    //        {
    //            Context.Variables["V" + i] = Values[i];
    //        }
    //    }

    //    private void UpdateExpressionContextCurrent(byte[] Values, int Number)
    //    {
    //        if (Number > 0)
    //        {
    //            Context.Variables["CV"] = Values[Number - 1];
    //            Context.Variables["CN"] = Number;
    //        }
    //        else
    //        {
    //            Context.Variables["CV"] = 0;
    //            Context.Variables["CN"] = 0;
    //        }
    //    }


    //    private byte[] Evaluate(GenericComExpression Exp)
    //    {
    //        string D = Exp.Expression;
    //        foreach (KeyValuePair<string, IGenericExpression<byte>> KV in Exp.ByteExpressions)
    //        {
    //            D = D.Replace(KV.Key, ((char)KV.Value.Evaluate()).ToString());
    //        }
    //        foreach (KeyValuePair<string, IGenericExpression<string>> KV in Exp.StringExpressions)
    //        {
    //            D = D.Replace(KV.Key, KV.Value.Evaluate());
    //        }
    //        return D.GetBytes();
    //    }



    //    private GenericComExpression ParseExpression(ExpressionContext Context, string Expression)
    //    {

    //        string ExpressionPrepared = Expression;


    //        Dictionary<string, IGenericExpression<byte>> ByteExpressions = new Dictionary<string, IGenericExpression<byte>>();
    //        Dictionary<string, IGenericExpression<string>> StringExpressions = new Dictionary<string, IGenericExpression<string>>();

    //        Regex Regex = new Regex("{.*?}");
    //        MatchCollection Matches = Regex.Matches(ExpressionPrepared);
    //        foreach (Match Match in Matches)
    //        {

    //            if (Match.Value.StartsWith("#"))
    //            {
    //                //need to return a byte expression
    //                string[] MatchParts = Match.Value.Substring(1).Split(',').Select(MP => MP.Trim()).ToArray();
    //                if (MatchParts.All(MV => MV.IsInteger() || (MV.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase) && MV.Substring(2).IsHexString())))
    //                {
    //                    List<char> R = new List<char>();
    //                    foreach (string S in Match.Value.Substring(1).Split(','))
    //                    {
    //                        int V;
    //                        if (S.IsInteger())
    //                        {
    //                            V = S.ToInteger();
    //                        }
    //                        else if (S.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase) && S.Substring(2).IsHexString())
    //                        {
    //                            V = Match.Value.Substring(3).HexToInt();
    //                        }
    //                        else
    //                        {
    //                            throw new Exception("Cant parse part {0} of {" + Match.Value + "}. Value must be in the range of 0 to 255 resp 0x0 to 0xff.".Build(S));
    //                        }
    //                        if (!V.IsBetween(0, 255))
    //                        {
    //                            throw new Exception("Cant parse part {0} of {" + Match.Value + "}. Value must be in the range of 0 to 255 resp 0x0 to 0xff.".Build(S));
    //                        }
    //                        R.Add((char)V);
    //                    }
    //                    ExpressionPrepared = ExpressionPrepared.Replace("{" + Match.Value + "}", new string(R.ToArray()));
    //                }
    //                else
    //                {
    //                    IGenericExpression<byte> EB = null;
    //                    try
    //                    {
    //                        EB = Context.CompileGeneric<byte>(Match.Value.Substring(1));

    //                    }
    //                    catch (Exception Ex)
    //                    {

    //                        throw new Exception("{" + Match.Value + "} is not a valid byte expression.", Ex);
    //                    }
    //                    ByteExpressions.Add("{" + Match.Value + "}", EB);
    //                }
    //            }
    //            else
    //            {
    //                IGenericExpression<string> ES = null;

    //                try
    //                {
    //                    ES = Context.CompileGeneric<string>(Match.Value);

    //                }
    //                catch (Exception E)
    //                {

    //                    throw new Exception("{" + Match.Value + "} is not a valid string expression.", E);
    //                }
    //                StringExpressions.Add("{" + Match.Value + "}", ES);
    //            }
    //        }

    //        return new GenericComExpression() { ByteExpressions = ByteExpressions, StringExpressions = StringExpressions, Expression = ExpressionPrepared };
    //    }

    //    private class GenericComExpression
    //    {
    //        public Dictionary<string, IGenericExpression<byte>> ByteExpressions = new Dictionary<string, IGenericExpression<byte>>();
    //        public Dictionary<string, IGenericExpression<string>> StringExpressions = new Dictionary<string, IGenericExpression<string>>();

    //        public string Expression = null;

    //    }


    //    byte[] OldValues = null;
    //    protected override void UpdateOutputs(byte[] OutputValues)
    //    {
    //        if (Port != null)
    //        {
    //            lock (PortLocker)
    //            {
    //                byte[] Data = null;
    //                UpdateExpressionContextValues(OutputValues);
    //                if (UpdateStart != null)
    //                {


    //                    try
    //                    {
    //                        Data = Evaluate(UpdateStart);
    //                    }
    //                    catch (Exception E)
    //                    {
    //                        throw new Exception("Could not evaluate the UpdateStartExpression {0} for {1} {2}.".Build(OpenConnectionExpression, this.GetType().Name, Name), E);
    //                    }
    //                    SendData(Data);
    //                }


    //                if (UpdateOutput != null)
    //                {
    //                    for (int i = 1; i <= NumberOfOutputs; i++)
    //                    {
    //                        UpdateExpressionContextCurrent(OutputValues, i);
    //                        try
    //                        {
    //                            Data = Evaluate(UpdateOutput);
    //                        }
    //                        catch (Exception E)
    //                        {
    //                            throw new Exception("Could not evaluate the UpdateOutputExpression {0} for {1} {2}.".Build(UpdateOutputExpression, this.GetType().Name, Name), E);
    //                        }
    //                        SendData(Data);
    //                    }
    //                    UpdateExpressionContextCurrent(OutputValues, -1);
    //                }


    //                if (UpdateEnd != null)
    //                {
    //                    Data = null;

    //                    try
    //                    {
    //                        Data = Evaluate(UpdateEnd);
    //                    }
    //                    catch (Exception E)
    //                    {
    //                        throw new Exception("Could not evaluate the UpdateEndExpression {0} for {1} {2}.".Build(OpenConnectionExpression, this.GetType().Name, Name), E);
    //                    }
    //                    SendData(Data);
    //                }

    //            }
    //        }
    //        else
    //        {
    //            throw new Exception("Port {0} for {1} {2} is not initialized.".Build(ComPort, this.GetType().Name, Name));
    //        }
    //    }





    //    protected override void ConnectToController()
    //    {
    //        lock (PortLocker)
    //        {
    //            try
    //            {
    //                if (Port != null)
    //                {
    //                    DisconnectFromController();
    //                }

    //                OldValues = null;

    //                Port = new SerialPort(ComPort, Baudrate, Parity, DataBits, StopBits);
    //                Port.Open();
    //            }
    //            catch (Exception E)
    //            {
    //                string Msg = "A exception occured while opening comport {2} for {0} {1}.".Build(this.GetType().Name, Name, ComPort);
    //                Log.Exception(Msg, E);
    //                throw new Exception(Msg, E);
    //            }

    //            if (OpenConnection != null)
    //            {
    //                byte[] Data = null;
    //                UpdateExpressionContextValues(new byte[NumberOfOutputs]);
    //                try
    //                {
    //                    Data = Evaluate(OpenConnection);
    //                }
    //                catch (Exception E)
    //                {
    //                    Log.Exception("Could not evaluate the OpenConnectionExpression {0} for {1} {2}.".Build(OpenConnectionExpression, this.GetType().Name, Name), E);
    //                }
    //                SendData(Data);
    //            }
    //        }
    //    }

    //    protected override void DisconnectFromController()
    //    {
    //        lock (PortLocker)
    //        {
    //            if (Port != null)
    //            {
    //                byte[] Data = null;
    //                UpdateExpressionContextValues(new byte[NumberOfOutputs]);
    //                try
    //                {
    //                    Data = Evaluate(CloseConnection);
    //                }
    //                catch (Exception E)
    //                {
    //                    Log.Exception("Could not evaluate the CloseConnectionExpression {0} for {1} {2}.".Build(CloseConnectionExpression, this.GetType().Name, Name), E);
    //                }

    //                try
    //                {
    //                    SendData(Data);

    //                }
    //                catch (Exception E)
    //                {
    //                    Log.Exception(E.Message, E);
    //                }
    //                Port.Close();
    //                Port = null;
    //                OldValues = null;
    //            }

    //        }
    //    }

    //    private void SendData(byte[] Data)
    //    {
    //        lock (PortLocker)
    //        {
    //            try
    //            {
    //                Port.Write(Data, 0, Data.Length);
    //            }
    //            catch (Exception E)
    //            {
    //                throw new Exception("Could not send data to com port {0} for {1} {2}.".Build(ComPort, this.GetType().Name, Name), E);
    //            }
    //        }
    //    }

    //}
}
