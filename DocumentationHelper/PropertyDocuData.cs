using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace DocumentationHelper
{
    public class PropertyDocuData
    {
        public PropertyInfo PropertyInfo;




        public string Summary
        {
            get
            {
                string N = "{0}.{1}.{2}".Build(PropertyInfo.DeclaringType.Namespace, PropertyInfo.DeclaringType.Name, PropertyInfo.Name);
                if (VSXmlDocu.PropertySummary.ContainsKey(N))
                {
                    return VSXmlDocu.PropertySummary[N];
                }
                return "";
            }
        }


        public string Value
        {
            get
            {
                string N = "{0}.{1}.{2}".Build(PropertyInfo.DeclaringType.Namespace, PropertyInfo.DeclaringType.Name, PropertyInfo.Name);
                if (VSXmlDocu.PropertyValue.ContainsKey(N))
                {
                    return VSXmlDocu.PropertyValue[N];
                }
                return "";
            }
        }

        public string GetDocu()
        {
            string S = "";

            S += "\\subsubsection {0}_{1}_{2} {2}\n\n".Build(PropertyInfo.ReflectedType.Namespace.Replace(".", "_"), PropertyInfo.ReflectedType.Name, Name);



            if (!Value.IsNullOrWhiteSpace() || !ValidValuesDescription.IsNullOrWhiteSpace())
            {
                if (!Value.IsNullOrWhiteSpace())
                {
                    S += Value;
                    S += "\n\n";
                };
                if (!ValidValuesDescription.IsNullOrWhiteSpace())
                {
                    S += ValidValuesDescription + "\n\n";
                }
            }
            else if (!Summary.IsNullOrWhiteSpace())
            {
                S += Summary;
                S += "\n\n";
            }

            //S += "__Value type__\n";
            //S += "The property {0} is of type _{1}_.\n".Build(Name, TypeName);
            //S += "This type is defined in namespace {0}.\n\n".Build(TypeNamespaceName);
            //S += "\n";


            string N = GetNestedPropertiesDocu(this);
            if (!N.IsNullOrWhiteSpace())
            {
                S += "__Nested Properties__\n\n";
                S += "The following nested propteries exist for {0}:\n".Build(Name);
                S += N;

                S += "\n";
            }

            if (!ValidValuesDescription.IsNullOrWhiteSpace())
            {
                S += "__Valid values__\n\n";
                S += ValidValuesDescription + "\n";

            }


            return S.ToString();

        }



        private string GetNestedPropertiesDocu(PropertyDocuData PDDP, int Level = 0)
        {
            string S = "";
            string Indent = new string(' ', Level * 2);
            List<PropertyDocuData> L = PDDP.ChildPropertyDocuDataList;
            if (L.Count > 0)
            {
                foreach (PropertyDocuData PDD in L)
                {
                    S += Indent + "* __{0}__<br/>".Build(PDD.Name);
                    if (!PDD.Value.IsNullOrWhiteSpace() || !PDD.ValidValuesDescription.IsNullOrWhiteSpace())
                    {
                        if (!PDD.Value.IsNullOrWhiteSpace())
                        {
                            S += Indent + "  "+PDD.Value;
                            S += "\n\n";
                        };
                        if (!PDD.ValidValuesDescription.IsNullOrWhiteSpace())
                        {
                            S += Indent + "  " + PDD.ValidValuesDescription + "\n\n";
                        }
                    }
                    else if (!PDD.Summary.IsNullOrWhiteSpace())
                    {
                        S += Indent + "  " + PDD.Summary;
                        S += "\n\n";
                    }
                    string N = GetNestedPropertiesDocu(PDD, Level + 1);
                    if (!N.IsNullOrWhiteSpace())
                    {
                        S += Indent + "  This property has the following childs:\n";
                        S += N;
                    }

                }
            }
            return S;
        }


        public string Name
        {
            get
            {
                return PropertyInfo.Name;
            }
        }

        public string TypeName
        {
            get
            {
                return PropertyInfo.PropertyType.Name;
            }
        }

        public string NamespaceName
        {
            get
            {
                return PropertyInfo.PropertyType.Namespace;
            }
        }

        public string ValidValuesDescription
        {
            get
            {
                string S = "";
                if (PropertyInfo.PropertyType.IsEnum)
                {
                    S = "The property {0} accepts the following values:\n\n".Build(Name);
                    S += string.Join("\n* ", Enum.GetNames(PropertyInfo.PropertyType));


                }

                return S;
            }
        }

        public List<PropertyDocuData> ChildPropertyDocuDataList
        {
            get
            {
                List<PropertyDocuData> L = new List<PropertyDocuData>();
                if (!PropertyInfo.PropertyType.IsValueType && PropertyInfo.PropertyType.Namespace.ToUpper() != "SYSTEM")
                {
                    if (PropertyInfo.PropertyType.IsGenericList())
                    {
                        Type ItemType = PropertyInfo.PropertyType.GetGetGenericCollectionTypeArguments()[0];
                        foreach (PropertyInfo PI in ItemType.GetXMLSerializableProperties())
                        {
                            L.Add(new PropertyDocuData() { PropertyInfo = PI });
                        }

                    }
                    else if (PropertyInfo.PropertyType.IsGenericDictionary())
                    {
                        throw new NotImplementedException();
                    }
                    else
                    {
                        foreach (PropertyInfo PI in PropertyInfo.PropertyType.GetXMLSerializableProperties())
                        {
                            L.Add(new PropertyDocuData() { PropertyInfo = PI });
                        }
                    }

                }
                return L;
            }

        }

    }
}
