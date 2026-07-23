# Backend — getting started

This is an ASP.NET Core API project to expose a series of REST endpoits to help manage characters HP

## Prerequisites

- .NET SDK 10.0+ (`dotnet --version` to check; currently pinned to
  `net10.0` in `woc_backend_challenge.csproj`)

## Project layout

- `Properties/launchSettings.json` — dev run configuration. Pins the app to `http://localhost:5080` so the port is predictable.

## Running it

```bash
dotnet restore   # first time only, pulls NuGet packages
dotnet run
```

The API listens on the follwoing URL: `http://localhost:5080`.

## Verifying it works

```bash
curl http://localhost:5080/api/healthz
# => {"message":"pong"}
```

## Running tests

```bash
dotnet test woc_backend_challenge.Tests/woc_backend_challenge.Tests.csproj
```

Covers `Character`'s business-logic methods, `CharacterRepository`, and the
handlers (called directly as plain methods, no HTTP server needed).

## Frontend integration note

The character id is a UUID assigned at load time, and there's currently no
endpoint to discover it from outside the process. `frontend/src/api/config.ts`
hardcodes the id this API assigns on this machine
(`1a91ce40-fadc-400a-9b68-5b6bc0bcf68c`) purely so the UI has something to
call — it's not a real client-facing lookup mechanism. If the API is
restarted and re-seeds with a new id, that constant needs updating (or the
API needs a discovery endpoint) for the frontend to keep working.

## What's here vs. what's not

## Assumptions
- Based on briv.json file, it is assumed the API should only manage a single player.
However I considered the scenario of handling multiple players, so it gets stored 
into a HashMap (I know other fancier solutions could be used, realized about that
too late)
- No information about the defenses. I assumed `resistance` means that the damage
being thrown at a character is halved somehow, I assumed 50%; for immunity, damage
would be zero. We could also have vulnerabilities in the same way, multiplying the 
damage
- There's no documentation about how stats should be used, so they are only shown.
Same for the class of the player
- I added an id which is an uuid, to identify uniquely the player

Routes are currently defined directly in `Program.cs`:

```csharp
app.MapGet("/api/ping", () => Results.Ok(new { message = "pong" }));
```
