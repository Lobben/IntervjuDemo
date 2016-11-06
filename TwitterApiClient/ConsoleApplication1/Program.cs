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
            var processor = new TwitterStreamProcessor("#yolo");
            while (true)
            {
                if (processor.PictureDataQueue.Any())
                {
                    using (var fileStream = new FileStream(@"C:\Users\Tom\Pictures\Yolo\"+processor.PictureDataQueue.Peek().FileName, FileMode.Create, FileAccess.Write))
                    {
                        fileStream.Write(processor.PictureDataQueue.Peek().Image, 0, processor.PictureDataQueue.Dequeue().Image.Length);
                    }
                }
                System.Console.WriteLine(processor.MatchCount);
            }
        }
    }
}
