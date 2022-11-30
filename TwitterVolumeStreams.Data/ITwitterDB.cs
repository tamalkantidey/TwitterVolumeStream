using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TwitterVolumeStreams.Data
{
    public interface ITwitterDB
    {
        Task<int> getCount(string connectionString);
        void saveTweet(string tweet,string connectionString);
    }
}
