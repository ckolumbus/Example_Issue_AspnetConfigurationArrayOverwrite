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

public class AspnetConfigurationArrayOverwrite
{
    public static IConfiguration Configuration { get; set; }

/*
Arrays are not complete replaced, but only the entries are
overwritten that are given in the config files being read 
later

Setup: 
 * read appsettings.json with two entries in 'wizard'  array
 * read additaonal appsettings2.json with only one array entry in 'wizards'

 * (My) Expected output: the array is replaced by the latest definition
   resulting in only one entry in the array
    [wizards, ]
    [wizards:0, ]
    [wizards:0:Name, Houdini]
    [wizards:0:Age, 300]

* Real Output: only the first entry of the existing array is overwritten
    [wizards, ]
    [wizards:1, ]
    [wizards:1:Name, Harry]
    [wizards:1:Age, 17]
    [wizards:0, ]
    [wizards:0:Name, Houdini]
    [wizards:0:Age, 300]


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

        foreach (var i in Configuration.AsEnumerable()) {
            Console.WriteLine(i);   
        }
        
        Console.WriteLine();

        Console.WriteLine("Press a key...");
        Console.ReadKey();
    }
}
