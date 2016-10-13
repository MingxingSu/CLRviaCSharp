using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Strings
{

    internal class Program
    {
        private static void Main(string[] args)
        {
            //String is reference type, immutable, stored in heap

            //String s = new String("Hi there."); // <-- Error,因为在CLR中是new对应newobj方法
            String s = new String("Hi there.".ToCharArray()); //这样是可以的

            Console.WriteLine(s);

            s = "hello\r\nworld\t!"; //对应CLR中的ldstr,不推荐这种换行方式，因为在Unix中,换行是\n
            Console.WriteLine(s);

            s = "hello" + Environment.NewLine + "new line!";
                //推荐, +号会在编译时就被去掉，变成一个合成的字符串，运行时动态编辑字符串要用stringbuilder,否则会容易引起gc collect
            Console.WriteLine(s);

            //magic @
            String file = @"C:\Windows\System32\Notepad.exe";
            Console.WriteLine(file);

            //immutable benefits:1.perform operations without changing it 
            //  2. No thread sync issue, share among  
            //  3. Saving memory, string interning.

            //如果需要展示给最终用户，尽量不要用StringComparison.InvariantCulture, StringComparison.InvariantCultureIgnoreCase
            //StringComparison.Ordinal速度最快
            //尽量使用s.ToUpperInvariant();,而不是s.ToLowerInvariant(),因为MS有优化，不要使用ToUpper,ToLower
            //string.CompareOrdinal(),s.CompareTo(), ==, !=尽量不要用

            //推荐使用一下几个方法比较字符串：
            //s.Equals()
            //String.Compare()
            //s.StartsWith()
            //s.EndsWith()

            //Lanauage
            Console.WriteLine("Current culture:" + Thread.CurrentThread.CurrentCulture);
                //display UI elements,lables, buttons,
            //Locale
            Console.WriteLine("Current UI culture:" + Thread.CurrentThread.CurrentUICulture); //Numbers, formats,

            String s1 = "Strasse";
            String s2 = "Straße";
            bool equal = String.Compare(s1, s2, StringComparison.Ordinal) == 0;
            Console.WriteLine("Ordinal comparison: '{0}' {2} '{1}'", s1, s2, equal ? "==" : "!=");

            CultureInfo ci = new CultureInfo("de-DE");
            ci = new CultureInfo("zh-CN");
            bool equalCulture = String.Compare(s1, s2, true, ci) == 0;
                //char expansion, no matter what culture pass into, ß always expaned to 'SS'
            Console.WriteLine("Ordinal comparison: '{0}' {2} '{1}'", s1, s2, equalCulture ? "==" : "!=");

            //如果需要更精细的掌握比较的过程，可以自己调用CultureInfo.CompareInfo.Compare
            CompareOptions customOptions = CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace;
            bool compareResult = ci.CompareInfo.Compare(s1, 0, s2, 0, customOptions) == 0;
            Console.WriteLine("Comparison with custom compare options: '{0}' {2} '{1}'", s1, s2,
                compareResult ? "==" : "!=");

            #region compare demos

            String output = String.Empty;
            String[] symbol = new String[] {"<", "=", ">"};
            Int32 x;
            // The code below demonstrates how strings compare
            // differently for different cultures.
            s1 = "coté";
            s2 = "côte";
            // Sorting strings for French in France.
            ci = new CultureInfo("fr-FR");
            x = Math.Sign(ci.CompareInfo.Compare(s1, s2));
            output += String.Format("{0} Compare: {1} {3} {2}",
                ci.Name, s1, s2, symbol[x + 1]);
            output += Environment.NewLine;
            // Sorting strings for Japanese in Japan.
            ci = new CultureInfo("ja-JP");
            x = Math.Sign(ci.CompareInfo.Compare(s1, s2));
            output += String.Format("{0} Compare: {1} {3} {2}",
                ci.Name, s1, s2, symbol[x + 1]);
            output += Environment.NewLine;
            // Sorting strings for the thread's culture
            ci = Thread.CurrentThread.CurrentCulture;
            x = Math.Sign(ci.CompareInfo.Compare(s1, s2));
            output += String.Format("{0} Compare: {1} {3} {2}",
                ci.Name, s1, s2, symbol[x + 1]);
            output += Environment.NewLine + Environment.NewLine;

            // The code below demonstrates how to use CompareInfo.Compare's
            // advanced options with 2 Japanese strings. One string represents
            // the word "shinkansen" (the name for the Japanese high-speed
            // train) in hiragana (one subtype of Japanese writing), and the
            // other represents the same word in katakana (another subtype of
            // Japanese writing).
            s1 = ""; // ("\u3057\u3093\u304B\u3093\u305b\u3093")
            s2 = ""; // ("\u30b7\u30f3\u30ab\u30f3\u30bb\u30f3")
            // Here is the result of a default comparison
            ci = new CultureInfo("ja-JP");
            x = Math.Sign(String.Compare(s1, s2, true, ci));
            output += String.Format("Simple {0} Compare: {1} {3} {2}",
                ci.Name, s1, s2, symbol[x + 1]);
            output += Environment.NewLine;

            // Here is the result of a comparison that ignores
            // kana type (a type of Japanese writing)
            CompareInfo compareInfo = CompareInfo.GetCompareInfo("ja-JP");
            x = Math.Sign(compareInfo.Compare(s1, s2, CompareOptions.IgnoreKanaType));
            output += String.Format("Advanced {0} Compare: {1} {3} {2}",
                ci.Name, s1, s2, symbol[x + 1]);
            //MessageBox.Show(output, "Comparing Strings For Sorting");


            #endregion

            #region String Intern

            s1 = "Hello";
            s2 = "Hello";
            Console.WriteLine(Object.ReferenceEquals(s1, s2));
                // Should be 'False', but return 'True' on .NET 4.5, because it was implicitly interned, but no interned when use NGen.exe, so this is not reliable feature

            //explicitly intern
            s1 = String.Intern(s1);
            s2 = String.Intern(s2);
            Console.WriteLine(Object.ReferenceEquals(s1, s2)); // 'True'


            #endregion

            //String Pooling:when compiling, emit multiple duplicated string as only 1 instance to the meta data to reduce size.

            //StringInfo class
            string str = "甴曱"; //每个字占两个Char,high surrogate and low surrogate
            StringInfo siMyName = new StringInfo(str);
            Console.WriteLine("{0} 's Length is :{1}", str, str.Length);
            Console.WriteLine("{0} 's LengthInTextElements is :{1}", str, siMyName.LengthInTextElements);
            var wordsValues = StringInfo.ParseCombiningCharacters(str);
            Int32[] textElemIndex = StringInfo.ParseCombiningCharacters(str);
            TextElementEnumerator charEnum = StringInfo.GetTextElementEnumerator(s);

            //StringBuilder:内部用Char Array实现， Capacity default:16, if no enough, will double it, and copy to new array
            StringBuilder sb = new StringBuilder();
            Console.WriteLine("String Builder's Max Capcity  is :{0}", sb.MaxCapacity);
            Console.WriteLine("String Builder's Default Capcity  is :{0}", sb.Capacity);
            sb = new StringBuilder(66);
            Console.WriteLine("String Builder's Custom Capcity  is :{0}", sb.Capacity);
            Console.WriteLine("String Builder's EnsureCapcity  is :{0}", sb.EnsureCapacity(100));

            sb = new StringBuilder("hello,max");
            foreach (char c in sb.ToString())
            {
                Console.Write(c + " ");

            }
            Console.WriteLine();

            s = sb.AppendFormat("{0} {1}", " Jeffrey", "Richter").Replace(' ', '-').Remove(4, 3).ToString();
            Console.WriteLine(s); // "Jeff-Richter"


            //Datetime support below format string:
            /*  d:short date
             *  D:long date
             *  M:month
             *  Y:year
             *  G:general
             *  T:long time
             *  s: sortable
             *  u:universal time ISO8601
             *  U:universal time
             *
             * Enum:
             *  G:general
             *  D:decimal
             *  X:hex
             *  F:flags
             *  
             * Numerics:
             *  C:currency
             *  D:decimal
             *  E:exponential notation
             *  F: fixed-point
             *  G:general
             *  N:Number
             *  P:percent
             *  R:round-trip
             *  X:hex
             * 
             * If Format string is null, the default will be General format
             * If IFormateProvider is null, the current thread's current culture will be used
             *            
             *
             */
            decimal price = 123.5M;
            Console.WriteLine(price.ToString("C", new CultureInfo("zh-CN"))); //￥123.50
            Console.WriteLine(price.ToString("C", CultureInfo.InvariantCulture));

            var chineseCI = new CultureInfo("zh-CN");
            DateTimeFormatInfo dtfi = (DateTimeFormatInfo) chineseCI.GetFormat(typeof (DateTimeFormatInfo));
            Console.WriteLine(dtfi.Calendar.AddDays(DateTime.Now, 2).ToString("f"));

            //实现IFormatProvider的类：CultureInfo, DateTimeFormatInfo, NumberFormatInfo

            //Decimal有四种overload
            Console.WriteLine(price.ToString());
            Console.WriteLine(price.ToString("F3"));
            Console.WriteLine(price.ToString(new CultureInfo("fr-FR")));

            //formate multi objects to one single string
            s = String.Format("On {0}, {1} is {2} years old.",
                new DateTime(2016, 7, 20, 11, 30, 5), "Jessica SU", 3);
            Console.WriteLine(s);

            s = String.Format("On {0:D}, {1} is {2:E} years old.", new DateTime(2016, 7, 20, 11, 30, 5), "Jessica SU", 3);
            Console.WriteLine(s);

            sb.AppendFormat("On {0:D}, {1} is {2:E} years old.", new DateTime(2016, 7, 20, 11, 30, 5), "Jessica SU", 3);
            Console.WriteLine(sb.ToString());

            //Console.WriteLine( )支持format,但是不支持cultureinfo,因为console的应用场景决定了默认自带的culture就足够了。

            //自定义的IFormatProvider
            Example.TestCustomFormatProvider();


            //BitConvert, Convert.ToXX
            Console.WriteLine(BitConverter.GetBytes('苏')[0].ToString("X")); //65会转化为16进制:41

            //Parse string to objects:Parse method for 
            string a = "41";
            int aInt = int.Parse(a, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            Console.WriteLine(aInt);

            Int32 x2 = Int32.Parse("    41", NumberStyles.AllowLeadingWhite, new CultureInfo("en-US"));
            Console.WriteLine(x2);

            //DatetimeFormatInfo：DateTime.ParseExact() can acept a picture format string

            //TryParse：转换会频繁抛出异常时，使用该方式,从而提高性能。TryParse 返回值是true or false,result放在out 参数里

            //Encodings:convert between chars and bytes

            //In the CLR, all characters are represented as 16-bit Unicode code values and all strings are composed of 16-bit Unicode code values 
            // 1 byte= 8 bits
            // 2 byte2 =16 bits= 1 char（unicode) = 1 uint= 1/2 int32
            // 4 bytes = 32 bits = 1 int

            //Encoding: when send string to file or network, binarywriter, filestreamwriter
            //Decoding:when read string from file and network. sometimes, default is UTF-8

            //Unicode==UTF-16, no compression, support most of languages, can convert big endian to little endian, or vice versa
            //UTF-8: <0x0080 work for US, take up 1 byte;  0x0080 ~ 0x007FF work for Europe and Mid East: take up 2bytes, above 0x0800 work for East Asia, 3 bytes,surrogate pairs: 4 bytes
            //大量东亚字符用UTF-16才高效
            //ASCII: 0x00 ~0x7F  0~127

            string myName = "苏明省";
            Encoding encodeUtf8 = Encoding.UTF8;

            var nameBytes = encodeUtf8.GetBytes(myName);
            Console.WriteLine("{0} 's UFT8 bytes string is :{1}", myName, BitConverter.ToString(nameBytes));
                //3 bytes per char,not efficient as Unicode
            Console.WriteLine("{0} 's Unicode bytes string is :{1}", myName,
                BitConverter.ToString(Encoding.Unicode.GetBytes(myName))); //2 bytes
            Console.WriteLine("{0} 's Unicode Bytes Count is :{1}", myName, Encoding.Unicode.GetByteCount(myName));
                //2 bytes
            Console.WriteLine("{0} 's UFT8 Bytes Count is :{1}", myName, Encoding.UTF8.GetByteCount(myName));
                //2 bytes, not fast, GetCharCount not fast too

            //Encoding是Singleton的

            // Encoding.Unicode这种方式是静态调用，实例调用会伤害性能 for UnicodeEncoding, UTF8Encoding, UTF32Encoding, and UTF7Encoding
            //UTF8Encoding encode = new UTF8Encoding();

            //互相转换：
            //Encoding.Unicode.GetBytes
            //Encoding.Unicode.GetString(bytes[]),Encoding.Unicode.GetChars()

            //GetMaxByteCount or GetMaxCharCount fast, will return worst case's count
            Console.WriteLine("GetPreamble for Unicode:" + BitConverter.ToString(Encoding.Unicode.GetPreamble()));

            DisplayAllEncodingInfo();

            //Decoder: Encoding.GetDecoder，used to decode chunks from bytes, Decoder在read bytes from stream时非常有用，能避免decoding data corruption, 截取后剩余的字符会被保存然后传递给下一次从而保证data sanity.
            //
            nameBytes = Encoding.Unicode.GetBytes(myName);
            var decoder = Encoding.Unicode.GetDecoder();
            Console.WriteLine(decoder.GetCharCount(nameBytes, 0, 2));
            Console.WriteLine(decoder.GetCharCount(nameBytes, 2,4));
            
            //GetEncoder用来encode strings in chunks:Encoding.Unicode.GetEncoder().GetByteCount(); Encoding.Unicode.GetEncoder().GetBytes()

            //Base-64 string:encoding and decoding
            string strBase64 = Convert.ToBase64String(nameBytes);
            Console.WriteLine("Base 64 string for {0} is {1}", myName, strBase64);
            Console.WriteLine("Bytes string for {0} is {1}", myName, BitConverter.ToString(Convert.FromBase64String(strBase64)));

            //Secure string: allocated as unmanaged memory

            //SecureString的应用：ProcessStartInfo as demo shows
            using (System.Security.SecureString safeStr = new SecureString())
            {
                ConsoleKeyInfo keyInfo;
                do
                {
                    keyInfo = Console.ReadKey(true);

                    safeStr.AppendChar(keyInfo.KeyChar);
                    Console.Write('*');

                } while (keyInfo.Key != ConsoleKey.Enter);

                Console.WriteLine();

                try
                {
                    //Process.Start("Notepad.exe", "Administrator", safeStr, "BJSLPSUMX");
                    // Password entered, display it for demonstration purposes
                    DisplaySecureString(safeStr);
                }
                catch (Win32Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    safeStr.Dispose();
                }
  

            }

            //SecureString的其他应用：
            //System.Security.Cryptography.CspParameters，
            //System.Security.Cryptography.X509Certificates.X509Certificate
            //System.Security.Cryptography.X509Certificates.X509Certificate2
            //System.Diagnostics.Eventing.Reader.EventLogSession
            //System.Windows.Controls.PasswordBox
            //SecureString not overritten the ToString to avoid risk  




            Console.Read();
        }

        //用字符串驻留来改善性能：应用场景，大量重复的的string待查
        private static Int32 NumTimesWordAppearsEquals(String word, String[] wordlist)
        {
            Int32 count = 0;
            for (Int32 wordnum = 0; wordnum < wordlist.Length; wordnum++)
            {
                if (word.Equals(wordlist[wordnum], StringComparison.Ordinal))
                    count++;
            }
            return count;
        }

        //请慎重使用，有时候数据量太小反而会得不偿失
        private static Int32 NumTimesWordAppearsEqualsIntern(String word, String[] wordList)
        {
            word = String.Intern(word);
            Int32 count = 0;
            for (Int32 wordnum = 0; wordnum < wordList.Length; wordnum++)
            {
                if (ReferenceEquals(word, wordList[wordnum]))
                    count++;
            }
            return count;
        }

        private static void DisplayAllEncodingInfo()
        {
            foreach (EncodingInfo ei in Encoding.GetEncodings())
            {
                Encoding e = ei.GetEncoding();
                Console.WriteLine("{1}{0}" +
                                  "\tCodePage={2}, WindowsCodePage={3}{0}" +
                                  "\tWebName={4}, HeaderName={5}, BodyName={6}{0}" +
                                  "\tIsBrowserDisplay={7}, IsBrowserSave={8}{0}" +
                                  "\tIsMailNewsDisplay={9}, IsMailNewsSave={10}{0}",
                    Environment.NewLine,
                    e.EncodingName, e.CodePage, e.WindowsCodePage,
                    e.WebName, e.HeaderName, e.BodyName,
                    e.IsBrowserDisplay, e.IsBrowserSave,
                    e.IsMailNewsDisplay, e.IsMailNewsSave);
            }
            return;
        }

        // This method is unsafe because it accesses unmanaged memory
        private unsafe static void DisplaySecureString(SecureString ss)
        {
            Char* pc = null;
            try
            {
                // Decrypt the SecureString into an unmanaged memory buffer
                pc = (Char*)Marshal.SecureStringToCoTaskMemUnicode(ss);
                // Access the unmanaged memory buffer that
                // contains the decrypted SecureString
                for (Int32 index = 0; pc[index] != 0; index++)
                    Console.Write(pc[index]);

            }
            finally
            {
                // Make sure we zero and free the unmanaged memory buffer that contains
                // the decrypted SecureString characters
                if (pc != null)
                    Marshal.ZeroFreeCoTaskMemUnicode((IntPtr)pc);
            }
        }
    }


}

