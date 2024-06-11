using System.Net.Http;
using System.Net.Sockets;
using System.Net.WebSockets;
using WebSocket = WebSocketSharp.WebSocket;


namespace WindowsApplication2.My;

public class Utils
{

        private static HttpClient CreateHttpClient()
        {
            return new HttpClient
            {
                DefaultRequestHeaders =
                {
                    { "Referer", "app:/cache/t1.bin/[[DYNAMIC]]/2" },
                    { "User-Agent", "Mozilla/5.0 (Windows; U; en-US) AppleWebKit/533.19.4 (KHTML, like Gecko) AdobeAIR/32.0" },
                }
            };
        }

        public static string CreateFirstRequest(int actorId, string actorName, string country, string accessToken)
        {
            return $"42[\"704\",{{\"roomName\":\"theDressup\",\"specificRoomName\":\"\",\"messageType\":704,\"actorId\":{actorId},\"messageContent\":{{\"actorName\":\"{actorName}\",\"petId\":0,\"faceExpression\":\"neutral\",\"petType\":0,\"soGUID\":null,\"blacklistedMessage\":null,\"actorAction\":\"enter\",\"whitelistedMessage\":null,\"message\":null,\"posY\":null,\"animation\":\"stand\",\"posX\":null,\"effect\":null,\"facing\":null,\"clickItemIdString\":null,\"actorId\":{actorId},\"cacheId\":1,\"json\":null,\"locationIndex\":0,\"senderProfileId\":null,\"client\":0,\"bonsterAnimation\":null,\"trickIdx\":-1,\"compressedActorData\":null}},\"isPrivateRoom\":true,\"senderProfileId\":null,\"roomType\":\"theDressup\",\"country\":\"{country}\",\"accessToken\":\"{accessToken}\"}}]";
        }

        public static string CreatePingRequest()
        {
            return "42[\"500\",{\"messageContent\":null,\"pingId\":52,\"lastPingDelay\":121,\"senderProfileId\":null,\"messageType\":500}]";
        }

        public static string CreateSecondRequest()
        {
            return "42[\"707\",{\"senderProfileId\":null,\"messageType\":707,\"messageContent\":{\"answerText\":\"finished-dressing\"}}]";
        }

        public static string CreateMoneyRequest()
        {
            return "42[\"717\",{\"messageContent\":{\"messageContent\":null,\"money\":120,\"senderProfileId\":null,\"fame\":0,\"messageType\":703},\"senderProfileId\":null,\"messageType\":717}]";
        }

        public static void SendMultipleRequests(WebSocket ws, string request, int count)
        {
            for (int i = 0; i < count; i++)
            {
                ws.Send(request);
            }
        }
    }

