using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.IO;
using System.Net;

namespace PointingMagnifier
{
    public static class JansenLib
    {
        //[Flags]
        //public enum ShortcutKeys : uint
        //{
        //    None = 0x00,
        //    Ctrl = 0x01,
        //    Alt = 0x02,
        //    Shift = 0x04
        //}
        //#region Extension Methods
        ///// <summary>
        ///// Includes an enumerated type and returns the new value
        ///// </summary>
        //public static T Include<T>(this Enum value, T append) {
        //    Type type = value.GetType();
 
        //    //determine the values
        //    object result = value;
        //    _Value parsed = new _Value(append, type);
        //    if (parsed.Signed is long) {
        //        result = Convert.ToInt64(value) | (long)parsed.Signed;
        //    }
        //    else if (parsed.Unsigned is ulong) {
        //        result = Convert.ToUInt64(value) | (ulong)parsed.Unsigned;
        //    }
 
        //    //return the final value
        //    return (T)Enum.Parse(type, result.ToString());
        //}
 
        ///// <summary>
        ///// Removes an enumerated type and returns the new value
        ///// </summary>
        //public static T Remove<T>(this Enum value, T remove) {
        //    Type type = value.GetType();
 
        //    //determine the values
        //    object result = value;
        //    _Value parsed = new _Value(remove, type);
        //    if (parsed.Signed is long) {
        //        result = Convert.ToInt64(value) & ~(long)parsed.Signed;
        //    }
        //    else if (parsed.Unsigned is ulong) {
        //        result = Convert.ToUInt64(value) & ~(ulong)parsed.Unsigned;
        //    }
 
        //    //return the final value
        //    return (T)Enum.Parse(type, result.ToString());
        //}
 
        ///// <summary>
        ///// Checks if an enumerated type contains a value
        ///// </summary>
        //public static bool Has<T>(this Enum value, T check) {
        //    Type type = value.GetType();
 
        //    //determine the values
        //    object result = value;
        //    _Value parsed = new _Value(check, type);
        //    if (parsed.Signed is long) {
        //        return (Convert.ToInt64(value) &
        //            (long)parsed.Signed) == (long)parsed.Signed;
        //    }
        //    else if (parsed.Unsigned is ulong) {
        //        return (Convert.ToUInt64(value) &
        //            (ulong)parsed.Unsigned) == (ulong)parsed.Unsigned;
        //    }
        //    else {
        //        return false;
        //    }
        //}
 
        ///// <summary>
        ///// Checks if an enumerated type is missing a value
        ///// </summary>
        //public static bool Missing<T>(this Enum obj, T value) {
        //    return !JansenLib.Has<T>(obj, value);
        //}
 
        //#endregion
 
        //#region Helper Classes
 
        ////class to simplfy narrowing values between
        ////a ulong and long since either value should
        ////cover any lesser value
        //private class _Value {
 
        //    //cached comparisons for tye to use
        //    private static Type _UInt64 = typeof(ulong);
        //    private static Type _UInt32 = typeof(long);
 
        //    public long? Signed;
        //    public ulong? Unsigned;
 
        //    public _Value(object value, Type type) {
 
        //        //make sure it is even an enum to work with
        //        if (!type.IsEnum) {
        //            throw new
        //                ArgumentException("Value provided is not an enumerated type!");
        //        }
 
        //        //then check for the enumerated value
        //        Type compare = Enum.GetUnderlyingType(type);
 
        //        //if this is an unsigned long then the only
        //        //value that can hold it would be a ulong
        //        if (compare.Equals(_Value._UInt32) || compare.Equals(_Value._UInt64)) {
        //            this.Unsigned = Convert.ToUInt64(value);
        //        }
        //        //otherwise, a long should cover anything else
        //        else {
        //            this.Signed = Convert.ToInt64(value);
        //        }
 
        //    }
 
        //}
 
        //#endregion
        /// <summary>
        /// This most excellent class allows me to make an HTTP request and upload a file to a nifty little PHP script
        /// originally created by Shaun Kane, modified a little by me to better suit my needs.  
        /// 
        /// This code was originally posted on StackOverflow:
        /// http://stackoverflow.com/questions/566462/upload-files-with-httpwebrequest-multipart-form-data
        /// </summary>
        /// <param name="url">the URI to make a request to</param>
        /// <param name="file">The file to post</param>
        /// <param name="paramName">identifying the name of the content (eg image or log)</param>
        /// <param name="contentType">the type of file being sent</param>
        /// <param name="nvc">any extra parameters which need to be included</param>
        public static void HttpUploadFile(object args)
        {
            Array argArray = new object[5];
            argArray = (Array)args;
            string url = (string)argArray.GetValue(0);
            string file = (string)argArray.GetValue(1);
            string paramName = (string)argArray.GetValue(2);
            string contentType = (string)argArray.GetValue(3);
            NameValueCollection nvc = (NameValueCollection)argArray.GetValue(4);

            //need to encapsulate this in a try-catch in case no internet!
            Console.WriteLine(string.Format("Uploading {0} to {1}", file, url));
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                wr.ContentType = "multipart/form-data; boundary=" + boundary;
                wr.Method = "POST";
                wr.KeepAlive = true;

                Stream rs = wr.GetRequestStream();

                string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                foreach (string key in nvc.Keys)
                {
                    rs.Write(boundarybytes, 0, boundarybytes.Length);
                    string formitem = string.Format(formdataTemplate, key, nvc[key]);
                    byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                    rs.Write(formitembytes, 0, formitembytes.Length);
                }
                rs.Write(boundarybytes, 0, boundarybytes.Length);

                string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
                string header = string.Format(headerTemplate, paramName, file, contentType);
                byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                rs.Write(headerbytes, 0, headerbytes.Length);

                FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                byte[] buffer = new byte[4096];
                int bytesRead = 0;
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    rs.Write(buffer, 0, bytesRead);
                }
                fileStream.Close();

                byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                rs.Write(trailer, 0, trailer.Length);
                rs.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            WebResponse wresp = null;
            try
            {
                wresp = wr.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                Console.WriteLine(string.Format("File uploaded, server response is: {0}", reader2.ReadToEnd()));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error uploading file", ex);
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
            }
            finally
            {
                wr = null;
            }
        }
    }
}
