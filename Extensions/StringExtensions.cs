using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using System.IO;
using System.Globalization;
using System.Xml.Serialization;
using System.Xml;
//using System.Web;


/// <summary>
/// Extends the String object with additional functionality.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Removes the specified number of characters from string
    /// </summary>
    /// <param name="Input">The input.</param>
    /// <param name="NumberOfCharsToRemove">The number of chars to remove.</param>
    /// <returns></returns>
    public static string RemoveLastChars(this string Input, int NumberOfCharsToRemove)
    {
        if (NumberOfCharsToRemove <= 0) return Input;
        return (Input.Length > NumberOfCharsToRemove ? Input.Substring(0, Input.Length - NumberOfCharsToRemove) : "");
    }


    /// <summary>
    /// Removes the specified end of a string.
    /// </summary>
    /// <param name="Input">The input.</param>
    /// <param name="StringEndToRemove">The string end to remove.</param>
    /// <param name="ComparisonType">Type of the comparison.</param>
    /// <returns></returns>
    public static string RemoveSpecifiedEnd(this string Input, string StringEndToRemove, StringComparison ComparisonType = StringComparison.CurrentCulture)
    {
        return Input.EndsWith(StringEndToRemove, ComparisonType) ? Input.RemoveLastChars(StringEndToRemove.Length) : Input;
    }



    /// <summary>
    /// Removes consecutive whitespaces from the string.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <returns></returns>
    public static string RemoveRemoveConsecutiveWhiteSpaces(this string Input)
    {
        return Regex.Replace(Input, @"\s+", " ");
    }

    /// <summary>
    /// Removes all whitespaces from the string.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <returns></returns>
    public static string RemoveWhitespaces(this string input)
    {
        return new string(input.ToCharArray()
            .Where(c => !Char.IsWhiteSpace(c))
            .ToArray());
    }

    /// <summary>
    /// Converts a Hexnumber to int
    /// </summary>
    /// <returns>Int of given Hexnumber</returns>
    public static int HexToInt(this string s)
    {
        return int.Parse(s, System.Globalization.NumberStyles.HexNumber);
    }
    /// <summary>
    /// Converts a Hexnumber to byte
    /// </summary>
    /// <returns>Byte of given Hexnumber</returns>
    public static int HexToByte(this string s)
    {
        return byte.Parse(s, System.Globalization.NumberStyles.HexNumber);
    }
    /// <summary>
    /// Determines whether the string is a hex number.
    /// </summary>
    /// <returns>true if the string contains only hexchars, otherwise true</returns>
    public static bool IsHexString(this string s)
    {
        return s.IsHexString(0, s.Length);
    }

    /// <summary>
    /// Determines whether the specified part of the string is a hex number.
    /// </summary>
    /// <param name="startindex">The startindex of the substring to check.</param>
    /// <returns>
    ///   <c>true</c> if the specified part of the string is a hex number, otherwise <c>false</c>.
    /// </returns>
    public static bool IsHexString(this string s, int startindex)
    {
        return s.IsHexString(startindex, s.Length - startindex);
    }

    /// <summary>
    /// Determines whether the specified part of the string is a hex number.
    /// </summary>
    /// <param name="startindex">The startindex of the substring to check.</param>
    /// <param name="length">The length of the substrng to check.</param>
    /// <returns>
    ///   <c>true</c> if the specified part of the string is a hex number, otherwise <c>false</c>.
    /// </returns>
    public static bool IsHexString(this string s, int startindex, int length)
    {
        if (string.IsNullOrWhiteSpace(s)) return false;

        if (startindex + length > s.Length) return false;

        return System.Text.RegularExpressions.Regex.IsMatch(s.Substring(startindex, length), @"\A\b[0-9a-fA-F]+\b\Z");
    }




    /// <summary>
    /// Converts the string to a byte array.
    /// </summary>
    /// <returns>Return a UTF8 encoded byte array with the contents of the string</returns>
    public static byte[] ToByteArray(this string s)
    {
        System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
        return encoding.GetBytes(s);
    }

    /// <summary>
    /// Converts the string to a StringBuilder object.
    /// </summary>
    /// <returns>StringBuilder object containing the contents of the string.</returns>
    public static StringBuilder ToStringBuilder(this string s)
    {
        if (string.IsNullOrEmpty(s))
        {
            return new StringBuilder("");
        }
        return new StringBuilder(s);
    }

    /// <summary>
    /// Returns a number of characters from the left-hand side of this instance. 
    /// </summary>
    /// <param name="length">The number of characters to return.</param>
    /// <returns>Returns a string containing the leftmost <paramref name="length"/> characters of the string.</returns>
    public static string Left(this string s, int length)
    {
        return s.Substring(0, length);
    }

    /// <summary>
    /// Returns a number of characters from the right-hand side of this instance. 
    /// </summary>
    /// <param name="length">The number of characters to return.</param>
    /// <returns>Returns a string containing the rightmost <paramref name="length"/> characters of the string.</returns>
    public static string Right(this string s, int length)
    {
        return s.Substring(s.Length - length, length);
    }

    /// <summary>
    /// Retrieves a substring from this instance. The substring starts at a specified character position and has a specified length. 
    /// <para>The behaviour of this method is the same as substring.</para>
    /// </summary>
    /// <param name="startIndex">The zero-based starting character position of a substring in this instance.</param>
    /// <param name="length">The number of characters in the substring. </param>
    /// <returns>A string that is equivalent to the substring of <paramref name="length"/> that begins at <paramref name="startIndex"/> in this instance, or Empty if <paramref name="startIndex"/> is equal to the length of this instance </returns>
    public static string Mid(this string s, int startIndex, int length)
    {
        return s.Substring(startIndex, length);
    }

    /// <summary>
    /// Converts the string to int
    /// </summary>
    /// <returns>The int value of the string.If the string can not be converted to int, the return value will be 0.</returns>
    public static int ToInteger(this string s)
    {
        int integerValue = 0;
        int.TryParse(s, out integerValue);
        return integerValue;
    }

    /// <summary>
    /// Converts the string to uint
    /// </summary>
    /// <returns>The uint value of the string. <para>If the string can not be converted to uint, the return value will be 0.</para></returns>
    public static uint ToUInt(this string s)
    {
        uint uintegerValue = 0;
        uint.TryParse(s, out uintegerValue);
        return uintegerValue;
    }

    /// <summary>
    /// Indicates whether the string contains a integer value
    /// </summary>
    /// <returns>true if the string can be converted to int, otherwise false.</returns>
    public static bool IsInteger(this string s)
    {
        int dummy;
        if (s.IsNullOrWhiteSpace()) return false;
        return int.TryParse(s, out dummy);
    }

    /// <summary>
    /// Indicates whether the string contains a unsigned integer value
    /// </summary>
    /// <returns>true if the string can be converted to uint, otherwise false.</returns>
    public static bool IsUInt(this string s)
    {
        uint dummy;
        if (s.IsNullOrWhiteSpace()) return false;
        return uint.TryParse(s, out dummy);
    }

    /// <summary>
    /// Indicates whether the string is Nothing or an Empty string.
    /// </summary>
    /// <returns>true if the string is Nothing or an empty string (""); otherwise, false.</returns>
    public static bool IsNullOrEmpty(this string s)
    {
        return string.IsNullOrEmpty(s);
    }

    /// <summary>
    /// Indicates whether the string is Nothing, empty, or consists only of white-space characters.
    /// </summary>
    /// <returns>true if the string is Nothing or String.Empty, or if value consists exclusively of white-space characters. </returns>
    public static bool IsNullOrWhiteSpace(this string s)
    {
        return string.IsNullOrWhiteSpace(s);
    }



    /// <summary>
    /// Replaces one or more format items in the string with the string representation of a specified object.
    /// </summary>
    /// <param name="arg0">The object to format.</param>
    /// <returns>A copy the string in which any format items are replaced by the string representation of <paramref name="arg0"/>.</returns>
    public static string Build(this string s, object arg0)
    {
        if (s == null) { return ""; }
        return string.Format(s, arg0);
    }

    /// <summary>
    /// Replaces the format items in the string with the string representation of two specified objects.
    /// </summary>
    /// <param name="arg0">The first object to format. </param>
    /// <param name="arg1">The second object to format. </param>
    /// <returns>A copy of the string in which format items are replaced by the string representations of <paramref name="arg0"/> and <paramref name="arg1"/>.</returns>
    public static string Build(this string s, object arg0, object arg1)
    {
        if (s == null) { return ""; }
        return string.Format(s, arg0, arg1);
    }

    /// <summary>
    /// Replaces the format items in the string with the string representation of three specified objects.
    /// </summary>
    /// <param name="arg0">The first object to format. </param>
    /// <param name="arg1">The second object to format. </param>
    /// <param name="arg2">The third object to format. </param>
    /// <returns>A copy of the string in which the format items have been replaced by the string representations of  <paramref name="arg0"/>, <paramref name="arg1"/>, and <paramref name="arg2"/>.</returns>
    public static string Build(this string s, object arg0, object arg1, object arg2)
    {
        if (s == null) { return ""; }
        return string.Format(s, arg0, arg1, arg2);
    }

    /// <summary>
    /// Replaces the format item in the string with the string representation of a corresponding object in a specified array.
    /// </summary>
    /// <param name="args">An object array that contains zero or more objects to format. </param>
    /// <returns>A copy of the string in which the format items have been replaced by the string representation of the corresponding objects in <paramref name="args"/>.</returns>
    //public static string Build(this string s, object[] args)
    //{
    //    if (s == null) { return ""; }
    //    return string.Format(s, args);
    //}

    /// <summary>
    /// Replaces the format item in the string with the string representation of a corresponding object in the list of paramaters.
    /// </summary>
    /// <param name="args">A list of parameter objects. </param>
    /// <returns>A copy of the string in which the format items have been replaced by the string representation of the corresponding objects in <paramref name="args"/>.</returns>
    public static string Build(this string s, params object[] args)
    {
        if (s == null) { return ""; }
        return string.Format(s, args);
    }

    /// <summary>
    /// Replaces the named format items (e.g. {ItemName}) with the corresponding values from the ConstructionValueDictionary.
    /// </summary>
    /// <param name="s">The s.</param>
    /// <param name="ConstructionValueDictionary">The construction value dictionary containing the values to be inserted.</param>
    /// <param name="IgnoreMissingConstructionValues">If set to <c>true</c> missing construction values will be ignored/not replaced. If set to <c>false</c> a exception ist thrown if a construcation value is missing.</param>
    /// <returns>A copy of the string with replaced format items.</returns>
    /// <exception cref="System.ArgumentException">The ConstructionValueDictionary does not contain a value for '{0}'</exception>
    public static string Construct(this string s, Dictionary<string, object> ConstructionValueDictionary, bool ReplaceNullValuesWithEmptyString = true, bool IgnoreMissingConstructionValues = false)
    {
        if (s.IsNullOrWhiteSpace()) return s;

        List<string> L = s.GetConstructionItemsList().Select(S => S.Substring(1, S.Length - 2)).ToList();
        foreach (string N in L)
        {
            string V = "";
            if (ConstructionValueDictionary.ContainsKey(N))
            {
                object O = ConstructionValueDictionary[N];
                if (O == null && ReplaceNullValuesWithEmptyString)
                {
                    V = "";
                }
                else
                {
                    V = O.ToString();
                }
                s = s.Replace("{{{0}}}".Build(N), V);
            }
            else if (!IgnoreMissingConstructionValues)
            {
                throw new ArgumentException("The ConstructionValueDictionary does not contain a value for '{0}'".Build(N), "ConstructionValueDictionary");
            }
        }
        return s;
    }

    /// <summary>
    /// Gets the list of named format items (e.g. {ItemName}) in the string
    /// </summary>
    /// <param name="s">The s.</param>
    /// <returns>List of named format items.</returns>
    public static List<string> GetConstructionItemsList(this string s)
    {
        List<string> L = new List<string>();
        if (!s.IsNullOrWhiteSpace())
        {
            Regex regex = new Regex("{.*?}");
            MatchCollection matches = regex.Matches(s);
            foreach (object match in matches)
            {
                string N = match.ToString();

                if (!L.Contains(N))
                {
                    L.Add(N);
                }

            }
        }
        return L;

    }

    /// <summary>
    /// Gets a dictionary containing entries for all named format items (e.g. {NamedItem}) in the string.
    /// The values of all dictionary entries is null.
    /// </summary>
    /// <param name="s">The s.</param>
    /// <returns>Dictionary containing entries for all named format items (e.g. {NamedItem}) in the string</returns>
    public static Dictionary<string, object> GetConstructionItemsDictionary(this string s)
    {
        Dictionary<string, object> D = new Dictionary<string, object>();
        List<string> L = s.GetConstructionItemsList();
        L.ForEach(N => D.Add(N, null));
        L = null;
        return D;

    }



    /// <summary>
    /// Indicates if the string is a valid email address.
    /// <para>Only the structure of the string is checked. No checks on existance of the domain or email address are performed.</para>
    /// </summary>
    /// <returns>true if the string is a valid email address, otherwise false.</returns>
    public static bool IsEmail(this string s)
    {
        Regex MailCheck = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
        return MailCheck.IsMatch(s);
    }

    /// <summary>
    /// Writes the string to a file
    /// </summary>
    /// <param name="FileName">The complete file path to write to.<para>
    /// If the file does not exist a new file will be created. If the file exists, the file will be overwritten.</para> </param>
    public static void WriteToFile(this string s, string FileName)
    {
        s.WriteToFile(FileName, false);
    }

    /// <summary>
    /// Writes the string to a file
    /// </summary>
    /// <param name="FileName">The complete file path to write to. </param>
    /// <param name="Append">Determines whether data is to be appended to the file. If the file exists and append is false, the file is overwritten. If the file exists and append is true, the data is appended to the file. Otherwise, a new file is created. </param>
    public static void WriteToFile(this string s, string FileName, bool Append)
    {
        string path = Path.GetFullPath(FileName);
        TextWriter tw = null;
        try
        {
            tw = new StreamWriter(FileName, Append);
            tw.Write(s);
        }
        catch (Exception e)
        {

            if (tw != null)
            {
                tw.Close();
            }
            throw e;
        }

        tw.Close();

        tw.Dispose();


    }

    /// <summary>
    /// Writes the string to a file
    /// </summary>
    /// <param name="File">The fileinfo object for the target file.</param>
    public static void WriteToFile(this string s, FileInfo File)
    {
        s.WriteToFile(File.FullName, false);
    }

    /// <summary>
    /// Writes the string to a file
    /// </summary>
    /// <param name="s">The s.</param>
    /// <param name="File">The fileinfo object for the target file.</param>
    /// <param name="Append">Determines whether data is to be appended to the file. If the file exists and append is false, the file is overwritten. If the file exists and append is true, the data is appended to the file. Otherwise, a new file is created. </param>
    public static void WriteToFile(this string s, FileInfo File, bool Append)
    {
        s.WriteToFile(File.FullName, Append);
    }


    /// <summary>
    /// Replaces all occurences of a string with another string.
    /// </summary>
    /// <param name="originalString">The original string.</param>
    /// <param name="oldValue">The old value.</param>
    /// <param name="newValue">The new value.</param>
    /// <param name="comparisonType">Type of the comparison.</param>
    /// <returns>String with replaced values.</returns>
    static public string Replace(this string originalString, string oldValue, string newValue, StringComparison comparisonType)
    {
        StringBuilder sb = new StringBuilder();

        int previousIndex = 0;
        int index = originalString.IndexOf(oldValue, comparisonType);
        while (index != -1)
        {
            sb.Append(originalString.Substring(previousIndex, index - previousIndex));
            sb.Append(newValue);
            index += oldValue.Length;

            previousIndex = index;
            index = originalString.IndexOf(oldValue, index, comparisonType);
        }
        sb.Append(originalString.Substring(previousIndex));

        return sb.ToString();

    }


    /// <summary>
    /// Determines whether the string contains only alphanmumeric chars.
    /// </summary>
    /// <returns>
    ///   <c>true</c> if the string is alphanumeric, otherwise <c>false</c>.
    /// </returns>
    public static bool IsAlphaNumeric(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return false;

        return !str.Any(C => !char.IsLetterOrDigit(C));

    }

    /// <summary>
    /// Removes the diacritics.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns>String without diacritics.</returns>
    public static string RemoveDiacritics(this string text)
    {
        string normalizedString = text.Normalize(NormalizationForm.FormD);
        StringBuilder stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }

    /// <summary>
    /// Return the specified alternative text, when the string is null or whitespace.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="Alternative">The alternative text.</param>
    /// <returns></returns>
    public static string AlternativeWhenNullOrWhiteSpace(this string text, string Alternative)
    {
        if (text.IsNullOrWhiteSpace()) return Alternative;
        return text;
    }


    /// <summary>
    /// Deserializes the xml content of the string as a object of the given type.
    /// </summary>
    /// <typeparam name="T">Type of the object</typeparam>
    /// <param name="Xml">The XML.</param>
    /// <returns></returns>
    public static T XMLDeserialize<T>(this string Xml)
    {
        byte[] xmlBytes = Encoding.Default.GetBytes(Xml);
        using (MemoryStream ms = new MemoryStream(xmlBytes))
        {
            try
            {
                return (T)new XmlSerializer(typeof(T)).Deserialize(ms);
            }
            catch (Exception E)
            {

                Exception Ex = new Exception("Could not deserialize {0} from XML data.".Build(typeof(T).Name), E);
                Ex.Data.Add("XML Data", Xml);
                throw Ex;
            }
        }



    }


    /// <summary>
    /// Deserializes the xml content of the string as a object of the given type.
    /// </summary>
    /// <typeparam name="T">Type of the object</typeparam>
    /// <param name="Xml">The XML.</param>
    /// <returns></returns>
    public static object XMLDeserialize(this string Xml, Type Type)
    {
        byte[] xmlBytes = Encoding.Default.GetBytes(Xml);
        using (MemoryStream ms = new MemoryStream(xmlBytes))
        {
            try
            {
                return new XmlSerializer(Type).Deserialize(ms);
            }
            catch (Exception E)
            {

                Exception Ex = new Exception("Could not deserialize {0} from XML data.".Build(Type.Name), E);
                Ex.Data.Add("XML Data", Xml);
                throw Ex;
            }
        }



    }

    /// <summary>
    /// Deserializes the xml content of the string as a object implementing the specified interface.
    /// </summary>
    /// <typeparam name="T">Type of the interface</typeparam>
    /// <param name="Xml">The XML.</param>
    /// <returns></returns>
    public static T XMLDeserializeInterface<T>(this string Xml)
    {
        byte[] xmlBytes = Encoding.Default.GetBytes(Xml);
        using (MemoryStream ms = new MemoryStream(xmlBytes))
        {
            using (XmlReader r = XmlReader.Create(ms))
            {
                List<Type> Types = new List<Type>(AppDomain.CurrentDomain.GetAssemblies().ToList().SelectMany(s => s.GetTypes()).Where(p => typeof(T).IsAssignableFrom(p) && !p.IsAbstract));

                if (Types.Any(Ty => Ty.FullName == r.LocalName))
                {
                    throw new Exception("The type {0} specified in the xml cant be assigned to {1}".Build(r.LocalName, typeof(T).FullName));
                }
                Type TargetType = Types.First(Ty => Ty.FullName == r.LocalName);
                try
                {
                    return (T)new XmlSerializer(TargetType).Deserialize(r);
                }
                catch (Exception E)
                {

                    Exception Ex = new Exception("Could not deserialize {0} from XML data.".Build(typeof(T).Name), E);
                    Ex.Data.Add("XML Data", Xml);
                    throw Ex;
                }
            }
        }


    }





    /// <summary>
    /// Determines whether the string is a valid path. The method does not check if the path exists or if the path is relative
    /// </summary>
    /// <param name="PathToCheck">The path to check.</param>
    /// <returns>
    ///   <c>true</c> if the path is valid; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsValidPath(this string PathToCheck)
    {
        try
        {
            Path.GetFullPath(PathToCheck);
        }
        catch
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Determines whether the string contains a relative path. The method does not check if the path exists.
    /// </summary>
    /// <param name="PathToCheck">The path to check.</param>
    /// <returns>
    ///   <c>true</c> if string contains a relative path; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsRelativePath(this string PathToCheck)
    {
        if (IsValidPath(PathToCheck))
        {
            if (!Path.IsPathRooted(PathToCheck))
            {
                return true;
            }
        }
        return false;
    }


    /// <summary>
    /// Converts the string to a DateTime
    /// </summary>
    /// <param name="DateString">The date string.</param>
    /// <param name="DateFormat">The date format.</param>
    /// <returns></returns>
    public static DateTime ToDateTime(this string DateString, string DateFormat)
    {
        return DateTime.ParseExact(DateString, DateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
    }


    /// <summary>
    /// Converts the string to a DateTime
    /// </summary>
    /// <param name="DateString">The date string.</param>
    /// <param name="DateFormats">Array of date formats.</param>
    /// <returns></returns>
    public static DateTime ToDateTime(this string DateString, string[] DateFormats)
    {
        return DateTime.ParseExact(DateString, DateFormats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
    }

    /// <summary>
    /// Determines whether the specified date string is a datetime.
    /// </summary>
    /// <param name="DateString">The date string.</param>
    /// <param name="DateFormats">Array of date formats.</param>
    /// <returns>
    ///   <c>true</c> if the specified date string is a date; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsDateTime(this string DateString, string[] DateFormats)
    {
        if (DateString == null) return false;
        DateTime Dummy;
        return (DateTime.TryParseExact(DateString, DateFormats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out Dummy));
    }

    /// <summary>
    /// Determines whether the specified date string is datetime.
    /// </summary>
    /// <param name="DateString">The date string.</param>
    /// <param name="DateFormat">The date format.</param>
    /// <returns>
    ///   <c>true</c> if the specified date string is a date; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsDateTime(this string DateString, string DateFormat)
    {
        if (DateString == null) return false;
        DateTime Dummy;
        return (DateTime.TryParseExact(DateString, DateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out Dummy));
    }


    /// <summary>
    /// Indents all lines in the string by prefixing them with the specified IndentionString
    /// </summary>
    /// <param name="StringToIndent">The string to indent.</param>
    /// <param name="IndentionString">The string to insert in front of every line of the string.</param>
    /// <returns>String where all lines are indented by prefixing them with the spcified IndentionString</returns>
    public static string IndentString(this string StringToIndent, string IndentionString)
    {
        if (StringToIndent == null) { return null; }
        string[] Parts = StringToIndent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
        return IndentionString + string.Join(Environment.NewLine + IndentionString, Parts);
    }

    /// <summary>
    /// Gets the bytes in a string.
    /// </summary>
    /// <returns>Array of bytes of the string.</returns>
    static public byte[] GetBytes(this string str)
    {
        byte[] bytes = new byte[str.Length * sizeof(char)];
        System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
        return bytes;
    }
}






