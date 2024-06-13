using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;


namespace Spotify.Servises
{
    public class MusicsService
    {
        private HttpClient _httpClient;
        private readonly string _clientId = "1dde10a6ff4e4e37bd6d7e8b9e6aadd6";
        private readonly string _clientSecret = "fb6a64fb0580425284a917a409723f22";

        public  MusicsService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var authToken = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{_clientId}:{_clientSecret}"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

            var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");
            request.Content = new FormUrlEncodedContent(new[]
            {
            new KeyValuePair<string, string>("grant_type", "client_credentials")
        });

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var token = JObject.Parse(content)["access_token"].ToString();

            return token;
        }

        public async Task<JObject> GetSpotifyDataAsync()
        {
            string accessToken = await GetAccessTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.GetAsync("https://api.spotify.com/v1/browse/new-releases");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JObject.Parse(content);
        }
    
    }
    public class Album
    {
        public string Name { get; set; }
        public List<Artist> Artists { get; set; }
        public ExternalUrls ExternalUrls { get; set; }
    }

    public class Artist
    {
        public string Name { get; set; }
    }

    public class ExternalUrls
    {
        public string Spotify { get; set; }
    }

    public class AlbumsResponse
    {
        public List<Album> Items { get; set; }
    }


}



