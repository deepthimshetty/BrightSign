using System;
using SQLite;

namespace BrightSign.Core.Utility.Interface
{
    public interface ISQLite
    {
        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <returns>The connection.</returns>
        SQLiteConnection GetConnection();

    }
}
