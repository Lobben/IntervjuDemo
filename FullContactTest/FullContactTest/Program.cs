using FullContactLib;
using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FullContactTest
{

    class Program {
        static async Task<int> AsyncMain()
        {
            string key = System.Configuration.ConfigurationManager.AppSettings["ApiKey"];
            FullContactApi proxy = new FullContactApi(key);
            string input="";

            do
            {
               input = await Task.Run(() => Console.ReadLine());
                if (input.IsEmail())
                {
                    Task.Run(()=>PrintFullContactPerson(proxy.LookupPersonByEmailAsync(input)));

                }
                else
                {
                    Console.WriteLine($"{input} is not a valid Email-adress!");
                }

            } while (input.ToUpper() != "EXIT");

            FullContactPerson personData = await proxy.LookupPersonByEmailAsync("lobben91@gmail.com");

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();

            return 0;

            
        }

        private static async Task PrintFullContactPerson(Task<FullContactPerson> task)
        {
            System.Console.WriteLine("\n - - - - \n"+(await task).likelihood);
        }

        static void Main(string[] args)
        {
            AsyncContext.Run(AsyncMain);
        }
    }
}
