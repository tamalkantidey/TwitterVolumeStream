using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwitterVolumeStreams
{
    public class Config
    {
        public ConnectionStrings ConnectionStrings { get; set; }
        public TwitterSettings TwitterSettings { get; set; }
    }
    public class ConnectionStrings
    {
        public string ConnectionString { get; set; }
    }
    public class TwitterSettings
    {
        public string AccessToken { get; set; }
        public string Stream_url { get; set; }
    }
}
