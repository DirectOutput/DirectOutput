using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

public static class ObjectExtensions
{
    /// <summary>
    /// Serializes to object to a XML string
    /// </summary>
    /// <typeparam name="T">Type of the object</typeparam>
    /// <param name="ObjectToSerialize">The object to serialize.</param>
    /// <returns>String containg the XML serialized data of the object.</returns>
    public static string XmlSerialize<T>(this T ObjectToSerialize)
    {
        Type RealType = ObjectToSerialize.GetType();

        string Xml = "";
        using (MemoryStream ms = new MemoryStream())
        {

            XmlSerializerNamespaces Namespaces = new XmlSerializerNamespaces();
            Namespaces.Add(string.Empty, string.Empty);

            new XmlSerializer(RealType).Serialize(ms, ObjectToSerialize,Namespaces);
            ms.Position = 0;
            using (StreamReader sr = new StreamReader(ms, Encoding.Default))
            {
                Xml = sr.ReadToEnd();
                sr.Dispose();
            }
        }

        return Xml;

    } 




}

