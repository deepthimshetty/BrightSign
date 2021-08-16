using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using BrightSign.Core.Models;
using BrightSign.Core.Utility.Interface;
using BrightSign.Core.Utility.Messages;
using BrightSign.Core.Utility.Web;
using BrightSign.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Sockets.Plugin;
using Zeroconf;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;
using PCLStorage;
using MvvmCross.Platform;
using System.Net;
using System.Net.Sockets;
using NtlmHttpHandler;

namespace BrightSign.Core.Utility
{
    public class BSUtility
    {
        private static BSUtility mInstance = null;
        static IFolder rootFolder;


        public static BSUtility Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new BSUtility();
                }

                return mInstance;
            }
        }



        public enum CellTypes
        {
            Label,
            TwoLabel,
            LabelSwitch,
            NavigationLabel,
            LabelWithTopMargin,
            LabelWithRadioGroup,
            AddButton,
            ActionItem
        }

        public ObservableCollection<ListViewItem> ActionItemSource
        {
            get; set;
        }

        public BSUtility()
        {
            ActionItemSource = new ObservableCollection<ListViewItem>();
        }

        public async Task<bool> GetDeviceRemoteData(bool downloadSnapshots = true)
        {
            bool IsSuccess = false;
            Constants.IsSnapShotsConfigurable = false;
            var actionsResponse = await HttpBase.Instance.GetDeviceRemoteData(Constants.ActiveDevice.IpAddress, "8080");
            if (actionsResponse.Item1)
            {
                if (!string.IsNullOrEmpty(actionsResponse.Item2))
                {
                    try
                    {
                        ParseRemoteDataXML(actionsResponse.Item2);

                        //var UserDefinedActions = DBHandler.Instance.GetActionsfromDB();
                        //foreach (var item in UserDefinedActions)
                        //{
                        //    Constants.BSActionList.Add(item);
                        //}
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }

                }
                if (downloadSnapshots)
                {
                    bool IsSnapshotsDownloaded = Constants.IsSnapShotsConfigurable = await GetSnapshotConfiguration();
                    IsSuccess = actionsResponse.Item1 && IsSnapshotsDownloaded;

                }
                else
                {
                    IsSuccess = actionsResponse.Item1;
                }

            }

            return IsSuccess;
        }

        private void ParseRemoteDataXML(string data)
        {
            Constants.BSActionList = new ObservableCollection<BSUdpAction>();

            XmlDocument doc = new XmlDocument();

            doc.LoadXml(data);

            var lastChild = doc.LastChild;

            var childrenList = lastChild.ChildNodes;

            foreach (XmlElement item in childrenList)
            {

                switch (item.Name)
                {
                    case "unitName":
                        Constants.ActiveDevice.Name = item.InnerText;
                        break;
                    case "unitNamingMethod":
                        Constants.ActiveDevice.NamingMethod = item.InnerText;
                        break;
                    case "unitDescription":
                        Constants.ActiveDevice.Description = item.InnerText;
                        break;
                    case "serialNumber":
                        Constants.ActiveDevice.SerialNumber = item.InnerText;
                        break;
                    case "autorunVersion":
                        Constants.ActiveDevice.AutorunVersion = item.InnerText;
                        break;
                    case "firmwareVersion":
                        Constants.ActiveDevice.FirmwareVersion = item.InnerText;
                        break;
                    case "bsnActive":
                        Constants.ActiveDevice.BsnActive = item.InnerText == "yes";
                        break;
                    case "snapshotsAvailable":
                        Constants.ActiveDevice.SnapshotsAvailable = item.InnerText == "yes";
                        break;
                    case "udpNotificationAddress":
                        Constants.ActiveDevice.UdpMulticastAddress = item.InnerText;
                        break;
                    case "contentPort":
                        Constants.contentPort = Convert.ToInt32(item.InnerText);
                        break;
                    case "receivePort":
                        Constants.actionPort = Convert.ToInt32(item.InnerText);
                        break;
                    case "destinationPort":
                        Constants.listenPort = Convert.ToInt32(item.InnerText);
                        break;
                    case "activePresentation":
                        Constants.ActivePresentation = item.InnerText;
                        break;
                    case "udpEvents":
                        XmlNodeList list = item.ChildNodes;
                        foreach (XmlNode action in list)
                        {
                            XmlNodeList sublist = action.ChildNodes;

                            BSUdpAction udpevent = new BSUdpAction();
                            foreach (XmlElement el in sublist)
                            {
                                switch (el.Name)
                                {
                                    case "label":
                                        udpevent.Label = el.InnerText;
                                        break;
                                    case "action":
                                        udpevent.DataUDP = el.InnerText;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            if (udpevent.Label != "<any>")
                            {
                                udpevent.IsUserDefined = false;
                                Constants.BSActionList.Add(udpevent);
                            }

                        }
                        break;

                    default:
                        break;
                }
            }

        }

        async Task GetDeviceDetails(string IPAddress)
        {
            var searchResponse = await HttpBase.Instance.GetDeviceStatus(IPAddress, "8080");
            if (searchResponse.Item1)
            {
                Constants.ActiveDevice = ParseDeviceInfoXML(searchResponse.Item2);
                Constants.ActiveDevice.IpAddress = IPAddress;
                if (Constants.ActiveDevice.SnapshotsAvailable)
                {
                    await GetSnapshotConfiguration();
                }
            }
        }


        public BSDevice ParseDeviceInfoXML(string data)
        {
            BSDevice bsdevice = new BSDevice();

            XmlDocument doc = new XmlDocument();

            doc.LoadXml(data);

            var lastChild = doc.LastChild;

            var childrenList = lastChild.ChildNodes;

            foreach (XmlElement item in childrenList)
            {
                switch (item.Name)
                {
                    case "unitName":
                        bsdevice.Name = item.InnerText;
                        break;
                    case "unitNamingMethod":
                        bsdevice.NamingMethod = item.InnerText;
                        break;
                    case "unitDescription":
                        bsdevice.Description = item.InnerText;
                        break;
                    case "serialNumber":
                        bsdevice.SerialNumber = item.InnerText;
                        break;
                    case "autorunVersion":
                        bsdevice.AutorunVersion = item.InnerText;
                        break;
                    case "firmwareVersion":
                        bsdevice.FirmwareVersion = item.InnerText;
                        break;
                    case "bsnActive":
                        bsdevice.BsnActive = item.InnerText == "yes";
                        break;
                    case "snapshotsAvailable":
                        bsdevice.SnapshotsAvailable = item.InnerText == "yes";
                        break;

                    case "udpNotificationAddress":
                        bsdevice.SerialNumber = item.InnerText;
                        break;
                    case "contentPort":
                        bsdevice.AutorunVersion = item.InnerText;
                        break;
                    case "receivePort":
                        bsdevice.FirmwareVersion = item.InnerText;
                        break;
                    case "destinationPort":
                        bsdevice.BsnActive = item.InnerText == "yes";
                        break;
                    case "activePresentation":
                        bsdevice.SnapshotsAvailable = item.InnerText == "yes";
                        break;
                    case "udpEvents":
                        bsdevice.SnapshotsAvailable = item.InnerText == "yes";
                        break;

                    default:
                        break;
                }
            }


            //Add the Acitons from the XML
            //parseDeviceInfoData Method from iOS

            return bsdevice;
        }

        public BSDeviceTemp ParseDeviceInfoXMLDummy(string data)
        {
            BSDeviceTemp bsdevice = new BSDeviceTemp();

            XmlDocument doc = new XmlDocument();

            doc.LoadXml(data);

            var lastChild = doc.LastChild;

            var childrenList = lastChild.ChildNodes;

            foreach (XmlElement item in childrenList)
            {
                switch (item.Name)
                {
                    case "unitName":
                        bsdevice.Name = item.InnerText;
                        break;
                    case "unitNamingMethod":
                        bsdevice.NamingMethod = item.InnerText;
                        break;
                    case "unitDescription":
                        bsdevice.Description = item.InnerText;
                        break;
                    case "serialNumber":
                        bsdevice.SerialNumber = item.InnerText;
                        break;
                    case "autorunVersion":
                        bsdevice.AutorunVersion = item.InnerText;
                        break;
                    case "firmwareVersion":
                        bsdevice.FirmwareVersion = item.InnerText;
                        break;
                    case "bsnActive":
                        bsdevice.BsnActive = item.InnerText == "yes";
                        break;
                    case "snapshotsAvailable":
                        bsdevice.SnapshotsAvailable = item.InnerText == "yes";
                        break;



                    case "udpNotificationAddress":
                        bsdevice.SerialNumber = item.InnerText;
                        break;
                    case "contentPort":
                        bsdevice.AutorunVersion = item.InnerText;
                        break;
                    case "receivePort":
                        bsdevice.FirmwareVersion = item.InnerText;
                        break;
                    case "destinationPort":
                        bsdevice.BsnActive = item.InnerText == "yes";
                        break;
                    case "activePresentation":
                        bsdevice.SnapshotsAvailable = item.InnerText == "yes";
                        break;
                    case "udpEvents":
                        bsdevice.SnapshotsAvailable = item.InnerText == "yes";
                        break;

                    default:
                        break;
                }
            }


            //Add the Acitons from the XML
            //parseDeviceInfoData Method from iOS

            return bsdevice;
        }


        public async Task<bool> GetSnapshotConfiguration()
        {
            bool IsSuccess = false;
            try
            {
                var searchResponse = await HttpBase.Instance.GetSnapshotConfiguration(Constants.ActiveDevice.IpAddress, "8080");
                if (searchResponse.Item1)
                {
                    ParseSnapshotConfiguration(searchResponse.Item2);
                    IsSuccess = true;
                }

                if (Constants.IsCredentialsRequiredforSnapshots)
                {
                    // Download the Image 

                    foreach (var item in Constants.BSSnapshotList)
                    {
                        var imageData = await GetSnapshotsAsync(item.ID);
                        if (imageData != null)
                        {
                            item.ImageDataObj.ImageData = imageData;
                            item.ImageDataObj.ImagePath = string.Format("{0}/{2}_{1}", rootFolder.Path, item.ID, Constants.ActivePresentation);
                        }
                    }
                }
                else
                {
                    foreach (var item in Constants.BSSnapshotList)
                    {
                        var imageData = await GetSnapshotsAsync(item.ID);
                        if (imageData != null)
                        {
                            item.ImageDataObj.ImageData = imageData;
                            item.ImageDataObj.ImagePath = string.Format("{0}/{2}_{1}", rootFolder.Path, item.ID, Constants.ActivePresentation);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {

            }

            return IsSuccess;
        }

        private async Task<Byte[]> GetSnapshotsAsync(string ID)
        {
            if (rootFolder == null)
            {
                rootFolder = FileSystem.Current.LocalStorage;

                //rootFolder = await FileSystem.Current.GetFolderFromPathAsync(Mvx.Resolve<StoragePath>().GetStoragePath());



            }

            string SnapShotID = string.Format("{0}_{1}", Constants.ActivePresentation, ID);

            //Check if File Exists
            var FileExists = await IsFileExists(SnapShotID);

            if (FileExists)
            {
                // Read the file and Assign the Byte Array
                var file = await rootFolder.GetFileAsync(SnapShotID);
                var fileData = await file.OpenAsync(PCLStorage.FileAccess.Read);
                var bytearray = ReadFully(fileData);
                return bytearray;
            }

            string url = string.Empty;
            HttpClientHandler httpHandler;
            url = string.Format("http://{0}:{1}/GetSnapshot?ID={2}", Constants.ActiveDevice.IpAddress, Constants.httpPort, ID);
            if (Constants.IsCredentialsRequiredforSnapshots)
            {


                NetworkCredential credentials = new NetworkCredential(Constants.LoginUser, Constants.LoginPwd);

                CredentialCache credentialCache = new CredentialCache { { Constants.ActiveDevice.IpAddress, Constants.httpPort, Constants.AuthorizationType, credentials } };

                //httpHandler = new HttpClientHandler()
                //{
                //    CookieContainer = new CookieContainer(),
                //    Credentials = credentialCache.GetCredential(Constants.ActiveDevice.IpAddress, Constants.httpPort, Constants.AuthorizationType),
                //};

                httpHandler = NtlmHttpHandlerFactory.Create();
                httpHandler.Credentials = credentialCache.GetCredential(Constants.ActiveDevice.IpAddress, Constants.httpPort, Constants.AuthorizationType);
            }
            else
            {
                httpHandler = new HttpClientHandler();
            }
            var httpClient = new HttpClient(httpHandler, false)
            {
                Timeout = new TimeSpan(0, 0, 0, 30)
            };
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        //var imageBase64 = ConvertToBase64(responseStream);
                        var _ms = new MemoryStream();
                        responseStream.CopyTo(_ms);
                        var fileResponse = await SaveFileAsync(SnapShotID, _ms);

                        // Read the file and Assign the Byte Array
                        var file = await rootFolder.GetFileAsync(SnapShotID);
                        var fileData = await file.OpenAsync(PCLStorage.FileAccess.Read);
                        var bytearray = ReadFully(fileData);
                        return bytearray;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }


        }

        public static Stream ConvertToBase64(Stream stream)
        {
            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }

            string base64 = Convert.ToBase64String(bytes);
            return new MemoryStream(Encoding.UTF8.GetBytes(base64));
        }


        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        public async Task<bool> IsFileExists(string ID)
        {
            var fileResult = await rootFolder.CheckExistsAsync(ID);
            return fileResult == ExistenceCheckResult.FileExists;
        }

        /// <summary>
        /// Save the Download Memory Stream to Local File System 
        /// </summary>
        /// <returns>The file async.</returns>
        /// <param name="fileName">File name.</param>
        /// <param name="inputStream">Input stream.</param>
        public static async Task<IFile> SaveFileAsync(string fileName, MemoryStream inputStream)
        {
            //Create a File or Replace a file in the Local Folder
            //var file = await FileSystem.Current.LocalStorage.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            //Mvx.Resolve<ICustomAlert>().ShowCustomAlert(true, bsDevice.Name, Strings.addedtolist);

            //var rootFolder = await FileSystem.Current.GetFolderFromPathAsync( Mvx.Resolve<StoragePath>().GetStoragePath() );

            //Create a File or Replace a file in the Documents Folder
            var file = await rootFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            using (var stream = await file.OpenAsync(PCLStorage.FileAccess.ReadAndWrite))
            {
                inputStream.WriteTo(stream);
            }

            return file;
        }

        public async void DisconnectSocket()
        {
            if (Constants.UdpReceiver != null)
            {
                try
                {
                    await Task.Run(async () =>
                    {
                        await Constants.UdpReceiver.DisconnectAsync();
                        Constants.UdpReceiver.Dispose();
                        Constants.UdpReceiver = null;
                    });
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

            }


        }

        public async Task InitializeSocketListening()
        {
            try
            {


                if (Constants.ActiveDevice != null && Constants.listenPort != 0)
                {
                    var port = Constants.listenPort;
                    var address = Constants.ActiveDevice.UdpMulticastAddress; // must be a valid multicast address


                    Constants.UdpReceiver = new UdpSocketMulticastClient();
                    Constants.UdpReceiver.TTL = 5;

                    Constants.UdpReceiver.MessageReceived += (sender, args) =>
                    {
                        if (!string.IsNullOrEmpty(args.RemoteAddress) && args.RemoteAddress == Constants.ActiveDevice.IpAddress)
                        {
                            var from = String.Format("{0}:{1}", args.RemoteAddress, args.RemotePort);
                            var data = Encoding.UTF8.GetString(args.ByteData, 0, args.ByteData.Length);

                            Debug.WriteLine("{0} - {1}", from, data);

                            if (data == "refresh")
                            {
                                if (Mvx.Resolve<IUserPreferences>().GetBool(Constants.USER_PREFS_AUTO_REFRESH))
                                {
                                    Mvx.Resolve<IMvxMessenger>().Publish(new ActiveDeviceMessage(this, DeviceStatus.Connected, true));

                                }
                                Mvx.Resolve<IMvxMessenger>().Publish(new ImageRefreshMessage(this));
                            }
                            else
                            {
                                Debug.WriteLine("other UDP Command received from the receiver");
                            }
                        }
                    };

                    // join the multicast address:port

                    await Constants.UdpReceiver.JoinMulticastGroupAsync(address, port);

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }




        private void ParseSnapshotConfiguration(string data)
        {
            XmlDocument doc = new XmlDocument();

            doc.LoadXml(data);

            var lastChild = doc.LastChild;

            var attributes = lastChild.Attributes;

            if (attributes.Count > 0)
            {
                Constants.SnapshotConfig = new SnapshotConfigModel();

                foreach (XmlNode item in attributes)
                {
                    switch (item.Name)
                    {
                        case "Count":
                            Constants.SnapshotConfig.Count = Convert.ToInt32(item.Value);
                            break;
                        case "CanConfigure":
                            Constants.SnapshotConfig.CanConfigure = Convert.ToBoolean(item.Value);
                            break;
                        case "Interval":
                            Constants.SnapshotConfig.Interval = Convert.ToInt32(item.Value) / 60;
                            break;
                        case "DisplayPortraitMode":
                            Constants.SnapshotConfig.DisplayPortraitMode = Convert.ToBoolean(item.Value);
                            break;
                        case "MaxImages":
                            Constants.SnapshotConfig.MaxImages = Convert.ToInt32(item.Value);
                            break;
                        case "Quality":
                            Constants.SnapshotConfig.Quality = Convert.ToInt32(item.Value);
                            break;
                        case "ResY":
                            Constants.SnapshotConfig.ResY = Convert.ToInt32(item.Value);
                            break;
                        case "ResX":
                            Constants.SnapshotConfig.ResX = Convert.ToInt32(item.Value);
                            break;
                        case "Enabled":
                            Constants.SnapshotConfig.Enabled = Convert.ToBoolean(item.Value);
                            break;

                        default:
                            break;
                    }
                }
            }


            var childrenList = lastChild.ChildNodes;
            Constants.BSSnapshotList = new System.Collections.Generic.List<BSSnapshot>();
            foreach (XmlNode item in childrenList)
            {
                XmlNodeList sublist = item.ChildNodes;
                BSSnapshot snapshot = new BSSnapshot();
                foreach (XmlElement el in sublist)
                {
                    switch (el.Name)
                    {
                        case "ID":
                            snapshot.ID = el.InnerText;
                            break;
                        case "Time":
                            snapshot.TimeStamp = el.InnerText;
                            break;
                        default:
                            break;
                    }

                }
                Constants.BSSnapshotList.Add(snapshot);
            }

        }


        public async Task EnumerateAllServicesFromAllHosts()
        {
            Constants.ScannedDevices = new List<BSDevice>();

            //IReadOnlyList<IZeroconfHost> results = await ZeroconfResolver.ResolveAsync("_http._tcp.local.", scanTime: TimeSpan.FromSeconds(10), retries: 10);
            IReadOnlyList<IZeroconfHost> results = await ZeroconfResolver.ResolveAsync("_http._tcp.local.");
            //ZeroconfResolver.BrowseDomainsAsync

            //IReadOnlyList<IZeroconfHost> results = await ZeroconfResolver.ResolveAsync("local.");

            ILookup<string, string> domains = await ZeroconfResolver.BrowseDomainsAsync();
            Console.WriteLine("Fetching everything");
            foreach(var item in domains)
            {
                Console.WriteLine("Domain " + item.ToString());
                //item
            }
            //var results = await ZeroconfResolver.ResolveAsync(domains.Select(g => g.Key));
            foreach (var item in results)
            {
                Console.WriteLine(item);

                if (!string.IsNullOrWhiteSpace(item.DisplayName) && item.DisplayName.StartsWith("BrightSign", StringComparison.CurrentCultureIgnoreCase))
                {

                    BSDevice bsDevice = new BSDevice();
                    bsDevice.IpAddress = item.IPAddress;
                    var services = item.Services;
                    foreach (var service in services)
                    {
                        if (service.Value != null && service.Value.Properties != null)
                        {
                            var properties = service.Value.Properties;
                            foreach (var property in properties)
                            {
                                if (property.ContainsKey("unitname"))
                                {
                                    bsDevice.Name = property["unitname"];
                                    bsDevice.SerialNumber = property["serialnumber"];
                                    bsDevice.NamingMethod = property["unitnamingmethod"];
                                    bsDevice.Description = property["unitdescription"];
                                    bsDevice.Functionality = property["functionality"];
                                    bsDevice.IsOnline = true;
                                    bsDevice.Image = BSUtility.Instance.GetImageString(bsDevice.SerialNumber);
                                }
                            }
                        }
                    }
                    
                    //if (!string.IsNullOrEmpty(bsDevice.Name))
                    {
                        Constants.ScannedDevices.Add(bsDevice);
                    }
                }
            }

            //            if (Constants.ActiveDevice == null)
            //            {
            //                if (Constants.ScannedDevices != null && Constants.ScannedDevices.Count > 0)
            //                {
            //                    ClearTimer();
            //                    Constants.ScannedDevices[0].IsDefault = true;
            //                    Constants.ActiveDevice = Constants.ScannedDevices[0];
            //#if DEBUG
            //                    //Constants.ActiveDevice = Constants.ScannedDevices[1];
            //#endif
            //        await LoadDeviceData();
            //    }
            //}
        }


        public string GetImageString(string serialNumber)
        {
            string imageString = string.Empty;
            string prefix = serialNumber.Substring(0, 2);
            switch (prefix)
            {
                case "24":
                case "25":
                case "42":
                case "43":
                    imageString = "ls424";
                    break;
                case "31":
                case "38":
                    imageString = "ls423";
                    break;
                case "33":
                    imageString = "hd223";
                    break;
                case "35":
                    imageString = "hd1023";
                    break ;
                case "44":
                case "45":
                case "TH":
                case "TJ":
                    imageString = "hd224";  
                    break;
                case "46":
                case "47":
                case "48":
                case "49":
                case "TK":
                case "TL":
                case "TM":
                case "TN":
                    imageString = "hd1024"; 
                    break;
                case "51":
                    imageString = "tpsspi4";
                    break;
                case "53":
                    imageString = "roadster"; // as per suggestion//Actually hs123
                    break;
                case "55":
                case "57":
                    imageString = "ops"; // HO523
                    break;
                case "58":
                case "59":
                    imageString = "roadster";
                    break;
                case "61":
                    imageString = "cvhd";  
                    break;
                case "62":
                case "63":
                    imageString = "cvuhd";  
                    break;
                case "64":
                    imageString = "sstbphd2"; 
                    break;
                case "65":
                case "TS":
                    imageString = "cvhd2"; 
                    break;
                case "66":
                    imageString = "hd1024";
                    break;
                case "67":
                case "68":
                case "TT":
                    imageString = "cvuhd2"; 
                    break;
                case "AA":
                case "AC":
                    imageString = "au325";
                    break;
                case "D1":
                case "D2":
                case "TU":
                case "TV":
                    imageString = "xd234"; 
                    break;
                case "D3":
                case "D4":
                case "TW":
                case "TX":
                    imageString = "xd1034"; 
                    break;
                case "D5":
                case "D6":
                case "TY":
                case "UA":
                    imageString = "xt244"; 
                    break;
                case "D7":
                case "D8":
                case "D9":
                case "E1":
                case "E2":
                case "E3":
                case "UC":
                case "UD":
                case "UE":
                case "UF":
                case "UJ":
                    imageString = "xt1144"; 
                    break;
                case "E4":
                case "E5":
                case "UG":
                case "UH":
                    imageString = "xd1034";
                    break;
                case "L6":
                case "R6":
                    imageString = "xt243";
                    break;
                case "L8":
                case "L9":
                case "R7":
                    imageString = "xt1143";
                    break;
                case "R1":
                    imageString = "xd233";
                    break;
                case "R2":
                case "R4":
                    imageString = "xd1033";
                    break;
                case "R3":
                    imageString = "xd233";
                    break;
                case "TD":
                case "TE":
                case "TF":
                case "TG":
                    imageString = "ls424";
                    break;
                case "TP":
                    imageString = "tpsspi4"; 
                    break;
                case "TR":
                    imageString = "sstbphd2"; 
                    break;
                
                default:
                    // Do nothing on default. Give empty string
                    break;
                //case "35":
                //    imageString = "HD1023";
                //    break;
                //case "36":
                //    imageString = "HD1023";
                //    break;
                //case "37":
                //    imageString = "HD1023"; // Not found copied previous image
                //    break;
                //case "38":
                //case "41":
                //case "31":
                //    imageString = "LS423";
                //    break;
                //case "57":
                //    imageString = "LS423";  // Not found copied previous image
                //    break;
                //case "61":
                //    imageString = "LS423";  // ignore
                //    break;
                //case "62":
                //    imageString = "LS423";  // ignore
                //    break;
                //case "64":
                //    imageString = "LS423";  // Not found copied previous image
                //    break;
                //case "93":
                //    imageString = "";
                //    break;
                //case "D1":
                //    imageString = "XD233";
                //    break;
                //case "D2":
                //    imageString = "XD233";
                //    break;
                //case "D3":
                //    imageString = "XD1033";
                //    break;
                //case "D4":
                //    imageString = "XD1033";
                //    break;
                //case "D5":
                //    imageString = "XT243";
                //    break;
                //case "D6":
                //    imageString = "XT243";
                //    break;
                //case "D7":
                //    imageString = "XT1143";
                //    break;
                //case "D8":
                //    imageString = "XT1143";
                //    break;
                //case "U6":
                //    imageString = "";
                //    break;
                //case "U7":
                //    imageString = "";
                //    break;
                //case "L8":
                //case "R7":
                //    imageString = "XT1143";
                //    break;

                //case "X5":
                //    imageString = "I4K242_fancy";
                //    break;
                //case "X4":
                //    imageString = "I4K1042_fancy";
                //    break;
                //case "X3":
                //    imageString = "I4K1142_fancy";
                //    break;
                //case "X2":
                //    imageString = "xd1230_skewed";
                //    break;
                //case "X1":
                //    imageString = "xd1030";
                //    break;
                //case "X0":
                //    imageString = "xd230";
                //    break;

                //case "32":
                //    imageString = "I120";
                //    break;
                //case "72":
                //    imageString = "hd220";
                //    break;
                //case "73":
                //    imageString = "hd220"; // ignore
                //    break;
                //case "74":
                //    imageString = "ls_front";
                //    break;
                //case "75":
                //    imageString = "ls_front";
                //    break;
                //case "76":
                //    imageString = "hd220"; // ignore
                //    break;
                //case "78":
                //    imageString = "hd1022";
                //    break;
                //case "77":
                //case "21":
                //    imageString = "HD222";
                //    break;
                //case "33":
                //    imageString = "HD223";
                //    break;
                //case "A2":
                //    imageString = "hd1020";
                //    break;
                //case "55":
                //case "56": //ignore
                //    imageString = "OPS";
                //    break;
                //case "L2":
                //    imageString = "XD232";
                //    break;
                //case "R1":
                //case "R3":
                //    imageString = "XD233";
                //    break;
                //case "L3":
                //case "L4":
                //    imageString = "XD1032_1132";
                //    break;
                //case "L5":
                //    imageString = "XD1032_1132";
                //    break;
                //case "R2":
                //case "R4":
                //    imageString = "XD1033";
                //    break;
                //case "R5":
                //    imageString = "XD1033";
                //    break;
                //case "R6":
                //case "L6":
                //    imageString = "XT243";
                //    break;
                //case "L7":
                //    imageString = "XT1143";
                //    break;
                //case "53":
                //    imageString = "roadster";
                //    break;
                //case "46":
                //    imageString = "hd1024";
                //    break;
                //case "42":
                //    imageString = "ls424";
                //    break;
                //default:
                //    break;
            }



            return imageString.ToLower();
            //return "OPS";
        }

        Timer scanTimer;


        void InitializeTimer()
        {
            if (scanTimer == null)
            {
                scanTimer = new Timer(ScanDevices, null, 0, Constants.PERIODIC_PING_INTERVAL);
            }
        }

        void ClearTimer()
        {
            if (scanTimer != null)
            {
                scanTimer.Cancel();
                scanTimer.Dispose();
                scanTimer = null;
            }
        }

        private async void ScanDevices(object state)
        {
            await EnumerateAllServicesFromAllHosts();

        }

        public async Task<string> DownloadFile(string url)
        {
            var filename = Mvx.Resolve<IFileManager>().GetFilePath();
            string extension = url.ToLower().Contains("log") ? "log" : "dump";
            filename = filename + extension;
            var byteData = await HttpBase.Instance.DownloadLog(url);

            try
            {
                var logFile = System.IO.File.Create(filename);
                using (var logWriter = new System.IO.StreamWriter(logFile))
                {
                    logWriter.WriteLine(byteData);
                }
            }
            catch (Exception e)
            {

            }

            return filename;
        }


        public Task<Tuple<bool, BSDevice>> GetDeviceStatus(string IPAddress)
        {
            return Task.Run(async () =>
            {
                bool IsConnected = false;
                BSDevice bsdeviceAdd = null;
                try
                {
                    var searchResponse = await HttpBase.Instance.GetDeviceStatus(IPAddress, "8080");
                    if (searchResponse.Item1)
                    {
                        bsdeviceAdd = new BSDevice();
                        IsConnected = true;
                        bsdeviceAdd = BSUtility.Instance.ParseDeviceInfoXML(searchResponse.Item2);
                        bsdeviceAdd.IpAddress = IPAddress;
                        bsdeviceAdd.IsOnline = true;
                        bsdeviceAdd.Image = BSUtility.Instance.GetImageString(bsdeviceAdd.SerialNumber);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

                return new Tuple<bool, BSDevice>(IsConnected, bsdeviceAdd);
            });


        }

        public async Task<bool> IsDeviceOnline()
        {
            var searchResponse = await HttpBase.Instance.GetDeviceStatus(Constants.ActiveDevice.IpAddress, "8080");
            return searchResponse.Item1;
        }
    }
}
