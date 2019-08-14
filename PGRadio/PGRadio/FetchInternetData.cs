using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using HtmlAgilityPack;
using Org.Apache.Http.Client;

namespace PGRadio
{
    static class FetchInternetData
    {

        static string imageURL { get; set; }
        static string infoText { get; set; }

        public static bool DataChange()
        {
            string requestURL = "https://www.purdueglobalradio.com/wp-content/uploads/2018/05/radio.html";


            HtmlWeb web = new HtmlWeb();


            //HtmlDocument doc = new HtmlDocument();
            var htmlDoc = web.Load(requestURL);            
            HtmlNode Nodes = htmlDoc.DocumentNode;
            
            foreach (var e in Nodes.ChildNodes)
            {
                if (e.HasAttributes)
                {
                    HtmlAttribute att = e.Attributes[0];
                    if(att.Name == "class")
                    {
                        if(att.Value == "radioco-nowPlaying")
                        {
                            infoText = e.InnerText;
                        }
                        else if(att.Value == "radioco-image")
                        {
                            imageURL = e.Attributes[1].Value;
                        }
                    }
                }
            }           
            return true;
            
        }




    }
}