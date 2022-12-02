using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MSMQ.Messaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TwitterVolumeStreams.Models;
using TwitterVolumeStreams.Service.Implementation;
using TwitterVolumeStreams.Service.Interface;

namespace TwitterVolumeStreams.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITwitterManager _twitterManager;
        IConfiguration _iconfiguration;
        private string _contentRootPath = "";

        public HomeController(ILogger<HomeController> logger, ITwitterManager twitterManager, IConfiguration iconfiguration, IHostingEnvironment env)
        {
            _logger = logger;
            _twitterManager = twitterManager;
            _iconfiguration = iconfiguration;
            _contentRootPath = env.ContentRootPath;
        }

        public async Task<IActionResult> Index()
        {             
            return View();
        }

        public void GetTweets()
        {
            string connectionString = GetConnectionString();

            string accessToken = _iconfiguration.GetSection("TwitterSettings").GetSection("AccessToken").Value;
            string stream_url = _iconfiguration.GetSection("TwitterSettings").GetSection("Stream_url").Value;
            _twitterManager.GetTweets(connectionString, accessToken, stream_url);
        }

        public int GetCountTweet()
        {
            string connectionString = GetConnectionString();
            return _twitterManager.GetTweetCount(connectionString).Result; 

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string GetConnectionString()
        {
            string connectionString = _iconfiguration.GetSection("ConnectionStrings").GetSection("ConnectionString").Value;

            if (connectionString.Contains("%CONTENTROOTPATH%"))
            {
                connectionString = connectionString.Replace("%CONTENTROOTPATH%", _contentRootPath);
            }

            return connectionString;
        }
    }
}
