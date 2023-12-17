using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using HtmlAgilityPack;

namespace WebScraper
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Program myProgram = new Program();
            myProgram.Start();
        }

        void Start()
        {
            string filename;
            WebsiteAttributes website = ReadUserInput(out filename);

            // Commenting out the following line as it's not defined in the provided code
            // ScrapeLinks(website.Url, website.XPath, filename);

            // Commenting out the following line as it's marked as under development
            // ScrapeTextToCsvFile(website.Url, website.XPath, filename);
        }

        static WebsiteAttributes ReadUserInput(out string filename)
        {
            WebsiteAttributes website = new WebsiteAttributes();
            string url;
            bool checkIfUrlIsValid;
            do
            {
                url = ReadString("Enter URL to scrape: ");
                checkIfUrlIsValid = CheckIfUrlIsValid(url);
            } while (!checkIfUrlIsValid);

            website.Url = url;
            website.XPath = ReadString("Enter XPath to scrape: ");
            filename = ReadString("Enter name of file to save the scraped data: ");
            return website;
        }

        static void ScrapeTextToCsvFile(string url, string xPath, string filename)
        {
            List<string> scrapedText = ScrapeInnerText(url, xPath);
            ExportInnerTextToCsvFile(scrapedText, filename);
        }

        static void ExportInnerTextToCsvFile(List<string> listOfInnerTexts, string filename)
        {
            using (StreamWriter writer = new StreamWriter($"{filename}.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(listOfInnerTexts);
            }
        }

        static List<string> ScrapeInnerText(string url, string xPath)
        {
            List<string> innerTexts = new List<string>();

            HtmlDocument doc = GetWebDocument(url);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(xPath);

            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    innerTexts.Add(node.InnerText);
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"XPath not found in this URL: {url}");
                Console.ResetColor();
            }

            return innerTexts;
        }

        static bool CheckIfUrlIsValid(string url)
        {
            HtmlWeb web = new HtmlWeb();

            try
            {
                HtmlDocument doc = web.Load(url);
                return true;
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"The format of the URI could not be determined, please try again");
                Console.ResetColor();
                return false;
            }
        }

        static HtmlDocument GetWebDocument(string url)
        {
            HtmlWeb web = new HtmlWeb();

            try
            {
                HtmlDocument doc = web.Load(url);
                return doc;
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Something happened: The format of the URI could not be determined, please try again");
                Console.ResetColor();
            }
            return null;
        }

        static string ReadString(string question)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(question);
            Console.ResetColor();
            return Console.ReadLine();
        }
    }

}
