using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Tweetinvi.Events;
using Tweetinvi.Models;
using Tweetinvi.Streaming;

namespace TwitterApiClient
{
    //The class that maintains the stream to twitter and manages the matches
    public class TwitterStreamProcessor
    {
        /*public*/
        public List<PictureMetaData> PictureMetaDataList;
  
        public DateTime StreamStart;    //Timestamp of the start of the stream
        public int Matches;             //Tracks number of matches to the filter from the stream 
        public int DownloadedPictures;  //Number of downloaded pictures

        /*private*/
        Queue<PictureData> PictureDataQueue;
        IFilteredStream stream;


        /*Constructor: takes the keys needed, last paramets are filters for the stream*/
        public TwitterStreamProcessor(string consumerKey, string consumerSecret,
            string accessToken, string accessTokenSecret, params string[] filters)
        {
            PictureDataQueue = new Queue<PictureData>();
            PictureMetaDataList = new List<PictureMetaData>();

            //The credentials for the twitter api
            var cred = new TwitterCredentials(consumerKey, consumerSecret, 
                accessToken, accessTokenSecret);
            stream = Tweetinvi.Stream.CreateFilteredStream(cred);

            //insert the filters
            foreach(var filter in filters){
                stream.AddTrack(filter);
            }

            //Delegate the method responsible for the handling of the matches
            stream.MatchingTweetReceived += handleMatchingTweet;

            //start the stream as a task: will run "forever"
            Task.Factory.StartNew(() => stream.StartStreamMatchingAllConditions());

            StreamStart = DateTime.Now;
        }

        /*public functions*/

        //returns true if the queue is not empty
        public bool AnyPicture()
        {
            return PictureDataQueue.Any();
        }

        //Treadsafe dequeueing of PictureDataQueue
        public PictureData DequeuePicture()
        {
            lock (PictureDataQueue)
            {
                return PictureDataQueue.Dequeue();
            }
        }
        //Treadsafe Enqueueing of PictureDataQueue
        public void EnqueuePicture(PictureData data)
        {
            lock (PictureDataQueue)
            {
                PictureDataQueue.Enqueue(data);
            }
        }

        /*private functions*/

        //Function responsible for handling the match to a tweet from the stream
        void handleMatchingTweet(object sender,MatchedTweetReceivedEventArgs args)
        {
            Matches++;
            foreach (var media in args.Tweet.Media)
            {
                //If the media is a photo
                //right now the only mediatype twitter gives us, but can change in the future
                if (media.MediaType == "photo")
                {
                    var url = media.MediaURL ?? media.MediaURLHttps;

                    //Filename is taken from the last section of the url
                    var fileName = url.Substring(url.LastIndexOf("/")+1);

                    //download picture to image
                    var image = GetDataByHttp(url);

                    //TODO: Find male and female faces in image and generate a Gender[]

                    //Store Metadata for the processed picture(Todo: pass Gender[] instead of null
                    PictureMetaData meta = new PictureMetaData(DateTime.Now, null);
                    PictureMetaDataList.Add(meta);

                    //save the picture in the pictureQueue(treadsafe)
                    EnqueuePicture(new PictureData(image, fileName, meta));

                    DownloadedPictures++;
                }
            }
        }

        //Get data from hhtp request as a byte[]
        byte[] GetDataByHttp(string url)
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
 