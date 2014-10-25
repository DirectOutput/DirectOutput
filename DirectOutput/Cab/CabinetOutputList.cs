using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using DirectOutput.Cab.Out;
namespace DirectOutput.Cab
{
    /// <summary>
    /// Readonly list containing all IOutput objects of all IOutputController objects in a cabinet.
    /// </summary>
    public class CabinetOutputList : IEnumerable<IOutput>
    {
        private Cabinet _Cabinet;
        private OutputControllerList OutputControllers { get { return _Cabinet.OutputControllers; } }
        #region Enumerator

        /// <summary>
        /// Returns the enumerator for this collection.<br/>
        /// Required for the implementation of IEnumerable&lt;IOutput&gt;.
        /// </summary>
        public IEnumerator<IOutput> GetEnumerator()
        {
            return new Enumerator(this);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }


        /// <summary>
        /// Enumerator class for IEnumerable&lt;IOutput&gt;.
        /// </summary>
        public class Enumerator : IEnumerator<IOutput>
        {
            private CabinetOutputList _CabinetOutputList;
            private int _Index;
            private IOutput _Current;

            internal Enumerator(CabinetOutputList CabinetOutputList)
            {
                this._CabinetOutputList = CabinetOutputList;
                this._Index = 0;
                this._Current = null;
            }

            public IOutput Current
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

                if (_Index < _CabinetOutputList.Count)
                {
                    _Current = _CabinetOutputList[_Index];
                    _Index++;
                    return true;
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
        /// Indexer looks up the first IOutput with the specified name or path.<br/>
        /// If several IOutput objects in the list are using the same name, only the first matching IOutput will be returned.<br/>
        /// Instead of a name and path consisting of {OutputControllerName}\\{OutputName} can be supplied. If no item matches the path, null will be returned.
        /// </summary>
        /// <param name="Name">Name of the IOutput to look up.</param>
        /// <returns>IOutput object if a match is found, null otherwise.</returns>
        public IOutput this[string Name]
        {
            get
            {
                string[] N = Name.Replace("/", "\\").Split('\\');
                if (N.Length==2)
                {
                    //it is a path
                    if (OutputControllers.Contains(N[0]))
                    {
                        IOutputController OC = OutputControllers[N[0]];
                        if (OC.Outputs.Contains(N[1]))
                        {
                            return OC.Outputs[N[1]];
                        }
                    }
                }
                else
                {
                    //just a simple name
                    foreach (IOutputController OC in this.OutputControllers)
                    {
                        if (OC.Outputs.Contains(Name))
                        {
                            return (IOutput)OC.Outputs[Name];
                        }
                    }
                }
                return null;
            }
        }


        /// <summary>
        /// Indexer for the list.
        /// </summary>
        /// <param name="Index">Index of the list item to return.</param>
        /// <returns>IOutput at the given index.</returns>
        public IOutput this[int Index]
        {
            get
            {
                int Cnt = this.Count;
                if (Index < Cnt)
                {
                    foreach (IOutputController OC in this.OutputControllers)
                    {
                        if (Index < OC.Outputs.Count)
                        {
                            return OC.Outputs[Index];
                        }
                        Cnt -= OC.Outputs.Count;
                        if (Cnt < 0) break;
                    }
                }
                throw new Exception("Enumeration index of CabinateOutputList out of range");
            }
        }
        #endregion

        #region Contains
        /// <summary>
        /// Checks if the specified IOutput exists in the list.
        /// </summary>
        /// <param name="Output">Output to check.</param>
        /// <returns>true/false</returns>
        public bool Contains(IOutput Output)
        {
            foreach (IOutputController OC in this.OutputControllers)
            {
                if (OC.Outputs.Contains(Output)) return true;

            }
            return false;
        }
        /// <summary>
        /// Checks if a IOutput with a specific name exists in the list.<br/>
        /// Instead of a name and path consisting of {OutputControllerName}\\{OutputName} can be supplied.
        /// </summary>
        /// <param name="Name">Name or path to check.</param>
        /// <returns>true if a IOputput with the given name or path exists, otherwiese false.</returns>
        public bool Contains(string Name)
        {
            return (this[Name] != null);
            
        }


        #endregion


        #region Count
        /// <summary>
        /// Returns the number of outputs for all output controlers.
        /// </summary>
        public int Count
        {
            get
            {
                int Cnt = 0;
                foreach (OutputControllerBase OC in OutputControllers)
                {
                    Cnt += OC.Outputs.Count;
                }
                return Cnt;
            }
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="CabinetOutputList"/> class.
        /// </summary>
        /// <param name="Cabinet">The cabinet to which the list belongs.</param>
        public CabinetOutputList(Cabinet Cabinet)
        {
            _Cabinet = Cabinet;
        }


    }
}
