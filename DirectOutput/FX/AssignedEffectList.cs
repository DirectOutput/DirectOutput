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
        public void Init(Pinball Pinball)
        {
            foreach (AssignedEffect TEE in this)
            {
                TEE.Init(Pinball);
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
        /// Sorts the list by Order and Name of the AssignedEffect objects in the list
        /// </summary>
        public new void Sort()
        {
            base.Sort((a, b) => (a.Order == b.Order ? a.EffectName.CompareTo(b.EffectName) : a.Order.CompareTo(b.Order)));
        }

        #region Event handling
        void TableElementEffect_OrderChanged(object sender, EventArgs e)
        {
            Sort();
        }

        void TableElementEffect_EffectNameChanged(object sender, EventArgs e)
        {
            Sort();

        }


        private void SubscribeEvents(AssignedEffect TableElementEffect)
        {
            TableElementEffect.EffectNameChanged += new EventHandler<EventArgs>(TableElementEffect_EffectNameChanged);
            TableElementEffect.OrderChanged += new EventHandler<EventArgs>(TableElementEffect_OrderChanged);
        }

        private void UnsubscribeEvents(AssignedEffect TableElementEffect)
        {
            TableElementEffect.EffectNameChanged -= new EventHandler<EventArgs>(TableElementEffect_EffectNameChanged);
            TableElementEffect.OrderChanged -= new EventHandler<EventArgs>(TableElementEffect_OrderChanged);
        }

        void TableElementEffectList_BeforeClear(object sender, EventArgs e)
        {
            foreach (AssignedEffect TEE in this)
            {
                UnsubscribeEvents(TEE);
            }
        }

        void TableElementEffectList_AfterRemove(object sender, RemoveEventArgs<AssignedEffect> e)
        {
            UnsubscribeEvents(e.Item);
        }

        void TableElementEffectList_AfterInsert(object sender, InsertEventArgs<AssignedEffect> e)
        {
            SubscribeEvents(e.Item);
        }

        void TableElementEffectList_AfterSet(object sender, SetEventArgs<AssignedEffect> e)
        {
            UnsubscribeEvents(e.OldItem);
            SubscribeEvents(e.NewItem);
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="AssignedStaticEffects"/> class.
        /// </summary>
        public AssignedEffectList()
        {
            this.AfterSet += new EventHandler<SetEventArgs<AssignedEffect>>(TableElementEffectList_AfterSet);
            this.AfterInsert += new EventHandler<InsertEventArgs<AssignedEffect>>(TableElementEffectList_AfterInsert);
            this.AfterRemove += new EventHandler<RemoveEventArgs<AssignedEffect>>(TableElementEffectList_AfterRemove);
            this.BeforeClear += new EventHandler<EventArgs>(TableElementEffectList_BeforeClear);
        }


        ~AssignedEffectList()
        {
            foreach (AssignedEffect TEE in this)
            {
                UnsubscribeEvents(TEE);
            }
        }

    }
}
