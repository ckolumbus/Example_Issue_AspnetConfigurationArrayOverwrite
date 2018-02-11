# Example Issue : AspnetConfigurationArrayOverwrite

Example on how ASP.Net Core handles overwriting loaded array configuration
when the later read config files contain less array entries than before.

Arrays are not complete replaced but only the first `n` entries that are
actully given in the later config files are being replaced, leaving


## Exampmle configuration files 

`appsettings.json`

```
{
  "wizards": [
    {
      "Name": "Gandalf",
      "Age": "1000"
    },
    {
      "Name": "Harry",
      "Age": "17"
    }
  ]
}
```

`appsettings2.json`

```
{
  "wizards": [
    {
      "Name": "Houdini",
      "Age": "300"
    }
  ]
}
```

## Setup

 * read appsettings.json with two entries in 'wizard'  array
 * read additaonal appsettings2.json with only one array entry in 'wizards'

## Code

```
var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddJsonFile("appsettings2.json");
```

## (My) Expected output

the array is replaced by the latest definition resulting in only one entry in the array
```
    [wizards, ]
    [wizards:0, ]
    [wizards:0:Name, Houdini]
    [wizards:0:Age, 300]
```
## Real Output
only the first entry of the existing array is overwritten, the second one remains
```
    [wizards, ]
    [wizards:1, ]
    [wizards:1:Name, Harry]
    [wizards:1:Age, 17]
    [wizards:0, ]
    [wizards:0:Name, Houdini]
    [wizards:0:Age, 300]
```

## Issue

when having multiple sources of configuration items it's somehwat
unexpected behavior that the ultimate entries of an array dependes
on how many entries have been given in the setting sources before.
