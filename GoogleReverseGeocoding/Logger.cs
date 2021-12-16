using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleReverseGeocoding
{
    public static class Logger
    {
        public static void WriteToFile(string message, string prefix = "Trace", string extraParams = "")
        {
            string writeEnable = ConfigurationManager.AppSettings[prefix + "Enable"];
            if (writeEnable != "1")
                return;

            try
            {
                string filePath = ConfigurationManager.AppSettings[prefix + "Path"];
                filePath = filePath.Replace("yyyy", DateTime.Now.ToString("yyyy"));
                filePath = filePath.Replace("MM", DateTime.Now.ToString("MM"));
                filePath = filePath.Replace("dd", DateTime.Now.ToString("dd"));
                filePath = filePath.Replace("{ThreadName}", Thread.CurrentThread.Name);

                FileInfo fileInfoFilePath = new FileInfo(filePath);
                if (!fileInfoFilePath.Directory.Exists)
                {
                    fileInfoFilePath.Directory.Create();
                }

                message = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff") + "\t" + (!string.IsNullOrEmpty(extraParams) ? extraParams + "\t" : string.Empty) + message + Environment.NewLine;
                File.AppendAllText(filePath, message);
            }
            catch (Exception)
            {

            }
        }
        public static void Log(Exception exception)
        {
            WriteToFile(exception.Message + "\t" + exception.StackTrace, "Log", string.Empty);
        }

        public static void Trace(string message, string extraParams = "")
        {
            WriteToFile(message, "Trace", extraParams);
        }
    }
}