# PracticeProject_Dotnet

A minimal ASP.NET Core 8.0 web application that responds with "Hello World!".

## Stack
- .NET 8.0 (ASP.NET Core minimal API)
- Project path: `PracticeProject_Dotnet/PracticeProject_Dotnet/`

## Run (dev)
The "Start application" workflow runs:
```
cd PracticeProject_Dotnet/PracticeProject_Dotnet && ASPNETCORE_URLS=http://0.0.0.0:5000 dotnet run --no-launch-profile
```
Listens on `0.0.0.0:5000` so the Replit preview proxy can reach it.

## Deployment
Configured as `autoscale`:
- Build: `dotnet publish -c Release -o /tmp/publish`
- Run: `dotnet /tmp/publish/PracticeProject_Dotnet.dll` with `ASPNETCORE_URLS=http://0.0.0.0:5000`
