using System;
using System.IO;

namespace OTEncrypt
{
	public class FileReader
	{
		public FileReader ()
		{
		}

		public static string ReadFile (string filePath)
		{
			string data;
			FileStream fileStream = new FileStream (filePath, FileMode.Open, FileAccess.Read);

			try {
				StreamReader m_streamReader = new StreamReader (fileStream); 
				data = m_streamReader.ReadToEnd ();
			} finally {
				fileStream.Close ();
			}
			return data;
		}

	}
}

