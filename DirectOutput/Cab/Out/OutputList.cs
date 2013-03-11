using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Xml.Serialization;
using System.Xml;
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
        /// Serializes the IOutput objects in this list to Xml.
        /// IOutput objects are serialized as the contained objects. The enclosing tags represent the object type
        /// WriteXml is part if the IXmlSerializable interface.
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
        /// Deserializes the IOutput objects in the XmlReader
        /// The IOutput objects are deserialized using the object name in the enclosing tags.
        /// ReadXml is part if the IXmlSerializable interface.
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

            while (Types.Contains(reader.LocalName))
            {
                Type T = Types[reader.LocalName];
                if (T != null)
                {
                    XmlSerializer serializer = new XmlSerializer(T);
                    Add((IOutput)serializer.Deserialize(reader));
                }
            }

            reader.ReadEndElement();
        }


        /// <summary>
        /// Method is required by the IXmlSerializable interface
        /// </summary>
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

        public delegate void OutputValueChangedEventHandler(object sender, OutputEventArgs e);



        #endregion
        #endregion

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
