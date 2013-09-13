using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.FX.RGBAFX;

namespace DocumentationHelper
{
    class Program
    {
        static void Main(string[] args)
        {
      


            DirectOutput.General.TypeList Types = new DirectOutput.General.TypeList(AppDomain.CurrentDomain.GetAssemblies().ToList().SelectMany(s => s.GetTypes()).Where(p => typeof(DirectOutput.Cab.Toys.IToy).IsAssignableFrom(p) && !p.IsAbstract));

            string S = "";
            foreach (Type T in Types)
            {
                TypeDocuData I = new TypeDocuData();

                I.Type = T;

                S += I.GetDocu();

            }


            Console.WriteLine(S);


            Console.ReadKey();
        }
    }
}
