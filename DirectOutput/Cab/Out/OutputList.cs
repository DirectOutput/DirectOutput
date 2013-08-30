using System;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using DirectOutput.General.Generic;

namespace DirectOutput.Cab.Out
{

    /// <summary>
    /// List of IOutput objects 
    /// </summary>
    public class OutputList : NamedItemList<IOutput>, IXmlSerializable
    {

        #region IXmlSerializable implementation
        /// <summary>
        /// Serializes the IOutput objects in this list to Xml.<br/>
        /// IOutput objects are serialized as the contained objects. The enclosing tags represent the object type.<br/>
        /// WriteXml is part of the IXmlSerializable interface.
        /// </summary>
        public void WriteXml(XmlWriter writer)
        {

            XmlSerializerNamespaces Namespaces = new XmlSerializerNamespaces();
            Namespaces.Add(string.Empty, string.Empty);
            foreach (IOutput E in this)
            {
                XmlSerializer serializer = new XmlSerializer(E.GetType());
                serializer.Serialize(writer, E, Namespaces);
            }
        }


        /// <summary>
        /// Deserializes the IOutput objects in the XmlReader.<br/>
        /// The IOutput objects are deserialized using the object name in the enclosing tags.<br/>
        /// ReadXml is part of the IXmlSerializable interface.
        /// </summary>
        public void ReadXml(XmlReader reader)
        {
            if (reader.IsEmptyElement)
            {
                reader.ReadStartElement();
                return;
            }
            General.TypeList Types = new General.TypeList(AppDomain.CurrentDomain.GetAssemblies().ToList().SelectMany(s => s.GetTypes()).Where(p => typeof(IOutput).IsAssignableFrom(p) && !p.IsAbstract));

            reader.Read();

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                Type T = Types[reader.LocalName];
                if (T != null)
                {
                    XmlSerializer serializer = new XmlSerializer(T);
                    IOutput O = (IOutput)serializer.Deserialize(reader);
                    if (!Contains(O.Name))
                    {
                        Add(O);
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
        /// Method is required by the IXmlSerializable interface.
        /// </summary>
        /// <returns>
        /// Does allway return null.
        /// </returns>
        public System.Xml.Schema.XmlSchema GetSchema() { return (null); }
        #endregion


        #region Events

        #region Event handling

        void OutputList_BeforeClear(object sender, EventArgs e)
        {
            foreach (IOutput O in this)
            {
                O.ValueChanged -= new Output.ValueChangedEventHandler(Item_ValueChanged);
            }
        }

        void OutputList_AfterRemove(object sender, RemoveEventArgs<IOutput> e)
        {
            e.Item.ValueChanged -= new Output.ValueChangedEventHandler(Item_ValueChanged);
        }

        void OutputList_AfterInsert(object sender, InsertEventArgs<IOutput> e)
        {
            e.Item.ValueChanged += new Output.ValueChangedEventHandler(Item_ValueChanged);
        }

        void OutputList_AfterSet(object sender, SetEventArgs<IOutput> e)
        {
            e.OldItem.ValueChanged -= new Output.ValueChangedEventHandler(Item_ValueChanged);
            e.NewItem.ValueChanged += new Output.ValueChangedEventHandler(Item_ValueChanged);
        }

        void Item_ValueChanged(object sender, OutputEventArgs e)
        {
            OnOutputValueChanged(e.Output);
        }

        #endregion

        #region ValueChanged Event


        /// <summary>
        /// Triggers the OutputValueChanged event when called.
        /// </summary>
        /// <param name="Output">The output which triggers the event.</param>
        protected void OnOutputValueChanged(IOutput Output)
        {
            if (OutputValueChanged != null)
            {
                OutputValueChanged(this, new OutputEventArgs(Output));
            }
        }

        /// <summary>
        /// Event fires if the value of any IOutput in the list has changed.
        /// </summary>
        public event OutputValueChangedEventHandler OutputValueChanged;

        /// <summary>
        /// Handler for the OutputValueChangedEvent.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The <see cref="OutputEventArgs"/> instance containing the event data.</param>
        public delegate void OutputValueChangedEventHandler(object sender, OutputEventArgs e);



        #endregion
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="OutputList"/> class.
        /// </summary>
        public OutputList()
        {
            this.AfterInsert += new EventHandler<InsertEventArgs<IOutput>>(OutputList_AfterInsert);
            this.AfterRemove += new EventHandler<RemoveEventArgs<IOutput>>(OutputList_AfterRemove);
            this.AfterSet += new EventHandler<SetEventArgs<IOutput>>(OutputList_AfterSet);
            this.BeforeClear += new EventHandler<EventArgs>(OutputList_BeforeClear);
        }



        ~OutputList()
        {
            foreach (IOutput O in this)
            {
                O.ValueChanged -= new Output.ValueChangedEventHandler(Item_ValueChanged);
            }
        }





    }
}
