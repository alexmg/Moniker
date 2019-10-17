

# Moniker

## What is it?

Moniker is a tiny .NET library and CLI tool for generating fun names. It started as a port of the [moniker](https://github.com/technosophos/moniker) Go package that is used by the [Helm](https://github.com/helm/helm) project to generate release names, but was extended to also include the name generator used by the [Moby](https://github.com/moby/moby) project for naming Docker containers.

Both name generators use a scheme that combines a random value from a list of adjectives with another random value from a list of nouns. In the case of Moniker the nouns are [animals](https://github.com/technosophos/moniker/blob/master/animals.txt) and for Moby they are [notable scientists and hackers](https://github.com/moby/moby/blob/master/pkg/namesgenerator/names-generator.go#L114).

## Moniker style examples

```
wobbly-whippet
hardy-lynx
rafting-deer
lame-gecko
```

## Moby style examples

```
relaxed-booth
stoic-franklin
thirsty-williamson
objective-gould
```

## .NET Standard Library

To use the .NET Standard 2.0 library install the package from NuGet.

```
Install-Package Moniker
```

Add a `using` statement for the `Moniker` namespace.

```csharp
using Moniker;
```

The static `NameGenerator` class contains methods for generating new monikers:

- `GenerateMoniker` - generates a Moniker style name
- `GenerateMoby` - generates a Moby style name
- `Generate` - generates a name using the provided `MonikerStyle` enum value

All methods accept an optional parameter for the delimiter to be used between the adjective and noun values that are concatenated in these naming schemes. This is a `string` parameter so the separator can contain multiple characters if required and a space is allowed. The default separator is the `-` character (e.g. `pensive-jennings` and `wishful-shrimp`).

```csharp
// Moniker examples
var moniker = MonikerGenerator.GenerateMoniker(); // Default delimiter
moniker = MonikerGenerator.GenerateMoniker("_"); // Custom delimiter
moniker = MonikerGenerator.Generate(MonikerStyle.Moniker); // Default delimiter
moniker = MonikerGenerator.Generate(MonikerStyle.Moniker, "_"); // Custom delimiter

// Moby examples
moniker = MonikerGenerator.GenerateMoby(); // Default delimiter
moniker = MonikerGenerator.GenerateMoby("_"); // Custom delimiter
moniker = MonikerGenerator.Generate(MonikerStyle.Moby); // Default delimiter
moniker = MonikerGenerator.Generate(MonikerStyle.Moby, "_"); // Custom delimiter
```

## .NET Core Global Tool

To install the .NET Core Global Tool run the following `dotnet` command.

```
dotnet tool install -g Moniker.Cli
```

Once installed the tool can be accessed using the `moniker` command. Run `moniker -h` to see the usage instructions.

```
Usage: moniker [options]

Options:
  --version                   Show version information
  -s|--style <STYLE>          The style of moniker to generate (Moniker or Moby)
  -d|--delimiter <DELIMITER>  The delimiter to use between name parts such as adjective and noun
  -?|-h|--help                Show help information
```

The `STYLE` option is not mandatory and can be specified in the short form `-s` or long form `--style`. The provided value should be either `Moniker` or `Moby` depending on the desired name generator style. If omitted the moniker style defaults to `Moniker`.

The `DELIMITER` option is not mandatory and can be specified in the short form `-d` or long form `--delimiter`. If omitted the default `-` delimiter will be used.

In the example below a Moby style name is generated with the non-default `_` delimiter. If you want to use the default `-` delimter the `-d` option can be omitted.

```
moniker -s Moby -d _
```

The long form can also be used for improved readabilty. As before, the `--delimiter` option can be omitted to use the default `-` delimiter.

```
moniker --style Moby --delimiter _
```

Capturing the generated name in a variable from the Windows command prompt requires some rather obscure syntax (use `%%i` from inside a batch file). Please let me know if you are aware of a better way.

```
for /f %i in ('moniker') do set NAME=%i
echo %NAME%
```

Capturing the output in a PowerShell variable is a simple assignment statement.

```PowerShell
$name = moniker
Write-Host $name
```

## A note about uniqueness

It should be noted that the lists are not particularly long and the total number of possible combinations is somewhat small (94,138 for Moniker and 25,380 for Moby). If you require names to be unique you will need to append an additional element such as a timestamp or detect duplicates yourself and request another name.

```
$moniker = "$(moniker)-$(Get-Date -Format 'yyMMdd')"
```

You could also generate names using both moniker styles to extend the total range of possible names.

```PowerShell
$moniker = "$(moniker -s moby)-owns-$(moniker -s moniker)"
```

This will provide longer but still fun sounding names like `vibrant-newton-owns-warped-rabbit` or `romantic-cannon-owns-deadly-elephant`.