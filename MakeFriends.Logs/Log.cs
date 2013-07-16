using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeFriends.Log
{
    public class Log
    {
        static string path;
        static object locker = new object();
        static string filename;
        public static string Path
        {
            get
            {
                if (path == null)
                {
                    path = (Configuration.GetConfiguration().Path.Contains(":")) ?
                        Configuration.GetConfiguration().Path :
                        string.Concat(System.AppDomain.CurrentDomain.BaseDirectory,
                                            Configuration.GetConfiguration().Path);
                }

                return path;
            }
        }

        [STAThread()]
        public static void Report(string log)
        {
            Report(log, Configuration.GetConfiguration().Enabled);
        }

        [STAThread()]
        private static void Report(string log, bool enabled)
        {
            try
            {
                if (!Configuration.GetConfiguration().Enabled)
                {
                    return;
                }

                if (!enabled)
                {
                    return;
                }

                lock (locker)
                {
                    filename = Log.Path.Replace("{0}", DateTime.Now.ToString("yyyy-MM-dd"));


                    using (TextWriter tw = new StreamWriter(filename, true))
                    {
                        tw.WriteLine(string.Format("{0}{1}{2}", DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt"), @"~", log));
                        tw.Close();

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("error in Logging  - Error : {0}", ex));
            }
        }

        [STAThread()]
        public static void Report(Exception ex)
        {
            Exception logException = ex;
            if (ex.InnerException != null)
                logException = ex.InnerException;
            string strErrorMsg = Environment.NewLine + "Error in Path :" + System.Web.HttpContext.Current.Request.Path;
            strErrorMsg += Environment.NewLine + "Raw Url :" + System.Web.HttpContext.Current.Request.RawUrl;
            strErrorMsg += Environment.NewLine + "Message :" + logException.Message;
            strErrorMsg += Environment.NewLine + "Source :" + logException.Source;
            strErrorMsg += Environment.NewLine + "Stack Trace :" + logException.StackTrace;
            strErrorMsg += Environment.NewLine + "TargetSite :" + logException.TargetSite;

            Report(strErrorMsg);

        }

        [STAThread()]
        public static void Recycle()
        {
            if (File.Exists(Log.Path))
                File.Delete(Log.Path);
        }



    }
}
