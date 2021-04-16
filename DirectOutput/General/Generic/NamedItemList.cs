using System;
using System.Collections.Generic;

namespace DirectOutput.General.Generic
{
    /// <summary>
    /// A list of uniquely named items which can be referenced by their name.
    /// </summary>
    /// <typeparam name="T">Type of the items contained in the list (must implement INamedItem).</typeparam>
    public class NamedItemList<T> : ExtList<T> where T : INamedItem
    {
        private Dictionary<string, T> _NameDict = new Dictionary<string, T>();
        
        /// <summary>
        /// Indexer returning the Item with the specified name.
        /// </summary>
        /// <param name="Name">Name of the item to retrieve.</param>
        public T this[string Name]
        {
            get { return _NameDict[Name]; }
        }


        /// <summary>
        /// Checks if a INamedItem object with the specified name exists in the list.
        /// </summary>
        /// <param name="Name">Name of the item to check.</param>
        /// <returns>true or false.</returns>
        public bool Contains(string Name)
        {
            return _NameDict.ContainsKey(Name);
        }
        

        /// <summary>
        /// Sorts the NamedItemList by the Name of the items.
        /// </summary>
        public new void Sort()
        {
            Sort((T a, T b) => (a.Name.CompareTo(b.Name)));
        }


        #region Event handling

        void NamedItemList_BeforeClear(object sender, EventArgs e)
        {
            foreach (T Item in this) {
                Item.BeforeNameChanged -= new EventHandler<NameChangeEventArgs>(Item_BeforeNameChange);
                Item.AfterNameChanged -= new EventHandler<NameChangeEventArgs>(Item_AfterNameChanged);
            }
        }

        void NamedItemList_AfterClear(object sender, EventArgs e)
        {
            _NameDict.Clear();
        }

        void Item_AfterNameChanged(object sender, NameChangeEventArgs e)
        {
    
             _NameDict.Remove(e.OldName);

            _NameDict.Add(e.NewName, (T)sender);

        }

        void Item_BeforeNameChange(object sender, NameChangeEventArgs e)
        {
            if (e.NewName != e.OldName)
            {
                if (_NameDict.ContainsKey(e.NewName))
                {
                    throw new ArgumentException("Cant the the name of the INamedItem from {0} to {1}, because the new name does already exist in a colelction to which this object is assigned.".Build(e.OldName, e.NewName));
                }
            }
        }


        void NamedItemList_AfterRemove(object sender, RemoveEventArgs<T> e)
        {
            if (_NameDict.ContainsKey(e.Item.Name))
            {
                _NameDict.Remove(e.Item.Name);
            }
            e.Item.BeforeNameChanged -= new EventHandler<NameChangeEventArgs>(Item_BeforeNameChange);
            e.Item.AfterNameChanged -= new EventHandler<NameChangeEventArgs>(Item_AfterNameChanged);
        }


        void NamedItemList_AfterInsert(object sender, InsertEventArgs<T> e)
        {
            _NameDict.Add(e.Item.Name, e.Item);
            e.Item.BeforeNameChanged += new EventHandler<NameChangeEventArgs>(Item_BeforeNameChange);
            e.Item.AfterNameChanged += new EventHandler<NameChangeEventArgs>(Item_AfterNameChanged);
        }



        void NamedItemList_BeforeInsert(object sender, InsertEventArgs<T> e)
        {
            if (_NameDict.ContainsKey(e.Item.Name))
            {
                throw new ArgumentException("Cant insert the INamedItem named {0}. The name does already exist in the collection.".Build(e.Item.Name));
            }
        }

        void NamedItemList_AfterSet(object sender, SetEventArgs<T> e)
        {
            if (_NameDict.ContainsKey(e.OldItem.Name))
            {
                _NameDict.Remove(e.OldItem.Name);
            };
            _NameDict.Add(e.NewItem.Name, e.NewItem);


            e.OldItem.BeforeNameChanged -= new EventHandler<NameChangeEventArgs>(Item_BeforeNameChange);
            e.OldItem.AfterNameChanged -= new EventHandler<NameChangeEventArgs>(Item_AfterNameChanged);

            e.NewItem.BeforeNameChanged += new EventHandler<NameChangeEventArgs>(Item_BeforeNameChange);
            e.NewItem.AfterNameChanged += new EventHandler<NameChangeEventArgs>(Item_AfterNameChanged);

        }

        void NamedItemList_BeforeSet(object sender, SetEventArgs<T> e)
        {
            if (!EqualityComparer<T>.Default.Equals(e.NewItem, e.OldItem))
            {
                if (!_NameDict.ContainsKey(e.NewItem.Name))
                {
                    //Name of the new item does not exist. 
                }
                else if (_NameDict.ContainsKey(e.NewItem.Name) && EqualityComparer<T>.Default.Equals(e.OldItem, _NameDict[e.NewItem.Name]))
                {
                    //Name does exist, but its the name of the old item.
                }
                else
                {
                    throw new ArgumentException("Cant set the INamedItem. The name {0} does already exist in the collection.".Build(e.NewItem.Name));
                }

            }
        }





        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedItemList{T}"/> class.
        /// </summary>
        public NamedItemList()
        {
            this.BeforeInsert += new EventHandler<InsertEventArgs<T>>(NamedItemList_BeforeInsert);
            this.AfterInsert += new EventHandler<InsertEventArgs<T>>(NamedItemList_AfterInsert);
            this.BeforeSet += new EventHandler<SetEventArgs<T>>(NamedItemList_BeforeSet);
            this.AfterSet += new EventHandler<SetEventArgs<T>>(NamedItemList_AfterSet);
            this.AfterRemove += new EventHandler<RemoveEventArgs<T>>(NamedItemList_AfterRemove);
            this.BeforeClear += new EventHandler<EventArgs>(NamedItemList_BeforeClear);
            this.AfterClear += new EventHandler<EventArgs>(NamedItemList_AfterClear);
        }



        /// <summary>
        /// Initializes a new instance of the <see cref="NamedItemList{T}"/> class.
        /// </summary>
        /// <param name="EnumerableList">A enumerable list of named items to be added to the list.</param>
        public NamedItemList(IEnumerable<T> EnumerableList)
            : this()
        {
            AddRange(EnumerableList);
        }
        #endregion

        /// <summary>
        /// Finalizes an instance of the <see cref="NamedItemList{T}"/> class.
        /// </summary>
        ~NamedItemList()
        {
            foreach (T Item in this)
            {
                Item.BeforeNameChanged -= new EventHandler<NameChangeEventArgs>(Item_BeforeNameChange);
                Item.AfterNameChanged -= new EventHandler<NameChangeEventArgs>(Item_AfterNameChanged);
            }
        }

    }
}
