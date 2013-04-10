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
    public class AssignedEffectList : ExtList<AssignedEffectOrder>
    {

        /// <summary>
        /// Adds the specified effect to the list. 
        /// </summary>
        /// <param name="EffectName">Name of the effect.</param>
        public void Add(string EffectName)
        {
            Add(new AssignedEffectOrder(EffectName));
        }


        /// <summary>
        /// Adds the specified effect to the list.
        /// </summary>
        /// <param name="EffectName">Name of the effect.</param>
        /// <param name="Order">The order of the effect.</param>
        public void Add(string EffectName, int Order)
        {
            Add(new AssignedEffectOrder(EffectName,Order));
        }

        /// <summary>
        /// Triggers all AssignedEffectOrder objects in the list.
        /// </summary>
        /// <param name="TableElementData">The table element data.</param>
        public void Trigger(TableElementData TableElementData)
        {
            foreach (AssignedEffectOrder TEE in this)
            {
                TEE.Trigger(TableElementData);
            }
        }


        /// <summary>
        /// Initializes the AssignedEffectOrder objects in the list.
        /// </summary>
        public void Init(Pinball Pinball)
        {
            foreach (AssignedEffectOrder TEE in this)
            {
                TEE.Init(Pinball);
            }
        }


        /// <summary>
        /// Fnishes the AssignedEffectOrder objects in the list.
        /// </summary>
        public void Finish()
        {
            foreach (AssignedEffectOrder TEE in this)
            {
                TEE.Finish();
            }
        }


        /// <summary>
        /// Sorts the list by Order and Name of the AssignedEffectOrder objects in the list
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


        private void SubscribeEvents(AssignedEffectOrder AssignedEffectOrder)
        {
            AssignedEffectOrder.EffectNameChanged += new EventHandler<EventArgs>(TableElementEffect_EffectNameChanged);
            AssignedEffectOrder.OrderChanged += new EventHandler<EventArgs>(TableElementEffect_OrderChanged);
        }

        private void UnsubscribeEvents(AssignedEffectOrder AssignedEffectOrder)
        {
            AssignedEffectOrder.EffectNameChanged -= new EventHandler<EventArgs>(TableElementEffect_EffectNameChanged);
            AssignedEffectOrder.OrderChanged -= new EventHandler<EventArgs>(TableElementEffect_OrderChanged);
        }

        void TableElementEffectList_BeforeClear(object sender, EventArgs e)
        {
            foreach (AssignedEffectOrder TEE in this)
            {
                UnsubscribeEvents(TEE);
            }
        }

        void TableElementEffectList_AfterRemove(object sender, RemoveEventArgs<AssignedEffectOrder> e)
        {
            UnsubscribeEvents(e.Item);
        }

        void TableElementEffectList_AfterInsert(object sender, InsertEventArgs<AssignedEffectOrder> e)
        {
            SubscribeEvents(e.Item);
        }

        void TableElementEffectList_AfterSet(object sender, SetEventArgs<AssignedEffectOrder> e)
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
            this.AfterSet += new EventHandler<SetEventArgs<AssignedEffectOrder>>(TableElementEffectList_AfterSet);
            this.AfterInsert += new EventHandler<InsertEventArgs<AssignedEffectOrder>>(TableElementEffectList_AfterInsert);
            this.AfterRemove += new EventHandler<RemoveEventArgs<AssignedEffectOrder>>(TableElementEffectList_AfterRemove);
            this.BeforeClear += new EventHandler<EventArgs>(TableElementEffectList_BeforeClear);
        }


        ~AssignedEffectList()
        {
            foreach (AssignedEffectOrder TEE in this)
            {
                UnsubscribeEvents(TEE);
            }
        }

    }
}
