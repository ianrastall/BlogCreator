# BlogCreator

BlogCreator is a separate Windows desktop publishing application for the Fooliosity website.

## Current milestone

The repository now contains the first buildable application scaffold:

- C# 14 and .NET 10.
- WinUI 3 through the Windows App SDK.
- MVVM through `CommunityToolkit.Mvvm`.
- Dark-mode three-pane publishing shell.
- Post metadata and Markdown editing models.
- Markdig-powered preview HTML.
- Unit tests and a Windows GitHub Actions build.

Publishing, local draft persistence, Monaco syntax highlighting, and connection to `ianrastall.github.io` are subsequent milestones.

## Project boundary

- `ianrastall/BlogCreator` is this desktop application.
- `ianrastall/ianrastall.github.io` is the public website source.
- `ianrastall/MDEdit` is an independent Markdown editor and remains untouched.

BlogCreator reuses MDEdit's proven architectural pattern without sharing or modifying its source tree.

## Build

```powershell
dotnet restore BlogCreator.slnx
dotnet build BlogCreator.slnx -c Debug /p:Platform=x64
dotnet test tests/BlogCreator.Tests/BlogCreator.Tests.csproj
```

Run after building:

```powershell
.\src\BlogCreator.WinUI\bin\Debug\net10.0-windows10.0.19041.0\win-x64\BlogCreator.WinUI.exe
```
