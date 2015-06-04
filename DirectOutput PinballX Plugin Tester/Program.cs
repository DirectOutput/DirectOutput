using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PinballX.Table2RomMapping;

namespace DirectOutput_PinballX_Plugin_Tester
{
    class Program
    {
        static void Main(string[] args)
        {

            TableNameMappings N = new TableNameMappings();
            N.Add(new Mapping() { TableName = "Tablename 1", RomName = "AA" });
            N.Add(new Mapping() { TableName = "Tablename 2", RomName = "BB" });
            N.SaveTableMappings(@"C:\Users\tom\Desktop\testtablemappings.xml");

            TableNameMappings M = TableNameMappings.LoadTableMappings(@"C:\Users\tom\Desktop\tablemappings.xml");
        }
    }
}
