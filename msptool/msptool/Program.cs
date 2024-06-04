z﻿using System;
using System.Data.SqlTypes;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using FluorineFx;
using Spectre.Console;
using static msptool.AMF;
using static msptool.Checksum;

namespace msptool
{
    internal class Program
    {
        private static readonly string currentVersion = "1.4";

        private static readonly string checkVersion =
            "https://raw.githubusercontent.com/l3c1d/star/main/msptool/version.txt";

        static void Main(string[] args)
        {
            if (!isCurrentVersion())
            {
                HttpClient client = new HttpClient();
                string latestVersion = client.GetStringAsync(checkVersion).Result;
                AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Update").LeftJustified());
                Console.Write("\n");
                AnsiConsole.Markup($"[#71d5fb]Go on and download last release[/] ・ [link=https://github.com/l3c1d/star/releases/tag/v{latestVersion}]github.com/l3c1d/star/releases/tag/v{latestVersion}[/]");
                Console.ReadKey();
                return;
                /*while (true)
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
                }*/
            }

            bool loggedIn = false;

            while (!loggedIn)
            {
                AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Login").LeftJustified());
                Console.Write("\n");
                var username = AnsiConsole.Prompt(new TextPrompt<string>("[[[#71d5fb]+[/]]] Enter username: ")
                                          .PromptStyle("#71d5fb"));

                var password = AnsiConsole.Prompt(new TextPrompt<string>("[[[#71d5fb]+[/]]] Enter password: ")
                                          .PromptStyle("#71d5fb")
                                          .Secret());

                var choices = new (string Name, string Value)[]
                {
                    ("United Kingdom", "GB"),
                    ("United States", "US"),
                    ("Türkiye", "TR"),
                    ("Sweden", "SE"),
                    ("France", "FR"),
                    ("Deutschland", "DE"),
                    ("Netherlands", "NL"),
                    ("Finland", "FI"),
                    ("Norway", "NO"),
                    ("Denmark", "DK"),
                    ("Canada", "CA"),
                    ("Australia", "AU"),
                    ("Poland", "PL"),
                    ("New Zealand", "NZ"),
                    ("Ireland", "IE"),
                    ("Spain", "ES")
                };

                var selectedCountry = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[[[#71d5fb]+[/]]] Select an server: ")
                        .PageSize(15)
                        .MoreChoicesText("[grey](Move up and down to reveal more servers)[/]")
                        .AddChoices(choices.Select(choice => choice.Name))
                );

                var selectedChoice = choices.First(choice => choice.Name == selectedCountry);
                
                dynamic login = null;
                string server = selectedChoice.Value;
                AnsiConsole.Status()
                    .SpinnerStyle(Spectre.Console.Style.Parse("#71d5fb"))
                    .Start("Login...", ctx =>
                    {
                        ctx.Refresh();
                        ctx.Spinner(Spinner.Known.Moon);
                        // Simulate some work
                        login = AMFConn(selectedChoice.Value, "MovieStarPlanet.WebService.User.AMFUserServiceWeb.Login",
                    new object[6]
                        { username, password, new object[] { 134744072 }, null, null, "MSP1-Standalone:XXXXXX" });
                        Thread.Sleep(1000);
                    });

                if (login == null) {
                    Console.WriteLine("\n\x1b[91mFAILED\u001b[39m > \x1b[93mUnknown [Click any key to return to login]");
                    Console.ReadKey();
                    Console.Clear();
                }
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
                    Console.Clear();

                    while (true)
                    {
                        AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home").LeftJustified().RoundedBorder());
                        Console.Write("\n");
                        AnsiConsole.Markup("[#71d5fb]1[/]  > Recycle None-Rare Clothes\n");
                        AnsiConsole.Markup("[#71d5fb]2[/]  > Buy Bonster\n");
                        AnsiConsole.Markup("[#71d5fb]3[/]  > Buy Animations\n");
                        AnsiConsole.Markup("[#71d5fb]4[/]  > Buy Clothes\n");
                        AnsiConsole.Markup("[#71d5fb]5[/]  > Buy Eyes\n");
                        AnsiConsole.Markup("[#71d5fb]6[/]  > Buy Nose\n");
                        AnsiConsole.Markup("[#71d5fb]7[/]  > Buy Lips\n");
                        AnsiConsole.Markup("[#71d5fb]8[/]  > Wear RareSkin\n");
                        AnsiConsole.Markup("[#71d5fb]9[/]  > Add to wishlist\n");
                        AnsiConsole.Markup("[#71d5fb]10[/] > Custom Status\n");
                        AnsiConsole.Markup("[#71d5fb]11[/] > Icon changer\n");
                        AnsiConsole.Markup("[#71d5fb]12[/] > Room changer\n");
                        AnsiConsole.Markup("[#71d5fb]13[/] > Logout\n\n");
                        AnsiConsole.Write(new Rule("[slowblink][#71d5fb]lucid & 6c0[/][/] ・ [link=https://discord.gg/msp1]discord.gg/msp1[/]").RightJustified().RoundedBorder());
                        var options = AnsiConsole.Prompt(new TextPrompt<string>("\n[[[#71d5fb]+[/]]] Pick an option: ")
                                          .PromptStyle("#71d5fb"));

                        switch (options)
                        {
                            case "1":
                                recycleNoneRareClothes(server, actorId, ticket);
                                break;
                            case "2":
                                buyBonster(server, actorId, ticket);
                                break;
                            case "3":
                                buyAnimation(server, actorId, ticket);
                                break;
                            case "4":
                                buyClothes(server, actorId, ticket);
                                break;
                            case "5":
                                buyEyes(server, actorId, ticket);
                                break;
                            case "6":
                                buyNose(server, actorId, ticket);
                                break;
                            case "7":
                                buyLips(server, actorId, ticket);
                                break;
                            case "8":
                                wearRareSkin(server, actorId, ticket);
                                break;
                            case "9":
                                addToWishlist(server, actorId, ticket);
                                break;
                            case "10":
                                customStatus(server, name, actorId, ticket);
                                break;
                            case "11":
                                iconChanger(server, actorId, ticket);
                                break;
                            case "12":
                                roomChanger(server, actorId, ticket);
                                break;
                            case "13":
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
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ Recycle None-Rare Clothes").LeftJustified().RoundedBorder());
            Console.Write("\n");
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
                    AnsiConsole.Markup($"[[[#71d5fb]![/]]] Recycled {cloth_name}");
                }
            }

            AnsiConsole.Markup("\n[#06c70c]SUCCESS[/] > [#f7b136][underline]Finished recycling[/] [[Click any key to return to Home]][/]");
            Console.ReadKey();
            Console.Clear();
        }

        static void buyBonster(string server, int actorId, string ticket)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ Buy Bonsters").LeftJustified().RoundedBorder());
            Console.Write("\n");
            int bonsterId = AnsiConsole.Prompt(new TextPrompt<int>("[[[#71d5fb]+[/]]] Enter BonsterId: ")
                                          .PromptStyle("#71d5fb"));

            dynamic bonster = AMFConn(server, "MovieStarPlanet.WebService.Bonster.AMFBonsterShopService.BuyBonster",
                new object[3]
                {
                    new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                    actorId,
                    bonsterId
                });
            if (bonster == null) {
                AnsiConsole.Markup("\n[#fa1414]FAILED[/] > [#f7b136][underline]Unknown[/] [[Click any key to return to Home]][/]");
                Console.ReadKey();
                Console.Clear();
                return;
            }
            if (bonster.ToString().Contains("(500) Internal Server Error")) {
                AnsiConsole.Markup("\n[#fa1414]FAILED[/] > [#f7b136][underline]Unknown[/] [[Click any key to return to Home]][/]");
                Console.ReadKey();
                Console.Clear();
                return;
            }
            if (bonster["ActorBonsterRelId"] != 0)
            {
                AnsiConsole.Markup("\n[#fa1414]FAILED[/] > [#f7b136][underline]Unknown[/] [[Click any key to return to Home]][/]");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                AnsiConsole.Markup("\n[#06c70c]SUCCESS[/] > [#f7b136][underline]Bonster bought![/] [[Click any key to return to Home]][/]");
                Console.ReadKey();
                Console.Clear();
            }
        }
        static void buyAnimation(string server, int actorId, string ticket)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ Buy Animations").LeftJustified().RoundedBorder());
            Console.Write("\n");
            int animationId = AnsiConsole.Prompt(new TextPrompt<int>("[[[#71d5fb]+[/]]] Enter AnimationId: ")
                                          .PromptStyle("#71d5fb"));

            dynamic animation = AMFConn(server, "MovieStarPlanet.WebService.Spending.AMFSpendingService.BuyAnimation",
                new object[3]
                {
                    new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                    actorId,
                    animationId
                });

            if (animation["Description"] != "null")
            {
                AnsiConsole.Markup("\n[#fa1414]FAILED[/] > [#f7b136][underline]"
                                  + (animation["Description"] ?? "Unknown") +
                                  "[/] [[Click any key to return to Home]][/]");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                AnsiConsole.Markup("\n[#06c70c]SUCCESS[/] > [#f7b136][underline]Animation bought![/] [[Click any key to return to Home]][/]");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void buyClothes(string server, int actorId, string ticket)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ Buy Clothes").LeftJustified().RoundedBorder());
            Console.Write("\n");
            int clothId = AnsiConsole.Prompt(new TextPrompt<int>("[[[#71d5fb]+[/]]] Enter ClothesId: ")
                                          .PromptStyle("#71d5fb"));
            string clothColor = AnsiConsole.Prompt(new TextPrompt<string>("[[[#71d5fb]+[/]]] Enter Color: ")
                                          .PromptStyle("#71d5fb"));

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
                AnsiConsole.Markup("\n[#fa1414]FAILED[/] > [#f7b136][underline]"
                                  + (cloth["Description"] ?? "Unknown") +
                                  "[/] [[Click any key to return to Home]][/]");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                AnsiConsole.Markup("\n[#06c70c]SUCCESS[/] > [#f7b136][underline]Clothing bought![/] [[Click any key to return to Home]][/]");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void buyNose(string server, int actorId, string ticket)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ Buy Nose").LeftJustified().RoundedBorder());
            Console.Write("\n");
            int noseId = AnsiConsole.Prompt(new TextPrompt<int>("[[[#71d5fb]+[/]]] Enter NoseId: ")
                                          .PromptStyle("#71d5fb"));

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
                AnsiConsole.Markup("\n[#fa1414]FAILED[/] > [#f7b136][underline]Unknown[/] [[Click any key to return to Home]][/]");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                AnsiConsole.Markup("\n[#06c70c]SUCCESS[/] > [#f7b136][underline]Nose bought![/] [[Click any key to return to Home]][/]");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void buyLips(string server, int actorId, string ticket)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ Buy Lips").LeftJustified().RoundedBorder());
            Console.Write("\n");
            int lipsId = AnsiConsole.Prompt(new TextPrompt<int>("[[[#71d5fb]+[/]]] Enter LipsId: ")
                                          .PromptStyle("#71d5fb"));
            string lipsColor = AnsiConsole.Prompt(new TextPrompt<string>("[[[#71d5fb]+[/]]] Enter Color: ")
                                          .PromptStyle("#71d5fb"));

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
                AnsiConsole.Markup("\n[#fa1414]FAILED[/] > [#f7b136][underline]Unknown[/] [[Click any key to return to Home]][/]");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                AnsiConsole.Markup("\n[#06c70c]SUCCESS[/] > [#f7b136][underline]Lips bought![/] [[Click any key to return to Home]][/]");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void buyEyes(string server, int actorId, string ticket)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ Buy Eyes").LeftJustified().RoundedBorder());
            Console.Write("\n");
            int eyeId = AnsiConsole.Prompt(new TextPrompt<int>("[[[#71d5fb]+[/]]] Enter EyeId: ")
                                          .PromptStyle("#71d5fb"));
            string eyeColor = AnsiConsole.Prompt(new TextPrompt<string>("[[[#71d5fb]+[/]]] Enter Color: ")
                                          .PromptStyle("#71d5fb"));

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
                AnsiConsole.Markup("\n[#fa1414]FAILED[/] > [#f7b136][underline]Unknown[/] [[Click any key to return to Home]][/]");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                AnsiConsole.Markup("\n[#06c70c]SUCCESS[/] > [#f7b136][underline]Eye bought![/] [[Click any key to return to Home]][/]");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void wearRareSkin(string server, int actorId, string ticket)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ RareSkin").LeftJustified().RoundedBorder());
            Console.Write("\n");
            string skincolor = AnsiConsole.Prompt(new TextPrompt<string>("[[[#71d5fb]+[/]]] Enter Color: ")
                                          .PromptStyle("#71d5fb"));

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
                AnsiConsole.Markup("\n[#fa1414]FAILED[/] > [#f7b136][underline]Unknown[/] [[Click any key to return to Home]][/]");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                AnsiConsole.Markup("\n[#06c70c]SUCCESS[/] > [#f7b136][underline]Skin bought![/] [[Click any key to return to Home]][/]");
                Console.ReadKey();
                Console.Clear();
            }
        }


        static void customStatus(string server, string name, int actorId, string ticket)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ Status").LeftJustified().RoundedBorder());
            Console.Write("\n");
            string statustxt = AnsiConsole.Prompt(new TextPrompt<string>("[[[#71d5fb]+[/]]] Enter Status: ")
                                          .PromptStyle("#71d5fb"));
            var choices = new (string Name, int Value)[]
            {
                ("Black", 0),
                ("Red", 13369344),
                ("Purple", 6684774),
                ("Light Purple", 6710988),
                ("Pink", 13369446),
                ("Green", 3368448),
                ("Orange", 16737792),
                ("Blue", 39372),
                ("Gray", 11187123)
            };

            var selectedColor = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[[[#71d5fb]+[/]]] Select a color: ")
                        .PageSize(10)
                        .AddChoices(choices.Select(choice => choice.Name))
                );

            var selectedChoice = choices.First(choice => choice.Name == selectedColor);

            dynamic status = AMFConn(server,
                "MovieStarPlanet.WebService.ActorService.AMFActorServiceForWeb.SetMoodWithModerationCall",
                new object[5]
                {
                    new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                    new
                        {
                            Likes = 0,
                            TextLine = statustxt,
                            TextLineLastFiltered = (object)null,
                            ActorId = actorId,
                            WallPostId = 0,
                            TextLineBlacklisted = "",
                            WallPostLinks = (object)null,
                            FigureAnimation = "Girl Pose",
                            FaceAnimation = "neutral",
                            MouthAnimation = "none",
                            SpeechLine = false,
                            IsBrag = false,
                            TextLineWhitelisted = ""
                        },
                    name,
                    selectedChoice.Value,
                    false
                });
            if (status["FilterTextResult"]["IsMessageOk"])
            {
                if (status["FilterTextResult"]["UnrestrictedPolicy"]["HasFilteredParts"])
                {
                    AnsiConsole.Markup("\n[#06c70c]SUCCESS[/] > [#f7b136][underline]Mood Set But Censored![/] [[Click any key to return to Home]][/]");
                    Console.ReadKey();
                    Console.Clear();
                    return;
                }
                AnsiConsole.Markup("\n[#06c70c]SUCCESS[/] > [#f7b136][underline]Mood Set![/] [[Click any key to return to Home]][/]");
                Console.ReadKey();
                Console.Clear();
                return;
            }
            else
            {
                AnsiConsole.Markup("\n[#fa1414]FAILED[/] > [#f7b136][underline]Unknown[/] [[Click any key to return to Home]][/]");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void addToWishlist(string server, int actorId, string ticket)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ WishList").LeftJustified().RoundedBorder());
            Console.Write("\n");
            int clothId = AnsiConsole.Prompt(new TextPrompt<int>("[[[#71d5fb]+[/]]] Enter ClothesId: ")
                                          .PromptStyle("#71d5fb"));
            string clothColor = AnsiConsole.Prompt(new TextPrompt<string>("[[[#71d5fb]+[/]]] Enter Color: ")
                                          .PromptStyle("#71d5fb"));

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
            if (wishlist != 0) {
                AnsiConsole.Markup("\n[#fa1414]FAILED[/] > [#f7b136][underline]Unknown[/] [[Click any key to return to Home]][/]");
            } else {
                AnsiConsole.Markup("\n[#06c70c]SUCCESS[/] > [#f7b136][underline]An cloth have been added in your wishlist[/] [[Click any key to return to Home]][/]");
            }
        }

        static void iconChanger(string server, int actorId, string ticket)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ IconChanger").LeftJustified().RoundedBorder());
            Console.Write("\n");
            AnsiConsole.Markup("[slowblink][[[#c70000]?![/]]] Use it at your own risk, we are not responsible for your misdeeds.[/]\n");
            string urlImage = AnsiConsole.Prompt(new TextPrompt<string>("[[[#71d5fb]+[/]]] Enter image url: ")
                                          .PromptStyle("#71d5fb"));
            System.Net.WebClient webClient = new System.Net.WebClient();
            byte[] array = webClient.DownloadData(urlImage);

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
                AnsiConsole.Markup("\n[#06c70c]SUCCESS[/] > [#f7b136][underline]Icon changed[/] [[Click any key to return to Home]][/]");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                AnsiConsole.Markup("\n[#fa1414]FAILED[/] > [#f7b136][underline]Unknown[/] [[Click any key to return to Home]][/]");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void roomChanger(string server, int actorId, string ticket)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ RoomChanger").LeftJustified().RoundedBorder());
            Console.Write("\n");
            AnsiConsole.Markup("[slowblink][[[#c70000]?![/]]] Use it at your own risk, we are not responsible for your misdeeds.[/]\n");
            string urlImage = AnsiConsole.Prompt(new TextPrompt<string>("[[[#71d5fb]+[/]]] Enter image url: ")
                                          .PromptStyle("#71d5fb"));
            System.Net.WebClient webClient = new System.Net.WebClient();
            byte[] array = webClient.DownloadData(urlImage);

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
                AnsiConsole.Markup("\n[#06c70c]SUCCESS[/] > [#f7b136][underline]Room changed[/] [[Click any key to return to Home]][/]");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                AnsiConsole.Markup("\n[#fa1414]FAILED[/] > [#f7b136][underline]Unknown[/] [[Click any key to return to Home]][/]");
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
