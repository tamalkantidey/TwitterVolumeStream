using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TwitterVolumeStreams.Service.Interface;
using TwitterVolumeStreams.Data;

namespace TwitterVolumeStreams.Service.Implementation
{
    public class TwitterManager : ITwitterManager
    {
        ITwitterDB _twitterDB;

        public TwitterManager(ITwitterDB twitterDB)
        {
            _twitterDB = twitterDB;
        }

        public async Task<int> GetTweetCount(string connectionString)
        {
            return _twitterDB.getCount(connectionString).Result;
        }

        public async void GetTweets(string connectionString, string accessToken, string stream_url)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);

                    var request = new HttpRequestMessage(HttpMethod.Get, stream_url);
                    request.Headers.Add("Authorization", "Bearer " + accessToken);

                    var response = httpClient.SendAsync(
                        request, HttpCompletionOption.ResponseHeadersRead).Result;
                    var stream = response.Content.ReadAsStreamAsync().Result;

                    using (var reader = new StreamReader(stream))
                    {
                        while (!reader.EndOfStream)
                        {
                            var currentLine = reader.ReadLine();
                            _twitterDB.saveTweet(currentLine, connectionString);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
