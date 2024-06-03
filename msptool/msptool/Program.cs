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
        private static readonly string currentVersion = "1.3";

        private static readonly string checkVersion =
            "https://raw.githubusercontent.com/l3c1d/star/main/msptool/version.txt";

        static void Main(string[] args)
        {
            if (!isCurrentVersion())
            {
                while (true)
                {
                    Console.Write("\x1b[94mSTAR\x1b[39m ・ Update\n\n");
                    Console.WriteLine("[\x1b[95m!\u001b[39m] \u001b[93mAn update was found !\n");
                    Console.WriteLine("\u001b[94m1\u001b[39m > Install new update");
                    Console.WriteLine("\u001b[94m2\u001b[39m > Update manually\n");
                    Console.Write("[\u001b[95mUPDATE\u001b[39m] Pick an option: ");
                    string options = Console.ReadLine();
                    switch (options)
                    {
                        case "1":
                            Console.Write("Soon");
                            return;
                        case "2":
                            Console.WriteLine("\n\x1b[95mUPDATE\u001b[39m > \x1b[93mGo on https://github.com/l3c1d/star [Click any key to close]");
                            Console.ReadKey();
                            return;
                        default:
                            Console.WriteLine("\n\u001b[91mERROR\u001b[39m > \u001b[93mChoose a option which exists !");
                            System.Threading.Thread.Sleep(2000);
                            Console.Clear();
                            break;
                    }
                }
            }

            bool loggedIn = false;

            while (!loggedIn)
            {
                Console.Write("\x1b[94mSTAR\x1b[39m ・ Login\n\n");
                Console.Write("[\u001b[94m+\u001b[39m] Enter username: ");
                string username = Console.ReadLine();

                Console.Write("[\u001b[94m+\u001b[39m] Enter password: ");
                string password = Console.ReadLine();

                Console.Write("[\u001b[94m+\u001b[39m] Enter server: ");
                string server = Console.ReadLine();

                dynamic login = AMFConn(server, "MovieStarPlanet.WebService.User.AMFUserServiceWeb.Login",
                    new object[6]
                        { username, password, new object[] { 134744072 }, null, null, "MSP1-Standalone:XXXXXX" });

                if (login["loginStatus"]["status"] != "Success")
                {
                    Console.WriteLine("\n\x1b[91mFAILED\u001b[39m > \x1b[93mLogin failed [Click any key to return to login]");
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
                        Console.WriteLine("\u001b[94mSTAR\u001b[39m ・ Home\n");
                        Console.WriteLine("\u001b[94m1\u001b[39m > Recycle None-Rare Clothes");
                        Console.WriteLine("\u001b[94m2\u001b[39m > Buy Animations");
                        Console.WriteLine("\u001b[94m3\u001b[39m > Buy Clothes");
                        Console.WriteLine("\u001b[94m4\u001b[39m > Buy Eyes");
                        Console.WriteLine("\u001b[94m5\u001b[39m > Buy Nose");
                        Console.WriteLine("\u001b[94m6\u001b[39m > Buy Lips");
                        Console.WriteLine("\u001b[94m7\u001b[39m > Wear RareSkin");
                        Console.WriteLine("\u001b[94m8\u001b[39m > add to wishlist");
                        Console.WriteLine("\u001b[94m9\u001b[39m > Custom Status");
                        Console.WriteLine("\u001b[94m10\u001b[39m > Icon changer");
                        Console.WriteLine("\u001b[94m11\u001b[39m > Room changer");
                        Console.WriteLine("\u001b[94m12\u001b[39m > - Logout\n");

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
                                buyEyes(server, actorId, ticket);
                                break;
                            case "5":
                                buyNose(server, actorId, ticket);
                                break;
                            case "6":
                                buyLips(server, actorId, ticket);
                                break;
                            case "7":
                                wearRareSkin(server, actorId, ticket);
                                break;
                            case "8":
                                addToWishlist(server, actorId, ticket);
                                break;
                            case "9":
                                customStatus(server, name, actorId, ticket);
                                break;
                            case "10":
                                iconChanger(server, actorId, ticket);
                                break;
                            case "11":
                                roomChanger(server, actorId, ticket);
                                break;
                            case "12":
                                Console.WriteLine("\n\x1b[97mBYE\u001b[39m > \u001b[93mLogging out...");
                                Console.Clear();
                                loggedIn = false;
                                break;
                            default:
                                Console.WriteLine("\n\u001b[91mERROR\u001b[39m > \u001b[93mChoose a option which exists !");
                                System.Threading.Thread.Sleep(2000);
                                Console.Clear();
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
            Console.Clear();
            Console.WriteLine("\u001b[94mSTAR\u001b[39m ・ Home ・ Recycle None-Rare Clothes\n");
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
                    Console.WriteLine($"[\u001b[94m!\u001b[39m] Recycled {cloth_name}");
                }
            }

            Console.WriteLine("\n\x1b[92mSUCCESS\u001b[39m > \u001b[93m\u001b[4mFinished recycling\u001b[24m [Click any key to return to Home]");
            Console.ReadKey();
            Console.Clear();
        }

        static void buyAnimation(string server, int actorId, string ticket)
        {
            Console.Clear();
            Console.WriteLine("\u001b[94mSTAR\u001b[39m ・ Home ・ Buy Animations\n\n");
            Console.Write("[\u001b[94m+\u001b[39m] Enter AnimationId: ");
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
                Console.WriteLine("\n\u001b[91mFAILED\u001b[39m > \u001b[93m\u001b[4"
                                  + (animation["Description"] ?? "Unknown") +
                                  "\u001b[24m [Click any key to return to Home]");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                Console.WriteLine("\n\u001b[92mSUCCESS\u001b[39m > \u001b[93m\u001b[4mAnimation bought!\u001b[24m [Click any key to return to Home]");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void buyClothes(string server, int actorId, string ticket)
        {
            Console.Clear();
            Console.WriteLine("\u001b[94mSTAR\u001b[39m ・ Home ・ Buy Clothes\n\n");
            Console.Write("[\u001b[94m+\u001b[39m] Enter ClothesId: ");
            int clothId = int.Parse(Console.ReadLine());
            Console.Write("[\u001b[94m+\u001b[39m] Enter Color: ");
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
                Console.WriteLine("\n\u001b[91mFAILED\u001b[39m > \u001b[93m\x1b[4m"
                                  + (cloth["Description"] ?? "Unknown") +
                                  "\u001b[24m [Click any key to return to Home]");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                Console.WriteLine("\n\u001b[92mSUCCESS\u001b[39m > \u001b[93m\u001b[4mClothing bought!\u001b[24m [Click any key to return to Home]");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void buyNose(string server, int actorId, string ticket)
        {
            Console.Clear();
            Console.WriteLine("\u001b[94mSTAR\u001b[39m ・ Home ・ Buy Nose\n\n");
            Console.Write("[\u001b[94m+\u001b[39m] Enter Nose Id: ");
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
                Console.WriteLine("\n\u001b[91mFAILED\u001b[39m > \u001b[93m\u001b[4mUnknown\u001b[24m [Click any key to return to Home]");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                Console.WriteLine("\n\u001b[92mSUCCESS\u001b[39m > \u001b[93m\u001b[4mNose bought!\u001b[24m [Click any key to return to Home]");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void buyLips(string server, int actorId, string ticket)
        {
            Console.Clear();
            Console.WriteLine("\u001b[94mSTAR\u001b[39m ・ Home ・ Buy Lips\n\n");
            Console.Write("[\u001b[94m+\u001b[39m] Enter Lips Id: ");
            int lipsId = int.Parse(Console.ReadLine());
            Console.Write("[\u001b[94m+\u001b[39m] Enter Lips Colors: ");
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
                Console.WriteLine("\n\u001b[91mFAILED\u001b[39m > \u001b[93mUnknown [Click any key to return to Home]");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                Console.WriteLine("\n\u001b[92mSUCCESS\u001b[39m > \u001b[93m\u001b[4mLips bought!\u001b[24m [Click any key to return to Home]");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void buyEyes(string server, int actorId, string ticket)
        {
            Console.Clear();
            Console.WriteLine("\u001b[94mSTAR\u001b[39m ・ Home ・ Buy Eyes\n\n");
            Console.Write("[\u001b[94m+\u001b[39m] Enter eyeId: ");
            int eyeId = int.Parse(Console.ReadLine());
            Console.Write("[\u001b[94m+\u001b[39m] Enter eye Color: ");
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
                Console.WriteLine("\n\u001b[91mFAILED\u001b[39m > \u001b[93m\u001b[4mUnknown\u001b[24m [Click any key to return to Home]");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                Console.WriteLine("\n\u001b[92mSUCCESS\u001b[39m > \u001b[93m\u001b[4mEye bought!\u001b[24m [Click any key to return to Home]");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void wearRareSkin(string server, int actorId, string ticket)
        {
            Console.Clear();
            Console.Write("\u001b[94mSTAR\u001b[39m ・ Home ・ RareSkin\n\n");
            Console.Write("[\u001b[94m+\u001b[39m] Enter rareSkin Color: ");
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
                Console.WriteLine("\n\u001b[91mFAILED\u001b[39m > \u001b[93\u001b[4mmUnknown\u001b[24m [Click any key to return to Home]");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                Console.WriteLine("\n\u001b[92mSUCCESS\u001b[39m > \u001b[93m\u001b[4mSkin bought!\u001b[24m [Click any key to return to Home]");
                Console.ReadKey();
                Console.Clear();
            }
        }


        static void customStatus(string server, string name, int actorId, string ticket)
        {
            Console.Clear();
            Console.WriteLine("\u001b[94mSTAR\u001b[39m ・ Home ・ Status\n\n");
            Console.Write("[\u001b[94m+\u001b[39m] Enter Status: ");
            string statustxt = (Console.ReadLine());
            Console.Write("[\u001b[94m+\u001b[39m] Enter Color: ");
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
            Console.WriteLine("\u001b[94mSTAR\u001b[39m ・ Home ・ WishList\n\n");
            Console.Write("[\u001b[94m+\u001b[39m] Enter clothesId: ");
            int clothId = int.Parse(Console.ReadLine());
            Console.Write("[\u001b[94m+\u001b[39m] Enter Clothes Color: ");
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

        static void iconChanger(string server, int actorId, string ticket)
        {
            Console.Clear();
            Console.WriteLine("\u001b[94mSTAR\u001b[39m ・ Home ・ IconChanger\n\n");
            Console.Write("[\u001b[91m?!\u001b[39m] Use it at your own risk, we are not responsible for your misdeeds.\n");
            Console.Write("[\u001b[94m+\u001b[39m] Enter image url: ");
            string clothId = Console.ReadLine();
            System.Net.WebClient webClient = new System.Net.WebClient();
            byte[] array = webClient.DownloadData(clothId);

            dynamic moviestar = AMFConn(server,
                "MovieStarPlanet.WebService.Snapshots.AMFGenericSnapshotService.CreateSnapshot",
                new object[5]
                {
                    new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                    actorId,
                    "moviestar",
                    array,
                    "jpg"
                });

            dynamic fullSizeMoviestar = AMFConn(server,
                "MovieStarPlanet.WebService.Snapshots.AMFGenericSnapshotService.CreateSnapshot",
                new object[5]
                {
                    new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                    actorId,
                    "fullSizeMoviestar",
                    array,
                    "jpg"
                });
            if (moviestar && fullSizeMoviestar)
            {
                Console.WriteLine("\n\u001b[92mSUCCESS\u001b[39m > \u001b[93m\u001b[4mIcon changed\u001b[24m [Click any key to return to Home]");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                Console.WriteLine("\n\u001b[91mFAILED\u001b[39m > \u001b[93\u001b[4mmUnknown\u001b[24m [Click any key to return to Home]");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void roomChanger(string server, int actorId, string ticket)
        {
            Console.Clear();
            Console.WriteLine("\u001b[94mSTAR\u001b[39m ・ Home ・ RoomChanger\n\n");
            Console.Write("[\u001b[91m?!\u001b[39m] Use it at your own risk, we are not responsible for your misdeeds.\n");
            Console.Write("[\u001b[94m+\u001b[39m] Enter image url: ");
            string clothId = Console.ReadLine();
            System.Net.WebClient webClient = new System.Net.WebClient();
            byte[] array = webClient.DownloadData(clothId);

            dynamic room = AMFConn(server,
                "MovieStarPlanet.WebService.Snapshots.AMFGenericSnapshotService.CreateSnapshot",
                new object[5]
                {
                    new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                    actorId,
                    "room",
                    array,
                    "jpg"
                });

            dynamic roomProfile = AMFConn(server,
                "MovieStarPlanet.WebService.Snapshots.AMFGenericSnapshotService.CreateSnapshot",
                new object[5]
                {
                    new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                    actorId,
                    "roomProfile",
                    array,
                    "jpg"
                });
            dynamic roomMedium = AMFConn(server,
                "MovieStarPlanet.WebService.Snapshots.AMFGenericSnapshotService.CreateSnapshot",
                new object[5]
                {
                    new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                    actorId,
                    "roomMedium",
                    array,
                    "jpg"
                });
            if (room && roomProfile && roomMedium)
            {
                Console.WriteLine("\n\u001b[92mSUCCESS\u001b[39m > \u001b[93m\u001b[4mRoom changed\u001b[24m [Click any key to return to Home]");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                Console.WriteLine("\n\u001b[91mFAILED\u001b[39m > \u001b[93\u001b[4mmUnknown\u001b[24m [Click any key to return to Home]");
                Console.ReadKey();
                Console.Clear();
            }
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
