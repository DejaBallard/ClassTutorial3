using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace Gallery4Universal
{
    /// <summary>
    /// This uses the HTTP protocols and REST to send actions and data from the windows form to the selfhost database
    /// </summary>
    public static class ServiceClient
    {
        /// <summary>
        /// Get all artist names from the selfhost
        /// </summary>
        /// <returns></returns>
        internal async static Task<List<string>> GetArtistNamesAsync() {
            using (HttpClient lcHttpClient = new HttpClient()) return JsonConvert.DeserializeObject<List<string>>(await lcHttpClient.GetStringAsync("http://localhost:60064/api/gallery/GetArtistNames/"));
        }

        /// <summary>
        /// Get one artist from the selfhost
        /// </summary>
        /// <param name="prArtistName">Name of the artist you want the information of</param>
        /// <returns></returns>
        internal async static Task<clsArtist> GetArtistAsync(string prArtistName)
        {
            using (HttpClient lcHttpClient = new HttpClient()) return JsonConvert.DeserializeObject<clsArtist>(await lcHttpClient.GetStringAsync("http://localhost:60064/api/gallery/GetArtist?Name=" + prArtistName));
        }

        /// <summary>
        /// insert a new artist into the selfhost
        /// </summary>
        /// <param name="prArtist">all of the artist's data</param>
        /// <returns></returns>
        internal async static Task<string> InsertArtistAsync(clsArtist prArtist)
        {
            return await InsertOrUpdateAsync(prArtist, "http://localhost:60064/api/gallery/PostArtist", "POST");
        }

        /// <summary>
        /// Send the updated data of an existing artist to the selfhost
        /// </summary>
        /// <param name="prArtist"></param>
        /// <returns></returns>
        internal async static Task<string> UpdateArtistAsync(clsArtist prArtist)
        {
            return await InsertOrUpdateAsync(prArtist, "http://localhost:60064/api/gallery/PutArtist", "PUT");
        }

        /// <summary>
        /// Insert a new artwork to the selfhost
        /// </summary>
        /// <param name="work">all the data of the new artwork</param>
        /// <returns></returns>
        internal async static Task<string> InsertWorkAsync(clsAllWork work)
        {
            return await InsertOrUpdateAsync(work, "Http://localhost:60064/api/gallery/PostArtWork", "POST");
        }

        /// <summary>
        /// send the updated data of an artwork to the selfhost
        /// </summary>
        /// <param name="work"></param>
        /// <returns></returns>
        internal async static Task<string> UpdateWorkAsync(clsAllWork work)
        {
            return await InsertOrUpdateAsync(work, "Http://localhost:60064/api/gallery/PutArtWork", "PUT");
        }

        /// <summary>
        /// A standardised method to sort classes into a REST / Json format to be sent over the network
        /// </summary>
        /// <typeparam name="TItem">A catch all for all possible classes we want to save.</typeparam>
        /// <param name="prItem">the class we want to turn into a Json file</param>
        /// <param name="prUrl">where we want to send the Json file to. it will be the controller and method within the controller</param>
        /// <param name="prRequest">the type of REST requst this data is. PUT,GET,POST,DELETE</param>
        /// <returns></returns>
        private async static Task<string> InsertOrUpdateAsync<TItem>(TItem prItem, string prUrl, string prRequest)
        {
            using (HttpRequestMessage lcReqMessage = new HttpRequestMessage(new HttpMethod(prRequest), prUrl))
            using (lcReqMessage.Content =
                new StringContent(JsonConvert.SerializeObject(prItem), Encoding.UTF8, "application/json"))
            using (HttpClient lcHttpClient = new HttpClient())
            {
                HttpResponseMessage lcRespMessage = await lcHttpClient.SendAsync(lcReqMessage);
                return await lcRespMessage.Content.ReadAsStringAsync();
            }
        }

        /// <summary>
        /// Send a Delete work request to the selfhost
        /// </summary>
        /// <param name="prWork">The name of the artwork you wish to delete</param>
        /// <returns></returns>
        internal async static Task<string> DeleteArtworkAsync(clsAllWork prWork)
        {
            using (HttpClient lcHttpClient = new HttpClient())
            {
                HttpResponseMessage lcRespMessage = await lcHttpClient.DeleteAsync
                ($"http://localhost:60064/api/gallery/DeleteArtWork?WorkName={prWork.Name}&ArtistName={prWork.ArtistName}");
                return await lcRespMessage.Content.ReadAsStringAsync();
            }
        }

        /// <summary>
        /// Send a delete request to delete an artist to the selfhost
        /// </summary>
        /// <param name="lcKey">the artist's ID, which is their name</param>
        /// <returns></returns>
        internal async static Task<string> DeleteArtist(string lcKey)
        {
            using (HttpClient lcHttpClient = new HttpClient())
            {
                HttpResponseMessage lcRespMessage = await lcHttpClient.DeleteAsync
                ($"http://localhost:60064/api/gallery/DeleteArtist?ArtistName={lcKey}");
                return await lcRespMessage.Content.ReadAsStringAsync();
            }
        }
    }
}
