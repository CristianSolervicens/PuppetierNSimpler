
//using PuppeteerSharp;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace PuppetierNSimpler;

public class Program
{
    public static async Task Main(string[] args)
    {
        ProxyList proxyes = new ProxyList();
        //proxyes.Add(new Proxy("http://34.140.70.242:8080"));
        //proxyes.Add(new Proxy("http://118.69.111.51:8080"));
        //proxyes.Add(new Proxy("http://15.204.161.192:18080"));
        //proxyes.Add(new Proxy("http://186.121.235.66:8080"));
        //proxyes.WriteProxiesToJsonFile("proxies.json");
        proxyes.ReadProxiesFromJsonFile("proxies.json");

        for (int i = 0; i < proxyes.Count; i++)
        {
            Console.WriteLine($"Proxy: {proxyes[i].Address}");
        }

        Console.WriteLine("");
        ProxyDef proxy = proxyes.GetRandomProxy();
        Console.WriteLine($"Random Proxy: {proxy.Address}");
        Console.WriteLine("");

        //Puppeteer
        try
        {
            var scrap = new Scrapper();
            Console.WriteLine("Scraping with Puppeteer");
            await scrap.Scrap(proxy);
        }
        catch (Exception e)
        {
            Console.WriteLine("Scrap " + e.Message);

        }
        
        Console.WriteLine("");

        ///Simple CON proxy
        try
        {
            var scrap = new Scrapper();
            Console.WriteLine("Scraping with HTTPRequest");
            var value = scrap.Scrapper2(proxy);
            Console.WriteLine($"Mi IP de Salida {value}");
        }
        catch (Exception e)
        {
            Console.WriteLine("Scrapper2 " + e.Message);

        }

        Console.WriteLine("");

        // WebRequest con Proxy
        try
        {
            var scrap = new Scrapper();
            var value = scrap.ScrapperWR(proxy);
            Console.WriteLine($"Mi IP de Salida WR con Proxy {value}");
        }
        catch (Exception e)
        {
            Console.WriteLine("ScrapperWR " + e.Message);
        }

        Console.WriteLine("");

        // HttpClient
        try
        {
            var scrap = new Scrapper();
            var value = scrap.ScrapperHttpClientAsync(proxy);
            Console.WriteLine($"Mi IP de Salida HttpClient con Proxy {value}");
        }
        catch (Exception e)
        {
            Console.WriteLine("ScrapperHttpClientAsync " + e.Message);
        }

        Console.WriteLine("");
        // PlayWright
        try
         {
            var scrap = new Scrapper();
            Console.WriteLine("Scraping with PlayWright");
            await scrap.ScrapPLayWright(proxy);
            
        }
        catch (Exception e)
        {
            Console.WriteLine("ScrapperPlaywright " + e.Message);
        }

        Console.WriteLine("");

        ///Simple SIN proxy
        try
        {
            var scrap = new Scrapper();
            var value = scrap.ScrapperNoProxy();
            Console.WriteLine($"Mi IP de Salida NORMAL {value}");
        }
        catch (Exception e)
        {
            Console.WriteLine("ScrapperNoProxy " + e.Message);
        }

        Console.WriteLine("");

    }
}

