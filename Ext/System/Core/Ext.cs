using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Threading;
using System.Globalization;

#if NET45
using System.Threading.Tasks;
#endif

namespace Ext.System.Core {
    public static class Ext {

        #region String

        public static int ToInt(this string str) {
            return Convert.ToInt32(str);
        }

        public static int ToInt(this string str, int Default) {
            int res = Default;
            int.TryParse(str, out res);
            return res;
        }

        public static int ToIntEx(this string str, int Radix) {
            return Convert.ToInt32(str, Radix);
        }

        public static double ToDouble(this string src) {
            return Convert.ToDouble(src);
        }

        public static double ToDouble(this string src, double Default) {
            var res = Default;
            double.TryParse(src, out res);
            return res;
        }

        public static bool IsNullOrEmpty(this string str) {
            return str == null || str == string.Empty;
        }

        public static bool IsNotNullAndNotEmpty(this string str) {
            return !str.IsNullOrEmpty();
        }

        public static string GetFilename(this string str) {
            return Path.GetFileName(str);
        }

        /// <summary>
        /// Convert string to System.Net.IpEndPoint. String format must be ip:port.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Throws if string has incorrect format</exception>
        public static IPEndPoint ToIpEndPoint(this string src) {
            var id = src.IndexOf(":");
            if (id == -1)
                throw new InvalidOperationException("Incorrect string format. Please use symbol ':' to split ip-address and port. Example: 127.0.0.1:100");
            return new IPEndPoint(IPAddress.Parse(src.Substring(0, id)), src.Substring(id + 1).ToInt());
        }

        public static WebProxy ToWebProxy(this string src) {
            var id = src.IndexOf(":");
            if(id == -1)
                return null;
            return new WebProxy(src.Substring(0, id), src.Substring(id + 1).ToInt());
        }

        public static MatchCollection GetAllMatches(this string str, string Pattern) {
            return Regex.Matches(str, Pattern);
        }

        public static Match GetMatch(this string str, string Pattern) {
            return Regex.Match(str, Pattern);
        }

        public static bool IsMatch(this string str, string Pattern) {
            return Regex.IsMatch(str, Pattern);
        }

        #endregion

        #region Integer

        /// <summary>
        /// Gets number of bytes in KB.
        /// </summary>
        /// <returns>src * 1024</returns>
        public static int KB(this int src) {
            return src * 1024;
        }

        /// <summary>
        /// Gets number of bytes in MB.
        /// </summary>
        /// <returns>src * 1024 * 1024</returns>
        public static int MB(this int src) {
            return src * 1024 * 1024;
        }

        /// <summary>
        /// Gets number of bytes in GB.
        /// </summary>
        /// <returns>src * 1024 * 1024 * 1024</returns>
        public static int GB(this int src) {
            return src * 1024 * 1024 * 1024;
        }

        public static int Abs(this int src) {
            return Math.Abs(src);
        }

        /// <summary>
        /// Gets string in format '10 B' if value is 10; '1 KB' if value is 1024 etc.
        /// </summary>
        public static string ToByteMetricString(this int src) {
            float f = src;
            string[] SizeMetric = new string[] { "B", "KB", "MB", "GB" };
            int MetricId = 0;
            while (f >= 1024) {
                f /= 1024;
                MetricId++;
            }
            var buf = f.ToString("F2").TrimEnd(new char[] { ',', '0' });
            if (buf == "")
                buf = "0";
            return string.Format("{0} {1}", buf, SizeMetric[MetricId]);
        }

        #endregion

        #region  Decimal

        public static int ToInt(this Decimal dc) {
            return Convert.ToInt32(dc);
        }

        public static int ToInt(this Decimal dc, int Default) {
            int res;
            if(!int.TryParse(dc.ToString(), out res))
                res = Default;
            return res;
        }

        public static string ToString(this int src, int Radix) {
            return Convert.ToString(src, Radix);
        }

        #endregion

        #region  Exeption

        public static void Log(this Exception ex) {
#if __LOG_EXCEPTION__
            try {
                string FilePath = DateTime.Now.ToString("dd MM yyyy.exc");
                using (StreamWriter sw = new StreamWriter(FilePath, true, Encoding.UTF8)) {
                    sw.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff"));
                    while (ex != null) {
                        sw.WriteLine(ex.GetType().ToString());
                        sw.WriteLine(ex.Message);
                        sw.WriteLine(ex.StackTrace);
                        sw.WriteLine();
                        ex = ex.InnerException;
                    }
                    sw.WriteLine();
                    sw.WriteLine();
                    sw.WriteLine();
                }
            } catch (Exception) {

            }
#endif
        }

        #endregion

        #region Template

        public static bool In<T>(this T src, params T[] args) {
            foreach(var v in args) {
                if (src.Equals(v))
                    return true;
            }
            return false;
        }

        public static T To<T>(this IConvertible src) {
            return (T)src.ToType(typeof(T), CultureInfo.CurrentCulture);
        }

#endregion

#region StringArray

        public static string[] ToStringArray(this GroupCollection src) {
            return src.OfType<Group>().Select(x => x.ToString()).ToArray();
        }

#endregion

#region ThreadsArray

#if NET45
        public static Task WaitAllAsyncTask(this Thread[] src) {
            return Task.Factory.StartNew(() => {
                foreach(var v in src)
                    v.Join();
            });
        }
#endif

        public static void WaitAll(this Thread[] src) {
            foreach(var v in src)
                v.Join();
        }


#endregion

#region Array

        public static int CompareTo(this byte[] arr0, byte[] arr1) {
            int Distinction = arr0.Length - arr1.Length;
            if(Distinction != 0)
                return Distinction;
            Distinction = 0;
            for(int i = 0; i < arr0.Length && Distinction==0; i++)
                Distinction = arr0[i] - arr1[i];
            return Distinction;
        }

        public static int CompareTo(this byte[] arr0, int StartIndex0, byte[] arr1, int StartIndex1, int Count) {
            if(arr0.Length < StartIndex0 + Count || arr1.Length < StartIndex1 + Count)
                throw new ArgumentOutOfRangeException();
            int Distinction = 0;
            for(int i = 0; i < Count && Distinction == 0; i++)
                Distinction = arr0[StartIndex0 + i] - arr1[StartIndex1 + i];
            return Distinction;
        }

        public static string ToHexString(this byte[] arr) {
            return string.Join("", arr.Select(x=>x.ToString("X02")).ToArray());
        }

        public static string ToHexString(this byte[] arr, string Separator) {
            return string.Join(Separator, arr.Select(x => x.ToString("X02")).ToArray());
        }

#endregion

    }
}
