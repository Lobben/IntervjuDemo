using FullContactLib;
using System;
using System.Threading.Tasks;

namespace FullContactTest
{

    class Program {
        static void ConsoleLoop()
        {
            //We get the key from the App.config
            string key = System.Configuration.ConfigurationManager.AppSettings["ApiKey"];

            //create one instance of our client that generates 
            //  and executes our requests to the Fullcontact Person API
            FullContactApi client = new FullContactApi(key);
            
            //Holds the input from the user
            string input="";

            do
            {
                input = Console.ReadLine();

                //Check if the input is in email-format
                if (input.IsEmail())
                {
                    //Print out the fullcontactperson data using the email from input.
                    //We don't have to wait for the task to finish because we will stay in the loop
                    //until we close the program
                    Task.Run(()=>PrintFullContactPerson(client.LookupPersonByEmailAsync(input)));

                }
                else
                {
                    Console.WriteLine($"{input} is not a valid Email-adress!");
                }

            } while (input.ToUpper() != "EXIT");

        }

        //async function that Prints out the info in the FullContactPerson when the task is done
        private static async Task PrintFullContactPerson(Task<FullContactPerson> task)
        {
            System.Console.WriteLine((await task).ToString());
        }

        static void Main(string[] args)
        {
            //Run a function responsible for the consoleApplication functionality
            ConsoleLoop();

        }
    }
}
