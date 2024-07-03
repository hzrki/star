using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using FluorineFx;
using FluorineFx.AMF3;
using FluorineFx.IO;

namespace msptool
{
    internal class Checksum
    {
        public static int markingID = new Random().Next(0, 1000);

        public static string actor(string ticket)
        {
            return ticket + getMarkingId();
        }

        public static string getMarkingId()
        {
            markingID++;
            byte[] bytes = Encoding.UTF8.GetBytes(markingID.ToString());
            return BitConverter.ToString(MD5.Create().ComputeHash(bytes)).Replace("-", "").ToLower() +
                   BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }

        public static string createTicket(object[] parms)
        {
            SHA1 sha1 = SHA1.Create();
            string tval = parms.OfType<TicketHeader>().Select(thrObj =>
            {
                var tps = thrObj.Ticket.Split(',');
                return
                    $"{tps.FirstOrDefault()}{tps.LastOrDefault()?.Substring(Math.Max(0, tps.LastOrDefault().Length - 5), Math.Min(tps.LastOrDefault().Length, 5))}";
            }).FirstOrDefault();

            if (tval == null)
                tval = "XSV7%!5!AX2L8@vn";

            string dth = $"{foi(parms)}2zKzokBI4^26#oiP{tval}";
            byte[] hbs = sha1.ComputeHash(Encoding.UTF8.GetBytes(dth));
            return BitConverter.ToString(hbs).Replace("-", "").ToLower();
        }

        private static string foi(object obj)
        {
            if (obj is TicketHeader || obj == null)
                return "";

            StringBuilder sb = new StringBuilder();
            switch (obj)
            {
                case string str:
                case int intval:
                case long longval:
                case double doubleval:
                case bool boolval:
                    sb.Append(obj);
                    break;
                case DateTime dtv:
                    sb.Append(dtv.ToString("yyyyMMdd"));
                    break;
                case byte[] bav:
                    sb.Append(bav.Length <= 20
                        ? BitConverter.ToString(bav).Replace("-", "").ToLower()
                        : BitConverter
                            .ToString(Enumerable.Range(0, 20).Select(i => bav[(bav.Length / 20) * i]).ToArray())
                            .Replace("-", "").ToLower());
                    break;
                case object[] arv:
                    foreach (object item in arv)
                        sb.Append(foi(item));
                    break;
                case ArrayCollection arrcv:
                    foreach (object item in arrcv.ToArray())
                        sb.Append(foi(item));
                    break;
                case ASObject as0val:
                    sb.Append(as0val.ToString());
                    break;
                default:
                    if (obj != null)
                        foreach (var prop in obj.GetType().GetProperties().OrderBy(prop => prop.Name))
                            sb.Append(foi(prop.GetValue(obj, null)));
                    else
                        sb.Append("");
                    break;
            }

            return sb.ToString();
        }
        
        public static object AMFConn(string server, string method, object[] parms)
        {
            AMFMessage loc = new AMFMessage(3);
            loc.AddHeader(new AMFHeader("sessionID", false,
                "NWZhNTNjNTgxYWQ1NDRlZjIzZjAyMjgyNmM0OGU0NWI3MTVkNzI3ODE4YTUwMg=="));
            loc.AddHeader(new AMFHeader("needClassName", false, false));
            loc.AddHeader(new AMFHeader("id", false, createTicket(parms)));
            loc.AddBody(new AMFBody(method, "/1", parms));

            using (MemoryStream memoryStream = new MemoryStream())
            {
                AMFSerializer loc2 = new AMFSerializer(memoryStream);
                loc2.WriteMessage(loc);
                loc2.Flush();

                WebClient webClient = new WebClient();
                webClient.Headers[HttpRequestHeader.UserAgent] =
                    "Mozilla/5.0 (Windows; U; en) AppleWebKit/533.19.4 (KHTML, like Gecko) AdobeAIR/32.0";
                webClient.Headers[HttpRequestHeader.Referer] = "app:/cache/t1.bin/[[DYNAMIC]]/2";
                webClient.Headers[HttpRequestHeader.ContentType] = "application/x-amf";


                try
                {
                    byte[] rbts = webClient.UploadData(
                        new Uri($"https://ws-{server}.mspapis.com/Gateway.aspx?method={method}"), "POST",
                        memoryStream.ToArray());

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


    internal class TicketHeader
    {
        public string Ticket { get; set; }

        public string anyAttribute { get; set; }
    }
}
