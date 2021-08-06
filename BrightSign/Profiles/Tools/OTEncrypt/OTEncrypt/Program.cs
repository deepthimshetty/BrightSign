using System;
using System.Text;
using System.IO;

namespace OTEncrypt
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			string folderPath = string.Empty;

			//Check whether any arguments are provided or not
			if (args.Length >= 1) {
				folderPath = args [0];
			} else {
				Console.WriteLine ("No arguments");
				return;
			}

			string filename = "plaintext";
			string destfilename = "configuration";

			string sourcepath = Path.Combine (folderPath, filename) + ".properties";

			if (File.Exists (sourcepath)) {
				string destpath = Path.Combine (folderPath, destfilename) + ".properties";

				string plainData = FileReader.ReadFile (sourcepath);

				CryptoService service = new CryptoService ();
				byte[] EncriptedData = service.EncryptAes (plainData);

				FileWriter.WriteToFile (destpath, EncriptedData);

				byte[] configuredDataa = File.ReadAllBytes (destpath);

				string decriptData = service.DecryptAes (configuredDataa);

				Console.WriteLine (decriptData);
			} else {
				Console.WriteLine ("Folder does not contain any file named \"plaintext.properties\"");
			}
		}
	}
}
