using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Serialization;
using DirectOutput.General.Generic;
using System.Xml;

namespace DirectOutput.General
{
    /// <summary>
    /// Represents a curve which can be used to map values (e.g. adjust a brighnetss value of a led to the brightness perception of the human eye).
    /// </summary>
    public class Curve : NamedItemBase, IXmlSerializable
    {
        /// <summary>
        /// Enumeration of predefined curves.
        /// </summary>
        public enum CurveTypeEnum
        {
            /// <summary>
            /// A linear curve, where each element will map to a value which is equal to the element index (0=0, 1=1 .... 254=254,255=255).
            /// </summary>
            Linear,
            /// <summary>
            /// A linear curve, which mapps to 0 to 255 value range into a new range of 0 to 192.
            /// </summary>
            Linear0To192,
            /// <summary>
            /// A linear curve, which mapps to 0 to 255 value range into a new range of 0 to 128.
            /// </summary>
            Linear0To128,
            /// <summary>
            /// A linear curve, which mapps to 0 to 255 value range into a new range of 0 to 64.
            /// </summary>
            Linear0To64,
            /// <summary>
            /// A linear curve, which mapps to 0 to 255 value range into a new range of 0 to 32.
            /// </summary>
            Linear0To32,
            /// <summary>
            /// A linear curve, which mapps to 0 to 255 value range into a new range of 0 to 16.
            /// </summary>
            Linear0To16,
            /// <summary>
            /// This is a inverted linera curve where 255=0, 254=1 and so on until 1=254 and 0=255.
            /// </summary>
            InvertedLinear,
            /// <summary>
            /// A fading curve for leds defined by SwissLizard. This curve is not fully correct when it comes to theoretically needed mapping values, but it is some kind of compromise between possible value range and the desired values.
            /// </summary>
            SwissLizardsLedCurve
        }

        private byte[] _Data;

        /// <summary>
        /// Gets or sets the curve data array.
        /// The curve array must have 256 elements.
        /// </summary>
        /// <value>
        /// The curve array (256 elements).
        /// </value>
        /// <exception cref="System.Exception">The curve array must have 256 elements, but a array with {0} elements has been supplied.</exception>
        public byte[] Data
        {
            get { return _Data; }
            set
            {
                if (value.Length != 256)
                {
                    throw new Exception("The curve array must have 256 elements, but a array with {0} elements has been supplied.".Build(value.Length));
                }
                _Data = value; }
        }


        /// <summary>
        /// Returns the value from the specified curve position.
        /// </summary>
        /// <param name="CurvePosition">The curve position.</param>
        /// <returns>Value from the specified position of the curve.</returns>
        public byte MapValue(byte CurvePosition)
        {
            return _Data[CurvePosition];
        }


        /// <summary>
        /// Returns the value from the specified curve position.
        /// </summary>
        /// <param name="CurvePosition">The curve position.</param>
        /// <returns>Value from the specified position of the curve.</returns>
        public byte MapValue(int CurvePosition)
        {
            try
            {
                return _Data[CurvePosition];
            }
            catch 
            {
                throw new ArgumentException("Positon {0} does not exist for the curve.".Build(CurvePosition),"CurvePosition");
            }
        }

        /// <summary>
        /// Sets the the fading curve to one of the predefined curves from the FadingCurveTypeEnum.
        /// </summary>
        /// <param name="CurveType">Type of the fading curve.</param>
        public void SetCurve(CurveTypeEnum CurveType)
        {
            Data = BuildCurve(CurveType);
        }

        private byte[] BuildCurve(CurveTypeEnum CurveType)
        {
            byte[] C = new byte[256];

            switch (CurveType)
            {
                case CurveTypeEnum.SwissLizardsLedCurve:
                    C = new byte[256] { 0, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11, 12, 12, 13, 13, 14, 14, 15, 15, 16, 16, 17, 17, 18, 18, 19, 19, 20, 20, 21, 21, 22, 22, 23, 23, 24, 24, 25, 25, 26, 27, 28, 29, 30, 30, 31, 32, 33, 34, 35, 36, 36, 37, 38, 39, 40, 41, 42, 43, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 102, 103, 104, 105, 106, 107, 108, 109, 110, 112, 113, 114, 115, 116, 117, 119, 120, 121, 122, 123, 124, 126, 127, 128, 129, 130, 132, 133, 134, 135, 136, 138, 139, 140, 141, 142, 144, 145, 146, 147, 149, 150, 151, 152, 154, 155, 156, 158, 159, 160, 161, 162, 164, 165, 167, 168, 169, 171, 172, 173, 174, 176, 177, 178, 180, 181, 182, 184, 185, 187, 188, 189, 191, 192, 193, 195, 196, 197, 199, 200, 202, 203, 204, 206, 207, 208, 210, 211, 213, 214, 216, 217, 218, 220, 221, 223, 224, 226, 227, 228, 230, 231, 233, 234, 236, 237, 239, 240, 242, 243, 245, 246, 248, 249, 251, 252, 254, 255 };
                    break;
                case CurveTypeEnum.InvertedLinear:
                    for (int i = 0; i <= 255; i++)
                    {
                        C[i] = (byte)(255-i);
                    }
                    break;
                case CurveTypeEnum.Linear0To192:
                    for (int i = 0; i <= 255; i++)
                    {
                        C[i] = (byte)((double)192/255*i).Limit(0,255);
                    }
                    break;
                case CurveTypeEnum.Linear0To128:
                    for (int i = 0; i <= 255; i++)
                    {
                        C[i] = (byte)((double)128 / 255 * i).Limit(0, 255);
                    }
                    break;
                case CurveTypeEnum.Linear0To64:
                    for (int i = 0; i <= 255; i++)
                    {
                        C[i] = (byte)((double)64 / 255 * i).Limit(0, 255);
                    }
                    break;
                case CurveTypeEnum.Linear0To32:
                    for (int i = 0; i <= 255; i++)
                    {
                        C[i] = (byte)((double)32 / 255 * i).Limit(0, 255);
                    }
                    break;
                case CurveTypeEnum.Linear0To16:
                    for (int i = 0; i <= 255; i++)
                    {
                        C[i] = (byte)((double)16 / 255 * i).Limit(0, 255);
                    }
                    break;
                case CurveTypeEnum.Linear:
                default:
                    for (int i = 0; i <= 255; i++)
                    {
                        C[i] = (byte)i;
                    }
                    break;
            }
            return C;
        }

        private CurveTypeEnum? GetCurveTypeEnum()
        {
            foreach (CurveTypeEnum FT in Enum.GetValues(typeof(CurveTypeEnum)))
            {
                byte[] C = BuildCurve(FT);
                if (C.CompareContents(Data))
                {
                    return FT;
                }
            }
            return null;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Curve"/> class.
        /// </summary>
        public Curve()
        {
            Data = BuildCurve(CurveTypeEnum.Linear);
        }



        /// <summary>
        /// Initializes a new instance of the <see cref="Curve"/> class.
        /// </summary>
        /// <param name="CurveType">Type of the curve.</param>
        public Curve(CurveTypeEnum CurveType)
        {
            Data = BuildCurve(CurveType);
        }




        #region IXmlSerializable Member

        /// <summary>
        /// Part of the IXmlSerializable interface. 
        /// Must always return null.
        /// </summary>
        /// <returns>
        /// Return always null.
        /// </returns>
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Set the data of the object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader" />-stream containing the data for ther object.</param>
        public void ReadXml(System.Xml.XmlReader reader)
        {
            if (reader.LocalName == GetType().Name)
            {
                while (reader.Read())
                {
                    switch (reader.LocalName)
                    {
                        case "Name":

                            Name = reader.ReadString();
                            break;
                        case "Curve":

                            string XmlData = reader.ReadString();

                            CurveTypeEnum CurveType = CurveTypeEnum.Linear;

                            if (Enum.TryParse(XmlData, true, out CurveType))
                            {
                                Data = BuildCurve(CurveType);
                            }
                            else
                            {

                                string[] V = XmlData.Split(new char[] { ',' });
                                if (V.Length == 256)
                                {
                                    int Cnt = V.Count(Value => !Value.IsUInt() || Value.ToUInt() > 255);
                                    if (Cnt == 0)
                                    {
                                        for (int i = 0; i < 255; i++)
                                        {
                                            Data[i] = (byte)V[i].ToInteger();
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("{0} value(s) in the curve data for fading curve {1} are not numeric values between 0-255. Supplied data was {2}.".Build(Cnt, Name,XmlData));
                                    }
                                }
                                else
                                {
                                    if (V.Length > 1)
                                    {
                                        throw new Exception("Cant parse data for curve {0}. 256 values are required, but {1} values have been found. Supplied data was {2}.".Build(Name, V.Length,XmlData));
                                    }
                                    else
                                    {
                                        throw new Exception("Cant parse data for curve {0}. One of the values from the CurveTypeEnum or 256 values between 0-255 are expected. Supplied data was: {1}.".Build(Name,XmlData));
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }

                }
            }
        }

        /// <summary>
        /// Converts the object to its xml representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter" />-stream, to which the object is serialized.</param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            CurveTypeEnum? CT = GetCurveTypeEnum();

            //writer.WriteStartElement(GetType().ToString());
            writer.WriteStartElement("Name");
            writer.WriteString(Name);
            writer.WriteEndElement();

            writer.WriteStartElement("Curve");
            if (CT.HasValue)
            {
                writer.WriteString(CT.ToString());
            }
            else
            {
                StringBuilder S = new StringBuilder();
                for (int i = 0; i < 256; i++)
                {
                    S.AppendFormat("{0}, ", Data[i]);
                }
                writer.WriteString(S.ToString().Substring(0, S.Length - 2));

            }
            writer.WriteEndElement();
            // writer.WriteEndElement();
        }

        #endregion
    }
}
