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

    //This class will act as client/proxy to the FullContact API
    public class FullContactApi : IFullContactApi
    {
        //These constants make up the URL for the http-request for the FullContact API
        const string Url = "https://api.fullcontact.com";
        const string Resource = "v2/person.json";
        const string QueryParameter = "email";
        const string AuthenticationHeaderField = "X-FullContact-APIKey";

        //This is the API-key
        readonly string secretKey;

        //The client that executes the request to the API
        RestClient client;

        //The only constructor, you need to pass a key to the FullContact API
        public FullContactApi(string secretKey)
        {
            this.client = new RestClient();
            this.client.BaseUrl = new Uri(Url);
            this.secretKey = secretKey;
        }
        
        //Async function that returns a task with a FullContactPerson.
        //Takes a string in email-format and access the FullContact Person API
        //  to generate the data.
        public async Task<FullContactPerson> LookupPersonByEmailAsync(string email)
        {
            RestRequest request = generateRequest(email);

            //Get responst from the trust-API
            //while we wait for the respons give the controll back to the caller
            IRestResponse response = await getResponseAsync(request);

            //If the request gave no response let the function return null
            if (response == null)
                return null;

            //convert response to a FullContactPerson
            FullContactPerson personData = getFullContactPerson(json: response.Content);

            return personData;
        }

        //Takes an email and generate a RestRequest with the query and key that is allready defined
        private RestRequest generateRequest(string email)
        {
            RestRequest request = new RestRequest();
            request.Resource = Resource;
            request.AddQueryParameter(QueryParameter, email);
            request.AddHeader(AuthenticationHeaderField, this.secretKey);

            return request;
        }

        //Async function that returns a task with the response from the http-request
        private async Task<IRestResponse> getResponseAsync(RestRequest request)
        {
            /*Uncomment this to test async
             * The client will executes the request in 0-60 sec
             * If many request are generated, they will be executed in a somewhat random order*/
            //await TaskEx.Delay(new Random().Next(60000));
            try
            {
                return await client.Execute(request);
            }
            catch(Exception ex)
            {
                return null;
            }
            
        }


        //Deserializes a string in JSON to FullContactPerson-obj.
        private FullContactPerson getFullContactPerson(string json)
        {
            return JsonConvert.DeserializeObject<FullContactPerson>(json);
        }


    }
}
