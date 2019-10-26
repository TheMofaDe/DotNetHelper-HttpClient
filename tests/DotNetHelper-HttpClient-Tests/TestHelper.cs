using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetHelper_HttpClient_Tests
{
    public static class TestHelper
    {
        public static Stream ToStream(this string s)
        {
            return s.ToStream(Encoding.UTF8);
        }

        public static Stream ToStream(this string s, Encoding encoding)
        {
            return new MemoryStream(encoding.GetBytes(s ?? ""));
        }


        public static string ToString(this Stream stream)
        {
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        public static string ToString(this Stream stream, Encoding encoding)
        {
            StreamReader reader = new StreamReader(stream, encoding);
            return reader.ReadToEnd();
        }

        public static byte[] ToBytes(this string s)
        {
            return System.Text.Encoding.Unicode.GetBytes(s);
        }
        public static byte[] ToBytes(this string s, Encoding encoding)
        {
            return encoding.GetBytes(s);
        }


        //https://stackoverflow.com/a/56553320/2445462
        // The most basic implementation, in platform-agnostic, safe C#
        public static bool Compare(this byte[] range1, int offset1, byte[] range2, int offset2, int count)
        {
            // Working backwards lets the compiler optimize away bound checking after the first loop
            for (int i = count - 1; i >= 0; --i)
            {
                if (range1[offset1 + i] != range2[offset2 + i])
                {
                    return false;
                }
            }

            return true;
        }


        //public static Stream GenerateStreamFromString(this string s, Encoding encoding = null)
        //{
        //    var stream = new MemoryStream();
        //    var writer = encoding == null ? new StreamWriter(stream) : new StreamWriter(stream,encoding);
        //    writer.Write(s);
        //    writer.Flush();
        //    stream.Position = 0;
        //    return stream;
        //}

    }
}
