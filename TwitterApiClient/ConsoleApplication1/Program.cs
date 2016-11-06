using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterApiClient;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            //The twitter Api keys from App.config
            string consumerKey = System.Configuration.ConfigurationManager.AppSettings["ConsumerKey"];
            string consumerSecret = System.Configuration.ConfigurationManager.AppSettings["ConsumerSecret"];
            string accessToken = System.Configuration.ConfigurationManager.AppSettings["AccessToken"];
            string accessTokenSecret = System.Configuration.ConfigurationManager.AppSettings["AccessTokenSecret"];

            //TODO: Let user seperate filters by space, then create string[] to pass as parameter
            string input = System.Console.ReadLine();
            var processor = new TwitterStreamProcessor(consumerKey,consumerSecret,
                accessToken,accessTokenSecret,input);

            int storedOnDisk = 0;

            //Create directory if it not exists
            Directory.CreateDirectory(@"C:\TwitterCampaignPictures");

            //quick'n'dirty output loop that also writes pictures to files
            while (true)
            {
                if (processor.AnyPicture())
                {
                    PictureData pictureData = processor.DequeuePicture();
                    using (var fileStream = new FileStream(@"C:\TwitterCampaignPictures\" + input +"_"+ pictureData.FileName, FileMode.Create, FileAccess.Write))
                    {
                        //byte[] image  = processor.DequeuePicture().Image;
                        fileStream.Write(pictureData.Image, 0, pictureData.Image.Length);
                        storedOnDisk++;
                    }
                }
                System.Console.WriteLine("Matches: " + processor.Matches + 
                    " || Downloaded Pictures: "+processor.DownloadedPictures +
                    " || Pictures stored on disk: " + storedOnDisk);
            }
        }
    }
}
