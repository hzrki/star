using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Spectre.Console;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using ProtoBuf;
using StarService.Enum;
using StarService.Utility;
using static StarService.Utility.AMFCall;
using static StarService.Utility.ChecksumCalculator;
using static StarService.Utility.SignatureCalculator;
using static msptool.localisation;
using Rule = Spectre.Console.Rule;
using WebSocketSharp;
using WebSocket = WebSocketSharp.WebSocket;

namespace msptool
{
    internal class Program
    {
        private static readonly string vloc1 = "1.6.4";

        private static readonly string vloc3 =
            "https://raw.githubusercontent.com/lcfidev/star/main/msptool/version.txt";

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            var spt1 = @"
                                                                                   
                                                   .=+**+-.                        
                                                  =*######*-                       
                                                 :###*--*###.                      
                                           :--===*##*-  =###*===--.                
                                         :*#########+   .+#########+:              
                                        :*###======-.    .-=====+###*.             
                                      .:=###*-.                :=###*.             
                                 :-+**########*+:.          .-+####*:              
                             .-+*################*         .*####+:                
                           :+####################=          =###+                  
                         -*#####################+.  .-**-.  .*###-                 
                       :*#######################=:-+#####*+-.+###=                 
                     .=###################*+=########*++*#######*.                 
                    -*###################+:  .=+*#*+-.  .-+*#*+-.                  
                  .=####################-                                          
                 .+###################*:                                           
                .+###################*:                                            
               .*###################*:                                             
              .+###################*-                                              
              =####################=                                               
             .*#*+-=##############*.                                               
              ::. .+##############=                                                
                  :*######*=#####*.                                                
                  :######*: =####=                                                 
                  -#####+.  .+###-                                                 
                  -####=     .+**.                                                 
                  :*#*:        ::                                                  
                  .=+:                                                             
                                                                                   
                                                                                   
";

            AnsiConsole.Clear();
            AnsiConsole.WriteLine(spt1);
            AnsiConsole.MarkupLine("[#71d5fb]Star Project by lcfi & 6c0[/]");
            AnsiConsole.WriteLine();

            AnsiConsole.Status()
                .Spinner(Spinner.Known.Star)
                .SpinnerStyle(Style.Parse("#71d5fb"))
                .Start("Loading...", ctx =>
                {
                    for (int i = 0; i < 100; i++)
                    {
                        Thread.Sleep(50);
                        ctx.Refresh();
                    }
                });

            Console.Clear();

            if (!vloc2())
            {
                HttpClient loc1 = new HttpClient();
                string vloc4 = loc1.GetStringAsync(vloc3).Result;
                AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Update").LeftJustified());
                Console.Write("\n");
                while (true)
                {
                    Console.WriteLine("[\x1b[95m!\u001b[39m] \u001b[93mAn update was found !\n");
                    Console.WriteLine("\u001b[94m1\u001b[39m > Install new update");
                    Console.WriteLine("\u001b[94m2\u001b[39m > Update manually\n");
                    Console.Write("[\u001b[95mUPDATE\u001b[39m] Pick an option: ");
                    string options = Console.ReadLine();
                    switch (options)
                    {
                        case "1":
                            await InstallUpdate(vloc4);
                            return;
                        case "2":
                            Console.WriteLine(
                                "\n\x1b[95mUPDATE\u001b[39m > \x1b[93mGo on https://github.com/lcfidev/star [Click any key to close]");
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

            var loc2 = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[[[#71d5fb]+[/]]] Select which MSP you want to use")
                    .PageSize(3)
                    .AddChoices(new[] { "MovieStarPlanet", "MovieStarPlanet 2" })
            );

            if (loc2 == "MovieStarPlanet")
                MSP1_Login();
            else
                MSP2_Login();

        }

        static void MSP1_Login()
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Choose Language").LeftJustified());
            Console.Write("\n");

            var loc1 = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[[[#71d5fb]+[/]]] Select your language")
                    .PageSize(11)
                    .AddChoices(new[]
                    {
                        "English", "French", "Turkish", "German", "Polish", "Swedish", "Dutch", "Finnish", "Norwegian",
                        "Danish", "Spanish"
                    })
            );
            Console.Clear();
            bool loc2 = false;
            while (!loc2)
            {

                AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Login MSP").LeftJustified());
                Console.Write("\n");
                var loc3 = AnsiConsole.Prompt(new TextPrompt<string>("[[[#71d5fb]+[/]]] Enter username: ")
                    .PromptStyle("#71d5fb"));

                var loc4 = AnsiConsole.Prompt(new TextPrompt<string>("[[[#71d5fb]+[/]]] Enter password: ")
                    .PromptStyle("#71d5fb")
                    .Secret());

                var loc5 = Enum.GetValues(typeof(WebServer))
                    .Cast<WebServer>()
                    .Select(ws => (ws.loc3().Item1, ws.loc3().Item2))
                    .ToArray();

                var loc6 = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[[[#71d5fb]+[/]]] Select a server: ")
                        .PageSize(15)
                        .MoreChoicesText("[grey](Move up and down to reveal more servers)[/]")
                        .AddChoices(loc5.Select(loc7 => loc7.Item1))
                );

                var loc8 = loc5.First(loc7 => loc7.Item1 == loc6);
                dynamic loc9 = null;
                string server = loc8.Item2;
                if (Msptoolhome.TryGetValue(loc1, out var loc11))
                    AnsiConsole.Status()
                        .SpinnerStyle(Spectre.Console.Style.Parse("#71d5fb"))
                        .Start("Login...", ctx =>
                        {
                            ctx.Refresh();
                            ctx.Spinner(Spinner.Known.Circle);
                            loc9 = AMFConn(server, "MovieStarPlanet.WebService.User.AMFUserServiceWeb.Login",
                                new object[6]
                                {
                                    loc3, loc4, new object[] { }, null, null, "MSP1-Standalone:XXXXXX"
                                });
                            Thread.Sleep(1000);
                        });

                if (loc9 == null)
                {
                    Console.WriteLine(
                        "\n\x1b[91mFAILED\u001b[39m > \x1b[93mUnknown [Click any key to return to login]");
                    Console.ReadKey();
                    Console.Clear();
                }

                if (loc9["loginStatus"]["status"] != "Success")
                {
                    Console.WriteLine(
                        "\n\x1b[91mFAILED\u001b[39m > \x1b[93mLogin failed [Click any key to return to login]");
                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    loc2 = true;
                    int actorId = loc9["loginStatus"]["actor"]["ActorId"];
                    string name = loc9["loginStatus"]["actor"]["Name"];
                    string ticket = loc9["loginStatus"]["ticket"];
                    string accessToken = loc9["loginStatus"]["nebulaLoginStatus"]["accessToken"];
                    string profileId = loc9["loginStatus"]["nebulaLoginStatus"]["profileId"];
                    var th = new JwtSecurityTokenHandler();
                    var jtoken = th.ReadJwtToken(accessToken);
                    var loginId = jtoken.Payload["loginId"].ToString();
                    string loc15 = loc9["loginStatus"]["actorLocale"][0];
                    string culture = loc15.Replace('_', '-');

                    Console.Write(culture);

                    Console.Clear();

                    while (true)
                    {
                        AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home").LeftJustified().RoundedBorder());
                        Console.Write("\n");
                        foreach (var loc12 in loc11)
                        {
                            AnsiConsole.Markup($"[#71d5fb]{loc12.Key}[/]  > {loc12.Value}\n");
                        }

                        AnsiConsole.Write(
                            new Rule(
                                    "[slowblink][#71d5fb]lcfi & 6c0[/][/]")
                                .RightJustified().RoundedBorder());
                        var loc13 = AnsiConsole.Prompt(new TextPrompt<string>("\n[[[#71d5fb]+[/]]] Pick an option: ")
                            .PromptStyle("#71d5fb"));

                        switch (loc13)
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
                                addToWishlist(server, ticket);
                                break;
                            case "10":
                                customStatus(server, name, actorId, ticket);
                                break;
                            case "11":
                                addSponsors(server, ticket);
                                break;
                            case "12":
                                blockDefaults(server, actorId, ticket);
                                break;
                            case "13":
                                recycleitems(server, actorId, ticket);
                                break;
                            case "14":
                                wheelspins(server, actorId, ticket);
                                break;
                            case "15":
                                lisaHack(server, actorId, ticket);
                                break;
                            case "16":
                                automatedPixeller(server, ticket);
                                break;
                            case "17":
                                mspquery(server, actorId, ticket);
                                break;
                            case "18":
                                usernameChecker();
                                break;
                            case "19":
                                clothesExtractor(server, ticket);
                                break;
                            case "20":
                                usernameToActorid(server);
                                break;
                            case "21":
                                actorIdToUsername(server);
                                break;
                            case "22":
                                itemTracker(server);
                                break;
                            case "23":
                                roomChanger(server, actorId, ticket);
                                break;
                            case "24":
                                animationsExtractor(server, ticket);
                                break;
                            case "25":
                                iconChanger(server, actorId, ticket);
                                break;
                            case "26":
                                botGenerator(server, culture);
                                break;
                            case "27":
                                Console.WriteLine("\n\x1b[97mBYE\u001b[39m > \u001b[93mLogging out...");
                                Console.Clear();
                                loc2 = false;
                                break;
                            default:
                                Console.WriteLine(
                                    "\n\u001b[91mERROR\u001b[39m > \u001b[93mChoose a option which exists !");
                                Thread.Sleep(2000);
                                Console.Clear();
                                break;
                        }

                        if (!loc2)
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
            dynamic loc1 = AMFConn(server,
                "MovieStarPlanet.WebService.ActorClothes.AMFActorClothes.GetActorClothesRelMinimals",
                new object[2]
                {
                    new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                    actorId
                });

            foreach (dynamic loc2 in loc1)
            {
                object loc3 = loc2["ActorClothesRelId"];

                dynamic loc4 = AMFConn(server,
                    "MovieStarPlanet.WebService.MovieStar.AMFMovieStarService.GetActorClothesRel",
                    new object[1] { loc3 });

                string loc5 = loc4["Cloth"]["ShopId"].ToString();
                string loc6 = loc4["Cloth"]["Name"] ?? "Unknown";

                if (loc5 != "-100")
                {
                    dynamic recycler = AMFConn(server,
                        "MovieStarPlanet.WebService.Profile.AMFProfileService.RecycleItem",
                        new object[4]
                        {
                            new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                            actorId,
                            loc3,
                            0
                        });
                    AnsiConsole.Markup($"[[[#71d5fb]![/]]] Recycled {loc6}");
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

            var loc1 = new (string Name, int Value)[]
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

            var loc2 = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[[[#71d5fb]+[/]]] Select a boonie: ")
                    .PageSize(10)
                    .AddChoices(loc1.Select(loc3 => loc3.Name))
            );

            var loc4 = loc1.First(loc3 => loc3.Name == loc2);


            dynamic loc5 = AMFConn(server,
                "MovieStarPlanet.WebService.Pets.AMFPetService.BuyClickItem",
                new object[3]
                {
                    new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                    actorId,
                    loc4.Value
                });
            if (loc5["SkinSWF"] != "femaleskin" && loc5["SkinSWF"] != "maleskin")
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
            int loc1 = AnsiConsole.Prompt(
                new TextPrompt<int>("[[[#71d5fb]+[/]]] Enter AnimationId: ")
                    .PromptStyle("#71d5fb"));

            dynamic loc2 = AMFConn(server,
                "MovieStarPlanet.WebService.Spending.AMFSpendingService.BuyAnimation",
                new object[3]
                {
                    new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                    actorId,
                    loc1
                });

            if (loc2["Code"] != 0)
            {
                AnsiConsole.Markup("\n[#fa1414]FAILED[/] > [#f7b136][underline]"
                                   + (loc2["Description"] ?? "Unknown") +
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
            int loc1 = AnsiConsole.Prompt(new TextPrompt<int>("[[[#71d5fb]+[/]]] Enter ClothesId: ")
                .PromptStyle("#71d5fb"));
            string loc2 = AnsiConsole.Prompt(
                new TextPrompt<string>("[[[#71d5fb]+[/]]] Enter Color: ")
                    .PromptStyle("#71d5fb"));
            try
            {
                dynamic loc3 = AMFConn(server, "MovieStarPlanet.WebService.AMFSpendingService.BuyClothes",
                    new object[4]
                    {
                        new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                        actorId,
                        new object[]
                        {
                            new
                            {
                                Color = loc2,
                                y = 0,
                                ActorClothesRelId = 0,
                                ActorId = actorId,
                                ClothesId = loc1,
                                IsWearing = 1,
                                x = 0
                            },
                        },
                        0
                    });

                if (loc3["Code"] != 0)
                {
                    AnsiConsole.Markup("\n[#fa1414]FAILED[/] > [#f7b136][underline]"
                                       + (loc3["Description"] ?? "Unknown") +
                                       "[/] [[Click any key to return to Home]][/]");
                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    AnsiConsole.Markup(
                        "\n[#06c70c]SUCCESS[/] > [#f7b136][underline]Clothing bought![/] [[Click any key to return to Home]][/]");
                }
            }
            catch (Exception)
            {
                AnsiConsole.Markup("\n[#fa1414]FAILED[/] > Hidden or Deleted [#f7b136][underline]"
                                   +
                                   "[/] [[Click any key to return to Home]][/]");
            }

            Console.ReadKey();
            Console.Clear();
        }

        static void buyNose(string server, int actorId, string ticket)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ Buy Nose").LeftJustified()
                .RoundedBorder());
            Console.Write("\n");
            int loc1 = AnsiConsole.Prompt(new TextPrompt<int>("[[[#71d5fb]+[/]]] Enter NoseId: ")
                .PromptStyle("#71d5fb"));

            dynamic loc2 = AMFConn(server,
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
                            ItemId = loc1,
                            Colors = "",

                        }
                    }
                });
            if (loc2[0]["InventoryId"] == 0)
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
            int loc1 = AnsiConsole.Prompt(new TextPrompt<int>("[[[#71d5fb]+[/]]] Enter LipsId: ")
                .PromptStyle("#71d5fb"));
            string loc2 = AnsiConsole.Prompt(
                new TextPrompt<string>("[[[#71d5fb]+[/]]] Enter Color: ")
                    .PromptStyle("#71d5fb"));

            dynamic loc3 = AMFConn(server,
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
                            ItemId = loc1,
                            Colors = loc2,

                        }
                    }
                });
            if (loc3[0]["InventoryId"] == 0)
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
            int loc1 = AnsiConsole.Prompt(new TextPrompt<int>("[[[#71d5fb]+[/]]] Enter EyeId: ")
                .PromptStyle("#71d5fb"));
            string loc2 = AnsiConsole.Prompt(
                new TextPrompt<string>("[[[#71d5fb]+[/]]] Enter Color: ")
                    .PromptStyle("#71d5fb"));

            dynamic loc3 = AMFConn(server,
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
                            ItemId = loc1,
                            Colors = loc2,
                            Type = 1,
                            IsWearing = true

                        }
                    }
                });
            if (loc3[0]["InventoryId"] == 0)
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
            string loc1 = AnsiConsole.Prompt(
                new TextPrompt<string>("[[[#71d5fb]+[/]]] Enter Color: ")
                    .PromptStyle("#71d5fb"));

            dynamic loc2 = AMFConn(server,
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
                            Colors = loc1,
                            IsWearing = true

                        }
                    }
                });
            if (loc2[0]["InventoryId"] == 0)
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
            string loc1 = AnsiConsole.Prompt(
                new TextPrompt<string>("[[[#71d5fb]+[/]]] Enter Status: ")
                    .PromptStyle("#71d5fb"));
            var loc2 = new (string Name, int Value)[]
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

            var loc3 = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[[[#71d5fb]+[/]]] Select a color: ")
                    .PageSize(10)
                    .AddChoices(loc2.Select(loc4 => loc4.Name))
            );

            var loc5 = loc2.First(loc4 => loc4.Name == loc3);

            dynamic loc6 = AMFConn(server,
                "MovieStarPlanet.WebService.ActorService.AMFActorServiceForWeb.SetMoodWithModerationCall",
                new object[5]
                {
                    new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                    new
                    {
                        Likes = 0,
                        TextLine = loc1,
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
                    loc5.Value,
                    false
                });
            if (loc6["FilterTextResult"]["IsMessageOk"])
            {
                if (loc6["FilterTextResult"]["UnrestrictedPolicy"]["HasFilteredParts"])
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
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ Add Sponsors").LeftJustified()
                .RoundedBorder());
            Console.Write("\n");

            List<int> loc1 = new List<int>
                { 273, 276, 277, 341, 418, 419, 420, 421, 83417, 83423, 83427, 83424 };

            foreach (int loc2 in loc1)
            {
                dynamic loc3 = AMFConn(server,
                    "MovieStarPlanet.WebService.AnchorCharacter.AMFAnchorCharacterService.RequestFriendship",
                    new object[2]
                    {
                        new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                        loc2
                    });
                if (loc3["Code"] != 0)
                {
                    AnsiConsole.Markup(
                        $"\n[#fa1414]FAILED[/] > [#f7b136][underline]{loc3["Description"] ?? "Unknown"}[/][/]"
                    );
                }
                else
                {
                    AnsiConsole.Markup(
                        $"\n[#06c70c]SUCCESS[/] > [#f7b136][underline]Anchor {loc2} has been added!![/][/]"
                    );
                }
            }

            AnsiConsole.Markup(
                "\n[#71d5fb][/] > [#f7b136][underline]Click any key to return to Home[/][/]"
            );
            Console.ReadKey();
            Console.Clear();
        }

        static void blockDefaults(string server, int actorId, string ticket)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ Block Defaults").LeftJustified()
                .RoundedBorder());
            Console.Write("\n");

            List<int> loc1 = new List<int>
                { 3, 4, 414 };

            foreach (int loc2 in loc1)
            {
                dynamic mspdefaults = AMFConn(server,
                    "MovieStarPlanet.WebService.ActorService.AMFActorServiceForWeb.BlockActor",
                    new object[3]
                    {
                        new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                        actorId,
                        loc2
                    });
                Console.WriteLine($"Blocked: {loc2}");

            }
        }

        static void recycleitems(string server, int actorId, string ticket)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ Status").LeftJustified()
                .RoundedBorder());

            Console.Write("\n");
            int loc1 = AnsiConsole.Prompt(
                new TextPrompt<int>("[[[#71d5fb]+[/]]] Enter Item relid: ")
                    .PromptStyle("#71d5fb"));

            dynamic loc2 = AMFConn(server,
                "MovieStarPlanet.WebService.Profile.AMFProfileService.RecycleItem",
                new object[4]
                {
                    new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                    actorId,
                    loc1,
                    0
                });
        }

        static void wheelspins(string server, int actorId, string ticket)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ Wheelspins").LeftJustified()
                .RoundedBorder());
            Console.Write("\n");

            loc1(server, ticket, "starwheel", 120, actorId, 4);
            loc1(server, ticket, "starVipWheel", 200, actorId, 4);
            loc1(server, ticket, "advertWheelDwl", 240, actorId, 2);
            loc1(server, ticket, "advertWheelVipDwl", 400, actorId, 2);
        }

        static void loc1(string server, string ticket, string awardType, int awardVal,
            int actorId,
            int count)
        {
            for (int i = 0; i < count; i++)
            {
                dynamic loc2 = AMFConn(server,
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


        static void addToWishlist(string server, string ticket)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ WishList").LeftJustified()
                .RoundedBorder());
            Console.Write("\n");
            int loc1 = AnsiConsole.Prompt(new TextPrompt<int>("[[[#71d5fb]+[/]]] Enter ClothesId: ")
                .PromptStyle("#71d5fb"));
            string loc2 = AnsiConsole.Prompt(
                new TextPrompt<string>("[[[#71d5fb]+[/]]] Enter Color: ")
                    .PromptStyle("#71d5fb"));

            dynamic loc3 = AMFConn(server,
                "MovieStarPlanet.WebService.Gifts.AMFGiftsService+Version2.AddItemToWishlist",
                new object[3]
                {
                    new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                    new object[]
                    {
                        loc1
                    },
                    new object[]
                    {
                        loc2
                    }
                });
            if (loc3 != 0)
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


        static void lisaHack(string server, int actorId, string ticket)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ Lisa Hack").LeftJustified()
                .RoundedBorder());
            Console.Write("\n");
            AnsiConsole.Markup(
                "[slowblink][[[#c70000]?![/]]] Use it at your own risk, we are not responsible for your misdeeds.[/]\n");

            bool loc1 = false;

            for (int i = 0; i < 100; i++)
            {
                dynamic loc2 = AMFConn(server,
                    "MovieStarPlanet.WebService.AMFAwardService.claimDailyAward",
                    new object[4]
                    {
                        new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                        "twoPlayerFame",
                        50,
                        actorId
                    });
                Console.WriteLine("Generated 50 fame");

                dynamic loc3 = AMFConn(server,
                    "MovieStarPlanet.WebService.AMFAwardService.claimDailyAward",
                    new object[4]
                    {
                        new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                        "twoPlayerMoney",
                        50,
                        actorId
                    });
                Console.WriteLine("Generated 50 starcoins");

                if (i == 99)
                {
                    loc1 = true;
                }
            }

            if (loc1)
            {
                AnsiConsole.Markup(
                    "\n[#06c70c]SUCCESS[/] > [#f7b136][underline]Stars are out your account has been levelled and has starcoins : )[/] [[Auto redirect in 2 seconds]][/]");
                Thread.Sleep(500);
                Console.Clear();
            }
        }

        static void mspquery(string server, int actorId, string ticket)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ Msp Query").LeftJustified()
                .RoundedBorder());
            var loc1 = AnsiConsole.Prompt(new TextPrompt<string>("[[[#71d5fb]+[/]]] Enter username: ")
                .PromptStyle("#71d5fb"));


            dynamic loc2 = AMFConn(server,
                "MovieStarPlanet.WebService.UserSession.AMFUserSessionService.GetActorIdFromName",
                new object[1] { loc1 });

            if (loc2 == -1)
            {
                Console.WriteLine(
                    "\n\x1b[91mFAILED\u001b[39m > \x1b[93mThe account doesn't exist or has been deleted [Click any key to return to login]");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                double loc3 = loc2;


                dynamic loc4 = AMFConn(server,
                    "MovieStarPlanet.WebService.Profile.AMFProfileService.LoadProfileSummary",
                    new object[3]
                    {
                        new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                        loc3,
                        actorId
                    });

                DateTime loc5 = loc4["Created"];

                dynamic loc6 = AMFConn(server,
                    "MovieStarPlanet.WebService.AMFActorService.BulkLoadActors",
                    new object[2]
                    {
                        new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                        new object[]
                        {
                            loc3
                        }
                    });

                string loc7 = loc6[0]["NebulaProfileId"];
                double loc8 = loc6[0]["ActorId"];
                string loc9 = loc6[0]["Name"];
                int loc10 = loc6[0]["Level"];
                double loc11 = loc6[0]["Fame"];
                int loc12 = loc6[0]["Money"];
                int loc13 = loc6[0]["Diamonds"];
                string loc14 = loc6[0]["SkinColor"];
                int loc15 = loc6[0]["EyeId"];
                string loc16 = loc6[0]["EyeColors"];
                int loc17 = loc6[0]["NoseId"];
                int loc18 = loc6[0]["MouthId"];
                string loc19 = loc6[0]["MouthColors"];
                DateTime loc20 = loc6[0]["MembershipTimeoutDate"];
                DateTime loc21 = loc6[0]["LastLogin"];

                AnsiConsole.MarkupLine("[bold white]Profile Information[/]");
                AnsiConsole.MarkupLine($"[bold blue]NebulaProfileId:[/] {loc7}");
                AnsiConsole.MarkupLine($"[bold blue]ActorId:[/] {loc8}");
                AnsiConsole.MarkupLine($"[bold blue]Name:[/] {loc9}");
                AnsiConsole.MarkupLine($"[bold blue]Level:[/] {loc10}");
                AnsiConsole.MarkupLine($"[bold blue]Fame:[/] {loc11}");
                AnsiConsole.MarkupLine($"[bold blue]Money:[/] {loc12}");
                AnsiConsole.MarkupLine($"[bold blue]Diamonds:[/] {loc13}");
                AnsiConsole.MarkupLine($"[bold blue]SkinColor:[/] {loc14}");
                AnsiConsole.MarkupLine($"[bold blue]EyeId:[/] {loc15}");
                AnsiConsole.MarkupLine($"[bold blue]EyeColors:[/] {loc16}");
                AnsiConsole.MarkupLine($"[bold blue]NoseId:[/] {loc17}");
                AnsiConsole.MarkupLine($"[bold blue]MouthId:[/] {loc18}");
                AnsiConsole.MarkupLine($"[bold blue]MouthColors:[/] {loc19}");
                AnsiConsole.MarkupLine($"[bold blue]Created:[/] {loc5}");
                AnsiConsole.MarkupLine($"[bold blue]MembershipTimeoutDate:[/] {loc20}");
                AnsiConsole.MarkupLine($"[bold blue]LastLogin:[/] {loc21}");

                AnsiConsole.MarkupLine(
                    $"\n[#06c70c]SUCCESS[/] > [#f7b136][underline]Queried {loc9} :)[/] [[Click any key to return to Home]][/]");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void automatedPixeller(string server, string ticket)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ Automated Pixeller").LeftJustified()
                .RoundedBorder());
            AnsiConsole.MarkupLine($"[#71d5fb]Login with second account : )[/]");
            var username = AnsiConsole.Prompt(new TextPrompt<string>("[[[#71d5fb]+[/]]] username: ")
                .PromptStyle("#71d5fb"));
            AnsiConsole.MarkupLine($"[#71d5fb]Coming next update : )[/]");

            Console.ReadKey();
            Console.Clear();
        }

        static void usernameChecker()
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ Username Checker").LeftJustified()
                .RoundedBorder());
            var loc5 = AnsiConsole.Prompt(new Spectre.Console.TextPrompt<string>("[[[#71d5fb]+[/]]] username: ")
                .PromptStyle("#71d5fb"));
            var loc1 = new string[][]
            {
                new string[] { "US", "CA", "AU", "NZ" },
                new string[] { "GB", "DE", "FR", "TR", "SE", "DK", "FI", "PL", "IE", "ES", "NL", "NO" }
            };

            foreach (var loc2 in loc1)
            {
                foreach (var server in loc2)
                {
                    var loc4 = AMFConn(server,
                        "MovieStarPlanet.WebService.AMFActorService.IsActorNameUsed",
                        new object[] { loc5 });

                    bool loc3 = Convert.ToBoolean(loc4);

                    if (loc3)
                    {
                        AnsiConsole.MarkupLine(
                            $"[#FF0000]{server} | {loc5} | Not available[/]");
                    }
                    else

                    {
                        AnsiConsole.MarkupLine(
                            $"[#00FF00]{server} | {loc5} | available[/]");
                    }
                }
            }

            AnsiConsole.MarkupLine(
                $"\n[#06c70c]SUCCESS[/] > [#f7b136][underline]checked all servers for username :)[/] [[Click any key to return to Home]][/]");
            Console.ReadKey();
            Console.Clear();
        }

        static void clothesExtractor(string server, string ticket)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ Clothes Extractor").LeftJustified()
                .RoundedBorder());
            var loc5 = AnsiConsole.Prompt(new Spectre.Console.TextPrompt<string>("[[[#71d5fb]+[/]]] username: ")
                .PromptStyle("#71d5fb"));

            dynamic loc1 = AMFConn(server,
                "MovieStarPlanet.WebService.UserSession.AMFUserSessionService.GetActorIdFromName",
                new object[1] { loc5 });

            if (loc1 == -1)
            {
                Console.WriteLine(
                    "\n\x1b[91mFAILED\u001b[39m > \x1b[93mThe account doesn't exist or has been deleted [Click any key to return to login]");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                double loc6 = loc1;

                dynamic loc2 = AMFConn(server,
                    "MovieStarPlanet.WebService.ActorClothes.AMFActorClothes.GetActorClothesRelMinimals",
                    new object[2]
                    {
                        new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                        loc6,
                    });

                foreach (var loc3 in loc2)
                {
                    int loc7 = Convert.ToInt32(loc3["ActorClothesRelId"]);

                    dynamic loc4 = AMFConn(server,
                        "MovieStarPlanet.WebService.MovieStar.AMFMovieStarService.GetActorClothesRel",
                        new object[1]
                            { loc7 });

                    string loc8 = loc4["Cloth"]["Name"] ?? "Unknown";
                    int loc9 = loc4["ClothesId"];
                    string loc10 = loc4["Color"].ToString();
                    string loc11 = loc4["Cloth"]["ShopId"].ToString();
                    int loc12 = loc4["Cloth"]["Vip"];
                    int loc13 = loc4["Cloth"]["DiamondsPrice"];

                    string loc15 = loc13 != 0 ? "Yes" : "No";
                    string loc14 = loc12 != 0 ? "Yes" : "No";
                    string loc16 = loc11 != "-100" ? "Yes" : "No";

                    AnsiConsole.MarkupLine($"[#71d5fb]ActorClothesRelId:[/] {loc7}");
                    AnsiConsole.MarkupLine($"[#71d5fb]ClothesName:[/] {loc8}");
                    AnsiConsole.MarkupLine($"[#71d5fb]ClothesId:[/] {loc9}");
                    if (!string.IsNullOrEmpty(loc10))
                    {
                        AnsiConsole.MarkupLine($"[#71d5fb]Colors:[/] {loc10}");
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[#71d5fb]Colors:[/] None");
                    }

                    AnsiConsole.MarkupLine($"[#71d5fb]IsRareItem:[/] {loc16}");
                    AnsiConsole.MarkupLine($"[#71d5fb]IsVipItem:[/] {loc14}");
                    AnsiConsole.MarkupLine($"[#71d5fb]IsDiamondItem:[/] {loc15}");
                    AnsiConsole.MarkupLine("");

                }
            }

            AnsiConsole.MarkupLine(
                $"\n[#06c70c]SUCCESS[/] > [#f7b136][underline]checked all {loc5} clothes :)[/] [[Click any key to return to Home]][/]");
            Console.ReadKey();
            Console.Clear();
        }

        static void usernameToActorid(string server)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ Username To ActorId").LeftJustified()
                .RoundedBorder());
            var loc2 = AnsiConsole.Prompt(new Spectre.Console.TextPrompt<string>("[[[#71d5fb]+[/]]] username: ")
                .PromptStyle("#71d5fb"));

            dynamic loc1 = AMFConn(server,
                "MovieStarPlanet.WebService.UserSession.AMFUserSessionService.GetActorIdFromName",
                new object[1] { loc2 });

            if (loc1 == -1)
            {
                Console.WriteLine(
                    "\n\x1b[91mFAILED\u001b[39m > \x1b[93mThe account doesn't exist or has been deleted [Click any key to return to login]");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                double loc3 = loc1;

                AnsiConsole.MarkupLine(
                    $"\n[#06c70c]SUCCESS[/] > [#f7b136][underline]ActorId: {loc3} | Username: {loc2} :)[/] [[Click any key to return to Home]][/]");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void actorIdToUsername(string server)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ ActorId to Username").LeftJustified()
                .RoundedBorder());
            int loc1 = AnsiConsole.Prompt(new TextPrompt<int>("[[[#71d5fb]+[/]]] actorid: ")
                .PromptStyle("#71d5fb"));

            dynamic loc5 = AMFConn(server,
                "MovieStarPlanet.WebService.UserSession.AMFUserSessionService.GetActorNameFromId",
                new object[1]
                    { loc1 });

            string loc2 = loc5;
            AnsiConsole.MarkupLine(
                $"\n[#06c70c]SUCCESS[/] > [#f7b136][underline]Username: {loc2} | ActorId: {loc1}  :)[/] [[Click any key to return to Home]][/]");
            Console.ReadKey();
            Console.Clear();
        }

        static void itemTracker(string server)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ Item Tracker").LeftJustified()
                .RoundedBorder());
            int loc1 = AnsiConsole.Prompt(new TextPrompt<int>("[[[#71d5fb]+[/]]] ActorClothesRelId: ")
                .PromptStyle("#71d5fb"));

            dynamic loc4 = AMFConn(server,
                "MovieStarPlanet.WebService.MovieStar.AMFMovieStarService.GetActorClothesRel",
                new object[1]
                    { loc1 });

            int loc2 = loc4["ActorId"];

            dynamic loc5 = AMFConn(server,
                "MovieStarPlanet.WebService.UserSession.AMFUserSessionService.GetActorNameFromId",
                new object[1]
                    { loc2 });

            string loc3 = loc5;

            string loc6 = loc4["Cloth"]["Name"] ?? "Unknown";
            int loc7 = loc4["ClothesId"];
            string loc8 = loc4["Color"].ToString();
            string loc9 = loc4["Cloth"]["ShopId"].ToString();
            int loc10 = loc4["Cloth"]["Vip"];
            int loc11 = loc4["Cloth"]["DiamondsPrice"];

            string loc12 = loc11 != 0 ? "Yes" : "No";
            string loc13 = loc10 != 0 ? "Yes" : "No";
            string loc14 = loc9 != "-100" ? "Yes" : "No";

            AnsiConsole.MarkupLine($"[#71d5fb]ActorClothesRelId:[/] {loc1}");
            AnsiConsole.MarkupLine($"[#71d5fb]ClothesName:[/] {loc6}");
            AnsiConsole.MarkupLine($"[#71d5fb]ClothesId:[/] {loc7}");
            if (!string.IsNullOrEmpty(loc8))
            {
                AnsiConsole.MarkupLine($"[#71d5fb]Colors:[/] {loc8}");
            }
            else
            {
                AnsiConsole.MarkupLine("[#71d5fb]Colors:[/] None");
            }

            AnsiConsole.MarkupLine($"[#71d5fb]IsRareItem:[/] {loc14}");
            AnsiConsole.MarkupLine($"[#71d5fb]IsVipItem:[/] {loc13}");
            AnsiConsole.MarkupLine($"[#71d5fb]IsDiamondItem:[/] {loc12}");
            AnsiConsole.MarkupLine("");
            AnsiConsole.MarkupLine(
                $"\n[#06c70c]SUCCESS[/] > [#f7b136][underline]Item tracked to {loc3}  :)[/] [[Click any key to return to Home]][/]");
            Console.ReadKey();
            Console.Clear();

        }

        static void roomChanger(string server, int actorId, string ticket)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ RoomChanger").LeftJustified().RoundedBorder());
            Console.Write("\n");
            AnsiConsole.Markup(
                "[slowblink][[[#c70000]?![/]]] Use it at your own risk, we are not responsible for your misdeeds.[/]\n");
            string loc1 = AnsiConsole.Prompt(new TextPrompt<string>("[[[#71d5fb]+[/]]] Enter image url: ")
                .PromptStyle("#71d5fb"));
            WebClient loc2 = new WebClient();
            byte[] loc3 = loc2.DownloadData(loc1);

            dynamic loc4 = AMFConn(server,
                "MovieStarPlanet.WebService.Snapshots.AMFGenericSnapshotService.CreateSnapshot",
                new object[5]
                {
                    new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                    actorId,
                    "room",
                    loc3,
                    "jpg"
                });

            dynamic loc5 = AMFConn(server,
                "MovieStarPlanet.WebService.Snapshots.AMFGenericSnapshotService.CreateSnapshot",
                new object[5]
                {
                    new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                    actorId,
                    "roomProfile",
                    loc3,
                    "jpg"
                });
            dynamic loc6 = AMFConn(server,
                "MovieStarPlanet.WebService.Snapshots.AMFGenericSnapshotService.CreateSnapshot",
                new object[5]
                {
                    new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                    actorId,
                    "roomMedium",
                    loc3,
                    "jpg"
                });
            if (loc4 && loc5 && loc6)
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

        static void animationsExtractor(string server, string ticket)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ Animation Extractor").LeftJustified()
                .RoundedBorder());
            var loc4 = AnsiConsole.Prompt(new TextPrompt<string>("[[[#71d5fb]+[/]]] username: ")
                .PromptStyle("#71d5fb"));

            dynamic loc1 = AMFConn(server,
                "MovieStarPlanet.WebService.UserSession.AMFUserSessionService.GetActorIdFromName",
                new object[1] { loc4 });

            if (loc1 == -1)
            {
                Console.WriteLine(
                    "\n\x1b[91mFAILED\u001b[39m > \x1b[93mThe account doesn't exist or has been deleted [Click any key to return to login]");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                double loc12 = loc1;

                dynamic loc2 = AMFConn(server,
                    "MovieStarPlanet.WebService.Media.AMFMediaService.GetMyAnimations",
                    new object[2]
                    {
                        new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                        loc12,
                    });

                foreach (var loc3 in loc2)
                {
                    string loc5 = loc3["Animation"]["Name"] ?? "Unknown";
                    int loc6 = loc3["Animation"]["AnimationId"] ?? -1;
                    int loc7 = loc3["ActorAnimationRelId"] ?? -1;
                    int loc8 = loc3["Animation"]["Deleted"];
                    int? loc9 = loc3["Animation"]["Vip"] as int?;

                    string loc10 = loc9.HasValue ? loc9.Value != 0 ? "Yes" : "No" : "None";
                    string loc11 = loc8 != 0 ? "Yes" : "No";

                    AnsiConsole.MarkupLine($"[#71d5fb]Name:[/] {loc5}");
                    AnsiConsole.MarkupLine($"[#71d5fb]AnimationId:[/] {loc6}");
                    AnsiConsole.MarkupLine($"[#71d5fb]ActorAnimationRelId:[/] {loc7}");
                    AnsiConsole.MarkupLine($"[#71d5fb]IsRareItem:[/] {loc11}");
                    AnsiConsole.MarkupLine($"[#71d5fb]IsVipItem:[/] {loc10}");
                    AnsiConsole.MarkupLine("");
                }
            }

            AnsiConsole.MarkupLine(
                $"\n[#06c70c]SUCCESS[/] > [#f7b136][underline]checked all {loc4} animations :)[/] [[Click any key to return to Home]][/]");
            Console.ReadKey();
            Console.Clear();
        }

        static void iconChanger(string server, int actorId, string ticket)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ Icon Changer").LeftJustified().RoundedBorder());
            Console.Write("\n");
            AnsiConsole.Markup(
                "[slowblink][[[#c70000]?![/]]] Use it at your own risk, we are not responsible for your misdeeds.[/]\n");
            string loc1 = AnsiConsole.Prompt(new TextPrompt<string>("[[[#71d5fb]+[/]]] Enter image url: ")
                .PromptStyle("#71d5fb"));
            WebClient loc2 = new WebClient();
            byte[] loc3 = loc2.DownloadData(loc1);

            dynamic loc4 = AMFConn(server,
                "MovieStarPlanet.WebService.Snapshots.AMFGenericSnapshotService.CreateSnapshot",
                new object[5]
                {
                    new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                    actorId,
                    "moviestar",
                    loc3,
                    "jpg"
                });

            dynamic loc5 = AMFConn(server,
                "MovieStarPlanet.WebService.Snapshots.AMFGenericSnapshotService.CreateSnapshot",
                new object[5]
                {
                    new TicketHeader { anyAttribute = null, Ticket = actor(ticket) },
                    actorId,
                    "fullSizeMoviestar",
                    loc3,
                    "jpg"
                });
            if (loc4 && loc5)
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

        static void botGenerator(string server, string culture)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ Bot Generator").LeftJustified()
                .RoundedBorder());

            int loc13 = AnsiConsole.Prompt(
                new Spectre.Console.TextPrompt<int>("[[[#71d5fb]+[/]]] Amount of bots: ").PromptStyle("#71d5fb"));

            for (int i = 0; i < loc13; i++)
            {
                string loc1 = CaptchaV3();
                WebClient loc5 = new WebClient();
                string loc6 = loc5.DownloadString("https://" + ((server.ToUpper() == "US") ? "us" : "eu") +
                                                  $".mspapis.com/profileidentity/v1/profiles/names/suggestions/?&gameId=5ooi&culture={culture}");
                System.Collections.Generic.List<string> loc7 = JsonConvert.DeserializeObject<List<string>>(loc6);
                string loc2 = string.Join("", loc7);
                string loc3 = "hasumsp123";
                string loc4 = BitConverter
                    .ToString(new HMACSHA256(Encoding.UTF8.GetBytes("7jA7^kAZSHtjxDAa")).ComputeHash(
                        Encoding.UTF8.GetBytes("5ooi" + server + loc3 + loc2 + "false"))).Replace("-", "").ToLower();
                WebClient loc8 = new WebClient();
                loc8.Headers[HttpRequestHeader.ContentType] = "application/json";
                string loc12 = loc8.UploadString(
                    "https://" + ((server.ToUpper() == "US") ? "us" : "eu") + ".mspapis.com/edgelogins/graphql/graphql",
                    JsonConvert.SerializeObject(new
                    {
                        query =
                            "mutation create ($loginName: String!, $password: String!, $gameId: String!, $isGuest: Boolean!, $countryCode: Region!, $checksum: String!, $recaptchaV3Token: String ){createLoginProfile(input: { name: $loginName, password: $password, gameId: $gameId, region: $countryCode, isGuest: $isGuest }, verify: {checksum: $checksum, recaptchaV3Token: $recaptchaV3Token } ) {success,loginProfile {loginId,loginName,profileId,profileName,isGuest},error}}",
                        variables = "{\"checksum\": \"" + loc4 + "\", \"loginName\": \"" + loc2 +
                                    "\", \"password\": \"" + loc3 +
                                    "\", \"gameId\": \"5ooi\", \"isGuest\": false, \"countryCode\": \"" +
                                    server.ToUpper() + "\", \"recaptchaV3Token\": \"" + loc1 + "\"}",
                        operationName = ""
                    }));
                JObject loc9 = JObject.Parse(loc12);
                bool loc10 = (bool)loc9["data"]["createLoginProfile"]["success"];
                dynamic loc15 = AMFConn(server, "MovieStarPlanet.WebService.User.AMFUserServiceWeb.Login",
                    new object[6]
                    {
                        loc2, loc3, new object[] { }, null, null, "MSP1-Standalone:XXXXXX"
                    });
                if (loc15["loginStatus"]["status"] != "ThirdPartyCreated")
                    Console.WriteLine($"not validated with msp1 {loc2}");
                else
                {
                    int loc27 = loc15["loginStatus"]["actor"]["ActorId"];
                    string loc28 = loc15["loginStatus"]["ticket"];
                    string loc25 = loc15["loginStatus"]["nebulaLoginStatus"]["accessToken"];
                    string loc26 = loc15["loginStatus"]["nebulaLoginStatus"]["profileId"];
                    Console.WriteLine($"attempting to validate with msp1 {loc2}");
                    
                    WebClient loc18 = new WebClient();
                    var loc16 =
                        loc18.DownloadData(
                            $"https://snapshots.mspcdns.com/v1/MSP/GB/snapshot/fullSizeMoviestar/1.jpg");
                    var loc17 =
                        loc18.DownloadData($"https://snapshots.mspcdns.com/v1/MSP/GB/snapshot/moviestar/4.jpg");
                    dynamic loc19 = AMFConn(server, "MovieStarPlanet.WebService.AMFActorService.ThirdPartySaveAvatar",
                        new object[]
                        {
                            new
                            {
                                Clothes = new object[]
                                {
                                    new
                                    {
                                        ActorClothesRelId = -10,
                                        ActorId = -1,
                                        ClothesId = 18671,
                                        IsWearing = 1,
                                        y = 0,
                                        Color = 0,
                                        x = 0
                                    },
                                    new
                                    {
                                        ActorClothesRelId = -14,
                                        ActorId = -1,
                                        ClothesId = 22252,
                                        IsWearing = 1,
                                        y = 0,
                                        Color = "0,0,0",
                                        x = 0
                                    },
                                    new
                                    {
                                        ActorClothesRelId = -15,
                                        ActorId = -1,
                                        ClothesId = 19121,
                                        IsWearing = 1,
                                        y = 0,
                                        Color = "0x000000,0xffffff,0x000000",
                                        x = 0
                                    },
                                    new
                                    {
                                        ActorClothesRelId = -16,
                                        ActorId = -1,
                                        ClothesId = 18931,
                                        IsWearing = 1,
                                        y = 0,
                                        Color = "0xFFFCF7,0xF5F6FF",
                                        x = 0
                                    }
                                },
                                MouthColors = "skincolor,0xB67676",
                                MouthId = 35,
                                NoseId = 28,
                                InvitedByActorId = -1,
                                ChosenActorName = loc2,
                                ChosenPassword = loc3,
                                SkinIsMale = true,
                                EyeColors = "0x0,0x000000",
                                EyeId = 36,
                                SkinColor = "14195824"
                            },
                            loc17,
                            loc16,
                            loc2,
                            loc3
                        });

                    var loc20 = new WebClient { Proxy = null };
                    var loc21 = server == "US"
                        ? "https://presence-us.mspapis.com/getServer"
                        : "https://presence.mspapis.com/getServer";
                    var loc22 = loc20.DownloadString(loc21).Replace('-', '.');
                    var loc23 = new WebSocket($"ws://{loc22}:10843/{loc22.Replace('.', '-')}/?transport=websocket");
                    loc23.Connect();
                    loc23.Send(
                        $"42[\"10\",{{\"messageType\":10,\"messageContent\":{{\"version\":3,\"applicationId\":\"APPLICATION_WEB\",\"country\":\"{server}\",\"username\":\"{loc26}\",\"access_token\":\"{loc25}\"}}}}]");

                    wheelspins(server, loc27, loc28);
                    lisaHack(server, loc27, loc28);
                    
                    if (loc10)
                    {
                        AnsiConsole.Markup(
                            $"\n[#06c70c]SUCCESS[/] > [#f7b136][underline]{loc2}:{loc3}[/][/]");
                        string loc11 = $"bots-{server}.txt";
                        File.AppendAllText(loc11, loc2 + Environment.NewLine);
                    }
                    else
                    {
                        AnsiConsole.Markup(
                            $"\n[#fa1414]FAILED[/] > [#f7b136][underline]{loc2}[/][/]");
                    }
                }
            }
            AnsiConsole.MarkupLine(
                "\n[#71d5fb][/] > [#f7b136][underline]created all bots :)[/] [[Click any key to return to Home]][/]");
            Console.ReadKey();
            Console.Clear();
        }

        static async Task MSP2_Login()
        {
            Console.Clear();
            bool loc1 = false;

            while (!loc1)
            {
                AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Login MSP2").LeftJustified());
                Console.Write("\n");
                var loc2 = AnsiConsole.Prompt(new TextPrompt<string>("[[[#71d5fb]+[/]]] Enter username: ")
                    .PromptStyle("#71d5fb"));

                var loc3 = AnsiConsole.Prompt(new TextPrompt<string>("[[[#71d5fb]+[/]]] Enter password: ")
                    .PromptStyle("#71d5fb")
                    .Secret());

                var loc4 = Enum.GetValues(typeof(WebServer))
                    .Cast<WebServer>()
                    .Select(ws => (ws.loc3().Item1, ws.loc3().Item2))
                    .ToArray();

                var loc5 = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[[[#71d5fb]+[/]]] Select a server: ")
                        .PageSize(15)
                        .MoreChoicesText("[grey](Move up and down to reveal more servers)[/]")
                        .AddChoices(loc4.Select(aloc1 => aloc1.Item1))
                );

                var loc6 = loc4.First(aloc1 => aloc1.Item1 == loc5).Item2;
                var loc7 = new[] { "US", "CA", "AU", "NZ" }.Contains(loc6) ? "us" : "eu";

                string loc8 = null;
                string loc9 = null;

                AnsiConsole.Status()
                    .SpinnerStyle(Spectre.Console.Style.Parse("#71d5fb"))
                    .Start("Login...", ctx =>
                    {
                        ctx.Refresh();
                        ctx.Spinner(Spinner.Known.Circle);

                        var loc10 = $"https://{loc7}-secure.mspapis.com/loginidentity/connect/token";

                        using (var loc11 = new WebClient())
                        {
                            var loc12 = new NameValueCollection
                            {
                                ["client_id"] = "unity.client",
                                ["client_secret"] = "secret",
                                ["grant_type"] = "password",
                                ["scope"] = "openid nebula offline_access",
                                ["username"] = $"{loc6}|{loc2}",
                                ["password"] = loc3,
                                ["acr_values"] = "gameId:j68d"
                            };

                            var loc13 = loc11.UploadValues(loc10, loc12);
                            var loc14 = Encoding.Default.GetString(loc13);
                            dynamic loc15 = JsonConvert.DeserializeObject(loc14);


                            var loc16 = loc15["access_token"].ToString();
                            var loc17 = loc15["refresh_token"].ToString();

                            var loc18 = new JwtSecurityTokenHandler();
                            var loc19 = loc18.ReadJwtToken(loc16);
                            var loc20 = loc19.Payload["loginId"].ToString();

                            string loc21 =
                                $"https://{loc7}.mspapis.com/profileidentity/v1/logins/{loc20}/profiles?&pageSize=100&page=1&filter=region:{loc6}";
                            loc11.Headers.Add(HttpRequestHeader.Authorization,
                                "Bearer " + loc16);
                            string loc22 = loc11.DownloadString(loc21);

                            var loc9 = JArray.Parse(loc22)[0]["id"].ToString();

                            var loc23 = new NameValueCollection
                            {
                                ["grant_type"] = "refresh_token",
                                ["refresh_token"] = loc17,
                                ["acr_values"] = $"gameId:j68d profileId:{loc9}"
                            };

                            loc11.Headers.Remove(HttpRequestHeader.Authorization);
                            loc11.Headers.Add(HttpRequestHeader.Authorization,
                                "Basic dW5pdHkuY2xpZW50OnNlY3JldA==");
                            var loc24 = loc11.UploadValues(loc10, loc23);

                            var loc25 = Encoding.Default.GetString(loc24);
                            dynamic loc26 = JsonConvert.DeserializeObject(loc25);

                            loc8 = loc26["access_token"].ToString();

                            Console.Clear();
                        }
                    });
                while (true)
                {
                    loc1 = true;
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
                                "[slowblink][#71d5fb]lcfi & 6c0[/][/]")
                            .RightJustified().RoundedBorder());
                    var options = AnsiConsole.Prompt(
                        new TextPrompt<string>("\n[[[#71d5fb]+[/]]] Pick an option: ")
                            .PromptStyle("#71d5fb"));

                    switch (options)
                    {
                        case "1":
                            moodChanger(loc7, loc8, loc9);
                            Thread.Sleep(2000);
                            break;
                        case "2":
                            genderChanger(loc7, loc8, loc9);
                            Thread.Sleep(2000);
                            break;
                        case "3":
                            deleteRoom(loc7, loc8, loc9);
                            Thread.Sleep(2000);
                            break;
                        case "4":
                            loc1 = false;
                            break;
                        default:
                            Console.WriteLine(
                                "\n\u001b[91mERROR\u001b[39m > \u001b[93mChoose an option which exists!");
                            System.Threading.Thread.Sleep(2000);
                            Console.Clear();
                            break;
                    }

                    if (!loc1)
                        break;
                }

                ;
            }
        }

        static async Task moodChanger(string region, string accessToken, string profileId)
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[#71d5fb]MSPTOOL[/] ・ Home ・ Change Mood").LeftJustified().RoundedBorder());

            var loc1 = new (string Name, string Value)[]
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

            var loc2 = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[[[#71d5fb]+[/]]] Select a mood: ")
                    .PageSize(10)
                    .AddChoices(loc1.Select(loc3 => loc3.Name))
            );

            var loc4 = loc1.First(loc3 => loc3.Name == loc2);

            using (HttpClient loc5 = new HttpClient())
            {
                loc5.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                loc5.DefaultRequestHeaders.UserAgent.ParseAdd(
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/125.0.0.0 Safari/537.36 Edg/125.0.0.0");

                string loc6 =
                    $"https://{region}.mspapis.com/profileattributes/v1/profiles/{profileId}/games/j68d/attributes";

                HttpResponseMessage loc7 = await loc5.GetAsync(loc6);

                string loc8 = await loc7.Content.ReadAsStringAsync();
                JObject loc9 = JObject.Parse(loc8);

                loc9["additionalData"]["Mood"] = loc4.Value;

                string loc10 = loc9.ToString();
                HttpContent loc11 = new StringContent(loc10);
                loc11.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage loc12 = await loc5.PutAsync(loc6, loc11);
                if (loc12.IsSuccessStatusCode)
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

            var loc1 = new (string Name, string Value)[]
            {
                ("Girl", "Girl"),
                ("Boy", "Boy"),
            };

            var loc2 = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[[[#71d5fb]+[/]]] Select a mood: ")
                    .PageSize(10)
                    .AddChoices(loc1.Select(loc3 => loc3.Name))
            );

            var loc4 = loc1.First(loc3 => loc3.Name == loc2);

            using (HttpClient loc5 = new HttpClient())
            {
                loc5.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                loc5.DefaultRequestHeaders.UserAgent.ParseAdd(
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/125.0.0.0 Safari/537.36 Edg/125.0.0.0");

                string loc6 =
                    $"https://{region}.mspapis.com/profileattributes/v1/profiles/{profileId}/games/j68d/attributes";

                HttpResponseMessage loc7 = await loc5.GetAsync(loc6);

                string loc8 = await loc7.Content.ReadAsStringAsync();
                JObject loc9 = JObject.Parse(loc8);

                loc9["additionalData"]["Gender"] = loc4.Value;

                string loc10 = loc9.ToString();
                HttpContent loc11 = new StringContent(loc10);
                loc11.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage loc12 = await loc5.PutAsync(loc6, loc11);
                if (loc12.IsSuccessStatusCode)
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

            using (HttpClient loc1 = new HttpClient())
            {
                loc1.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                loc1.DefaultRequestHeaders.UserAgent.ParseAdd(
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/125.0.0.0 Safari/537.36 Edg/125.0.0.0");

                string loc2 =
                    $"https://{region}.mspapis.com/profileattributes/v1/profiles/{profileId}/games/j68d/attributes";

                HttpResponseMessage loc3 = await loc1.GetAsync(loc2);


                string loc4 = await loc3.Content.ReadAsStringAsync();
                JObject loc5 = JObject.Parse(loc4);

                if (loc5["additionalData"]?["DefaultMyHome"] != null)
                {
                    loc5["additionalData"]["DefaultMyHome"].Parent.Remove();
                }

                string loc6 = loc5.ToString();
                HttpContent loc7 = new StringContent(loc6);
                loc7.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage loc8 = await loc1.PutAsync(loc2, loc7);

                if (loc8.IsSuccessStatusCode)
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

        static bool vloc2()
        {
            using (HttpClient loc1 = new HttpClient())
            {
                string vloc4 = loc1.GetStringAsync(vloc3).Result;
                return vloc1.Trim() == vloc4.Trim();
            }
        }

        private static async Task InstallUpdate(string latestVersion)
        {
            string loc1 = XIU(latestVersion);

            string loc2 = string.Empty;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                loc2 = string.Format($"https://github.com/lcfidev/star/releases/download/{loc1}/msptool.exe", loc1);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                loc2 = string.Format(
                    $"https://github.com/lcfidev/star/releases/download/{loc1}/msptool_macos_arm64.zip", loc1);
            }

            string loc3 = string.Format(loc2, loc1);
            string loc4 = Path.Combine(Path.GetTempPath(), "msptool");
            string loc5 = Process.GetCurrentProcess().MainModule.FileName;
            using HttpClient loc6 = new HttpClient();

            byte[] loc7 = await loc6.GetByteArrayAsync(loc3);
            File.WriteAllBytes(loc4, loc7);
            File.Replace(loc4, loc5, null);
            Process.Start(new ProcessStartInfo { FileName = loc5, UseShellExecute = true });
            Environment.Exit(0);
        }

        private static string XIU(string version)
        {
            version = version.Replace("\r", "").Replace("\n", "");
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                version = version.Replace(c, '_');
            }

            return "v" + version.Trim();
        }

        [ProtoContract(SkipConstructor = true)]
        internal class ProtobufData
        {
            [ProtoMember(1)] public string A { get; set; }

            [ProtoMember(2)] public string B { get; set; }

            [ProtoMember(4)] public string C { get; set; }

            [ProtoMember(5)] public string D { get; set; }

            [ProtoMember(6)] public string E { get; set; }

            [ProtoMember(8)] public string F { get; set; }

            [ProtoMember(14)] public string G { get; set; }

            [ProtoMember(16)] public string H { get; set; }
        }

        public static string CaptchaV3()
        {
            try
            {
                WebClient loc1 = new WebClient();
                MemoryStream loc2 = new MemoryStream();
                string loc3 = loc1.DownloadString(
                    "https://www.google.com/recaptcha/api2/anchor?ar=1&k=6LcxuOsUAAAAAI2IYDfxOvAZrwRg2T1E7sJq96eg&co=aHR0cHM6Ly93d3cubW92aWVzdGFycGxhbmV0LmNvbTo0NDM.&hl=fr&v=vP4jQKq0YJFzU6e21-BGy3GP&size=invisible&cb=oul7f799qkr6");
                Serializer.Serialize<ProtobufData>((Stream)loc2, new ProtobufData
                {
                    A = "qljbK_DTcvY1PzbR7IG69z1r",
                    B = loc3.Split(new string[1] { "value=" }, StringSplitOptions.None)[1].TrimStart('"').Split('"')[0],
                    C =
                        "!7Oqg6u8KAAQeF6g9bQEHDwLHaMPdyY7ouljPgQLRkFk2F1_itEwRVMtNNtAClz4c9AJDkWZ0NDXld44MdB1GiLK3E3ykbGIqxFYsRce3-5wFxCJ8MDLUgEO21E4ZdXE05UJAytb9NZWAzdh9D0hVUfS1xCqJ5LGAEuAqvx6GwI76CT8bWF2EAtnEIeuK7YdFYVMCMfkwroua67Hs0vQXjCp3aC9aOL1dsjdH5QG5FjvX7bUyFWxm0du9GvS-O4ZD5ABvfNxd4GW9GfPWlSy2TKIx0eaPPvb4cGxFwHSbDpCvkENiej4PZw8d4oXCitcFzK2QbmbV5WlikvzC2GRRhyIYC2FNm1leYH1ZzwHBRXim1YA0JswO2-lBZ-Hk3Fo-q1LL6ZAIM3FoMwk5ZIUUhhady-Mp4HfP3ZW4vaZELKq6tbH0cNt-LXItIv7obnR5g865bfiI3ghOuJMPlEbdZBt5RG5j1gSoTMaOIgKXDjBrHfLdaK-L9uz6P6RegHy9aaTFWOJF092IwxB_7fdLqQtbOWYCmPrio2TDMGsnz8Q0AuhtZjBrqrHm3sNdXKJHj-ThNAmCdSeZN2dYtIuk9YbnqKdqxq6TQvH5F1yVQ23EKIK-Si27ovMKZd_TRqXYOcg-XtMhxr4VL2QUI55RZcjzs7bhl8NPFneQHffJhn7PPUn36UKEhMIJGHh4YRpVGeT8cRDZSMEbCsoXSQUvH1kOyTjJgnrC9eKYMU018jtMELoU4diIwVkJHKYidbvT134CAPBZM0trcy8KCkOjhJYbl9PQft2ELyoCHJ-YHnKnm_YPfSycArIdWh4q4FVz4IC_EZM-cdGvk4TH92f4iBOUINOioeEP9MS7TjyRT9p6KH0Jotojc2V0N7fmlcthcBqe3F9ll85q-tC4V1R0Ek3-K1quSfodSmbe8bAXdLRMYrGqa1RXh0G0H_aAbXeh7La7wZL1xOUnnAA03e9-8Q2jo_oE0ixGjMFUHqqfBN8qqNeWpOHzUZQJRtP1vb8r9g0F4S6j4MXL5rBnGsPcXQ",
                    D = "217035401",
                    E = "q",
                    F = "login_create",
                    G = "6LcxuOsUAAAAAI2IYDfxOvAZrwRg2T1E7sJq96eg",
                    H =
                        "0Z0HWX9hanGDpYuQm6nPsbrB0_Xb4OrJL-njLPMhV1E4SmxSW2JwlniBiJq8oquywObI0djrDPL4AxE3GSIpO11DSFNhh2lyeYutk5ijsde5wsmOsZdVbAYUBZBFu5JHZZftrBJP4dAWHI3oJijiGOpUdqiKdFrcXqDikP347jBSQCMIdrl_USexjxVLOa-I1znnWgclLsj4CiwSGyIwVjhBSFp8YmtygKaIkZiqzLK7wtD22OHo-x0DCBMhRykyOP4hKxEO0QMo5yGjTaPhBsknfQe0-xWX3dwpI3k8KhfN3757-cfSVFXf_e9xo73b1fSN0JW4wpj_MPacxsQCMFZsamUeYSZJUyz3LW8R45DLIV_eB5k-yPsdOyEz5SvtEBn_pTNhf5mXnk-SV3qD2eg6IN8MflyGXKpcpnyGyAo4XmSGdSZpLlFa_Je0eqjG4NLZltmewcsiLuETLU9ZU_1ABSgxi5nL0oy9uA40Dhi-VC40cbvqDBY0JtQa3AMIT3kvNRM4bpEalTKVSwVLafNk2uCy4QcRFzHPEdb6AAYbln_2hzlrhaOhnFWYXYCKPDZ_qdv2EBYsxgjN8PtExhBCXHp0iyxzNFthR4R6qMbg3uWW2Z7By7Hu4RM1UzlL_UAFKDHQApymWBGsHiRi3M5ovtCKwSJH0gAeKEJQUlRiWFp1HmEmSVNg50T3mUPFQ1Dq4Td56-4f6bdB_6GLZh-x9IJbxpdJe527obBlqLrcwsvSl7rAptEyXIMIRxlTITLs-rjLTZtcl4EXrLfYirjW8QLppumu0dvBZvEjRWNJWA1QFTvF_Bonpme2CKI0ZsD-hIp5BskmRE3oGjRSTFp0doSGjIM0ezxjabc0grDW3P7tnuHwFfgBC8zz-ZwFSC2cNniBQ3GrrZvBx7HL2eiFyI2wutTmaM48v1CDSM9ZcmFXgX6AoqSW3O8Rxx3ezeNmC0FbAigKK-msTT_1xxFDaXNxc32fka-6V5pfgohmX-7r8v0aDOJsLvC3UJ7E5qhzWNq8v0xSgJ7EutC-zOLU8u2e4abJ02I26Rs1V1VcBUgNMDnUBVOBn7m3wm-ywObI0didwMps9zhyhKrBFqSyzHbxmtVrsX9hMyCzAYbNI8XHiT9pf3lj6YOxNEoT6aPWS23AakwNl56kGgPWVMb4OixmtGqQ_v0GIJp8nlS6oMdI3nWLVKNUn7UesQPcz0lcBWNt833rhSsVy4pUTaxCfEpnfk_WCHqMikg-2BJEaekKAL5AUqQSaSI0xzDWvFKoV10C1SuRH7jzRLOlWsn7MSQBh8ID6X-OL8obmj9Np6Ir3XP6E-6QieQWgDIcPdAfAOcAanwieQpktsC2_Ms8zlSCwNNRT5SfCSudN6nz4WPFa91H6aulZAXLzkRpyFVcMnABkGpMebQ2JAnUwkPp3EHsamTmqO78dqTG3FMo0llu_Vrk1ol3AWsdS8TXaWv0103--ReF5BG3xVe50-VTnkQtcEakJagWNC28mpfKtD8IZkTKRI4FQrDHBSZUlql3RIK9o30TtbfBSwE7TS-5r4nLZiN9k_ZDtZxZp-lj5dAuXApL4nxiI_6wVkSqxOJkNoi1-Q54iow2jRqNZ1DnKSrY72mjBOr5N0l2vR9FKuHzhPfd65EXuSuyMCZPwURSX-3Ilox-HLWsptvWUDogUkDl8MrNEqDqOVM0pylHeHMJZnR_VYeAmvCq1TMha2nn7U_Z49oEFR9lyBofleB5vIGD3fRqFGIPliQCpKbQolBCdMJgzoyTOIswwoTrUOtpMwk28NLtd8GfJQvJi0zHybNdt3HPYWPp80HEBVfKPDJgkoACh_6QjrDSQDpMPfRKcGrgFkzueQY07tky0MbderFnlaOBn5UPyQ_Fsz1m0Vc1i5WHqauOC3HLdQMxT2V3Zh9iJ0pQBiwmIH48YliiH6nb_hgCEMYIzfD7CWdUvt0DNOc9Byzu9O_JowW3Hd9JN1kPPd8Nj5GwGf99l75jalACiCJX8dQeNIojyoRCFG5cxm_t9HMIsvkGlIqUoyje5Q7ZA6SukSsc8r0myaO5XzXH-YOOK1XkGT95p5nvtae9vI4cqY-eY-bEHojdk8Hf-fASr_K32uCHPMZcw0BeaINFk4xKeKq4urDGxNbRhsmOsbtJi0zbCSdBQzn3Of8iKD18HYxaEEK8foiGfBKo5vUC1HcQevz7CNLMQ1SWrUpsMmB-nKqhTpFWeYMlr2X3ZXvhH9mfZiQ6UB47xd-po9W_-b_id-oXrf_2oDocGpy6rFb4ctA26JK8QgAyVFZ4cxxjJEtQ_qF_FXuts7z24dP1Bvln5bN16A0wMadKGBZraffNlAaMOjh-JN4Evs_t_DYMmkhO5LZFDqRCmS7MZumPNReM912OvO7U28EnaTtJP9FTSdumNCnL_lOV2_VnzlP1h_oIWeyCRHaHzjh6dDWz4fweEDbMEtf7ALqZemyurDpojqCutVaYxrjKuX6hq03_yZMVqCoD5YwZpGX3_ksxY32XpZRNkFV4BjxqONaoRlj2yTLgGvTHISoIOlR2aHKEfqCelK9Um1yDibthTAFQFRftpCnnJZ-BTzW8OaBF89obidCKpDIgptxWJLopCkRCbQ8AnyfqGDZMZmkGSQ4xO50HMaqdr0kvLac5IAFPDZuBW11_ahRFpGJr3Vuxq_nn6hiRk2GTrcfR5H3AhaiyOOpFLrhikTI8ulDa-Pc473Wi9WMUn3GzHVdlK008AY_Z403Hag-5c4UsWatFe81cNoBd9B4UEpBKV86f7ihi-M7k3mSy1MrNWtjejEck5r1THUb9JuS3yUM4002PPVdBXwH7LV8Ne0EkYatZtFXnfcQBlKH8ejiuND2Ludfx6A6n6q_S2F7wtsy_aMJEWzDi0PcNrxmjsP_NKyzLtKLQ7wkDDb_E"
                });
                loc1 = new WebClient();
                loc1.Headers[HttpRequestHeader.ContentType] = "application/x-protobuffer";
                return Encoding.Default
                    .GetString(loc1.UploadData(
                        "https://www.google.com/recaptcha/api2/reload?k=6LcxuOsUAAAAAI2IYDfxOvAZrwRg2T1E7sJq96eg",
                        loc2.ToArray())).Split(',')[1].Replace("\"", "");
            }
            catch
            {
                return "Error";
            }
        }
    }
}
