using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using FluorineFx;
using FluorineFx.AMF3;

namespace msptoolui
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
            return BitConverter.ToString(MD5.Create().ComputeHash(bytes)).Replace("-", "").ToLower() + BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
        public static string createTicket(object[] parms)
        {
            SHA1 sha1 = SHA1.Create();
            string tval = parms.OfType<TicketHeader>().Select(thrObj =>
            {
                var tps = thrObj.Ticket.Split(',');
                return $"{tps.FirstOrDefault()}{tps.LastOrDefault()?.Substring(Math.Max(0, tps.LastOrDefault().Length - 5), Math.Min(tps.LastOrDefault().Length, 5))}";
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
    sb.Append(bav.Length <= 20 ?
        BitConverter.ToString(bav).Replace("-", "").ToLower() :
        BitConverter.ToString(Enumerable.Range(0, 20).Select(i => bav[(bav.Length / 20) * i]).ToArray()).Replace("-", "").ToLower());
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
    }
}

 internal class TicketHeader
{
    public string Ticket { get; set; }

    public string anyAttribute { get; set; }
}
