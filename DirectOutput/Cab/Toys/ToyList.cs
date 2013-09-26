using System;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using DirectOutput.General.Generic;

namespace DirectOutput.Cab.Toys
{

    /// <summary>
    /// List of IToy objects 
    /// </summary>
    public class ToyList : NamedItemList<IToy>, IXmlSerializable
    {

        #region IXmlSerializable implementation
        /// <summary>
        /// Serializes the IToy objects in this list to Xml.
        /// IEffect objects are serialized as the contained objects. The enclosing tags represent the object type
        /// WriteXml is part of the IXmlSerializable interface.
        /// </summary>
        public void WriteXml(XmlWriter writer)
        {

            XmlSerializerNamespaces Namespaces = new XmlSerializerNamespaces();
            Namespaces.Add(string.Empty, string.Empty);
            foreach (IToy T in this)
            {
                XmlSerializer serializer = new XmlSerializer(T.GetType());
                serializer.Serialize(writer, T, Namespaces);
            }
        }


        /// <summary>
        /// Deserializes the IToy objects in the XmlReader
        /// The IToy objects are deserialized using the object name in the enclosing tags.
        /// ReadXml is part of the IXmlSerializable interface.
        /// </summary>
        public void ReadXml(XmlReader reader)
        {
            if (reader.IsEmptyElement)
            {
                reader.ReadStartElement();
                return;
            }
            General.TypeList Types = new General.TypeList(AppDomain.CurrentDomain.GetAssemblies().ToList().SelectMany(s => s.GetTypes()).Where(p => typeof(IToy).IsAssignableFrom(p) && !p.IsAbstract));

            reader.Read();

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                Type T = Types[reader.LocalName];

                if (T != null)
                {
                    XmlSerializer serializer = new XmlSerializer(T);
                    IToy Toy = (IToy)serializer.Deserialize(reader);
                    if (!Contains(Toy.Name))
                    {
                        Add(Toy);
                    }
                }
                else
                {
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

        /// <summary>
        /// Initializes all IToy objects in the list 
        /// </summary>
        /// <param name="Cabinet">Cabinet object to which the IToy objects in the list belong.</param>
        public void Init(Cabinet Cabinet)
        {
            foreach (IToy T in this)
            {
                T.Init(Cabinet);
            }

        }

        /// <summary>
        /// Resets all toys in the list.
        /// </summary>
        public void Reset()
        {
            foreach (IToy T in this)
            {
                T.Reset();
            }
        }



        /// <summary>
        /// Finishes all toys in the list.
        /// </summary>
        public void Finish()
        {
            foreach (IToy T in this)
            {
                T.Finish();
            }
        }


        /// <summary>
        /// Updates the outputs of all toys implementing the IToyUpdatable interface.
        /// </summary>
        public void UpdateOutputs()
        {
            foreach (IToy T in this)
            {
                if (T is IToyUpdatable)
                {
                   ((IToyUpdatable)T).UpdateOutputs();
                }
            }
        }
    }
}
