using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput;
using DirectOutput.Cab.Toys;
using System.Reflection;
using System.Xml.Serialization;
using System.IO;

using DirectOutput.FX;
namespace DirectOutput_SampleXMLBuilder
{
    public static class SampleBuilder
    {

        public static void CreateEffectSamplesPage(string Filename)
        {
            StringBuilder S = new StringBuilder();

            S.AppendLine("\\subsection builtinfx_samplexml Builtin effects sample XML");
            S.AppendLine();
            S.AppendLine("Effects are configured in the table config XML files. Below you can find XML fragments for all built in effects. Please read the docu on the effect classes for more details on the meaning and function of the members of the effect classes.");
            S.AppendLine();

            DirectOutput.General.TypeList Types = new DirectOutput.General.TypeList(AppDomain.CurrentDomain.GetAssemblies().ToList().SelectMany(s => s.GetTypes()).Where(p => typeof(IEffect).IsAssignableFrom(p) && !p.IsAbstract));
            foreach (Type T in Types)
            {
                
                S.AppendFormat("\\subsection builtinfx_{0} {1}\n", T.Name.ToLower(), T.Name);
                S.AppendLine();
                S.AppendLine("~~~~~~~~~~~~~~~~~~~~~~~~~~(.xml)");
                S.AppendLine(GetEffectSampleXml(T));
                S.AppendLine("~~~~~~~~~~~~~~~~~~~~~~~~~~");
                S.AppendLine();
            }
            Console.WriteLine(S.ToString());
        }


        public static string GetEffectSampleXml(Type EffectType)
        {
            string S = "";

            DirectOutput.General.TypeList Types = new DirectOutput.General.TypeList(AppDomain.CurrentDomain.GetAssemblies().ToList().SelectMany(s => s.GetTypes()).Where(p => typeof(IEffect).IsAssignableFrom(p) && !p.IsAbstract));
            if (Types.Contains(EffectType))
            {

                object O = Activator.CreateInstance(EffectType);


                foreach (PropertyInfo PI in EffectType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    if (PI.CanWrite)
                    {
                        if (PI.PropertyType == typeof(string) && PI.Name == "Name")
                        {
                            PI.SetValue(O, "Effect name", null);
                        }
                        else if (PI.PropertyType.IsNumber())
                        {
                            //                               PI.SetValue(O, 0, null);
                        }
                        else if (PI.PropertyType == typeof(bool))
                        {
                            PI.SetValue(O, false, null);
                        }
                        else if (PI.PropertyType == typeof(string) && PI.Name.ToLower().Contains("output"))
                        {
                            PI.SetValue(O, "Name of a output", null);
                        }
                        else if (PI.PropertyType == typeof(string) && PI.Name.ToLower().Contains("toy"))
                        {
                            PI.SetValue(O, "Name of a toy", null);
                        }
                        else if (PI.PropertyType == typeof(string))
                        {
                            string V = (string)PI.GetValue(O, null);
                            if (V.IsNullOrWhiteSpace())
                            {
                                PI.SetValue(O, "{0} value".Build(PI.Name), null);
                            }
                        }
                        else if (PI.PropertyType == typeof(DateTime))
                        {
                            PI.SetValue(O, DateTime.MaxValue, null);
                        }
                    }
                }

                try
                {
                    XmlSerializerNamespaces EmptyNamepsaces = new XmlSerializerNamespaces(new[] { System.Xml.XmlQualifiedName.Empty });
                    var Serializer = new XmlSerializer(EffectType);
                    var Settings = new System.Xml.XmlWriterSettings();
                    Settings.Indent = true;
                    Settings.OmitXmlDeclaration = true;


                    using (var Stream = new StringWriter())
                    using (var Writer = System.Xml.XmlWriter.Create(Stream, Settings))
                    {
                        Serializer.Serialize(Writer, O, EmptyNamepsaces);
                        S = Stream.ToString();
                    }

                }
                catch (Exception E)
                {
                    S = "XML Serialization failed.\nException:\n{0}".Build(E.Message);
                }

            }
            else
            {
                S = "Effect not found";
            }
            return S;
        }


        private static string GetToySampleXml(Type ToyType)
        {
            string S = "";


            if (ToyType is IToy)
            {
                object O = Activator.CreateInstance(ToyType);


                foreach (PropertyInfo PI in ToyType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    if (PI.CanWrite)
                    {
                        if (PI.PropertyType == typeof(string) && PI.Name == "Name")
                        {
                            PI.SetValue(O, "Toy name", null);
                        }
                        else if (PI.PropertyType.IsNumber())
                        {
                            //                               PI.SetValue(O, 0, null);
                        }
                        else if (PI.PropertyType == typeof(bool))
                        {
                            PI.SetValue(O, false, null);
                        }
                        else if (PI.PropertyType == typeof(string) && PI.Name.ToLower().Contains("output"))
                        {
                            PI.SetValue(O, "Name of a output", null);
                        }
                        else if (PI.PropertyType == typeof(string))
                        {
                            string V = (string)PI.GetValue(O, null);
                            if (V.IsNullOrWhiteSpace())
                            {
                                PI.SetValue(O, "{0} value".Build(PI.Name), null);
                            }
                        }
                        else if (PI.PropertyType == typeof(DateTime))
                        {
                            PI.SetValue(O, DateTime.MaxValue, null);
                        }
                    }
                }

                try
                {
                    XmlSerializerNamespaces EmptyNamepsaces = new XmlSerializerNamespaces(new[] { System.Xml.XmlQualifiedName.Empty });
                    var Serializer = new XmlSerializer(ToyType);
                    var Settings = new System.Xml.XmlWriterSettings();
                    Settings.Indent = true;
                    Settings.OmitXmlDeclaration = true;


                    using (var Stream = new StringWriter())
                    using (var Writer = System.Xml.XmlWriter.Create(Stream, Settings))
                    {
                        Serializer.Serialize(Writer, O, EmptyNamepsaces);
                        S = Stream.ToString();
                    }

                }
                catch (Exception E)
                {
                    S = "XML Serialization failed.\nException:\n{0}".Build(E.Message);
                }

            }
            else
            {
                S = "Type not found";
            }
            return S;

        }
    }
}
