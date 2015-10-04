using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebInfoCrawler
{
    class SingtelIphone6sUpdate
    {
        public const string NA = "Not Available";
        public List<IPhoneStatus> parseHtmlStatus(string responseFromServer)
        {
            HtmlDocument resultat = new HtmlDocument();
            resultat.LoadHtml(responseFromServer);
            var querytable = resultat.DocumentNode.SelectNodes("//table").Cast<HtmlNode>().Take(1);

            // define the result status list
            List<IPhoneStatus> iphoneList = new List<IPhoneStatus>();

            HtmlDocument rowHtml = new HtmlDocument();
            rowHtml.LoadHtml(querytable.First().ChildNodes[1].InnerHtml);
            var insideRow = from row in rowHtml.DocumentNode.SelectNodes("//tr").Cast<HtmlNode>()
                            select row;
            //Console.WriteLine(cell.InnerText);
            
            foreach (var rowItem in insideRow)
            {
                
                StringBuilder sb = new StringBuilder();
                HtmlDocument cellHtml = new HtmlDocument();
                cellHtml.LoadHtml(rowItem.InnerHtml);

                if (cellHtml.DocumentNode.SelectNodes("//th")!=null)
                {
                    continue;
                }

                var cellRow = from row in cellHtml.DocumentNode.SelectNodes("//td").Cast<HtmlNode>()
                              select row;
                IPhoneStatus iphone = new IPhoneStatus();
                iphone.branch = parseCellValue(cellRow.ToList()[0]);             
                iphone.status16G = parseCellValue(cellRow.ToList()[1]);
                iphone.status64G = parseCellValue(cellRow.ToList()[2]);
                iphone.status128G = parseCellValue(cellRow.ToList()[3]);
                iphoneList.Add(iphone);
            }
            return iphoneList;
        }

        private string parseCellValue(HtmlNode enumerable)
        {
            string retValue;
            string originalText = enumerable.InnerText.Replace("\t", "");
            string[] textArr = originalText.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (textArr.Length > 1)
            {
                retValue = (textArr[0].Trim());
            }
            else
            {
                retValue = (originalText.Trim());
            }
            return retValue;
        }
    }

    public class IPhoneStatus
    {
        public enum StorageSize : int { GB16 = 16, GB64 = 64, GB128 = 128 };
        public enum StorageStatus : int { Avaiable = 0, SellingFast = 1, NotAvailable = 2 };

        public string branch { get; set; }
        public string status16G { get; set; }
        public string status64G { get; set; }
        public string status128G { get; set; }
    }
 
}
