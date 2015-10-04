using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebInfoCrawler
{
    class Program
    {
        const string singtelUpdateUrl = "http://info.singtel.com/personal/phones-plans/mobile/ios/iphone6s-updates?type=1&device=2550&colour=";
        static void Main(string[] args)
        {
            List<IPhoneStatus> iphoneRoseGoldList = crawleWebURL(singtelUpdateUrl+"rosegold");
            var availableList = iphoneRoseGoldList.Where(a => !a.status64G.Equals(SingtelIphone6sUpdate.NA)).ToList();

            //List<IPhoneStatus> iphoneGoldList = crawleWebURL(singtelUpdateUrl + "gold");
            //var availableGoldList = iphoneRoseGoldList.Where(a => !a.status64G.Equals(SingtelIphone6sUpdate.NA)).ToList();
            Console.WriteLine(availableList.Count);
            if (availableList.Count>0)
            {
                new EmailHelper().sendAlertEmail(availableList);
            }
        }

        private static List<IPhoneStatus> crawleWebURL(string url)
        {
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            WebResponse response = (HttpWebResponse)request.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();//File.ReadAllText("C:\\repsonse.html");//
            //string responseFromServer = File.ReadAllText("C:\\repsonse.html");// local test
            // Display the content.
            //Console.WriteLine(responseFromServer);

            responseFromServer = WebUtility.HtmlDecode(responseFromServer);
            HtmlDocument resultat = new HtmlDocument();
            resultat.LoadHtml(responseFromServer);
            SingtelIphone6sUpdate update = new SingtelIphone6sUpdate();
            List<IPhoneStatus> iphoneList = update.parseHtmlStatus(responseFromServer);
            return iphoneList;
        }
    }
}
