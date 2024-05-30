using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static msptool.AMF;
using static msptool.Checksum;

namespace msptool
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter username: ");
            string username = Console.ReadLine();

            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            Console.Write("Enter server: ");
            string server = Console.ReadLine();

            dynamic login = AMFConn(server, "MovieStarPlanet.WebService.User.AMFUserServiceWeb.Login",
                                    new object[6] { username, password, new object[] { 134744072 }, null, null, "MSP1-Standalone:XXXXXX" });

            if (login["loginStatus"]["status"] != "Success")
            {
                Console.WriteLine("Login failed!");
            }
            else
            {
                int actorid = login["loginStatus"]["actor"]["ActorId"];
                string ticket = login["loginStatus"]["ticket"];
                Console.WriteLine("Login successful!");

                dynamic cloth = AMFConn(server, "MovieStarPlanet.WebService.ActorClothes.AMFActorClothes.GetActorClothesRelMinimals",
                                        new object[2]
                                        {
                                            new TicketHeader
                                            {
                                                Ticket = actor(ticket)
                                            },
                                            actorid
                                        });

                foreach (dynamic obj in cloth)
                {
                    object actorClothesRelId = obj["ActorClothesRelId"]; 

                    dynamic clothinfo = AMFConn(server, "MovieStarPlanet.WebService.MovieStar.AMFMovieStarService.GetActorClothesRel",
                                             new object[1] { actorClothesRelId });

            string  shop_id = clothinfo["Cloth"]["ShopId"].ToString();
                    string cloth_name = clothinfo["Cloth"]["Name"] ?? "Unknown";

                    if (shop_id != "-100")
                    {
                        dynamic recycler = AMFConn(server, "MovieStarPlanet.WebService.Profile.AMFProfileService.RecycleItem",
                                                 new object[4]
                                                 {
                                                     new TicketHeader
                                                     {
                                                         Ticket = actor(ticket)
                                                     },
                                                     actorid,
                                                    actorClothesRelId,
                                                     0
                                                 });
                        Console.WriteLine($"Recycled {cloth_name}");


                    }
                }
                Console.WriteLine("Finished recycling. Press any key to exit.");
                Console.ReadKey();
            }
        }
    }
}
