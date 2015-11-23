using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.General;
using System.IO;
using System.Xml.Serialization;

namespace DirectOutput.FX.MatrixFX.BitmapShapes
{
    public class ShapeDefinitions
    {
        private FilePattern _BitmapFilePattern;

        /// <summary>
        /// Gets or sets the file pattern which is used to load the bitmap file for the effect.
        /// </summary>
        /// <value>
        /// The bitmap file pattern which is used to load the bitmap file for the effect.
        /// </value>
        [XmlIgnore]
        public FilePattern BitmapFilePattern
        {
            get { return _BitmapFilePattern; }
            set { _BitmapFilePattern = value; }
        }

        private ShapeList _Shapes=new ShapeList();

        public ShapeList Shapes
        {
            get { return _Shapes; }
            set { _Shapes = value; }
        }


        #region Serialization

        /// <summary>
        /// Returns a serialized XML representation of the ShapeDefinitions ShapeDefinitionsuration.
        /// </summary>
        /// <returns>XMLString</returns>
        public string GetShapeDefinitionsXml()
        {
            string Xml = "";
            using (MemoryStream ms = new MemoryStream())
            {
                new XmlSerializer(typeof(ShapeDefinitions)).Serialize(ms, this);
                ms.Position = 0;
                using (StreamReader sr = new StreamReader(ms, Encoding.Default))
                {
                    Xml = sr.ReadToEnd();
                    sr.Dispose();
                }
            }

            return Xml;
        }


        /// <summary>
        /// Serializes the ShapeDefinitions ShapeDefinitionsuration to a XML file.
        /// </summary>
        /// <param name="FileName">Name of the XML file.</param>
        public void SaveShapeDefinitionsXmlFile(string FileName)
        {
            GetShapeDefinitionsXml().WriteToFile(FileName);
        }


        /// <summary>
        /// Instanciates a ShapeDefinitions object from a ShapeDefinitions ShapeDefinitionsuration in a XML file.
        /// </summary>
        /// <param name="FileName">Name of the XML file.</param>
        /// <returns>ShapeDefinitions object</returns>
        public static ShapeDefinitions GetShapeDefinitionsFromShapeDefinitionsXmlFile(string FileName)
        {
            string Xml;
            try
            {
                Xml = General.FileReader.ReadFileToString(FileName);
            }
            catch (Exception E)
            {
                Log.Exception("Could not load ShapeDefinitions from {0}.".Build(FileName), E);
                throw new Exception("Could not read ShapeDefinitions file {0}.".Build(FileName), E);
            }

            return GetShapeDefinitionsFromShapeDefinitionsXml(Xml);
        }


        /// <summary>
        /// Tests a ShapeDefinitions ShapeDefinitions in a XML file.
        /// </summary>
        /// <param name="FileName">Name of the file.</param>
        /// <returns>true is the file contains a valid ShapeDefinitions, otherwise false.</returns>
        public static bool TestShapeDefinitionsShapeDefinitionsXmlFile(string FileName)
        {
            ShapeDefinitions C = null;
            try
            {
                C = GetShapeDefinitionsFromShapeDefinitionsXmlFile(FileName);
            }
            catch
            {
                return false;
            }
            return C != null;

        }

        /// <summary>
        /// Instanciates a ShapeDefinitions object from a ShapeDefinitions ShapeDefinitionsuration in a XML file.
        /// </summary>
        /// <param name="ShapeDefinitionsShapeDefinitionsFile">FileInfo object for the ShapeDefinitions file.</param>
        /// <returns>ShapeDefinitions object</returns>
        public static ShapeDefinitions GetShapeDefinitionsFromShapeDefinitionsXmlFile(FileInfo ShapeDefinitionsShapeDefinitionsFile)
        {
            return GetShapeDefinitionsFromShapeDefinitionsXmlFile(ShapeDefinitionsShapeDefinitionsFile.FullName);
        }

        /// <summary>
        /// Instanciates a ShapeDefinitions object from a ShapeDefinitions ShapeDefinitionsuration in a XML string.
        /// </summary>
        /// <param name="ShapeDefinitionsXml">XML string</param>
        /// <returns>ShapeDefinitions object</returns>
        public static ShapeDefinitions GetShapeDefinitionsFromShapeDefinitionsXml(string ShapeDefinitionsXml)
        {
            byte[] xmlBytes = Encoding.Default.GetBytes(ShapeDefinitionsXml);
            using (MemoryStream ms = new MemoryStream(xmlBytes))
            {
                try
                {
                    return (ShapeDefinitions)new XmlSerializer(typeof(ShapeDefinitions)).Deserialize(ms);
                }
                catch (Exception E)
                {

                    Exception Ex = new Exception("Could not deserialize the ShapeDefinitions from XML data.", E);
                    Ex.Data.Add("XML Data", ShapeDefinitionsXml);
                    Log.Exception("Could not load ShapeDefinitions from XML data.", Ex);
                    throw Ex;
                }
            }
        }
        #endregion




    }
}
