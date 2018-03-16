/**
 * File: AspnetConfigurationArrayOverwrite.cs
 * Created Date: Sunday February 11th 2018
 * Author: Chris Drexler, ckolumbus@ac-drexler.de
 * -----
 * Copyright (c) 2018 Chris Drexler
 * 
 * MIT License : http://www.opensource.org/licenses/MIT
 */


// example adapted from : https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/


using System;
using System.IO;
using System.Linq;
// Requires NuGet package
// Microsoft.Extensions.Configuration.Json
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

public class AspnetConfigurationArrayOverwrite
{
    public static IConfiguration Configuration { get; set; }

/*
Arrays are not complete replaced, but only the entries are
overwritten that are given in the config files being read 
later

Setup: 
 * read appsettings.json with two entries in 'ipAddresses'  array
 * read additional appsettings2.json with only one array entry in 'ipAddresses'

 * (My) Expected output: the array is replaced by the latest definition
   resulting in only one entry in the array
    [ipAdresses:0, "127.0.0.1" ]

* Real Output: only the first entry of the existing array is overwritten
    [ipAdresses:0, "127.0.0.1"]
    [ipAdresses:1, "10.0.0.1" ]

  only the first entry from `appsettings.json` ("192.168.56.1") has been
  overwritten.

Issues:
  when having multiple sources of configuration items it's somehwat
  unexpected behavior that the ultimate entries of an array dependes
  on how many entries have been given in the setting sources before.


 */

    public static void Main(string[] args = null)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings2.json");

        Configuration = builder.Build();

        var ipList = new List<string>();
        Configuration.GetSection("ipAddresses").Bind(ipList);

        //Console.WriteLine(ipList.Count);
        foreach (var i in ipList) {
            Console.WriteLine(i);   
        }
        
        
    }
}
