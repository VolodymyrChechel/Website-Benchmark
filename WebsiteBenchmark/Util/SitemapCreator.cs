using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace WebsiteBenchmark.Util
{
    public class SitemapCreator
    {
        private string _url;
        private Dictionary<string, TimeSpan> urls;
        private string aTagRegEx = "<a\\s+(?:[^>]*?\\s+)?href=([\"'])(.*?)\\1";
        private List<UrlInfo> urlList;

        public SitemapCreator(string url)
        {
            if(url.EndsWith("/"))
                _url = url;
            else
                _url = url + "/";

            StartCrawling();
        }

        private void StartCrawling()
        {
            //WebClient client = new WebClient();

            //// some sites block robots, so we imitate real user adding the header with user-agent
            //client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

            //string content =
            //    client.DownloadString(_url);

            var htmlWeb = new HtmlWeb();
            var htmlDoc = htmlWeb.Load(_url);

            var aSelected = htmlDoc.DocumentNode.DescendantsAndSelf("a").Where(a => a.Attributes["href"] != null);

            //// TEST
            //var aSelectedVIEW = aSelected.Select(a => new
            //{
            //    Href = a.Attributes["href"].Value,
            //    Title = a.InnerHtml

            //}).ToList();

            //select links on page with "http" prefix
            var hrefs = aSelected.Where(a => a.Attributes["href"].Value.StartsWith("http", StringComparison.InvariantCultureIgnoreCase)).
                         Select(a => new UrlInfo { Url = a.Attributes["href"].Value, Title = a.InnerHtml}).
                         ToList();

            //select links on page without "http" prefix
            var hrefsPart = aSelected.Where(a => a.Attributes["href"].Value.Length > 1 && 
            !a.Attributes["href"].Value.StartsWith("http", StringComparison.InvariantCultureIgnoreCase)).
                Select(a => new UrlInfo() { Url = _url + a.Attributes["href"].Value, Title = a.InnerHtml}).
                ToList();

            hrefs.AddRange(hrefsPart);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var hrefTimings = new List<Tuple<string, double>>();
			
			// trying to assess time of all pages
            foreach (var href in hrefs)
            {
                watch.Restart();
                htmlWeb.Load(href.Url);
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                href.RequestTime = elapsedMs;

                //// Create a request using a URL that can receive a post.   
                //WebRequest request = WebRequest.Create(href.Href);
                //// Set the Method property of the request to POST.  
                //request.Method = "GET";
                //// Create POST data and convert it to a byte array.  
                //// Get the response.  
                //WebResponse response = request.GetResponse();
                //// Display the status.  
                //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                //// Get the stream containing content returned by the server.  
                //dataStream = response.GetResponseStream();
                //// Open the stream using a StreamReader for easy access.  
                //StreamReader reader = new StreamReader(dataStream);
                //// Read the content.  
                //string responseFromServer = reader.ReadToEnd();
                //// Display the content.  
                //Console.WriteLine(responseFromServer);
                //// Clean up the streams.  
                //reader.Close();
                //dataStream.Close();
                //response.Close();
            }


            //get all tags with href attribute using regular expression
            //var aTagsList = Regex.Matches(content, aTagRegEx)
            //    .Cast<Match>().Select(m => m.Value).ToList();

            //for (int i = 0; i < aTagsList.Count; i++)
            //{

            //    aTagsList[i] = aTagsList[i].Substring();
            //}

        }
    }
}