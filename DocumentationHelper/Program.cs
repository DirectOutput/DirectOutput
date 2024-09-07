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


            Types.Sort((T1, T2) => T1.FullName.CompareTo(T2.FullName));

            string S = "Built in Toys  {#toy_builtin}\n";
            S += "==========\n";
            foreach (Type T in Types.OrderBy(T=>T.Name))
            {
                TypeDocuData I = new TypeDocuData();

                I.Type = T;

                S += I.GetDocu();

            }

            S.WriteToFile(@"..\..\Documentation\64_Toys_BuiltIn.md");


            Types = new DirectOutput.General.TypeList(AppDomain.CurrentDomain.GetAssemblies().ToList().SelectMany(s => s.GetTypes()).Where(p => typeof(DirectOutput.FX.IEffect).IsAssignableFrom(p) && !p.IsAbstract));

            Types.Sort((T1, T2) => T1.FullName.CompareTo(T2.FullName));

             S = "Built in Effects  {#fx_builtin}\n";
            S += "==========\n";
            foreach (Type T in Types.OrderBy(T=>T.Name))
            {
                TypeDocuData I = new TypeDocuData();

                I.Type = T;

                S += I.GetDocu();

            }

            S.WriteToFile(@"..\..\Documentation\61_FX_BuiltIn.md");

            Types = new DirectOutput.General.TypeList(AppDomain.CurrentDomain.GetAssemblies().ToList().SelectMany(s => s.GetTypes()).Where(p => typeof(DirectOutput.Cab.Out.IOutputController).IsAssignableFrom(p) && !p.IsAbstract));

            S = "Built in Output controllers  {#outputcontrollers_builtin}\n";
            S += "==========\n";
            foreach (Type T in Types.OrderBy(T => T.Name))
            {

                TypeDocuData I = new TypeDocuData();

                I.Type = T;

                S += I.GetDocu();

            }

            S.WriteToFile(@"..\..\Documentation\66_OutputControllers_BuiltIn.md");




        }
    }
}
