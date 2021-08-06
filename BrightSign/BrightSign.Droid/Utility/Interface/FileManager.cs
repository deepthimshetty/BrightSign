using System;
using System.IO;
using System.Threading.Tasks;
using BrightSign.Core.Utility;
using BrightSign.Core.Utility.Interface;
using BrightSign.Core.Utility.Web;

namespace BrightSign.Droid.Utility.Interface
{
    public class FileManager : IFileManager
    {
        public FileManager()
        {
        }

        public string GetFilePath()
        {
            string path = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            var filename = Path.Combine(path, Constants.logFileName);
            return filename;
        }
        public async Task<string> DownloadFile(string url)
        {
            var filename= GetFilePath();
            string extension = url.ToLower().Contains("log") ? "log" : "dump";
            filename = filename + extension;
            var byteData = await HttpBase.Instance.DownloadLog(url);

            try
            {

                // Writing file 
                using (var streamWriter = new StreamWriter(filename))
                {
                    streamWriter.WriteLine(byteData);
                }
            }
            catch (Exception e)
            {

            }

            return filename;
        }
    }
}
