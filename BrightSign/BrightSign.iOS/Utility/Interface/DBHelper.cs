using System;
using System.IO;
using BrightSign.Core.Utility.Interface;

namespace BrightSign.iOS.Utility.Interface
{
    public class DBHelper : IDBInfo
    {
        public string GetDbPath(string filename)
        {
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

            if (!Directory.Exists(libFolder))
            {
                Directory.CreateDirectory(libFolder);
            }

            return Path.Combine(libFolder, filename);
        }
    }
}
