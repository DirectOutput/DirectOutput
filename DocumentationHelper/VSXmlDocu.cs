using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DocumentationHelper
{
    public static class VSXmlDocu
    {
        public static Dictionary<string, string> TypeSummary = new Dictionary<string, string>();
        public static Dictionary<string, string> PropertySummary = new Dictionary<string, string>();
        public static Dictionary<string, string> PropertyValue = new Dictionary<string, string>();


         static VSXmlDocu()
        {
            Load();
        }


         private static string Unindent(string S)
         {
             if (S.StartsWith("\r\n"))
             {
                 S = S.Substring(2);
             }
             int L = 0;
             for (int i = 0; i < S.Length; i++)
             {
                 if (S.Substring(i, 1) != " ")
                 {
                     L = i;
                     break;
                 }

             }
             if (L > 0)
             {
                 string Spaces = new string(' ', L);

                 string X = "";
                 string Y = "";
                 foreach (string A in S.Split('\n'))
                 {
                     X = A.Replace("\r", "");
                     if (X.StartsWith(Spaces))
                     {
                         X = X.Substring(L);
                     }
                     X = X.TrimEnd();
                     X = X + "\n";
                     Y += X;
                 }
                 return Y;
             }
             return S;


         }

        public static void Load()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"DirectOutput.XML");


            XmlNode docNode = null;
            foreach (XmlNode N in doc.ChildNodes)
            {
                docNode = N;
                if (N.Name == "doc") break;
            }

            if (docNode == null) return;
            XmlNode membersNode = null;
            foreach (XmlNode N in docNode.ChildNodes)
            {
                membersNode = N;
                if (N.Name == "members") break;
            }

            if (membersNode == null) return;

            foreach (XmlNode M in membersNode)
            {
                if (M.Attributes != null)
                {
                    string Name = "";

                    foreach (XmlAttribute A in M.Attributes)
                    {
                        if (A.Name == "name")
                        {
                            Name = A.Value;
                            break;
                        }
                    }

                    if (Name.StartsWith("T:"))
                    {
                        //Its a type
                        Name = Name.Substring(2);

                        foreach (XmlNode TChild in M.ChildNodes)
                        {
                            switch (TChild.Name)
                            {
                                case "summary":
                                    TypeSummary.Add(Name, Unindent(TChild.InnerXml));
                                    break;
                                case "remarks":
                                case "param":
                                    break;
                                default:
                                    break;
                            }


                        }


                    }
                    else if (Name.StartsWith("P:"))
                    {
                        //Property
                        Name = Name.Substring(2);

                        foreach (XmlNode TChild in M.ChildNodes)
                        {
                            switch (TChild.Name)
                            {
                                case "summary":
                                    PropertySummary.Add(Name, Unindent(TChild.InnerXml));
                                    break;
                                case "value":
                                    PropertyValue.Add(Name, Unindent(TChild.InnerXml));
                                    break;
                                case "param":
                                   // PropertyParam.Add(Name, TChild.InnerXml.Replace("\r\n", ""));
                                    break;
                                case "returns":
                                case "exception":
                                case "#text":
                                case "note":
                                    break;
                                default:
                                    break;
                            }


                        }
                    }

                }




            }




        }
    }
}
