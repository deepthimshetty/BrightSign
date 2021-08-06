using System;
using System.IO;
using System.Net;

namespace BrightSign.iOS.Utility.Interface
{
    public class DocumentDownloader_iOS
    {
        WebClient m_webClient = new WebClient();
        string filename;
        Stream documentStream;
        public DocumentDownloader_iOS()
        {
        }

        public Stream DownloadPdfStream(string URL, string documentName, bool openfile = false)
        {
            var uri = new System.Uri(URL);

            //Returns the PDF document stream from the given URL
            documentStream = m_webClient.OpenRead(uri);

            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string filePath = Path.Combine(path, documentName + ".pdf");
            try
            {
                FileStream fileStream = File.Open(filePath, FileMode.Create);
                //docstream.Position = 0;
                documentStream.CopyTo(fileStream);
                fileStream.Flush();
                fileStream.Close();
            }
            catch (Exception e)
            {

            }

            if (File.Exists(filePath))
            {
                return new MemoryStream(File.ReadAllBytes(filePath));
            }
            return documentStream;
        }

        public bool SaveFile(Byte[] arr, string filename)
        {
            MemoryStream stream = new MemoryStream(arr);

            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string filePath = Path.Combine(path, filename + ".pdf");
            try
            {
                FileStream fileStream = File.Open(filePath, FileMode.Create);
                //docstream.Position = 0;
                stream.CopyTo(fileStream);
                fileStream.Flush();
                fileStream.Close();
            }
            catch (Exception e)
            {

            }

            if (File.Exists(filePath))
            {
                return true;
            }
            return false;

        }

    }
}
