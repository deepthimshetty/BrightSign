using System;
using BrightSign.Core.Utility.Interface;

namespace BrightSign.Droid.Utility.Interface
{
    public class StoragePathHelper: StoragePath
    {
        public StoragePathHelper()
        {
        }

        public string GetStoragePath(){
            string DocumentsPath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments).AbsolutePath;
            return DocumentsPath;
        }
    }
}
