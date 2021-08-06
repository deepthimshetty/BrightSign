using System;
namespace BrightSign.Core.Utility
{
    public enum DeviceStatus
    {
        Connected,
        Disconnected
    }

    public enum ActionTypes 
    {
        Small,
        Medium,
        Large
    }

    public enum UdpDataTypes
    {
        refresh
    }

    public enum TitleType
    {
        Variables,
        Actions,
        Diagnostics,
        Gallery,
        Settings
    }
}
