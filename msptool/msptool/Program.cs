using System;
 using System.Net.Http;
 using System.Threading.Tasks;
 using FluorineFx;
 using static msptool.AMF;
using static msptool.Checksum;

namespace msptool
{
    internal class Program
    {
        private static readonly string currentVersion = "1.2";

        private static readonly string checkVersion =
            "https://raw.githubusercontent.com/l3c1d/star/main/msptool/version.txt";

        static void Main(string[] args)
        {
            if (!isCurrentVersion())
            {
                Console.WriteLine("VERSION CHECK |" +
                                  " Update your application to the newest version! " +
                                  "| [Click any key to exit]");
                Console.ReadKey();
                return;
            }

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
                    string name = login["loginStatus"]["actor"]["Name"];
                    string ticket = login["loginStatus"]["ticket"];
                    string accessToken = login["loginStatus"]["nebulaLoginStatus"]["accessToken"];
                    string profileId = login["loginStatus"]["nebulaLoginStatus"]["profileId"];
                    Console.WriteLine("Login successful!");
                    Console.Clear();

                    while (true)
                    {
                        Console.WriteLine("\n<Home>");
                        Console.WriteLine("1 | recycle None-Rare Clothes");
                        Console.WriteLine("2 | buy Animations");
                        Console.WriteLine("3 | buy Clothes");
                        Console.WriteLine("4 | wear RareSkin");
                        Console.WriteLine("5 | buy eyes");
                        Console.WriteLine("6 | add to wishlist");
                        Console.WriteLine("7 | buy Nose");
                        Console.WriteLine("8 | buy Lips");
                        Console.WriteLine("9 | custom Status");
                        Console.WriteLine("10 | Logout");

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
                            case "4":
                                wearRareSkin(server, actorId, ticket);
                                break;
                            case "5":
                                buyEyes(server, actorId, ticket);
                                break;
                            case "6":
                                addToWishlist(server, actorId, ticket);
                                break;
                            case "7":
                                buyNose(server, actorId, ticket);
                                break;
                            case "8":
                                buyLips(server, actorId, ticket);
                                break;
                            case "9":
                                customStatus(server, name, actorId, ticket);
                                break;
                            case "10":
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
            string clothColor = Console.ReadLine();

            dynamic cloth = AMFConn(server, "MovieStarPlanet.WebService.AMFSpendingService.BuyClothes",
                new object[4]
                {
                    new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                    actorId,
                    new object[]
                    {
                        new
                        {
                            Color = clothColor,
                            y = 0,
                            ActorClothesRelId = 0,
                            ActorId = actorId,
                            ClothesId = clothId,
                            IsWearing = 1,
                            x = 0
                        },
                    },
                    0
                });

            if (cloth["Code"] != 0)
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

        static void buyNose(string server, int actorId, string ticket)
        {
            Console.Clear();
            Console.WriteLine("Enter Nose Id: ");
            int noseId = int.Parse(Console.ReadLine());


            dynamic nose = AMFConn(server,
                "MovieStarPlanet.WebService.BeautyClinic.AMFBeautyClinicService.BuyManyBeautyClinicItems",
                new object[3]
                {
                    new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                    actorId,
                    new object[]
                    {
                        new
                        {
                            IsOwned = false,
                            Type = 4,
                            IsWearing = true,
                            InventoryId = 0,
                            ItemId = noseId,
                            Colors = "",

                        }
                    }
                });
            if (nose[0]["InventoryId"] == 0)
            {
                Console.WriteLine("Failed | Unknown" +
                                  " | [Click any key to return to Home]");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                Console.WriteLine("Success | " +
                                  " Nose bought! " +
                                  "| [Click any key to return to Home]");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void buyLips(string server, int actorId, string ticket)
        {
            Console.Clear();
            Console.WriteLine("Enter Lips Id: ");
            int lipsId = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Lips Colors: ");
            string lipsColor = Console.ReadLine();


            dynamic lips = AMFConn(server,
                "MovieStarPlanet.WebService.BeautyClinic.AMFBeautyClinicService.BuyManyBeautyClinicItems",
                new object[3]
                {
                    new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                    actorId,
                    new object[]
                    {
                        new
                        {
                            IsOwned = false,
                            Type = 3,
                            IsWearing = true,
                            InventoryId = 0,
                            ItemId = lipsId,
                            Colors = lipsColor,

                        }
                    }
                });
            if (lips[0]["InventoryId"] == 0)
            {
                Console.WriteLine("Failed | Unknown" +
                                  " | [Click any key to return to Home]");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                Console.WriteLine("Success | " +
                                  " Lips bought! " +
                                  "| [Click any key to return to Home]");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void buyEyes(string server, int actorId, string ticket)
        {
            Console.Clear();
            Console.WriteLine("Enter eyeId: ");
            int eyeId = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter eye Color: ");
            string eyeColor = Console.ReadLine();


            dynamic eyes = AMFConn(server,
                "MovieStarPlanet.WebService.BeautyClinic.AMFBeautyClinicService.BuyManyBeautyClinicItems",
                new object[3]
                {
                    new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                    actorId,
                    new object[]
                    {
                        new
                        {
                            InventoryId = 0,
                            IsOwned = false,
                            ItemId = eyeId,
                            Colors = eyeColor,
                            Type = 1,
                            IsWearing = true

                        }
                    }
                });
            if (eyes[0]["InventoryId"] == 0)
            {
                Console.WriteLine("Failed | Unknown" +
                                  " | [Click any key to return to Home]");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                Console.WriteLine("Success | " +
                                  " Eye bought! " +
                                  "| [Click any key to return to Home]");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void wearRareSkin(string server, int actorId, string ticket)
        {
            Console.Clear();
            Console.WriteLine("Enter rareSkin Color: ");
            string skincolor = Console.ReadLine();


            dynamic skin = AMFConn(server,
                "MovieStarPlanet.WebService.BeautyClinic.AMFBeautyClinicService.BuyManyBeautyClinicItems",
                new object[3]
                {
                    new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                    actorId,
                    new object[]
                    {
                        new
                        {
                            InventoryId = 0,
                            Type = 5,
                            ItemId = -1,
                            Colors = skincolor,
                            IsWearing = true

                        }
                    }
                });
            if (skin[0]["InventoryId"] == 0)
            {
                Console.WriteLine("Failed | Unknown" +
                                  " | [Click any key to return to Home]");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                Console.WriteLine("Success | " +
                                  " Skin bought! " +
                                  "| [Click any key to return to Home]");
                Console.ReadKey();
                Console.Clear();
            }
        }


        static void customStatus(string server, string name, int actorId, string ticket)
        {
            Console.Clear();
            Console.WriteLine("Enter Status: ");
            string statustxt = (Console.ReadLine());
            Console.WriteLine("Enter Color: ");
            string statusColor = Console.ReadLine();

            dynamic status = AMFConn(server,
                "MovieStarPlanet.WebService.ActorService.AMFActorServiceForWeb.SetMoodWithModerationCall",
                new object[6]
                {
                    new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                    actorId,
                    new object[]
                    {
                        new
                        {
                            WallPostLinks = "",
                            FigureAnimation = "stand",
                            FaceAnimation = "neutral",
                            TextLine = statustxt,
                            SpeechLine = false,
                            IsBrag = false,
                            TextLineWhitelisted = "",
                            Likes = 0,
                            TextLineBlacklisted = "",
                            TextLineLastFiltered = "",
                            ActorId = actorId,
                            WallPostId = 0
                        },
                    },
                    name,
                    statusColor,
                    false
                });

        }

        static void addToWishlist(string server, int actorId, string ticket)
        {
            Console.Clear();
            Console.WriteLine("Enter clothesId: ");
            int clothId = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Clothes Color: ");
            string clothColor = Console.ReadLine();

            dynamic wishlist = AMFConn(server,
                "MovieStarPlanet.WebService.Gifts.AMFGiftsService+Version2.AddItemToWishlist",
                new object[3]
                {
                    new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                    new object[]
                    {
                        clothId
                    },
                    new object[]
                    {
                        clothColor
                    }
                });
        }

        static bool isCurrentVersion()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string latestVersion = client.GetStringAsync(checkVersion).Result;
                    return currentVersion.Trim() == latestVersion.Trim();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR");
                    return true; 
                }
            }
        }
    }
}
