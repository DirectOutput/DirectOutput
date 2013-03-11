using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.FX
{
    /// <summary>
    /// Provides combined access and enumeration of the content of several effect list.
    /// </summary>
    public class CombinedEffectList : IEnumerable<IEffect>
    {
        
        private IEnumerable<EffectList> EffectLists;


        #region Enumerator

        /// <summary>
        /// Returns the enumerator for this collection.<br/>
        /// Required for the implementation of IEnumerable&lt;IEffect&gt;.
        /// </summary>
        public IEnumerator<IEffect> GetEnumerator()
        {
            return new Enumerator(this);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }


        /// <summary>
        /// Enumerator class for IEnumerable&lt;IEffect&gt;.
        /// </summary>
        public sealed class Enumerator : IEnumerator<IEffect>
        {
            private CombinedEffectList _CombinedEffectList;
            private int _Index;
            private IEffect _Current;

            internal Enumerator(CombinedEffectList CombinedEffectList)
            {
                this._CombinedEffectList = CombinedEffectList;
                this._Index = 0;
                this._Current = null;
            }

            public IEffect Current
            {
                get { return this._Current; }
            }

            public void Dispose()
            {
                this.Reset();
                
            }

            object System.Collections.IEnumerator.Current
            {
                get { return this.Current; }
            }

            public bool MoveNext()
            {

                if (_Index < _CombinedEffectList.Count)
                {
                    _Current = _CombinedEffectList[_Index];
                    _Index++;
                }
                return false;
            }

            public void Reset()
            {
                this._Index = 0;
                this._Current = null;
            }
        }
        #endregion


        #region Indexer

        /// <summary>
        /// Indexer looks up the first IOutput with the specified name.<br/>
        /// If several IOutput objects in the list are using the same name, only the first matching IOutput will be returned.
        /// </summary>
        /// <param name="Name">Name of the IOutput to look up.</param>
        /// <returns>IOutput object if a match is found, null otherwise.</returns>
        public IEffect this[string Name]
        {
            get
            {
                foreach (EffectList EL in EffectLists)
                {
                    if (EL.Contains(Name))
                    {
                        return EL[Name];
                    }
                }
                return null;
            }
        }


        /// <summary>
        /// Indexer for the CombinedEffectList.
        /// </summary>
        /// <value>
        /// The <see cref="IEffect"/>.
        /// </value>
        /// <param name="Index">Index of the list item to return.</param>
        /// <returns>
        /// IEffect at the given index.
        /// </returns>
        /// <exception cref="System.ArgumentException">Enumeration index out of range.</exception>
        public IEffect this[int Index]
        {
            get
            {
                int Cnt = this.Count;
                if (Index < Cnt)
                {
                    foreach (EffectList EL in EffectLists)
                    {
                        if (Index < EL.Count)
                        {
                            return EL[Index];
                        }
                        Cnt -= EL.Count;
                        if (Cnt < 0) break;
                    }
                }
                throw new ArgumentException("Enumeration index out of range");
            }
        }
        #endregion

        #region Contains
        /// <summary>
        /// Checks if the specified IEffect exists in the list.
        /// </summary>
        /// <param name="Effect">Effect to check.</param>
        /// <returns>true/false</returns>
        public bool Contains(IEffect Effect)
        {
            foreach (EffectList EL in EffectLists)
            {
                if (EL.Contains(Effect)) return true;

            }
            return false;
        }
        /// <summary>
        /// Checks if a IEffect with a specific name exists in the list.
        /// </summary>
        /// <param name="Name">Name to check.</param>
        /// <returns>
        /// true/false
        /// </returns>
        public bool Contains(string Name)
        {
            foreach (EffectList EL in EffectLists)
            {
                if (EL.Contains(Name)) return true;

            }
            return false;
        }


        #endregion


        #region Count
        /// <summary>
        /// Returns the number of effects for all effect lists.
        /// </summary>
        public int Count
        {
            get
            {
                int Cnt = 0;
                foreach (EffectList EL in EffectLists)
                {
                    Cnt += EL.Count;
                }
                return Cnt;
            }
        }
        #endregion



        /// <summary>
        /// Initializes a new instance of the <see cref="CombinedEffectList"/> class.
        /// </summary>
        public CombinedEffectList() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="CombinedEffectList"/> class.
        /// </summary>
        /// <param name="EffectLists">Enumerable list of EffectList objectes to be used in the <see cref="CombinedEffectList"/>.</param>
        public CombinedEffectList(IEnumerable<EffectList> EffectLists)
            : this()
        {
            this.EffectLists = EffectLists;
        }


    }
}
