using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Reflection.Metadata;
using System.Threading.Tasks.Dataflow;
using HtmlAgilityPack;

namespace WebScraper // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            Program myProgram = new Program();
            myProgram.Start();

        }

        void Start(){

            //ScrapeLinks() Scrapes anchors values in a given page and saves result in a given text file.
            //ScrapeText() Scrapes inner text from a given xpath and saves it in a given text file.
            
        }


        void ScrapeText(string url, string xPath, string filename){
            
            List<string> scrapedText = ScrapeInnerText(url, xPath);
            ExportInnerTextToFile(scrapedText, filename);
            
        }

        void ExportInnerTextToFile(List<string> listOfInnerTexts, string filename){
            StreamWriter writer = new StreamWriter(filename);

            foreach(string text in listOfInnerTexts){
                writer.WriteLine(text);
            }
            writer.Close();
        }

        List<string> ScrapeInnerText(string url, string xPath){
            List<string> innerTexts = new List<string>();

            HtmlDocument doc = GetWebDocument(url);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(xPath);

            if(nodes != null){
                foreach(var node in nodes ){
                    innerTexts.Add(node.InnerText);
                
                }
            }
            else
            {
                System.Console.WriteLine($"{xPath} not found in this URL: {url}");
            }
        

            return innerTexts;
        }


        static HtmlDocument GetWebDocument(string url)
        {
            HtmlWeb Web = new HtmlWeb();
            HtmlDocument doc = Web.Load(url);
            return doc;
        }

        static List<string> GetLinksInPage(string url, string xPath){

            List<string> scrapedLinks = new List<string>();
            HtmlDocument doc = GetWebDocument(url);
            HtmlNodeCollection linkNodes = doc.DocumentNode.SelectNodes(xPath);
            var baseUri = new Uri(url);
            
                foreach(var link in linkNodes){
                    var hrefAttribute = link.Attributes["href"];

                    if (hrefAttribute != null)
                    {
                        string href = hrefAttribute.Value;
                        scrapedLinks.Add(new Uri(baseUri, href).AbsoluteUri);
                    }
                    
                }
            
            return scrapedLinks;
        }


        void extportToTextFile(string filename, List<string> dataToExport){

            StreamWriter writer = new StreamWriter(filename);

            foreach(string lineOfData in dataToExport){
                writer.WriteLine(lineOfData);
                
            }
            writer.Close();
        }    


        void extportToTextFile(string filename, string keyword, List<string> dataToExport){

            StreamWriter writer = new StreamWriter(filename);

            foreach(string lineOfData in dataToExport){
                if(lineOfData.Contains(keyword)){
                    writer.WriteLine(lineOfData);
                }
                
            }
            writer.Close();
        }

        void ScrapeLinks(string url, string xPath, string filename){
            List<string> linksFound = new List<string>();

            linksFound = GetLinksInPage(url, xPath);

            int linksCounter = 0;
            foreach(string link in linksFound){
                linksCounter++;
            }

            Console.WriteLine($"links found: {linksCounter}");

            extportToTextFile(filename,  linksFound);
            
        }

        void ScrapeLinks(string url, string keyword, string xPath, string filename){
            List<string> linksFound = new List<string>();

            linksFound = GetLinksInPage(url, xPath);

            int linksCounter = 0;
            foreach(string link in linksFound){
                linksCounter++;
            }

            Console.WriteLine($"links found: {linksCounter}");

            extportToTextFile(filename, keyword,  linksFound);
            
        }
    }
}