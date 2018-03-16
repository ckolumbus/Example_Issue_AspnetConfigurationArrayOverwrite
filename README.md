# Example Issue : AspnetConfigurationArrayOverwrite

Example on how ASP.Net Core handles overwriting loaded array configuration
when the later read config files contain less array entries than before.

Arrays are not complete replaced but only the first `n` entries that are
actully given in the later config files are being replaced, leaving


## Exampmle configuration files 

`appsettings.json`

```
{
  "ipAddresses": [
    "192.168.57.1",
    "10.0.0.1"
  ]
}

```

`appsettings2.json`

```
{
  "ipAddresses": [
    "127.0.0.1"
  ]
}
```

## Setup

 * read appsettings.json with two entries in 'ipAddresses'  array
 * read additaonal appsettings2.json with only one array entry in 'ipAddresses'

## Code

```
var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddJsonFile("appsettings2.json");
```

## (My) Expected output
I wanted to set the array of `ipAddreses` to "127.0.0.1" **only**.
So the array should be replaced by the latest definition resulting in only 
one entry in the array.

```
    127.0.0.1
```

## Real Output
Only the first entry from `appsettings.json` ("192.168.56.1") has been
overwritten. Resulting in a mix from both setting files:

```
   127.0.0.1
   10.0.0.1
```

## Issue

when having multiple sources of configuration items it's somehwat
unexpected behavior that the ultimate entries of an array dependes
on how many entries have been given in the setting sources before.
