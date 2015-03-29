using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using FuzzyStrings;
namespace PinballX.Table2RomMapping
{
    public class TableNameMappings : List<Mapping>
    {


        public Mapping GetBestMatchingMapping(string TableName)
        {
            foreach (Mapping M in this)
            {
                if (M.TableName.Equals(TableName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return M;
                }
            }


            Mapping BestMatch = null;
            double BestScore=-1;
            foreach (Mapping M in this)
            {
                double Score = FuzzyStrings.FuzzyText.DiceCoefficient(TableName, M.TableName);

                if (Score > BestScore)
                {
                    BestMatch = M;
                    BestScore = Score;
                }
            }
            if (BestScore > 0.3)
            {
                return BestMatch;
            }
            else
            {
                return null;
            }
        }




        public static TableNameMappings LoadTableMappings(string Filename)
        {
            if (!string.IsNullOrEmpty(Filename))
            {
                if (new FileInfo(Filename).Exists)
                {
                    string Data = null;
                    try
                    {
                        StreamReader streamReader = new StreamReader(Filename);
                        Data = streamReader.ReadToEnd();
                        streamReader.Close();
                        streamReader.Dispose();

                    }
                    catch (Exception E)
                    {

                        throw new Exception("Could not read data from TableMapping file: " + Filename, E);
                    }
                    byte[] xmlBytes = Encoding.Default.GetBytes(Data);
                    using (MemoryStream ms = new MemoryStream(xmlBytes))
                    {
                        try
                        {
                            return (TableNameMappings)new XmlSerializer(typeof(TableNameMappings)).Deserialize(ms);
                        }
                        catch (Exception E)
                        {

                            Exception Ex = new Exception("Could not deserialize the TableNameMappings config from XML data: " + E.Message, E);
                            Ex.Data.Add("XML Data", Data);
                            throw Ex;
                        }
                    }

                }
                else
                {
                    throw new FileNotFoundException("File does not exist: " + Filename, Filename);
                }
            }
            else
            {
                return new TableNameMappings();
            }
        }
    }
}
