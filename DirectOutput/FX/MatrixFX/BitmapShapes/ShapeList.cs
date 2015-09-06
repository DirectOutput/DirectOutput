using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.General.Generic;
using System.Xml.Serialization;
using System.Xml;

namespace DirectOutput.FX.MatrixFX.BitmapShapes
{
    public class ShapeList : NamedItemList<Shape>, IXmlSerializable
    {


        #region IXmlSerializable implementation
        /// <summary>
        /// Serializes the ShapeDefinition objects in this list to Xml.
        /// ShapeDefinition objects are serialized as the contained objects. The enclosing tags represent the object type
        /// WriteXml is part of the IXmlSerializable interface.
        /// </summary>
        public void WriteXml(XmlWriter writer)
        {

            XmlSerializerNamespaces Namespaces = new XmlSerializerNamespaces();
            Namespaces.Add(string.Empty, string.Empty);
            foreach (Shape T in this)
            {
                XmlSerializer serializer = new XmlSerializer(T.GetType());
                serializer.Serialize(writer, T, Namespaces);
            }
        }


        /// <summary>
        /// Deserializes the ShapeDefinition objects in the XmlReader
        /// The ShapeDefinition objects are deserialized using the object name in the enclosing tags.
        /// ReadXml is part of the IXmlSerializable interface.
        /// </summary>
        public void ReadXml(XmlReader reader)
        {
            if (reader.IsEmptyElement)
            {
                reader.ReadStartElement();
                return;
            }
            General.TypeList Types = new General.TypeList(AppDomain.CurrentDomain.GetAssemblies().ToList().SelectMany(s => s.GetTypes()).Where(p => typeof(Shape).IsAssignableFrom(p) && !p.IsAbstract));

            reader.Read();

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                Type T = Types[reader.LocalName];

                if (T != null)
                {
                    XmlSerializer serializer = new XmlSerializer(T);
                    Shape Shape = (Shape)serializer.Deserialize(reader);
                    if (!Contains(Shape.Name))
                    {
                        Add(Shape);
                    }
                }
                else
                {
                    Log.Warning("ShapeDefinition type {0} not found during deserialization of cabinet data.".Build(reader.LocalName));
                    reader.Skip();
                }


            }
            reader.ReadEndElement();
        }


        /// <summary>
        /// Method is required by the IXmlSerializable interface
        /// </summary>
        /// <returns>Returns always null</returns>
        public System.Xml.Schema.XmlSchema GetSchema() { return (null); }
        #endregion



    }
}
