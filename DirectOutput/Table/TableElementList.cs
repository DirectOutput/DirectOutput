using System;
using System.Collections.Generic;
using DirectOutput.General.Generic;


namespace DirectOutput.Table
{
    /// <summary>
    /// List of TableElement objects.
    /// </summary>
    public class TableElementList : ExtList<TableElement>
    {
        //Dictionary for fast lookups
        private Dictionary<TableElementTypeEnum, Dictionary<int, TableElement>> _TableElementsDictionary;


        /// <summary>
        /// Initializes the AssignedEffects for all TableElements in this list.
        /// </summary>
        public void InitAssignedEffects(Table Table)
        {
            foreach (TableElement TE in this)
            {
                TE.AssignedEffects.Init(Table);
            }
        }

        /// <summary>
        /// Finishes the AssignedEffects for all TableElements in this list.
        /// </summary>
        public void FinishAssignedEffects()
        {
            foreach (TableElement TE in this)
            {
                TE.AssignedEffects.Finish();
            }
        }

        #region Direct access to TableElementType Dictionaries

        /// <summary>
        /// Returns a dictionary for the specified TableElementType
        /// </summary>
        public Dictionary<int, TableElement> GetTableElementDictonaryForType(TableElementTypeEnum Type)
        {
            return _TableElementsDictionary[Type];
        }


        /// <summary>
        /// Returns a list of the TableElement objects with the specified type.<br/>
        /// \note This method does internaly create a new list of the specified table elements on every call. This is not very fast.
        /// </summary>
        public List<TableElement> GetTableElementListForType(TableElementTypeEnum Type)
        {
            return new List<TableElement> (_TableElementsDictionary[Type].Values);
        }

        /// <summary>
        ///         Returns a dictionary of Switch table elements
        /// </summary>
        public Dictionary<int, TableElement> Switch
        {
            get
            {
                return _TableElementsDictionary[TableElementTypeEnum.Switch];
            }
        }
        /// <summary>
        /// Returns a dictionary of Solenoid table elements
        /// </summary>
        public Dictionary<int, TableElement> Solenoid
        {
            get
            {
                return _TableElementsDictionary[TableElementTypeEnum.Solenoid];
            }
        }
        /// <summary>
        /// Returns a dictionary of Lamp table elements
        /// </summary>
        public Dictionary<int, TableElement> Lamp
        {
            get
            {
                return _TableElementsDictionary[TableElementTypeEnum.Lamp];
            }
        }
        /// <summary>
        /// Returns a dictionary of GIString table elements
        /// </summary>
        public Dictionary<int, TableElement> GIString
        {
            get
            {
                return _TableElementsDictionary[TableElementTypeEnum.GIString];
            }
        }

        /// <summary>
        /// Returns a dictionary of Mech table elements
        /// </summary>
        public Dictionary<int, TableElement> Mech
        {
            get
            {
                return _TableElementsDictionary[TableElementTypeEnum.Mech];
            }
        }
        #endregion

        #region Indexer
        /// <summary>
        /// Indexer for for List
        /// </summary>
        /// <param name="TableElementType">TableElementType of the TableElement</param>
        /// <param name="Number">Number of TheTableElement</param>
        /// <returns>TableElement with specified TableElementType and Number</returns>
        public TableElement this[TableElementTypeEnum TableElementType, int Number]
        {
            get
            {
                return _TableElementsDictionary[TableElementType][Number];
            }
        }


        #endregion

        #region UpdateState()


        /// <summary>
        /// Method to update the state and/or add a entry to the list
        /// </summary>
        /// <param name="TableElementType">Type of entry to update</param>
        /// <param name="Number">Number of entry to update</param>
        /// <param name="State">State of entry to update</param>
        public void UpdateState(TableElementTypeEnum TableElementType, int Number, int State)
        {
            if (Contains(TableElementType, Number))
            {
                _TableElementsDictionary[TableElementType][Number].Value = State;

            }
            else
            {
                Add(TableElementType, Number, State);
            }

        }

        #endregion

        #region Add

        /// <summary>
        /// Adds a TableElement to the list.
        /// </summary>
        /// <param name="TableElement">The table element to add.</param>
        /// <exception cref="System.Exception">
        /// Cant add null to the list of table elements
        /// or
        /// The TableElement {Type} {Number} cant be added to the list. Another entry with the same type and number does already exist.</exception>
        public new void Add(TableElement TableElement)
        {
            if (TableElement==null)
            {
                throw new Exception("Cant add null to the list of table elements.");
            }
            if (Contains(TableElement))
            {
                throw new Exception("The TableElement {0} {1} cant be added to the list. Another entry with the same type and number does already exist.".Build(TableElement.TableElementType, TableElement.Number));
            }
            _TableElementsDictionary[TableElement.TableElementType].Add(TableElement.Number, TableElement);
            base.Add(TableElement);
        }


        /// <summary>
        /// Method for adding a entry to the list.
        /// </summary>
        /// <param name="TableElementType">Type of entry to add.</param>
        /// <param name="Number">Number of entry to add.</param>
        /// <param name="State">State of entry to add.</param>
        /// <exception cref="System.Exception">
        /// Cant add null to the list of table elements
        /// or
        /// The TableElement {Type} {Number} cant be added to the list. Another entry with the same type and number does already exist.
        /// </exception>
        public void Add(TableElementTypeEnum TableElementType, int Number, int State)
        {
            Add(new TableElement(TableElementType, Number, State));
        }





        #endregion
        #region Contains


        /// <summary>
        /// Checks if a specified TableElement is contained in the list.
        /// </summary>
        /// <param name="TableElement">TableElement to check.</param>
        /// <returns>true/false</returns>
        public new bool Contains(TableElement TableElement)
        {
            return _TableElementsDictionary[TableElement.TableElementType].ContainsKey(TableElement.Number) || base.Contains(TableElement);
        }

        /// <summary>
        /// Checks if a specified TableElement is contained in the list.
        /// </summary>
        /// <param name="TableElementType">Type of the TableElement to check.</param>
        /// <param name="Number">Number of TableElement to check.</param>
        /// <returns>true/false</returns>
        public bool Contains(TableElementTypeEnum TableElementType, int Number)
        {
            return _TableElementsDictionary[TableElementType].ContainsKey(Number);
        }




        #endregion
        #region Remove

        /// <summary>
        /// Removes the specified TableElement from the List.
        /// </summary>
        /// <param name="TableElement">TableElement to remove.</param>
        /// <returns>true if TableElement has been removed, otherwise false.</returns>
        public new bool Remove(TableElement TableElement)
        {

            _TableElementsDictionary[TableElement.TableElementType].Remove(TableElement.Number);
            return base.Remove(TableElement);

        }

        /// <summary>
        /// Removes the TableElement with the specified TableElementType and Number from the list.
        /// </summary>
        /// <param name="TableElementType">TableElementType of the TableElement to remove.</param>
        /// <param name="Number">Number of the TableElement to remove.</param>
        public bool Remove(TableElementTypeEnum TableElementType, int Number)
        {
            if (!Contains(TableElementType, Number)) return false;
            Remove(_TableElementsDictionary[TableElementType][Number]);
            _TableElementsDictionary[TableElementType].Remove(Number);

            return true;

        }
        #endregion


        #region Events & Event handling
        void TableElementList_AfterSet(object sender, SetEventArgs<TableElement> e)
        {
            _TableElementsDictionary[e.OldItem.TableElementType].Remove(e.OldItem.Number);
            e.OldItem.ValueChanged -= new EventHandler<TableElementValueChangedEventArgs>(Item_ValueChanged);
            _TableElementsDictionary[e.NewItem.TableElementType].Add(e.NewItem.Number, e.NewItem);
            e.NewItem.ValueChanged += new EventHandler<TableElementValueChangedEventArgs>(Item_ValueChanged);
        }

        void TableElementList_BeforeSet(object sender, SetEventArgs<TableElement> e)
        {
            if (!Contains(e.NewItem.TableElementType, e.NewItem.Number) || (e.NewItem.TableElementType == e.OldItem.TableElementType && e.NewItem.Number == e.OldItem.Number))
            {

            }
            else
            {
                throw new Exception("Another TableElement with type {0} and number {1} does already exist in the list.".Build(e.NewItem.TableElementType, e.NewItem.Number));
            }
        }


        void TableElementList_AfterRemove(object sender, RemoveEventArgs<TableElement> e)
        {
            e.Item.ValueChanged -= new EventHandler<TableElementValueChangedEventArgs>(Item_ValueChanged);
        }

        void TableElementList_AfterInsert(object sender, InsertEventArgs<TableElement> e)
        {
            e.Item.ValueChanged += new EventHandler<TableElementValueChangedEventArgs>(Item_ValueChanged);
        }

        void Item_ValueChanged(object sender, TableElementValueChangedEventArgs e)
        {
            OnTableElementValueChanged(e);
        }

        #region "ableElement Value Changed Event


        private void OnTableElementValueChanged(TableElementValueChangedEventArgs e)
        {
            if (TableElementValueChanged != null)
            {
                TableElementValueChanged(this, e);
            }
        }



        /// <summary>
        /// Is fired on changes of the value of any TableElement in this collection
        /// </summary>
        public event TableElementValueChangedEventHandler TableElementValueChanged;

        /// <summary>
        /// EventHandler for TableElementValueChanged events.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="TableElementValueChangedEventArgs"/> instance containing the event data.</param>
        public delegate void TableElementValueChangedEventHandler(object sender, TableElementValueChangedEventArgs e);


        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TableElementList"/> class.
        /// </summary>
        public TableElementList()
        {
            //Init internal dictionary
            _TableElementsDictionary = new Dictionary<TableElementTypeEnum, Dictionary<int, TableElement>>();
            foreach (TableElementTypeEnum T in Enum.GetValues(typeof(TableElementTypeEnum)))
            {
                _TableElementsDictionary.Add(T, new Dictionary<int, TableElement>());
            }
            this.AfterInsert += new EventHandler<InsertEventArgs<TableElement>>(TableElementList_AfterInsert);
            this.AfterRemove += new EventHandler<RemoveEventArgs<TableElement>>(TableElementList_AfterRemove);
            this.BeforeSet += new EventHandler<SetEventArgs<TableElement>>(TableElementList_BeforeSet);
            this.AfterSet += new EventHandler<SetEventArgs<TableElement>>(TableElementList_AfterSet);
        }





        #endregion

        /// <summary>
        /// Finalizes an instance of the <see cref="TableElementList"/> class.
        /// </summary>
        ~TableElementList()
        {
            foreach (TableElement TE in this)
            {
                TE.ValueChanged -= new EventHandler<TableElementValueChangedEventArgs>(Item_ValueChanged);

            }

        }
    }
}
