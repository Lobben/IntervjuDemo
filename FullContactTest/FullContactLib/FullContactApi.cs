using Newtonsoft.Json;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;
using System;
using System.Threading.Tasks;


namespace FullContactLib
{
    public interface IFullContactApi
    {
        Task<FullContactPerson> LookupPersonByEmailAsync(string email);
    }

    //This class will act as proxy to the FullContact API
    public class FullContactApi : IFullContactApi
    {
        //TODO: bra beskrivning etc this is what makes up the URI for the request.
        const string Url = "https://api.fullcontact.com";
        const string Resource = "1.1/statuses/filter.json";
        const string QueryParameter = "email";
        const string AuthenticationHeaderField = "X-FullContact-APIKey";

        //This is the API-key
        readonly string secretKey;

        //The client that executes the request to the API
        RestClient client;

        public FullContactApi(string secretKey)
        {
            this.client = new RestClient();
            this.client.BaseUrl = new Uri(Url);

            this.secretKey = secretKey;
        }
 
        public async Task<FullContactPerson> LookupPersonByEmailAsync(string email)
        {
            //Generate Request
            RestRequest request = generateRequest(email);

            //Get responst from API
            IRestResponse response = await getResponseAsync(request);

            if (response == null)
                return null;

            //convert response to FullContactPerson
            FullContactPerson personData = getFullContactPerson(json: response.Content);

            return personData;
        }

        private RestRequest generateRequest(string email)
        {
            RestRequest request = new RestRequest();
            request.Resource = Resource;
            request.AddQueryParameter(QueryParameter, email);
            request.AddHeader(AuthenticationHeaderField, this.secretKey);

            return request;
        }

        private async Task<IRestResponse> getResponseAsync(RestRequest request)
        {
            //await Task.Delay(new Random().Next(60000));
            try
            {
                return await client.Execute(request);
            }
            catch
            {
                return null;
            }
            
        }

        private FullContactPerson getFullContactPerson(string json)
        {
            return JsonConvert.DeserializeObject<FullContactPerson>(json);
        }


    }
}
