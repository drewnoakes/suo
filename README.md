# suo

[![Nuget version](https://img.shields.io/nuget/v/suo)](https://www.nuget.org/packages/Suo/)
[![Nuget download count](https://img.shields.io/nuget/dt/suo)](https://www.nuget.org/packages/Suo/)

A dotnet global tool for viewing the contents of Visual Studio user option (`.suo`) files.

# Installation

```text
dotnet tool install --global suo
```

# Usage

There are two sub-commands, `keys` and `view`.

## `keys` command

This command lists the keys found in a SUO file.

```text
suo keys <path>
```

Options:

- `--no-empty` Exclude keys whose values are empty
- `--show-size` Show the size in bytes alongside the key name

## `view` command

This command shows values associated with keys in a SUO file.

View all keys and values from the file (may be very long):

```
suo view <path>
```

View the contents of a specific key:

```text
suo view <key> <path>
```

You can specify more than one key

```text
suo view <key1> <key2> <path>
```

By default the output of a key is shown as a hex dump. You can control the formatting by specifying one of:

- `--format=utf16` uses UTF-16, which is the most common encoding for text in SUO files
- `--format=utf8` uses UTF-8
- `--format=ascii` uses ASCII
- `--format=hex` shows the hex dump (default)

