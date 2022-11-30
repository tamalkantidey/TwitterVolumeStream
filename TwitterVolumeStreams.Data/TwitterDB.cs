using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Configuration;

namespace TwitterVolumeStreams.Data
{
    public class TwitterDB : ITwitterDB
    {
        public async Task<int> getCount(string connectionString)
        {

            string queryString = "SELECT Count(*) from dbo.Tweet";
            string configvalue1 = ConfigurationManager.AppSettings["connectionString"];
            //string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Twitter\TwitterVolumeStreams\TwitterVolumeStreams.Data\Twitter.mdf;Integrated Security=True";
            int count = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    try
                    {
                        connection.Open();
                        count = (int)command.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
                    
            }
            return count;
        }

        public async void saveTweet(string tweet,string connectionString)
        {
            //string configvalue1 = ConfigurationManager.AppSettings["connectionString"];
            string queryString = "insert into Tweet(Tweet) values (@tweet)";
            //string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Twitter\TwitterVolumeStreams\TwitterVolumeStreams.Data\Twitter.mdf;Integrated Security=True";
           
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@tweet", tweet);
                    try
                    {
                        connection.Open();
                        int resultStat = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }

            }
        }
    }
}
