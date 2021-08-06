using System;
using System.IO;
using BrightSign.Core.Utility.Interface;

namespace BrightSign.Droid.Utility.Interface
{
    public class DBHelper : IDBInfo
    {
        public string GetDbPath(string filename)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }
    }
}
