using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.General
{

    /// <summary>
    /// List for Type objects.
    /// </summary>
    public class TypeList : List<Type>
    {

        /// <summary>
        /// Indexer which does return the first Type matching the specified TypeName.
        /// </summary>
        /// <param name="TypeName">Name of the Type.</param>
        /// <returns>Type object with matching TypeName or null if no match is found-</returns>
        public Type this[string TypeName]
        {
            get
            {
                foreach (Type Type in this)
                {
                    if (Type.Name == TypeName)
                    {
                        return Type;
                    }

                }
                return null;
                
            }
        }


        /// <summary>
        /// Checks if a Type with the specified TypeName exists in the list.
        /// </summary>
        /// <param name="TypeName">Name of the Type.</param>
        /// <returns>true or false.</returns>
        public bool Contains(string TypeName)
        {
            return this[TypeName] != null;
        }


        public TypeList() : base() { }
        public TypeList(int capacity) : base(capacity) { }
        public TypeList(IEnumerable<Type> collection) : base(collection) { }

    }
}
