using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using DirectOutput.General.Generic;
using DirectOutput.Table;
namespace DirectOutput.FX
{
    /// <summary>
    /// List of effects which are assigned to some other object.
    /// </summary>
    public class AssignedEffectList : ExtList<AssignedEffect>
    {

        /// <summary>
        /// Adds the specified effect to the list. 
        /// </summary>
        /// <param name="EffectName">Name of the effect.</param>
        public void Add(string EffectName)
        {
            Add(new AssignedEffect(EffectName));
        }

        /// <summary>
        /// Triggers all AssignedEffect objects in the list.
        /// </summary>
        /// <param name="TableElementData">The table element data.</param>
        public void Trigger(TableElementData TableElementData)
        {
            foreach (AssignedEffect TEE in this)
            {
                TEE.Trigger(TableElementData);
            }
        }


        /// <summary>
        /// Initializes the AssignedEffect objects in the list.
        /// </summary>
        /// <param name="Table">The table contining the AssignedEffectList.</param>
        public void Init(Table.Table Table)
        {
            foreach (AssignedEffect TEE in this)
            {
                TEE.Init(Table);
            }
        }


        /// <summary>
        /// Fnishes the AssignedEffect objects in the list.
        /// </summary>
        public void Finish()
        {
            foreach (AssignedEffect TEE in this)
            {
                TEE.Finish();
            }
        }




        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public AssignedEffectList()
        {
        }






        #region ICollection Member

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IEnumerable Member

        public new IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
