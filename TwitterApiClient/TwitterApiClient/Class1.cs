using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Events;
using Tweetinvi.Models;
using Tweetinvi.Streaming;

namespace TwitterApiClient
{
    public class TwitterStreamProcessor
    {
        /*private*/public Queue<PictureData> PictureDataQueue;
        public DateTime StreamStart;
        public int MatchCount;
        public int MaleFaceCount, FemaleFaceCount;

        IFilteredStream stream;

        public TwitterStreamProcessor(params string[] filters)
        {
            PictureDataQueue = new Queue<PictureData>();

            var cred = new TwitterCredentials("ACH1FT0hoXfW9MNdimYKWKjcA", "AYJsK9uLtNQ9Z0s7Tcc2huqmFa2W3jVnV4gVVP05sLcEymsspe", 
                "355657573-m0tniU7Ed6dYnG7JY4tdukE4iOomthJv83jb2oMc", "ZhjcvwBVAvXNtoy5VEhw0IKMiEPkgFuNM8zz1EZ5cXGe7");
            stream = Tweetinvi.Stream.CreateFilteredStream(cred);

            foreach(var filter in filters){
                stream.AddTrack(filter);
            }
            stream.MatchingTweetReceived += handleMatchingTweet;
            Task.Factory.StartNew(() => stream.StartStreamMatchingAllConditions());
        }

        void handleMatchingTweet(object sender,MatchedTweetReceivedEventArgs args)
        {
            //Twitter only has Photo as type for now, futuresafe
            foreach (var media in args.Tweet.Media)
            {
                if (media.MediaType == "photo")
                {
                    var url = media.MediaURL ?? media.MediaURLHttps;
                    var fileName = url.Substring(url.LastIndexOf("/")+1);
                    var imageBuffer = GetDataByHttp(url);

                    PictureDataQueue.Enqueue(new PictureData(imageBuffer, fileName, null));
                }
            }

            //Lägg till +1 på matchCount
            MatchCount++;
        }

        public byte[] GetDataByHttp(string url)
        {
            var httpWebRequest = HttpWebRequest.Create(url);

            using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponseAsync().Result)
            using (var responseStream = httpWebResponse.GetResponseStream())
            using (var memoryStream = new MemoryStream())
            {
                responseStream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
 