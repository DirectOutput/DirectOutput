using System;
using System.Collections.Generic;

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


        /// <summary>
        /// Initializes a new instance of the <see cref="TypeList"/> class.
        /// </summary>
        public TypeList() : base() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeList"/> class.
        /// </summary>
        /// <param name="capacity">The number of elements that the new list can initially store.</param>
        public TypeList(int capacity) : base(capacity) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeList"/> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public TypeList(IEnumerable<Type> collection) : base(collection) { }

    }
}
