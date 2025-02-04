﻿using PuppeteerSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PuppetierNSimpler;


public class ProxyDef
{
    public string Address { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    public ProxyDef(string address, string username = "", string password = "")
    {
        this.Address = address;
        this.Username = username;
        this.Password = password;
    }
}


public class ProxyList: List<ProxyDef>
{
    
    public ProxyDef GetRandomProxy()
    {
        Random random = new Random();
        int randomIndex = random.Next(this.Count);
        return this[randomIndex];
    }

    public int ReadProxiesFromJsonFile(string filePath)
    {
        if (!System.IO.File.Exists(filePath))
        {
            return 0;
        }
        // Read the file
        string json = System.IO.File.ReadAllText(filePath);
        // Deserialize the JSON
        List<ProxyDef> proxyList = System.Text.Json.JsonSerializer.Deserialize<List<ProxyDef>>(json);
        // Add the proxies to the list
        foreach (ProxyDef proxy in proxyList)
        {
            this.Add(proxy);
        }
        return proxyList.Count;
    }

    public int WriteProxiesToJsonFile(string filePath)
    {
        if (this.Count == 0)
        {
            return 0;
        }
        // Serialize the list of proxies to JSON
        string json = System.Text.Json.JsonSerializer.Serialize(this);
        // Write the JSON to the file
        System.IO.File.WriteAllText(filePath, json);
        return this.Count;
    }

}

