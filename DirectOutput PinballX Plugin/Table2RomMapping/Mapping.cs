using System;
using System.Collections.Generic;
using System.Text;

namespace PinballX.Table2RomMapping
{
    /// <summary>
    /// Simple class containing the mappinging between tablename and romname
    /// </summary>
    public class Mapping
    {
        /// <summary>
        /// Gets or sets the name of the table.
        /// This is not necessarly the same as the name of the table file. Use fuzzy text matching to find the right TableName.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        public string TableName { get; set; }
        /// <summary>
        /// Gets or sets the name of the rom of the table. Typically the property will contain the short romname of the table (same as in the ini files).
        /// </summary>
        /// <value>
        /// The name of the rom.
        /// </value>
        public string RomName { get; set; }
    }
}
