using System;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using DirectOutput.General.Generic;

namespace DirectOutput.FX
{

    /// <summary>
    /// Collection of IEffect objects.
    /// Every object can only exist once in the list and every objects needs to have a unique name.
    /// </summary>
    public class EffectList : NamedItemList<IEffect>, IXmlSerializable
    {

        #region IXmlSerializable implementation
        /// <summary>
        /// Serializes the IEffect objects in this list to Xml.
        /// IEffect objects are serialized as the contained objects. The enclosing tags represent the object type
        /// WriteXml is part if the IXmlSerializable interface.
        /// </summary>
        public void WriteXml(XmlWriter writer)
        {

            XmlSerializerNamespaces Namespaces = new XmlSerializerNamespaces();
            Namespaces.Add(string.Empty, string.Empty);
            foreach (IEffect E in this)
            {
                XmlSerializer serializer = new XmlSerializer(E.GetType());
                serializer.Serialize(writer, E, Namespaces);
            }
        }


        /// <summary>
        /// Deserializes the IEffect objects in the XmlReader
        /// The IEffect objects are deserialized using the object name in the enclosing tags.
        /// ReadXml is part if the IXmlSerializable interface.
        /// </summary>
        public void ReadXml(XmlReader reader)
        {
            if (reader.IsEmptyElement)
            {
                reader.ReadStartElement();
                return;
            }

            General.TypeList Types = new General.TypeList(AppDomain.CurrentDomain.GetAssemblies().ToList().SelectMany(s => s.GetTypes()).Where(p => typeof(IEffect).IsAssignableFrom(p) && !p.IsAbstract));

            reader.Read();

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                Type T = Types[reader.LocalName];
                if (T != null)
                {
                    XmlSerializer serializer = new XmlSerializer(T);
                    IEffect E = (IEffect)serializer.Deserialize(reader);
                    if (!Contains(E.Name))
                    {
                        Add(E);
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
        public System.Xml.Schema.XmlSchema GetSchema() { return (null); }
        #endregion

        /// <summary>
        /// Calls Finish on all IEffect objects in the list. 
        /// </summary>
        public void Finish()
        {
            foreach (IEffect Effect in this)
            {
                Effect.Finish();
            }
        }


        /// <summary>
        /// Calls Init on all IEffect objects in the list.
        /// </summary>
        public void Init(Pinball Pinball)
        {
            foreach (IEffect Effect in this)
            {
                Effect.Init(Pinball);
            }
        }


        public EffectList()
        {
            
        }



    }
}
