using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TwitterVolumeStreams.Service.Interface
{
    public interface ITwitterManager
    {
        public void GetTweets(string connectionString,string accessToken,string stream_url);
       public Task<int> GetTweetCount(string connectionString);
    }
}
