using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterVolumeStreams.Data
{
    public class Tweet
    {
        public int Id { get; set; }
        public string TweetData { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
