using System;
using System.Collections;
using System.Collections.Generic;

namespace DirectOutput.General.Generic
{
    /// <summary>
    /// Extended version of the generic List class supporting events for various actions on the list.
    /// </summary>
    /// <typeparam name="T">Type of the items contained in the list</typeparam>
    public class ExtList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, ICollection, IList
    {
        private List<T> _InternalList = new List<T>();


        /// <summary>
        /// Determines the index of a specific item.
        /// </summary>
        /// <param name="Item">Item to locate.</param>
        /// <returns>Index of the specified item.</returns>
        public int IndexOf(T Item)
        {
            
            return _InternalList.IndexOf(Item);
        }

        /// <summary>
        /// Copies the elements of the ExtList to an Array, starting at a particular Array index.        
        /// </summary>  
        /// <param name="Array">The one-dimensional Array that is the destination of the elements copied from ExtList.The Array must have zero-based indexing.</param>
        /// <param name="ArrayIndex">The zero-based index in <paramref name="Array"/> at which copying begins.</param>
        public void CopyTo(T[] Array, int ArrayIndex)
        {
            _InternalList.CopyTo(Array, ArrayIndex);
        }


        /// <summary>
        /// This ExtList objects are not readonly.<br/>
        /// Will always return false.
        /// </summary>
        /// <value>Always false.</value>
        public bool IsReadOnly
        {
            get { return false; }
        }


        /// <summary>
        /// Returns an enumerator that iterates through a ExtList.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            return _InternalList.GetEnumerator();
        }


        /// <summary>
        /// Returns an enumerator that iterates through a ExtList.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Adds a new item to the ExtList.
        /// </summary>
        /// <param name="NewListItem">Item to add.</param>
        public void Add(T NewListItem)
        {
            if (BeforeInsert != null)
            {
                BeforeInsert(this, new InsertEventArgs<T>(-1, NewListItem));
            }
            _InternalList.Add(NewListItem);
            if (AfterInsert != null)
            {
                AfterInsert(this, new InsertEventArgs<T>(_InternalList.IndexOf(NewListItem), NewListItem));
            }
        }


        /// <summary>
        /// Adds a list of items to the ExtList.
        /// </summary>
        /// <param name="Collection">Collection of items to add.</param>
        public void AddRange(IEnumerable<T> Collection)
        {
            foreach (T Item in Collection)
            {
                Add(Item);
            }
        }


        /// <summary>
        /// Creates a clone of the ExtList
        /// </summary>
        /// <returns>A clone of the ExtList.</returns>
        public ExtList<T> Clone()
        {
            ExtList<T> L = new ExtList<T>();
            foreach (T Item in this)
            {
                L.Add(Item);
            }
            return L;
        }



        /// <summary>
        /// Inserts an item to the ExtList at the specified Index.
        /// </summary>
        /// <param name="Index">Index at which to insert the item.</param>
        /// <param name="Item">Item to insert.</param>
        public void Insert(int Index, T Item)
        {
            if (BeforeInsert != null)
            {
                BeforeInsert(this, new InsertEventArgs<T>(Index, Item));
            }

            _InternalList.Insert(Index, Item);

            if (AfterInsert != null)
            {
                AfterInsert(this, new InsertEventArgs<T>(Index, Item));
            }


        }



        /// <summary>
        /// Clears the ExtList.
        /// </summary>
        public void Clear()
        {
            if (BeforeClear != null)
            {
                BeforeClear(this, new EventArgs());
            }
            _InternalList.Clear();
            if (AfterClear != null)
            {
                AfterClear(this, new EventArgs());
            }
        }



        /// <summary>
        /// Number of items in the ExtList.
        /// </summary>
        public int Count
        {
            get { return _InternalList.Count; }
        }



        /// <summary>
        /// Romves a item from the ExtList.
        /// </summary>
        /// <param name="ItemToRemove">Item to remove.</param>
        public bool Remove(T ItemToRemove)
        {
            bool ItemRemoved = false;
            if (_InternalList.Contains(ItemToRemove))
            {
                int Index = _InternalList.IndexOf(ItemToRemove);
                if (BeforeRemove != null)
                {
                    BeforeRemove(this, new RemoveEventArgs<T>(Index, ItemToRemove));
                }

                _InternalList.Remove(ItemToRemove);
                ItemRemoved = true;
                if (AfterRemove != null)
                {
                    AfterRemove(this, new RemoveEventArgs<T>(Index, ItemToRemove));
                }
            }
            return ItemRemoved;
        }


        /// <summary>
        /// Removes a item at a specified index.
        /// </summary>
        /// <param name="Index">Index of the item to remove.</param>
        public void RemoveAt(int Index)
        {
            T ItemToRemove = this[Index];
            if (BeforeRemove != null)
            {
                BeforeRemove(this, new RemoveEventArgs<T>(Index, ItemToRemove));
            }

            _InternalList.RemoveAt(Index);

            if (AfterRemove != null)
            {
                AfterRemove(this, new RemoveEventArgs<T>(Index, ItemToRemove));
            }

        }



        /// <summary>
        /// Checks wether the specified item is contained in the ExtList.
        /// </summary>
        /// <param name="ItemToCheck">Item to check.</param>
        /// <returns>true or false.</returns>
        public bool Contains(T ItemToCheck)
        {
            return _InternalList.Contains(ItemToCheck);
        }



        /// <summary>
        /// Indexer of the ExtList.<br/>
        /// Returns the item at the specified index.
        /// </summary>
        /// <param name="Index">Index of the item to return.</param>
        /// <value>Item at specified index.</value>
        public T this[int Index]
        {

            get { return _InternalList[Index]; }
            set
            {
                T OldItem = _InternalList[Index];
                if (BeforeSet != null)
                {
                    BeforeSet(this, new SetEventArgs<T>(Index, OldItem, value));
                }

                _InternalList[Index] = value;
                if (AfterSet != null)
                {
                    AfterSet(this, new SetEventArgs<T>(Index, OldItem, value));
                }
            }
        }


        /// <summary>
        /// Sorts the ExtList.
        /// </summary>
        public void Sort()
        {
            _InternalList.Sort();
        }


        /// <summary>
        /// Sorts the ExtList.
        /// </summary>
        /// <param name="Comparer">Comparer to using for sorting.</param>
        public void Sort(IComparer<T> Comparer)
        {
            _InternalList.Sort(Comparer);
        }
        /// <summary>
        /// Sorts the elements in the entire ExtList using the specified System.Comparison&lt;T&gt;.
        /// </summary>
        /// <param name="Comparison">Comparer to using for sorting.</param>
        public void Sort(Comparison<T> Comparison)
        {
            _InternalList.Sort(Comparison);
        }



        /// <summary>
        /// Returns a array containg the ExtList items.
        /// </summary>
        /// <returns>Array of ExtList items.</returns>
        public T[] ToArray()
        {
            return _InternalList.ToArray();
        }


        #region Events
        #region Clear Events


        /// <summary>
        /// Fires before the ExtList is cleared.<br/>
        /// If a exception is trown within the events, the list is not cleared.
        /// </summary>
        public event EventHandler<EventArgs> BeforeClear;



        /// <summary>
        /// Fires after the ExtList is cleared.
        /// </summary>
        public event EventHandler<EventArgs> AfterClear;


        #endregion
        #region Insert Events

        /// <summary>
        /// Fires before a new item is inserted into the ExtList.<br/>
        /// If a exception is occurs in the event, to item is not added.
        /// OnValidate is called prior to this method.<br/>
        /// </summary>
        public event EventHandler<InsertEventArgs<T>> BeforeInsert;

        /// <summary>
        /// Fires after a new item is inserted into the ExtList.<br/>
        /// OnValidate is called prior to this method.<br/>
        /// </summary>
        public event EventHandler<InsertEventArgs<T>> AfterInsert;


        #endregion
        #region Remove Events

        /// <summary>
        /// Fires before a item is removed from the ExtList.
        /// </summary>
        public event EventHandler<RemoveEventArgs<T>> BeforeRemove;


        /// <summary>
        /// Fires after a item is removed from the ExtList.
        /// </summary>
        public event EventHandler<RemoveEventArgs<T>> AfterRemove;

        #endregion
        #region Set Events


        /// <summary>
        /// Fires before a item is set in the ExtList.
        /// OnValidate is called prior to this method.
        /// </summary>
        public event EventHandler<SetEventArgs<T>> BeforeSet;


        /// <summary>
        /// Fires after a item has been set in the ExtList.
        /// </summary>
        public event EventHandler<SetEventArgs<T>> AfterSet;



        #endregion

        #endregion


        public ExtList()
        {

        }

        public ExtList(IEnumerable<T> EnumerableList)
        {
            AddRange(EnumerableList);
        }



        #region ICollection Member

        public void CopyTo(Array array, int index)
        {
            ((ICollection)_InternalList).CopyTo(array, index);
        }

        public bool IsSynchronized
        {
            get { return ((ICollection)_InternalList).IsSynchronized; }
        }

        public object SyncRoot
        {
            get { return ((ICollection)_InternalList).SyncRoot; }
        }

        #endregion

        #region IList Member


        public int Add(object value)
        {
            if(value.GetType() != typeof(T)) {
                throw new ArgumentException(" Value is of a wrong type.");
            }
            Add((T)value);
            return _InternalList.Count - 1;
        }


        public bool Contains(object value)
        {
            if (value.GetType() != typeof(T))
            {
                throw new ArgumentException(" Value is of a wrong type.");
            }
            return _InternalList.Contains((T)value);
        }

        public int IndexOf(object value)
        {
            if (value.GetType() != typeof(T))
            {
                throw new ArgumentException(" Value is of a wrong type.");
            }
            return _InternalList.IndexOf((T)value);
        }

        public void Insert(int index, object value)
        {
            if (value.GetType() != typeof(T))
            {
                throw new ArgumentException(" Value is of a wrong type.");
            }
            Insert(index, (T)value);
        }

        public bool IsFixedSize
        {
            get { return ((IList)_InternalList).IsFixedSize; }
        }

        public void Remove(object value)
        {
            if (value.GetType() != typeof(T))
            {
                throw new ArgumentException(" Value is of a wrong type.");
            }
            Remove((T)value);
        }

        object IList.this[int index]
        {
            get
            {
                return _InternalList[index];
            }
            set
            {
                this[index] = (T)value;
            }
        }

        #endregion
    }

}
