using System;
using System.Diagnostics;
using System.IO;
using BrightSign.Core.Utility.Interface;
using SQLite;

namespace BrightSign.Droid.Utility.Interface
{
    public class SQLiteAndroid : ISQLite
    {
        private readonly string sqliteFilename = "BrightSign.db3";
        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <returnsxxx>The connection.</returnsxxx>
        SQLiteConnection ISQLite.GetConnection()
        {
            SQLiteConnection con = null;
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
            var SQLitePath = Path.Combine(documentsPath, sqliteFilename);

            try
            {
                con = new SQLiteConnection( SQLitePath);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            // Return the database connection 
            return con;
        }
    }
}
