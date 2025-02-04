﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PuppeteerSharp;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Playwright;


namespace PuppetierNSimpler;

class Scrapper
{
    public async Task Scrap(ProxyDef proxy)
    {

        PuppeteerSharp.IBrowser ext_browser = null;
        BrowserFetcher browserFetcher = new();
        await browserFetcher.DownloadAsync();
        try { 
            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                Args = new[] { $"--proxy-server={proxy.Address}" }
            });
            ext_browser = browser;
            await using var page = await browser.NewPageAsync();
            if (proxy.Username != "" && proxy.Password != "")
            {
                Console.WriteLine("Proxy with credentials!!!");
                await page.AuthenticateAsync(new Credentials { Username = proxy.Username, Password = proxy.Password });
            }

            // Navigate to target URL
            await page.GoToAsync("http://wtfismyip.com/text");

            // Retreive page content
            var pageContent = await page.GetContentAsync();
            Console.WriteLine(pageContent);

            await browser.CloseAsync();
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            // Close the browser
            if (ext_browser != null)
                await ext_browser.CloseAsync();
        }
         
    }


    public async Task ScrapPLayWright(ProxyDef proxy)
    {
        var playwright = await Playwright.CreateAsync();

        Proxy pw_proxy = new Proxy
        {
            Server = proxy.Address
        };

        if (proxy.Username != "" && proxy.Password != "")
        {
            pw_proxy.Username = proxy.Username;
            pw_proxy.Password = proxy.Password;
        }

        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Proxy = pw_proxy
        });
        var context = await browser.NewContextAsync();
        var page = await context.NewPageAsync();
        await page.GotoAsync("http://wtfismyip.com/text");
        var pageContent = await page.TextContentAsync("body");
        Console.WriteLine(pageContent);
        await browser.CloseAsync();
    }


    public string Scrapper2(ProxyDef proxy)
    {
        using (WebClient client = new WebClient())
        {
            WebProxy wp = new WebProxy(proxy.Address);
            client.Proxy = wp;

            try
            {
                return client.DownloadString("http://wtfismyip.com/text");
            }
            catch (WebException e)
            {
                return $"{e.Message}";
            }
        }
    }


    //WebRequest DEPRECADO usar HttpClient
    public string ScrapperWR(ProxyDef proxy)
    {
        WebRequest request = WebRequest.Create("http://wtfismyip.com/text");
        WebProxy wp = new WebProxy(proxy.Address);
        if (proxy.Username != "" && proxy.Password != "")
        {
            wp.Credentials = new NetworkCredential(proxy.Username, proxy.Password);
        }
        request.Proxy = wp;
        try
        {
            WebResponse response = request.GetResponse();
            using (System.IO.Stream dataStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(dataStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
        catch (WebException e)
        {
            return $"{e.Message}";
        }
    }


    public async Task<string> ScrapperHttpClientAsync(ProxyDef proxy)
    {
        HttpClientHandler handler = new HttpClientHandler();
        handler.Proxy = new WebProxy(proxy.Address);
        if (proxy.Username != "" && proxy.Password != "")
        {
            handler.Credentials = new NetworkCredential(proxy.Username, proxy.Password);
        }
        HttpClient client = new HttpClient(handler);
        try
        {
            using HttpResponseMessage response = await client.GetAsync("http://wtfismyip.com/text");
            string responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }
        catch (HttpRequestException e)
        {
            return $"Request failed with error: {e.Message}";
        }
    }


    public string ScrapperNoProxy()
    {
        using (WebClient client = new WebClient())
        {
            try
            {
                return client.DownloadString("http://wtfismyip.com/text");
            }
            catch (WebException e)
            {
                return $"{e.Message}";
            }
        }
    }
}

