
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BrightSign.Core.Models;
using Sockets.Plugin;

namespace BrightSign.Core.Utility
{
    public class Constants
    {
        public static string ACCEPT = "Accept";
        public static string ACCEPT_ENCODING = "Accept-Encoding";
        public static string CONTENT_TYPE_URLENCODED = "application/x-www-form-urlencoded";
        public static string CONTENT_TYPE_APPLICATIONJSON = "application/json";
        public static string CONTENT_TYPE_MULTIPART = "multipart/form-data; boundary=Boundary++++++";
        public const string AuthorizationType = "NTLM";
        public static string LoginUser = string.Empty;
        public static string LoginPwd = string.Empty;
        public static List<BSDevice> ScannedDevices;
        public static List<BSDevice> FullDevices;
        public static BSDevice ActiveDevice = null;
        public static string ActivePresentation;

        public static ObservableCollection<BSUdpAction> BSActionList;
        public static ObservableCollection<BSUdpAction> UserDefinedActionsList;
        public static List<BSSnapshot> BSSnapshotList;
        public static SnapshotConfigModel SnapshotConfig;
        public static UdpSocketMulticastClient UdpReceiver;

        public static bool IsLWSCredentialsRequired;
        public static bool IsSnapShotsConfigurable;

        public static bool IsCredentialsRequiredforSnapshots = false;


        public const int PERIODIC_PING_INTERVAL = 20000;

        #region SharedPreferences Strings

        public const string USER_PREFS_FILE = "ANDROIDUSERPREFERENCES";
        public const string USER_PREFS_BUTTON_TYPE = "ACTION_BUTTON_TYPE";
        public const string USER_PREFS_AUTO_REFRESH = "AUTO_REFRESH";
        public const string USER_PREFS_DFAULT_DEVICE_ID = "DFAULT_DEVICE";

        public const string DateFormatStr = "yyyyMMddTHHmmss";

        public static int httpPort = 8080;
        public static int contentPort;

        /// <summary>
        /// receivePort,The action port.
        /// </summary>
        public static int actionPort;

        /// <summary>
        /// destinationPort,The listen port.
        /// </summary>
        public static int listenPort;

        public static TitleType CurrentTab;

        public static string logFileName = "brightsign.";
        #endregion

        #region EventHandlers

        public static event Action DiagnosticsTabSelected;

        public static void OnDiagnosticsTabSelected()
        {
            if (DiagnosticsTabSelected != null)
            {
                DiagnosticsTabSelected.Invoke();
            }
        }

        #endregion
    }
}
