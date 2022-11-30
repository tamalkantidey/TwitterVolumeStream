using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSMQ.Messaging;
using TwitterVolumeStreams.Service.Interface;
using TwitterVolumeStreams.Data;

namespace TwitterVolumeStreams.Service.Implementation
{
    public class TwitterManager : ITwitterManager
    {
        //public string OAuthConsumerKey= "m3phSt1NghehKNudC6HXqKl3c";
        //public string OAuthConsumerSecret= "Da0djMCGaj6zGRmFLKC6E0pLi6adCNEAIYrhTUQEXjf4FTPlSk";

        ITwitterDB _twitterDB;

        public TwitterManager(ITwitterDB twitterDB)
        {
            _twitterDB = twitterDB;
        }

        //public async Task<string> GetAccessToken()
        //{
        //    var httpClient = new HttpClient();
        //    var request = new HttpRequestMessage(HttpMethod.Post, "https://api.twitter.com/oauth2/token ");
        //    var customerInfo = Convert.ToBase64String(new UTF8Encoding()
        //                              .GetBytes(OAuthConsumerKey + ":" + OAuthConsumerSecret));
        //    request.Headers.Add("Authorization", "Basic " + customerInfo);
        //    request.Content = new StringContent("grant_type=client_credentials",
        //                                            Encoding.UTF8, "application/x-www-form-urlencoded");

        //    HttpResponseMessage response = await httpClient.SendAsync(request);

        //    string json = await response.Content.ReadAsStringAsync();
        //    dynamic item = JsonConvert.DeserializeObject<object>(json);
        //    return item["access_token"];
        //}

        public async Task<int> GetTweetCount(string connectionString)
        {
            return _twitterDB.getCount(connectionString).Result;
        }

        //public int GetCountTweetQueue()
        //{
        //    return tweetQueue.Count;
        //}

        public async void GetTweets(string connectionString, string accessToken, string stream_url)
        {
            int wait = 250;
            //string accessToken = "AAAAAAAAAAAAAAAAAAAAAGxWjwEAAAAAAH1PXX1Al6SF%2BPKj2jFfRVGZQew%3Dano0StaA5XOChG7LMJ9R5ChMO85PaQRCO4XapCZ1v7w7CRvKFK"; //await GetAccessToken();
            //string stream_url = "https://api.twitter.com/2/tweets/sample/stream";

            //while (true)
            //{
                try
                {
                //var request = WebRequest.Create(stream_url);
                //request.Method = "GET";
                //request.Headers.Add("Authorization", "Bearer " + accessToken);

                //using var webResponse1 = request.GetResponse();
                //using var webStream = webResponse1.GetResponseStream();

                //using var reader = new StreamReader(webStream);

                //UpdateQueue(reader);


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
                catch (WebException ex)
                {
                    Console.WriteLine(ex.Message);
                    //logger.append(ex.Message, Logger.LogLevel.ERROR);
                    if (ex.Status == WebExceptionStatus.ProtocolError)
                    {
                        //-- From Twitter Docs -- 
                        //When a HTTP error (> 200) is returned, back off exponentially. 
                        //Perhaps start with a 10 second wait, double on each subsequent failure, 
                        //and finally cap the wait at 240 seconds. 
                        //Exponential Backoff
                        if (wait < 10000)
                            wait = 10000;
                        else
                        {
                            if (wait < 240000)
                                wait = wait * 2;
                        }
                    }
                    else
                    {
                        //-- From Twitter Docs -- 
                        //When a network error (TCP/IP level) is encountered, back off linearly. 
                        //Perhaps start at 250 milliseconds and cap at 16 seconds.
                        //Linear Backoff
                        if (wait < 16000)
                            wait += 250;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    Console.WriteLine("Waiting: " + wait);
                    Thread.Sleep(wait);
                }
           // }

            
        }


        //private async void UpdateQueue(StreamReader reader)
        //{
        //    var data = "";
        //    while ((data = reader.ReadLine()) != null)
        //    {
        //        MessageQueue twitterQ = new MessageQueue(".\\Private$\\twitterQueue");
        //        twitterQ.Send(data);
        //    }
        //}
    }
}
