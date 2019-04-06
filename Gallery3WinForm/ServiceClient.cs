using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace Gallery3WinForm
{
    public static class ServiceClient
    {
        internal async static Task<List<string>> GetArtistNamesAsync() { using (HttpClient lcHttpClient = new HttpClient()) return JsonConvert.DeserializeObject<List<string>>(await lcHttpClient.GetStringAsync("http://localhost:60064/api/gallery/GetArtistNames/")); }

        internal async static Task<clsArtist> GetArtistAsync(string prArtistName)
        {
            using (HttpClient lcHttpClient = new HttpClient()) return JsonConvert.DeserializeObject<clsArtist>(await lcHttpClient.GetStringAsync("http://localhost:60064/api/gallery/GetArtist?Name=" + prArtistName));
        }

        internal async static Task<string> InsertArtistAsync(clsArtist prArtist)
        {
            return await InsertOrUpdateAsync(prArtist, "http://localhost:60064/api/gallery/PostArtist", "POST");
        }

        internal async static Task<string> UpdateArtistAsync(clsArtist prArtist)
        {
            return await InsertOrUpdateAsync(prArtist, "http://localhost:60064/api/gallery/PutArtist", "PUT");
        }

        internal async static Task<string> InstertWorkAsync(clsAllWork work)
        {
            return await InsertOrUpdateAsync(work, "Http://localhost:60064/api/gallery/PostArtWork", "POST");
        }

        internal async static Task<string> UpdateWorkAsync(clsAllWork work)
        {
            return await InsertOrUpdateAsync(work, "Http://localhost:60064/api/gallery/PutArtWork", "PUT");
        }

        private async static Task<string> InsertOrUpdateAsync<TItem>(TItem prItem, string prUrl, string prRequest)
        {
            using (HttpRequestMessage lcReqMessage = new HttpRequestMessage(new HttpMethod(prRequest), prUrl))
            using (lcReqMessage.Content =
                new StringContent(JsonConvert.SerializeObject(prItem), Encoding.Default, "application/json"))
            using (HttpClient lcHttpClient = new HttpClient())
            {
                HttpResponseMessage lcRespMessage = await lcHttpClient.SendAsync(lcReqMessage);
                return await lcRespMessage.Content.ReadAsStringAsync();
            }
        }
    }
}
