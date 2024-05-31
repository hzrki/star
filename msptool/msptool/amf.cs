using FluorineFx.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static msptool.Checksum;

namespace msptool
{
    internal class AMF
    {
        public static string server = "";
        public static object AMFConn(string server, string method, object[] parms)
        {
            AMFMessage loc = new AMFMessage(3);
            loc.AddHeader(new AMFHeader("sessionID", false, "NWZhNTNjNTgxYWQ1NDRlZjIzZjAyMjgyNmM0OGU0NWI3MTVkNzI3ODE4YTUwMg=="));
            loc.AddHeader(new AMFHeader("needClassName", false, false));
            loc.AddHeader(new AMFHeader("id", false, createTicket(parms)));
            loc.AddBody(new AMFBody(method, "/1", parms));

            using (MemoryStream memoryStream = new MemoryStream())
            {
                AMFSerializer loc2 = new AMFSerializer(memoryStream);
                loc2.WriteMessage(loc);
                loc2.Flush();

                WebClient webClient = new WebClient();
                webClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; en) AppleWebKit/533.19.4 (KHTML, like Gecko) AdobeAIR/32.0";
                webClient.Headers[HttpRequestHeader.Referer] = "app:/cache/t1.bin/[[DYNAMIC]]/2";
                webClient.Headers[HttpRequestHeader.ContentType] = "application/x-amf";


                try
                {
                    byte[] rbts = webClient.UploadData(new Uri($"https://ws-{server}.mspapis.com/Gateway.aspx?method={method}"), "POST", memoryStream.ToArray());

                    using (MemoryStream msr = new MemoryStream(rbts))
                    {
                        return DecodeAMF(msr.ToArray());
                    }
                }
                catch (Exception ex)
                {
                    return "ERROR! " + ex.ToString();
                }
            }
        }

        public static dynamic DecodeAMF(byte[] body)
        {
            return new AMFDeserializer(new MemoryStream(body)).ReadAMFMessage().Bodies[0].Content;
        }
    }
}
