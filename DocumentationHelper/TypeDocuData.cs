using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;
using System.IO;
using System.Xml.Serialization;
using DirectOutput.General.Generic;

namespace DocumentationHelper
{
    public class TypeDocuData
    {

        private Type _Type;

        public Type Type
        {
            get { return _Type; }
            set { _Type = value; }
        }


        public string Summary
        {
            get
            {
                string N = "{0}.{1}".Build(Type.Namespace, Type.Name);
                if (VSXmlDocu.TypeSummary.ContainsKey(N))
                {
                    return VSXmlDocu.TypeSummary[N];
                }
                return "";
            }
        }


        public string GetDocu()
        {
            string S = "";



            S += "\\section use_{0}_{1} {1}\n\n".Build(NamespaceName.Replace(".", "_"), Name);

            if (!Summary.IsNullOrWhiteSpace())
            {
                S += "\\subsection use_{0}_{1}_summary Summary\n\n".Build(NamespaceName.Replace(".", "_"), Name);
                S += Summary;
                S += "\n\n";
            }

            string Xml = GetSampleXML();
            if (!Xml.IsNullOrWhiteSpace())
            {
                S += "\\subsection use_{0}_{1}_samplexml Sample XML\n\n".Build(NamespaceName.Replace(".", "_"), Name);
                S += "A configuration section for {0} might resemble the following structure:\n\n".Build(Name);
                S += "~~~~~~~~~~~~~{.xml}\n";
                S += Xml;
                S += "\n~~~~~~~~~~~~~\n";
            }

            List<PropertyDocuData> PDL=GetPropertyDocuDataList();
            PDL.Sort((PDD1, PDD2) => PDD1.Name.CompareTo(PDD2.Name));
            if (PDL.Count > 0)
            {
                S += "\\subsection use_{0}_{1}_properties Properties\n\n".Build(NamespaceName.Replace(".", "_"), Name);
                S += "{0} has the following {1} configurable properties:\n\n".Build(Name, PDL.Count);
                foreach (PropertyDocuData PDD in PDL)
                {
                    S += PDD.GetDocu();                    
                }

            }


            return S;



        }



        public string Name { get { return Type.Name; } }

        public string NamespaceName { get { return Type.Namespace; } }




        public string GetSampleXML()
        {
            object O = GetSampleReferenceType(Type);

            string Xml = "";
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {

                    XmlSerializerNamespaces Namespaces = new XmlSerializerNamespaces();
                    Namespaces.Add(string.Empty, string.Empty);
                    new XmlSerializer(Type).Serialize(ms, O, Namespaces);
                    ms.Position = 0;
                    using (StreamReader sr = new StreamReader(ms, Encoding.Default))
                    {
                        Xml = sr.ReadToEnd();
                        sr.Dispose();
                    }
                }

                Xml = Xml.Substring(Xml.IndexOf("\n")+1);
            }
            catch { }

            return Xml;

        }

        public List<PropertyDocuData> GetPropertyDocuDataList()
        {
            List<PropertyDocuData> L = new List<PropertyDocuData>();
            foreach (PropertyInfo PI in Type.GetXMLSerializableProperties())
            {
                L.Add(new PropertyDocuData() { PropertyInfo = PI });
            }
            return L;
        }








        private object GetSampleReferenceType(Type RefType)
        {
            Type T=RefType;
            if (T.IsInterface)
            {
                DirectOutput.General.TypeList Types = new DirectOutput.General.TypeList(AppDomain.CurrentDomain.GetAssemblies().ToList().SelectMany(s => s.GetTypes()).Where(p => T.IsAssignableFrom(p) && !p.IsAbstract));
                if (Types.Count > 0)
                {
                    T = Types[0];
                }
                else
                {
                    return null;
                }
            }
            

            Object O = Activator.CreateInstance(T);


            foreach (PropertyInfo PI in O.GetType().GetXMLSerializableProperties())
            {
                if (PI.PropertyType == typeof(string))
                {
                    string V = (string)PI.GetValue(O, null);
                    if (V.IsNullOrWhiteSpace())
                    {
                        if (PI.Name.ToUpper() == "NAME")
                        {
                            PI.SetValue(O, "Name of {0}".Build(O.GetType().Name), null);
                        }
                        else if (PI.Name.ToUpper().EndsWith("NAME"))
                        {
                            PI.SetValue(O, "Name of {0}".Build(PI.Name.Substring(0, PI.Name.Length - 4)), null);
                        }
                        else
                        {
                            PI.SetValue(O, "{0} string".Build(PI.Name), null);
                        }
                    }
                }
                else if (PI.PropertyType.IsArray)
                {
                    object A = PI.GetValue(O,null);
                    if (A == null)
                    {
                        object[] SampleArray = GetSampleArray(PI.PropertyType);
                        PI.SetValue(O, SampleArray, null);
                    }
                }
                else if (PI.PropertyType.IsGenericList())
                {

                    IList EN = GetSampleGenericList(PI.PropertyType);
                    PI.SetValue(O, EN, null);
                }
                else if (typeof(IDictionary).IsAssignableFrom(PI.PropertyType) && PI.PropertyType.IsGenericType)
                {
                    IDictionary EN = GetSampleGenericDictionary(PI.PropertyType);
                    PI.SetValue(O, EN, null);
                }
                else if (PI.PropertyType.IsValueType)
                {
                    if (PI.PropertyType == typeof(char))
                    {
                        PI.SetValue(O, GetSampleValueType(PI.PropertyType), null);
                    }
                }
                else if (PI.PropertyType == typeof(DateTime))
                {
                    PI.SetValue(O, DateTime.Now, null);
                }
                else if (PI.PropertyType == typeof(TimeSpan))
                {
                    PI.SetValue(O, TimeSpan.Zero, null);
                }
                else if (!PI.PropertyType.IsValueType)
                {
                    PI.SetValue(O, GetSampleReferenceType(PI.PropertyType), null);
                }
                else
                {
                    throw new Exception("Unknow type {0}".Build(PI.PropertyType.Name));
                }

            }
            return O;

        }

        private object[] GetSampleArray(System.Type type)
        {

       

            object[] A  = (object[])Array.CreateInstance(typeof(object),3);

            Type GT = type.GetElementType();
            object G;
            for (int i = 0; i < 3; i++)
            {

                if (GT.IsValueType)
                {
                    G = GetSampleValueType(GT);
                }
                else
                {
                    G = GetSampleReferenceType(GT);
                }
                A[i]=G;
            }


            return A;
        }






        private IDictionary GetSampleGenericDictionary(System.Type T)
        {
            if (!(typeof(IDictionary).IsAssignableFrom(T) && T.IsGenericType)) return null;
            IDictionary C = (IDictionary)Activator.CreateInstance(T);
            if (T.GetGenericArguments().Length == 2)
            {
                Type KT = T.GetGenericArguments()[0];
                Type VT = T.GetGenericArguments()[1];
                object K;
                object V;
                for (int i = 0; i < 3; i++)
                {
                    if (KT == typeof(string))
                    {
                        K = "Key {0}".Build(i);
                    }
                    else if (KT.IsValueType)
                    {
                        K = GetSampleValueType(KT);
                        if (KT.IsNumber())
                        {
                            K = Convert.ChangeType(i, KT);
                        }
                    }
                    else
                    {
                        K = GetSampleReferenceType(KT);
                    }
                    if (VT == typeof(string))
                    {
                        V = "Value {0}".Build(i);
                    }
                    else if (KT.IsValueType)
                    {
                        V = GetSampleValueType(KT);
                        if (KT.IsNumber())
                        {
                            K = Convert.ChangeType(i, KT);
                        }
                    }
                    else
                    {
                        V = GetSampleReferenceType(KT);
                    }
                    C.Add(K, V);

                }
            }
            return C;
        }





        private IList GetSampleGenericList(Type T)
        {
            if (!(typeof(IList).IsAssignableFrom(T) )) return null;
            IList C = (IList)Activator.CreateInstance(T);

            //if (T.GetGenericArguments().Length == 1)
            {
                object G;
                Type GT = T.GetGetGenericCollectionTypeArguments()[0];

                for (int i = 0; i < 3; i++)
                {

                    if (GT.IsValueType)
                    {
                        G = GetSampleValueType(GT);
                    }
                    else
                    {
                        G = GetSampleReferenceType(GT);
                    }
                    C.Add(G);
                }
            }

            return C;
        }

        private object GetSampleValueType(Type T)
        {
            if (!T.IsValueType) return null;

            object O = Activator.CreateInstance(T);
            if (T == typeof(char))
            {
                O = '?';
            }

            return O;
        }


    }
}
