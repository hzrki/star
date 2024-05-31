using System;
using static msptool.AMF;
using static msptool.Checksum;

namespace msptool
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool loggedIn = false;

            while (!loggedIn)
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
                    Console.WriteLine("Login failed [Click any key to return to login]");
                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    loggedIn = true;
                    int actorid = login["loginStatus"]["actor"]["ActorId"];
                    string ticket = login["loginStatus"]["ticket"];
                    Console.WriteLine("Login successful!");
                    Console.Clear();

                    while (true)
                    {
                        Console.WriteLine("\n<Home>");
                        Console.WriteLine("1 | Recycle None-Rare Clothes");
                        Console.WriteLine("2 | Logout");

                        Console.Write("pick an option: ");
                        string options = Console.ReadLine();

                        switch (options)
                        {
                            case "1":
                                recycleNoneRareClothes(server, actorid, ticket);
                                break;
                            case "2":
                                Console.WriteLine("Logging out...");
                                Console.Clear();
                                loggedIn = false;
                                break;
                            default:
                                Console.WriteLine("choose a option which exists!");
                                break;
                        }

                        if (!loggedIn)
                            break;
                    }
                }
            }
        }

        static void recycleNoneRareClothes(string server, int actorid, string ticket)
        {
            dynamic cloth = AMFConn(server, "MovieStarPlanet.WebService.ActorClothes.AMFActorClothes.GetActorClothesRelMinimals",
                                    new object[2]
                                    {
                                        new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                                        actorid
                                    });

            foreach (dynamic obj in cloth)
            {
                object actorClothesRelId = obj["ActorClothesRelId"];

                dynamic clothinfo = AMFConn(server, "MovieStarPlanet.WebService.MovieStar.AMFMovieStarService.GetActorClothesRel",
                                         new object[1] { actorClothesRelId });

                string shop_id = clothinfo["Cloth"]["ShopId"].ToString();
                string cloth_name = clothinfo["Cloth"]["Name"] ?? "Unknown";

                if (shop_id != "-100")
                {
                    dynamic recycler = AMFConn(server, "MovieStarPlanet.WebService.Profile.AMFProfileService.RecycleItem",
                                             new object[4]
                                             {
                                                 new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                                                 actorid,
                                                 actorClothesRelId,
                                                 0
                                             });
                    Console.WriteLine($"Recycled {cloth_name}");
                }
            }
            Console.WriteLine("Finished recycling [Click any key to return to Home]");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
