# Have-Fun V1.0 Business Requirements

## Summary

Have-Fun should grow from a single Word Scramble game into a local multi-game party app. The app still runs on one host computer, players still join from browsers on the same LAN, and all state remains in memory.

V1.0 focuses on the shared app shell, role-based Home page, avatar selection, game selection, and Word Scramble master-screen improvements.

## Goals

- Support multiple games from one shared app shell.
- Let the Game Master select the active game for all players.
- Use SignalR to notify player browsers when the selected game changes.
- Let each game define its own master dashboard and player page.
- Keep the app LAN-only, account-free, and database-free.
- Keep Word Scramble playable while preparing the app for additional games.

## Layout Changes

- Move the `Hi, ...` greeting to the center of the top bar.
- Keep navigation role-aware.
- Show game names in the menu, starting with `Word Scramble`.

## Home Page

After registration, both roles navigate to `/home`.

The Master Home page shows:

- The shared LAN URL with a prompt to enter it in another browser's address bar.
- Registered players with their selected avatars.

The Player Home page shows:

- The player's selected avatar.
- A message such as `Wait for Game Start`.
- Avatar selection so the player can choose or change their avatar.

The master uses a nice hard-coded avatar.

## Game Selection

The menu lists available games. V1.0 includes one game: `Word Scramble`.

The Game Master can select a game from the menu. Players can see game menu entries, but they cannot manually select games.

When the Game Master selects a game, the app sends a SignalR message to all players so their browsers navigate to the selected game page.

Each game may have game-specific communication between the master and players. Each game must provide its own master dashboard and its own player page.

## Word Scramble Changes

The Word Scramble master dashboard can be shared on a common screen, so it must not reveal the sentence while a round is active.

The Word Scramble master dashboard shows:

- Start button.
- Restart button.
- Finish button.
- Timer.
- Sortable submission and results grid.

The submission and results grid shows player names, avatars, submission duration, per-sentence score, and total score. All columns are sortable, and the default sort is by player name.

Start begins the first or next sentence. Restart resets the game from the first sentence. Finish ends the current sentence immediately without waiting for the timer to expire.

When a sentence finishes, the app shows the correct sentence, scores each player's current partial sentence, and increments each player's cumulative total score. Total score is the sum of correct words across sentences.

After results are shown, the master clicks Start to begin the next sentence.

The Word Scramble player page stays functionally as-is for V1.0.

## Avatar Feature

Players choose an avatar on the Home page after registration.

Avatar images live in `assets/avatars`. The app loads avatar definitions in memory and stores the selected avatar filename in browser session storage with the current user's name and role.

The master avatar is hard-coded and does not need to be selected.

## Constraints

- Use the existing .NET / ASP.NET Core Blazor Web App.
- Use Blazor `InteractiveServer` render mode.
- Use MudBlazor for web UI.
- Keep implementation code under `src`.
- Keep all game state in memory.
- Do not add a database, cloud service, public hosting, authentication, accounts, or passwords in V1.0.
