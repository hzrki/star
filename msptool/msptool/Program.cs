﻿using System;
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
                    new object[6]
                        { username, password, new object[] { 134744072 }, null, null, "MSP1-Standalone:XXXXXX" });

                if (login["loginStatus"]["status"] != "Success")
                {
                    Console.WriteLine("Login failed [Click any key to return to login]");
                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    loggedIn = true;
                    int actorId = login["loginStatus"]["actor"]["ActorId"];
                    string ticket = login["loginStatus"]["ticket"];
                    Console.WriteLine("Login successful!");
                    Console.Clear();

                    while (true)
                    {
                        Console.WriteLine("\n<Home>");
                        Console.WriteLine("1 | recycle None-Rare Clothes");
                        Console.WriteLine("2 | buy Animations");
                        Console.WriteLine("3 | buy Clothes");
                        Console.WriteLine("6 | Logout");

                        Console.Write("pick an option: ");
                        string options = Console.ReadLine();

                        switch (options)
                        {
                            case "1":
                                recycleNoneRareClothes(server, actorId, ticket);
                                break;
                            case "2":
                                buyAnimation(server, actorId, ticket);
                                break;
                            case "3":
                                buyClothes(server, actorId, ticket);
                                break;
                            case "6":
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

        static void recycleNoneRareClothes(string server, int actorId, string ticket)
        {
            dynamic cloth = AMFConn(server,
                "MovieStarPlanet.WebService.ActorClothes.AMFActorClothes.GetActorClothesRelMinimals",
                new object[2]
                {
                    new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                    actorId
                });

            foreach (dynamic obj in cloth)
            {
                object actorClothesRelId = obj["ActorClothesRelId"];

                dynamic clothinfo = AMFConn(server,
                    "MovieStarPlanet.WebService.MovieStar.AMFMovieStarService.GetActorClothesRel",
                    new object[1] { actorClothesRelId });

                string shop_id = clothinfo["Cloth"]["ShopId"].ToString();
                string cloth_name = clothinfo["Cloth"]["Name"] ?? "Unknown";

                if (shop_id != "-100")
                {
                    dynamic recycler = AMFConn(server,
                        "MovieStarPlanet.WebService.Profile.AMFProfileService.RecycleItem",
                        new object[4]
                        {
                            new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                            actorId,
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

        static void buyAnimation(string server, int actorId, string ticket)
        {
            Console.Clear();
            Console.WriteLine("Enter AnimationId: ");
            int animationId = int.Parse(Console.ReadLine());

            dynamic animation = AMFConn(server, "MovieStarPlanet.WebService.Spending.AMFSpendingService.BuyAnimation",
                new object[3]
                {
                    new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                    actorId,
                    animationId
                });
            
            if (animation["Description"] != "null")
            {
                Console.WriteLine("Failed | "
                                  + (animation["Description"] ?? "Unknown") + 
                                  " | [Click any key to return to Home]");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                Console.WriteLine("Success | " +
                                  " Animation bought! " +
                                  "| [Click any key to return to Home]");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void buyClothes(string server, int actorId, string ticket)
        {
            Console.Clear();
            Console.WriteLine("Enter ClothesId: ");
            int clothId = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Color: ");
            string clothcolor = Console.ReadLine();

            

            dynamic cloth = AMFConn(server, "MovieStarPlanet.WebService.AMFSpendingService.BuyClothes",
                new object[4]
                {
                    new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                    actorId,
                    new 
                    {
                        Color = clothcolor,
                        y = 0,
                        ActorClothesRelId = 0,
                        ActorId = actorId,
                        ClothesId = clothId,
                        IsWearing = 1,
                        x = 0
                    },
                    0
                });
            
            if (cloth["Description"] != "null")
            {
                Console.WriteLine("Failed | "
                                  + (cloth["Description"] ?? "Unknown") +
                                  " | [Click any key to return to Home]");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                Console.WriteLine("Success | " +
                                  " Clothing bought! " +
                                  "| [Click any key to return to Home]");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}