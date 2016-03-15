using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CefClient.WebServices
{
    public enum Verbs
    {
        GET,
        POST,
        PUT,
        DELETE,
    }
    public class TokenInfo
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
    }
    public interface IApiService
    {
        TEntity TransformResponseString<TEntity>(string inputStr);
        Task<TEntity> TransformResponse<TEntity>(HttpResponseMessage response);
        TokenInfo GetUserInfo();
        Task<TEntity> CallApi<TEntity>(string path, bool skipAuthorization = false, Verbs verb = Verbs.GET, object model = null);
    }
    public  class ApiService : IApiService
    {
        private  string baseAdress { get; set; } = "http://localhost:10450/";
        public  string CachePath { get; set; } = "MyCache";
        public  string LogFile = "Mylog";
        private  string _localStorageFileName { get; set; }
        private  string _localStoragePath { get; set; }
        public  JsonSerializerSettings SerializerSettings { get; set; } = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Objects };

        public  TEntity TransformResponseString<TEntity>(string inputStr)
        {
            if (!string.IsNullOrEmpty(inputStr))
            {
                var result = JsonConvert.DeserializeObject<TEntity>(inputStr, SerializerSettings);
                return result;
            }
            else
                return default(TEntity);
        }
        public async  Task<TEntity> TransformResponse<TEntity>(HttpResponseMessage response)
        {
            if (response != null)
            {
                var resultStr = await response.Content.ReadAsStringAsync();
                return TransformResponseString<TEntity>(resultStr);
            }
            else
                return default(TEntity);
        }
        public  TokenInfo GetUserInfo()
        {
            if (string.IsNullOrEmpty(_localStoragePath))
            {
                if (string.IsNullOrEmpty(_localStorageFileName))
                {
                    StringBuilder builder = new StringBuilder(baseAdress);
                    builder.Replace(@"://", "_");
                    builder.Replace(':', '_');
                    builder.Replace("/", ".localstorage");
                    _localStorageFileName = builder.ToString();
                }
                var dir = Directory.GetCurrentDirectory();
                _localStoragePath = "Data Source=" + dir + @"\" + CachePath + @"\Local Storage\" + _localStorageFileName + ";Version=3;";
            }

            TokenInfo retval = null;
            using (var conn = new SQLiteConnection(_localStoragePath))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"SELECT key, value FROM ItemTable WHERE key = 'ls.authData'";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var s = reader.GetString(reader.GetOrdinal("key"));
                        var v = reader.GetString(reader.GetOrdinal("value"));
                        retval = JsonConvert.DeserializeObject<TokenInfo>(v);

                    }
                }
            }
            return retval;
        }
        public async  Task<TEntity> CallApi<TEntity>(string path, bool skipAuthorization = false, Verbs verb = Verbs.GET, object model = null)
        {
            HttpResponseMessage retval = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAdress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (!skipAuthorization)
                {
                    var tokenInfo = GetUserInfo();
                    if (tokenInfo != null && !string.IsNullOrEmpty(tokenInfo.AccessToken))
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenInfo.AccessToken);
                }
                var adress = baseAdress + path;
                switch (verb)
                {
                    case Verbs.GET: retval = await client.GetAsync(adress); break;
                    case Verbs.POST: retval = await client.PostAsync(adress, new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json")); break;
                    case Verbs.PUT: retval = await client.PutAsync(adress, new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json")); break;
                    case Verbs.DELETE: retval = await client.DeleteAsync(adress); break;
                }

            }
            if (retval.IsSuccessStatusCode)
            {
                return await TransformResponse<TEntity>(retval);
            }
            return default(TEntity);
        }
    }
}
