using System;
using System.IO;
using System.Text;

namespace OTEncrypt
{
	public class FileWriter
	{
		public FileWriter ()
		{
		}

		public static void WriteToFile (string filePath, byte[] bytes)
		{
			using (FileStream fs = File.Create (filePath)) {
				fs.Write (bytes, 0, bytes.Length);
			}
		}
	}
}

