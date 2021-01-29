using System;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using DirectOutput.General.Generic;

namespace DirectOutput.Cab.Out
{

    /// <summary>
    /// List of IOutputController objects 
    /// </summary>
    public class OutputControllerList : NamedItemList<IOutputController>, IXmlSerializable
    {

        #region IXmlSerializable implementation
        /// <summary>
        /// Serializes the IOutputController objects in this list to Xml.<br/>
        /// IOutputController objects are serialized as the contained objects. The enclosing tags represent the object type.<br/>
        /// WriteXml is part of the IXmlSerializable interface.
        /// </summary>
        public void WriteXml(XmlWriter writer)
        {

            XmlSerializerNamespaces Namespaces = new XmlSerializerNamespaces();
            Namespaces.Add(string.Empty, string.Empty);
            foreach (IOutputController OC in this)
            {
                XmlSerializer serializer = new XmlSerializer(OC.GetType());
                serializer.Serialize(writer, OC, Namespaces);
            }
        }


        /// <summary>
        /// Deserializes the IOutputController objects in the XmlReader.<br/>
        /// The IOutputController objects are deserialized using the object name in the enclosing tags.<br/>
        /// ReadXml is part of the IXmlSerializable interface.
        /// </summary>
        public void ReadXml(XmlReader reader)
        {
            if (reader.IsEmptyElement)
            {
                reader.ReadStartElement();
                return;
            }

            General.TypeList Types = new General.TypeList(AppDomain.CurrentDomain.GetAssemblies().ToList().SelectMany(s => s.GetTypes()).Where(p => typeof(IOutputController).IsAssignableFrom(p) && !p.IsAbstract));

            reader.Read();

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    Type T = Types[reader.LocalName];
                    if (T != null)
                    {
                        XmlSerializer serializer = new XmlSerializer(T);
                        IOutputController O = null;
                        try {
                            O = (IOutputController)serializer.Deserialize(reader);
                        } catch (Exception E) {
                            Log.Exception("DirectOutput framework has encountered a exception during initialization.", E);
                            throw new Exception("DirectOutput framework has encountered a exception during initialization.\n Inner exception: {0}".Build(E.Message), E);
                        }
                        if (!Contains(O.Name))
                        {
                            //Log.Write("OutputControlleRList.ReadXml...adding: " + O.Name);
                            Add(O);
                        }
                    }
                    else
                    {
                        Log.Warning("Output controller type {0} not found during deserialization of data.".Build(reader.LocalName));
                        reader.Skip();
                    }
                }
                else
                {
                    //Not a xml element. Probably a comment. Skip.
                    reader.Skip();
                }
            }

            reader.ReadEndElement();
        }


        /// <summary>
        /// Method is required by the IXmlSerializable interface.
        /// </summary>
        /// <returns>Returns always null</returns>
        public System.Xml.Schema.XmlSchema GetSchema() { return (null); }
        #endregion



        /// <summary>
        /// Initializes all IOutputController objects in the list.
        /// </summary>
        /// <param name="Cabinet">The Cabinet object which is using the list of IOutputController objects.</param>
        public void Init(Cabinet Cabinet)
        {
            Log.Debug("Initializing output controllers");
            foreach (IOutputController OC in this)
            {
                OC.Init(Cabinet);
            }
            Log.Debug("Output controllers initialized");
        }

        /// <summary>
        /// Finishes all IOutputController objects in the list.
        /// </summary>
        public void Finish()
        {
            Log.Debug("Finishing output controllers");
            foreach (IOutputController OC in this)
            {
                OC.Finish();
            }
            Log.Debug("Output controllers finished");
        }

        /// <summary>
        /// Updates all IOutputController objects in the list.
        /// </summary>
        public void Update()
        {
            foreach (IOutputController OC in this)
            {
                OC.Update();
            }
        }



    }
}
