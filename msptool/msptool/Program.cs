using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;
using Spectre.Console;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using static msptool.AMF;
using static msptool.Checksum;
using WebClient = System.Net.WebClient;

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
                AnsiConsole.Markup(
                    $"[#71d5fb]Go on and download last release[/] ・ [link=https://github.com/l3c1d/star/releases/tag/v{latestVersion}]github.com/l3c1d/star/releases/tag/v{latestVersion}[/]");
                Console.ReadKey();
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
                            Console.WriteLine(
                                "\n\x1b[95mUPDATE\u001b[39m > \x1b[93mGo on https://github.com/l3c1d/star [Click any key to close]");
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

            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Choose").LeftJustified());
            Console.Write("\n");

            var selectedLogin = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[[[#71d5fb]+[/]]] Select which MSP you want to use")
                    .PageSize(3)
                    .AddChoices(new[] { "MovieStarPlanet", "MovieStarPlanet 2" })
            );

            if (selectedLogin == "MovieStarPlanet")
                MSP1_Login();
            else
                MSP2_Login();

        }

        static void MSP1_Login()
        {
            Console.Clear();
            bool loggedIn = false;
            while (!loggedIn)
            {
                AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Login MSP").LeftJustified());
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
                        login = AMFConn(selectedChoice.Value, "MovieStarPlanet.WebService.User.AMFUserServiceWeb.Login",
                            new object[6]
                            {
                                username, password, new object[] {  }, null, null, "MSP1-Standalone:XXXXXX"
                            });
                        Thread.Sleep(1000);
                    });

                if (login == null)
                {
                    Console.WriteLine(
                        "\n\x1b[91mFAILED\u001b[39m > \x1b[93mUnknown [Click any key to return to login]");
                    Console.ReadKey();
                    Console.Clear();
                }

                if (login["loginStatus"]["status"] != "Success")
                {
                    Console.WriteLine(
                        "\n\x1b[91mFAILED\u001b[39m > \x1b[93mLogin failed [Click any key to return to login]");
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
                    var th = new JwtSecurityTokenHandler();
                    var jtoken = th.ReadJwtToken(accessToken);
                    var loginId = jtoken.Payload["loginId"].ToString();
                    Console.Clear();

                    while (true)
                    {
                        AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home").LeftJustified().RoundedBorder());
                        Console.Write("\n");
                        AnsiConsole.Markup("[#71d5fb]1[/]  > Recycle None-Rare Clothes\n");
                        AnsiConsole.Markup("[#71d5fb]2[/]  > Buy Boonie\n");
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
                        AnsiConsole.Markup("[#71d5fb]13[/] > Add Sponsors\n");
                        AnsiConsole.Markup("[#71d5fb]14[/] > Block Zac, Pixi, nova\n");
                        AnsiConsole.Markup("[#71d5fb]15[/] > Recycle Diamond Items\n");
                        AnsiConsole.Markup("[#71d5fb]16[/] > Wheel Spins\n");
                        AnsiConsole.Markup("[#71d5fb]17[/] > Lisa Hack\n");
                        AnsiConsole.Markup("[#71d5fb]18[/] > Automated Autographer\n");
                        AnsiConsole.Markup("[#71d5fb]19[/] > Automated Pixeller\n");
                        AnsiConsole.Markup("[#71d5fb]20[/] > Item Glitcher\n");
                        AnsiConsole.Markup("[#71d5fb]21[/] > Logout\n\n");
                        AnsiConsole.Write(
                            new Rule(
                                    "[slowblink][#71d5fb]lucid & 6c0[/][/] ・ [link=https://discord.gg/starmsp]discord.gg/starmsp[/]")
                                .RightJustified().RoundedBorder());
                        var options = AnsiConsole.Prompt(new TextPrompt<string>("\n[[[#71d5fb]+[/]]] Pick an option: ")
                            .PromptStyle("#71d5fb"));

                        switch (options)
                        {
                            case "1":
                                recycleNoneRareClothes(server, actorId, ticket);
                                break;
                            case "2":
                                buyBoonie(server, actorId, ticket);
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
                                addSponsors(server, ticket);
                                break;
                            case "14":
                                blockDefaults(server, actorId, ticket);
                                break;
                            case "15":
                                recycleitems(server, actorId, ticket);
                                break;
                            case "16":
                                wheelspins(server, actorId, ticket);
                                break;
                            case "17":
                                lisaHack(server, ticket);
                                break;
                            case "18":
                                automatedAutographer(server, ticket);
                                break;
                            case "19":
                                automatedPixeller(server, ticket);
                                break;
                            case "20":
                                itemGlitcher(server, ticket);
                                break;
                            case "21":
                                Console.WriteLine("\n\x1b[97mBYE\u001b[39m > \u001b[93mLogging out...");
                                Console.Clear();
                                loggedIn = false;
                                break;
                            default:
                                Console.WriteLine(
                                    "\n\u001b[91mERROR\u001b[39m > \u001b[93mChoose a option which exists !");
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
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ Recycle None-Rare Clothes")
                .LeftJustified()
                .RoundedBorder());
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

            AnsiConsole.Markup(
                "\n[#06c70c]SUCCESS[/] > [#f7b136][underline]Finished recycling[/] [[Click any key to return to Home]][/]");
            Console.ReadKey();
            Console.Clear();
        }

        static void buyBoonie(string server, int actorId, string ticket)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ Buy Boonie").LeftJustified()
                .RoundedBorder());

            var bonnieOptions = new (string Name, int Value)[]
            {
                    ("Light Side Boonie", 1),
                    ("Dark Side Boonie", 2),
                    ("VIP Boonie", 3),
                    ("FOX", 4),
                    ("DOG", 5),
                    ("PLANT", 6),
                    ("DRAGON", 7),
                    ("Metat Eater", 8),
                    ("Xmas Boonie", 9),
                    ("Valentine Boonie", 10),
                    ("Diamond Boonie", 11),
                    ("Easter Bunny", 12),
                    ("Diamond Squirrel", 13),
                    ("Poodle", 14),
                    ("Summer Boonie", 15),
                    ("Gamer Bunny", 16),
                    ("Brad Pet", 17),
                    ("Magazine Pet", 18),
                    ("Puppy", 19),
                    ("Halloween Boonie", 20),
                    ("Space Boonie", 21),
                    ("Xmax Boonie 2012", 22),
                    ("New Years Boonie 2012", 23),
                    ("Elements 2013 Boonie", 24),
                    ("Valentines 2013 Boonie", 25),
                    ("Australia 2013 Boonie", 26),
                    ("EgmontMagazine1Boonie", 27),
                    ("Easter 2013 Boonie", 29),
                    ("Tutti Frutti 2013 Boonie", 30),
                    ("Birthday 2013 Boonie", 31),
                    ("Mexican 2013 Boonie", 32),
                    ("Fastfood 2013 Boonie", 33),
                    ("Rio 2013 Boonie", 34),
                    ("Night Sky 2013 Boonie", 35),
                    ("Wonderland Boonie", 37),
                    ("Robots 2013", 38),
                    ("Halloween 2013", 39),
                    ("Winter Wonderland 2013", 40),
                    ("Christmas 2013", 41),
                    ("New Year 2013", 42),
                    ("Egmont Mag 10", 43),
                    ("Egmont Mag 2014", 46)
            };

            var selectedBoonie = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[[[#71d5fb]+[/]]] Select a boonie: ")
                    .PageSize(10)
                    .AddChoices(bonnieOptions.Select(choice => choice.Name))
            );

            var selectedChoice = bonnieOptions.First(choice => choice.Name == selectedBoonie);


            dynamic boonie = AMFConn(server,
                "MovieStarPlanet.WebService.Pets.AMFPetService.BuyClickItem",
                new object[3]
                {
                        new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                        actorId,
                        selectedChoice.Value
                });
            if (boonie["SkinSWF"] != "femaleskin" && boonie["SkinSWF"] != "maleskin")
            {
                AnsiConsole.Markup(
                    "\n[#fa1414]FAILED[/] > [#f7b136][underline]Unknown[/] [[Click any key to return to Home]][/]");
                Console.ReadKey();
                Console.Clear();
                return;
            }
            else
            {
                AnsiConsole.Markup(
                    "\n[#06c70c]SUCCESS[/] > [#f7b136][underline]Boonie bought![/] [[Click any key to return to Home]][/]");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void buyAnimation(string server, int actorId, string ticket)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ Buy Animations").LeftJustified()
                .RoundedBorder());
            Console.Write("\n");
            int animationId = AnsiConsole.Prompt(
                new TextPrompt<int>("[[[#71d5fb]+[/]]] Enter AnimationId: ")
                    .PromptStyle("#71d5fb"));

            dynamic animation = AMFConn(server,
                "MovieStarPlanet.WebService.Spending.AMFSpendingService.BuyAnimation",
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
                AnsiConsole.Markup(
                    "\n[#06c70c]SUCCESS[/] > [#f7b136][underline]Animation bought![/] [[Click any key to return to Home]][/]");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void buyClothes(string server, int actorId, string ticket)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ Buy Clothes").LeftJustified()
                .RoundedBorder());
            Console.Write("\n");
            int clothId = AnsiConsole.Prompt(new TextPrompt<int>("[[[#71d5fb]+[/]]] Enter ClothesId: ")
                .PromptStyle("#71d5fb"));
            string clothColor = AnsiConsole.Prompt(
                new TextPrompt<string>("[[[#71d5fb]+[/]]] Enter Color: ")
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
                AnsiConsole.Markup(
                    "\n[#06c70c]SUCCESS[/] > [#f7b136][underline]Clothing bought![/] [[Click any key to return to Home]][/]");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void buyNose(string server, int actorId, string ticket)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ Buy Nose").LeftJustified()
                .RoundedBorder());
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
                AnsiConsole.Markup(
                    "\n[#fa1414]FAILED[/] > [#f7b136][underline]Unknown[/] [[Click any key to return to Home]][/]");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                AnsiConsole.Markup(
                    "\n[#06c70c]SUCCESS[/] > [#f7b136][underline]Nose bought![/] [[Click any key to return to Home]][/]");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void buyLips(string server, int actorId, string ticket)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ Buy Lips").LeftJustified()
                .RoundedBorder());
            Console.Write("\n");
            int lipsId = AnsiConsole.Prompt(new TextPrompt<int>("[[[#71d5fb]+[/]]] Enter LipsId: ")
                .PromptStyle("#71d5fb"));
            string lipsColor = AnsiConsole.Prompt(
                new TextPrompt<string>("[[[#71d5fb]+[/]]] Enter Color: ")
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
                AnsiConsole.Markup(
                    "\n[#fa1414]FAILED[/] > [#f7b136][underline]Unknown[/] [[Click any key to return to Home]][/]");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                AnsiConsole.Markup(
                    "\n[#06c70c]SUCCESS[/] > [#f7b136][underline]Lips bought![/] [[Click any key to return to Home]][/]");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void buyEyes(string server, int actorId, string ticket)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ Buy Eyes").LeftJustified()
                .RoundedBorder());
            Console.Write("\n");
            int eyeId = AnsiConsole.Prompt(new TextPrompt<int>("[[[#71d5fb]+[/]]] Enter EyeId: ")
                .PromptStyle("#71d5fb"));
            string eyeColor = AnsiConsole.Prompt(
                new TextPrompt<string>("[[[#71d5fb]+[/]]] Enter Color: ")
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
                AnsiConsole.Markup(
                    "\n[#fa1414]FAILED[/] > [#f7b136][underline]Unknown[/] [[Click any key to return to Home]][/]");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                AnsiConsole.Markup(
                    "\n[#06c70c]SUCCESS[/] > [#f7b136][underline]Eye bought![/] [[Click any key to return to Home]][/]");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void wearRareSkin(string server, int actorId, string ticket)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ RareSkin").LeftJustified()
                .RoundedBorder());
            Console.Write("\n");
            string skincolor = AnsiConsole.Prompt(
                new TextPrompt<string>("[[[#71d5fb]+[/]]] Enter Color: ")
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
                AnsiConsole.Markup(
                    "\n[#fa1414]FAILED[/] > [#f7b136][underline]Unknown[/] [[Click any key to return to Home]][/]");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                AnsiConsole.Markup(
                    "\n[#06c70c]SUCCESS[/] > [#f7b136][underline]Skin bought![/] [[Click any key to return to Home]][/]");
                Console.ReadKey();
                Console.Clear();
            }
        }


        static void customStatus(string server, string name, int actorId, string ticket)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ Status").LeftJustified()
                .RoundedBorder());
            Console.Write("\n");
            string statustxt = AnsiConsole.Prompt(
                new TextPrompt<string>("[[[#71d5fb]+[/]]] Enter Status: ")
                    .PromptStyle("#71d5fb"));
            var colorOptions = new (string Name, int Value)[]
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
                    .AddChoices(colorOptions.Select(choice => choice.Name))
            );

            var selectedChoice = colorOptions.First(choice => choice.Name == selectedColor);

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
                    AnsiConsole.Markup(
                        "\n[#06c70c]SUCCESS[/] > [#f7b136][underline]Mood Set But Censored![/] [[Click any key to return to Home]][/]");
                    Console.ReadKey();
                    Console.Clear();
                    return;
                }

                AnsiConsole.Markup(
                    "\n[#06c70c]SUCCESS[/] > [#f7b136][underline]Mood Set![/] [[Click any key to return to Home]][/]");
                Console.ReadKey();
                Console.Clear();
                return;
            }
            else
            {
                AnsiConsole.Markup(
                    "\n[#fa1414]FAILED[/] > [#f7b136][underline]Unknown[/] [[Click any key to return to Home]][/]");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void addSponsors(string server, string ticket)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ Status").LeftJustified()
                .RoundedBorder());
            Console.Write("\n");

            List<int> anchorCharacterList = new List<int>
                    { 273, 276, 277, 341, 418, 419, 420, 421, 83417, 83423, 83424 };

            foreach (int anchorId in anchorCharacterList)
            {
                dynamic anchor = AMFConn(server,
                    "MovieStarPlanet.WebService.AnchorCharacter.AMFAnchorCharacterService.RequestFriendship",
                    new object[2]
                    {
                            new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                            anchorId
                    });
                Console.WriteLine($"Added: {anchorId}");

            }
        }

        static void blockDefaults(string server, int actorId, string ticket)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ Status").LeftJustified()
                .RoundedBorder());
            Console.Write("\n");

            List<int> mspDefaults = new List<int>
                    { 3, 4, 414 };

            foreach (int defaultId in mspDefaults)
            {
                dynamic mspdefaults = AMFConn(server,
                    "MovieStarPlanet.WebService.ActorService.AMFActorServiceForWeb.BlockActor",
                    new object[3]
                    {
                            new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                            actorId,
                            defaultId
                    });
                Console.WriteLine($"Blocked: {defaultId}");

            }
        }

        static void recycleitems(string server, int actorId, string ticket)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ Status").LeftJustified()
                .RoundedBorder());
            Console.Write("\n");

            Console.Write("Enter Item relid: ");
            int relId = int.Parse(Console.ReadLine());


            dynamic recycleitems = AMFConn(server,
                "MovieStarPlanet.WebService.Profile.AMFProfileService.RecycleItem",
                new object[4]
                {
                        new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                        actorId,
                        relId,
                        0
                });
        }

        static void wheelspins(string server, int actorId, string ticket)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ Status").LeftJustified()
                .RoundedBorder());
            Console.Write("\n");

            ClaimDailyAward(server, ticket, "starwheel", 120, actorId, 4);
            ClaimDailyAward(server, ticket, "starVipWheel", 200, actorId, 4);
            ClaimDailyAward(server, ticket, "advertWheelDwl", 240, actorId, 2);
            ClaimDailyAward(server, ticket, "advertWheelVipDwl", 400, actorId, 2);
        }

        static void ClaimDailyAward(string server, string ticket, string awardType, int awardVal,
            int actorId,
            int count)
        {
            for (int i = 0; i < count; i++)
            {
                dynamic result = AMFConn(server,
                    "MovieStarPlanet.WebService.Awarding.AMFAwardingService.claimDailyAward",
                    new object[4]
                    {
                            new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                            awardType,
                            awardVal,
                            actorId
                    });
                Console.WriteLine("Spinning Wheels...");
                Console.Clear();
            }
        }


        static void addToWishlist(string server, int actorId, string ticket)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ WishList").LeftJustified()
                .RoundedBorder());
            Console.Write("\n");
            int clothId = AnsiConsole.Prompt(new TextPrompt<int>("[[[#71d5fb]+[/]]] Enter ClothesId: ")
                .PromptStyle("#71d5fb"));
            string clothColor = AnsiConsole.Prompt(
                new TextPrompt<string>("[[[#71d5fb]+[/]]] Enter Color: ")
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
            if (wishlist != 0)
            {
                AnsiConsole.Markup(
                    "\n[#fa1414]FAILED[/] > [#f7b136][underline]Unknown[/] [[Click any key to return to Home]][/]");
            }
            else
            {
                AnsiConsole.Markup(
                    "\n[#06c70c]SUCCESS[/] > [#f7b136][underline]An cloth have been added in your wishlist[/] [[Click any key to return to Home]][/]");
            }
        }

        static void iconChanger(string server, int actorId, string ticket)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ IconChanger").LeftJustified()
                .RoundedBorder());
            Console.Write("\n");
            AnsiConsole.Markup(
                "[slowblink][[[#c70000]?![/]]] Use it at your own risk, we are not responsible for your misdeeds.[/]\n");
            string urlImage = AnsiConsole.Prompt(
                new TextPrompt<string>("[[[#71d5fb]+[/]]] Enter image url: ")
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
                AnsiConsole.Markup(
                    "\n[#06c70c]SUCCESS[/] > [#f7b136][underline]Icon changed[/] [[Click any key to return to Home]][/]");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                AnsiConsole.Markup(
                    "\n[#fa1414]FAILED[/] > [#f7b136][underline]Unknown[/] [[Click any key to return to Home]][/]");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void roomChanger(string server, int actorId, string ticket)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ RoomChanger").LeftJustified()
                .RoundedBorder());
            Console.Write("\n");
            AnsiConsole.Markup(
                "[slowblink][[[#c70000]?![/]]] Use it at your own risk, we are not responsible for your misdeeds.[/]\n");
            string urlImage = AnsiConsole.Prompt(
                new TextPrompt<string>("[[[#71d5fb]+[/]]] Enter image url: ")
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
                AnsiConsole.Markup(
                    "\n[#06c70c]SUCCESS[/] > [#f7b136][underline]Room changed[/] [[Click any key to return to Home]][/]");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                AnsiConsole.Markup(
                    "\n[#fa1414]FAILED[/] > [#f7b136][underline]Unknown[/] [[Click any key to return to Home]][/]");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void lisaHack(string server, string ticket)
        {
            Console.Write("soon");
            Console.ReadKey();
            Console.Clear();
        }

        static void automatedAutographer(string server, string ticket)
        {
            Console.Write("soon");
            Console.ReadKey();
            Console.Clear();
        }
        static void automatedPixeller(string server, string ticket)
        {
            Console.Write("soon");
            Console.ReadKey();
            Console.Clear();
        }
        static void itemGlitcher(string server, string ticket)
        {
            Console.Write("soon");
            Console.ReadKey();
            Console.Clear();
        }

        static async Task MSP2_Login()
        {
            Console.Clear();
            bool loggedIn2 = false;

            while (!loggedIn2)
            {
                AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Login MSP2").LeftJustified());
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
                        .Title("[[[#71d5fb]+[/]]] Select a server: ")
                        .PageSize(15)
                        .MoreChoicesText("[grey](Move up and down to reveal more servers)[/]")
                        .AddChoices(choices.Select(choice => choice.Name))
                );

                var server = choices.First(choice => choice.Name == selectedCountry).Value;
                var region = new[] { "US", "CA", "AU", "NZ" }.Contains(server) ? "us" : "eu";

                string accessToken = null;
                string profileId = null;

                AnsiConsole.Status()
                    .SpinnerStyle(Spectre.Console.Style.Parse("#71d5fb"))
                    .Start("Login...", ctx =>
                    {
                        ctx.Refresh();
                        ctx.Spinner(Spinner.Known.Moon);

                        var tep = $"https://{region}-secure.mspapis.com/loginidentity/connect/token";

                        using (var msptclient = new WebClient())
                        {
                            var val = new NameValueCollection
                            {
                                ["client_id"] = "unity.client",
                                ["client_secret"] = "secret",
                                ["grant_type"] = "password",
                                ["scope"] = "openid nebula offline_access",
                                ["username"] = $"{server}|{username}",
                                ["password"] = password,
                                ["acr_values"] = "gameId:j68d"
                            };

                            var resp = msptclient.UploadValues(tep, val);
                            var resp1 = Encoding.Default.GetString(resp);
                            dynamic resp2 = JsonConvert.DeserializeObject(resp1);


                            var accessToken_first = resp2["access_token"].ToString();
                            var refreshToken = resp2["refresh_token"].ToString();

                            var th = new JwtSecurityTokenHandler();
                            var jtoken = th.ReadJwtToken(accessToken_first);
                            var loginId = jtoken.Payload["loginId"].ToString();

                            string pid =
                                $"https://{region}.mspapis.com/profileidentity/v1/logins/{loginId}/profiles?&pageSize=100&page=1&filter=region:{server}";
                            msptclient.Headers.Add(HttpRequestHeader.Authorization,
                                "Bearer " + accessToken_first);
                            string resp3 = msptclient.DownloadString(pid);

                            profileId = JArray.Parse(resp3)[0]["id"].ToString();

                            var val2 = new NameValueCollection
                            {
                                ["grant_type"] = "refresh_token",
                                ["refresh_token"] = refreshToken,
                                ["acr_values"] = $"gameId:j68d profileId:{profileId}"
                            };

                            msptclient.Headers.Remove(HttpRequestHeader.Authorization);
                            msptclient.Headers.Add(HttpRequestHeader.Authorization,
                                "Basic dW5pdHkuY2xpZW50OnNlY3JldA==");
                            var resp4 = msptclient.UploadValues(tep, val2);

                            var resp5 = Encoding.Default.GetString(resp4);
                            dynamic resp6 = JsonConvert.DeserializeObject(resp5);

                            accessToken = resp6["access_token"].ToString();

                            Console.Clear();
                        }
                    });
                while (true)
                {
                    loggedIn2 = true;
                    Console.Clear();
                    AnsiConsole.Write(
                        new Rule("[#71d5fb]MSPTOOL[/] ・ Home").LeftJustified().RoundedBorder());
                    Console.Write("\n");
                    AnsiConsole.Markup("[#71d5fb]1[/]  > Mood Changer\n");
                    AnsiConsole.Markup("[#71d5fb]2[/]  > Gender Changer\n");
                    AnsiConsole.Markup("[#71d5fb]3[/]  > Delete Room\n");
                    AnsiConsole.Markup("[#71d5fb]4[/]  > Logout\n\n");
                    AnsiConsole.Write(
                        new Rule(
                                "[slowblink][#71d5fb]lucid & 6c0[/][/] ・ [link=https://discord.gg/starmsp]discord.gg/starmsp[/]")
                            .RightJustified().RoundedBorder());
                    var options = AnsiConsole.Prompt(
                        new TextPrompt<string>("\n[[[#71d5fb]+[/]]] Pick an option: ")
                            .PromptStyle("#71d5fb"));

                    switch (options)
                    {
                        case "1":
                            moodChanger(region, accessToken, profileId);
                            Thread.Sleep(2000);
                            break;
                        case "2":
                            genderChanger(region, accessToken, profileId);
                            Thread.Sleep(2000);
                            break;
                        case "3":
                            deleteRoom(region, accessToken, profileId);
                            Thread.Sleep(2000);
                            break;
                        case "4":
                            loggedIn2 = false;
                            break;
                        default:
                            Console.WriteLine(
                                "\n\u001b[91mERROR\u001b[39m > \u001b[93mChoose an option which exists!");
                            System.Threading.Thread.Sleep(2000);
                            Console.Clear();
                            break;
                    }

                    if (!loggedIn2)
                        break;
                };
            }
        }

        static async Task moodChanger(string region, string accessToken, string profileId)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ Change Mood").LeftJustified().RoundedBorder());

            var moodOptions = new (string Name, string Value)[]
            {
                ("Bunny", "bunny_hold"),
                ("Ice Skating", "noshoes_skating"),
                ("Swimming", "swim_new"),
                ("Spider Crawl", "2023_spidercrawl_lsz"),
                ("Bubblegum", "bad_2022_teenwalk_dg"),
                ("Like a Frog", "very_2022_froglike_lsz"),
                ("Cool Slide", "cool_slide"),
                ("Like Bambi", "bambislide"),
                ("Freezing", "xmas_2022_freezing_lsz"),
            };

            var selectedMood = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[[[#71d5fb]+[/]]] Select a mood: ")
                    .PageSize(10)
                    .AddChoices(moodOptions.Select(choice => choice.Name))
            );

            var selectedChoice = moodOptions.First(choice => choice.Name == selectedMood);

            using (HttpClient mt2client = new HttpClient())
            {
                mt2client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                mt2client.DefaultRequestHeaders.UserAgent.ParseAdd(
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/125.0.0.0 Safari/537.36 Edg/125.0.0.0");

                string moodApi =
                    $"https://{region}.mspapis.com/profileattributes/v1/profiles/{profileId}/games/j68d/attributes";

                HttpResponseMessage resp = await mt2client.GetAsync(moodApi);

                string resp2 = await resp.Content.ReadAsStringAsync();
                JObject moodData = JObject.Parse(resp2);

                moodData["additionalData"]["Mood"] = selectedChoice.Value;

                string loc1 = moodData.ToString();
                HttpContent loc2 = new StringContent(loc1);
                loc2.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage resp3 = await mt2client.PutAsync(moodApi, loc2);
                if (resp3.IsSuccessStatusCode)
                {
                    AnsiConsole.Markup(
                        "\n[#06c70c]SUCCESS[/] > [#f7b136][underline]Mood changed[/] [[Auto redirect in 2 seconds]][/]");
                }
                else
                {
                    AnsiConsole.Markup(
                        "\n[#fa1414]FAILED[/] > [#f7b136][underline]Unknown[/] [[Auto redirect in 2 seconds]][/]");
                    
                }
            }
        }

        static async Task genderChanger(string region, string accessToken, string profileId)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ Change Gender").LeftJustified().RoundedBorder());

            var genderOptions = new (string Name, string Value)[]
            {
                ("Girl", "Girl"),
                ("Boy", "Boy"),
            };

            var selectedGender = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[[[#71d5fb]+[/]]] Select a mood: ")
                    .PageSize(10)
                    .AddChoices(genderOptions.Select(choice => choice.Name))
            );

            var selectedChoice = genderOptions.First(choice => choice.Name == selectedGender);

            using (HttpClient mt2client = new HttpClient())
            {
                mt2client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                mt2client.DefaultRequestHeaders.UserAgent.ParseAdd(
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/125.0.0.0 Safari/537.36 Edg/125.0.0.0");

                string genderApi =
                    $"https://{region}.mspapis.com/profileattributes/v1/profiles/{profileId}/games/j68d/attributes";

                HttpResponseMessage resp = await mt2client.GetAsync(genderApi);

                string resp2 = await resp.Content.ReadAsStringAsync();
                JObject genderData = JObject.Parse(resp2);

                genderData["additionalData"]["Gender"] = selectedChoice.Value;

                string loc1 = genderData.ToString();
                HttpContent loc2 = new StringContent(loc1);
                loc2.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage resp3 = await mt2client.PutAsync(genderApi, loc2);
                if (resp3.IsSuccessStatusCode)
                {
                    AnsiConsole.Markup(
                        "\n[#06c70c]SUCCESS[/] > [#f7b136][underline]Gender changed[/] [[Auto redirect in 2 seconds]][/]");
                }
                else
                {
                    AnsiConsole.Markup(
                        "\n[#fa1414]FAILED[/] > [#f7b136][underline]Unknown[/] [[Auto redirect in 2 seconds]][/]");
                }
            }
        }

        static async Task deleteRoom(string region, string accessToken, string profileId)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ Delete Room").LeftJustified().RoundedBorder());

            using (HttpClient mt2client = new HttpClient())
            {
                mt2client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                mt2client.DefaultRequestHeaders.UserAgent.ParseAdd(
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/125.0.0.0 Safari/537.36 Edg/125.0.0.0");

                string roomApi =
                    $"https://{region}.mspapis.com/profileattributes/v1/profiles/{profileId}/games/j68d/attributes";

                HttpResponseMessage resp = await mt2client.GetAsync(roomApi);


                string resp1 = await resp.Content.ReadAsStringAsync();
                JObject roomData = JObject.Parse(resp1);

                if (roomData["additionalData"]?["DefaultMyHome"] != null)
                {
                    roomData["additionalData"]["DefaultMyHome"].Parent.Remove();
                }

                string loc1 = roomData.ToString();
                HttpContent loc2 = new StringContent(loc1);
                loc2.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage resp3 = await mt2client.PutAsync(roomApi, loc2);

                if (resp3.IsSuccessStatusCode)
                {
                    AnsiConsole.Markup(
                        "\n[#06c70c]SUCCESS[/] > [#f7b136][underline]Room deleted[/] [[Auto redirect in 2 seconds]][/]");
                }
                else
                {
                    AnsiConsole.Markup(
                        "\n[#fa1414]FAILED[/] > [#f7b136][underline]Unknown[/] [[Auto redirect in 2 seconds]][/]");
                }

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
