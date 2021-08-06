using System;
using System.IO;
using BrightSign.Core.Utility.Interface;
using Foundation;
using SQLite;

namespace BrightSign.iOS.Utility.Interface
{
    public class SQLiteiOS : ISQLite
    {
        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <returns>The connection.</returns>
        public SQLiteConnection GetConnection()
        {
            //db name
            var sqliteFilename = "BrightSign.db3";

            var libFile = NSFileManager.DefaultManager.GetUrls(NSSearchPathDirectory.LibraryDirectory, NSSearchPathDomain.User)[0];
            string libPath = libFile.Path;
            var SQLitePath = Path.Combine(libPath, sqliteFilename);
            var con = new SQLiteConnection(SQLitePath);
            // Return the database connection 
            return con;
        }
    }
}
