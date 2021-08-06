using System;
using System.Net.Http;

namespace BrightSign.Core.Utility.Web
{
    public class HttpServiceClient : HttpClient
    {
        internal HttpServiceClient() : base()
        {
            this.Timeout = new TimeSpan(0, 0, 10);
            //this.Timeout = new TimeSpan(0, 0, 90);
        }
    }
}
