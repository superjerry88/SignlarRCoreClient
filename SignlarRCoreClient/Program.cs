using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.VisualBasic;

namespace SignlarRCoreClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:5001/aas")
                .Build();

            Console.WriteLine("attempting to connect...");

            //setup an event where server reply us something
            connection.On<string>("ServerReply", OnShowMessage);

            // Loop is here to wait until the server is running
            while (true)
            {
                try
                {
                    await connection.StartAsync();
                    Console.WriteLine("connected i think ... sending message");
                    break;
                }
                catch
                {
                    await Task.Delay(1000);
                }
            }

            while (true)
            {
                Console.WriteLine("Send 'c' to end sending mode. Your message: ");
                var msg = Console.ReadLine();
                
                //this only send message and wait no reply
                await connection.SendCoreAsync("PostMessage", new object[] { msg });

                //end loop
                if (msg == "c") break;
            }

            Console.WriteLine("Hit enter to END program");
            Console.ReadLine();
        }


        private static void OnShowMessage(string obj)
        {
            Console.WriteLine($"Server say: {obj}");
        }
    }

  
}
