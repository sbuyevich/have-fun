# Have-Fun Agent Guide

## Project Intent

Have-Fun is a local LAN party-game web app. One computer hosts the server, and friends join from browsers on the same network. V1 focuses on a single Word Scramble game.

Use these files as the source of truth:

- `design/spec.md` for product behavior and constraints.
- `design/stages/*/plan.md` for stage-level implementation plans.
- `design/stages/*/tasks/*.md` for task-sized implementation work.

## Stack

- Use .NET / ASP.NET Core.
- Use Blazor Web App with `InteractiveServer` render mode.
- Keep implementation code under `src`.
- Solution: `src/HaveFun.sln`.
- Projects:
  - `src/HaveFun.Web`: Blazor UI and runnable web host.
  - `src/HaveFun.Core`: reusable backend models and services.

## Current Architecture Rules

- Keep app state in memory.
- Do not add a database for V1.
- Do not add cloud services, public hosting, accounts, passwords, or authentication for V1.
- Keep reusable game/configuration/sentence logic in `HaveFun.Core`.
- Keep Blazor pages, startup, app settings, and web-specific services in `HaveFun.Web`.
- Prefer typed models and services over ad hoc JSON or string handling.
- Keep razor and its code behind code in separate files
- Add services and data access should be in Core project.

## Repository Layout

```text
src/
  HaveFun.sln                Main solution
  HaveFun.Web/                 Blazor app, pages, layout, hubs, static files
  HaveFun.Core/                Services, models, EF Core data access, options
scripts/
  publish.ps1                  Publish one target package
  dist.bat                     Build Windows and macOS packages and zip them
design/                        Product, database, stage, and launch notes
```

### Folder Structure

#### HaveFun.Core project
- Services are in Services folder with Contracts subfolder with interfaces.
- Data access repos are in Data folder with Contracts subfolder with interfaces.
- Models are in Models folder.
- Ignore folders and sub folders namespace generation
- Keep `HaveFun.Core` namespace for all classes excepts Services

#### HaveFun.Web project
- Set explicit namespace in each razor file
- Move all layouts in Layouts folder
- Move all pages in Pages folder
- Move all component in Components folder
- Ignore folders and sub folders namespace generation
- Keep `HaveFun.Web` namespace for all classes 


## Configuration and Data

- Master name comes from `Game:MasterName` in `src/HaveFun.Web/appsettings.json`.
- Predefined sentences live in `src/HaveFun.Web/assets/sentences.json`.
- Sentence shape:

```json
[
  {
    "text": "The quick brown fox jumps over the lazy dog",
    "timeLimitInSeconds": 30
  }
]
```

- Validate required configuration and sentence data at startup.
- Fail clearly when local configuration/data is invalid.

## Implementation Workflow

- Work stage by stage.
- For Stage 1, complete tasks in `design/stages/Stage 1 - App Shell and Configuration/tasks`.
- Do not implement behavior from later stages unless the current task explicitly requires it.
- Keep Stage 1 free of name entry, player registration, dashboard routing, rounds, and gameplay.

## Verification

Run builds from `src`:

```powershell
dotnet build .\HaveFun.sln
```

When running the web app:

```powershell
dotnet run --project .\HaveFun.Web\HaveFun.Web.csproj
```

Before finishing implementation work, verify:

- Solution builds successfully.
- `HaveFun.Web` starts.
- No database or cloud dependency is introduced.
- Changes match the relevant task file and `design/spec.md`.
