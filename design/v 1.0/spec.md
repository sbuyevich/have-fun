# Have-Fun V1.0 Specification

## Summary

Have-Fun V1.0 turns the current Word Scramble app into a local multi-game party app foundation. V1.0 keeps the existing LAN-only, in-memory Blazor Server model while adding a role-based Home page, avatar selection, SignalR-driven game selection, and improved Word Scramble master controls and scoring.

## Goals

- Preserve the current local LAN party flow.
- Add `/home` as the post-registration landing page for both roles.
- Let players choose avatars after registration.
- Let the Game Master select the active game and move players to that game with SignalR.
- Give each game its own master dashboard and player page.
- Keep Word Scramble playable while adding cumulative scoring across sentences.
- Keep the app ready for more games later.

## Non-Goals

- No database or persistent leaderboard.
- No cloud service, public internet hosting, accounts, passwords, or authentication.
- No manual player registration by the master.
- No custom sentence editing in V1.0.
- No major Word Scramble player-page redesign in V1.0.

## Users and Roles

### Game Master

The Game Master is the user whose entered name matches `Game:MasterName` from `appsettings.json`.

The Game Master can:

- Register and land on `/home`.
- See the shared LAN URL.
- See all joined players and their avatars.
- Select `Word Scramble` from the menu.
- Send players to the selected game page through SignalR.
- Control Word Scramble with Start, Restart, and Finish.
- View the timer and sortable player results.

The Game Master uses a hard-coded master avatar.

### Player

Players are users whose entered names do not match the configured master name.

Players can:

- Register and land on `/home`.
- Select or change their avatar on `/home`.
- See `Wait for Game Start` on Home until the master selects a game.
- See game menu entries but not manually select games.
- Be navigated to the selected game page when the master selects a game.
- Play Word Scramble with the existing player interaction flow.

## Routes and Navigation

- `/` remains the registration page.
- `/home` is the post-registration landing page for both roles.
- Word Scramble uses a game-specific master dashboard and a game-specific player page.
- Master game selection sends a SignalR message that navigates players to the selected game page.
- If a user opens a role-restricted page without the required session state, the app sends them back to registration or shows a clear role/session error.

## Layout and Menu

- The top bar greeting `Hi, ...` is centered.
- Menu entries are role-aware.
- Game entries appear in the menu, starting with `Word Scramble`.
- Master game entries are selectable.
- Player game entries are visible but disabled or non-clickable because game navigation is controlled by the master.

## Home Page

### Master Home

Required UI:

- Shared LAN URL.
- QR code for the shared LAN URL.
- Prompt text telling users to enter the shared URL in a browser address bar.
- Joined player list.
- Player avatars.

Behavior:

- Master Home updates from in-memory player registry state.
- The QR code is shown only on Master Home.
- The QR code encodes the same shared LAN URL shown as text.
- The master does not choose an avatar.
- The player list shows self-registered players only.

### Player Home

Required UI:

- Player name.
- Current selected avatar.
- Avatar selection control.
- Waiting message such as `Wait for Game Start`.

Behavior:

- Player can select or change avatar manually.
- Selected avatar filename is stored in session storage with the player name and role.
- Player remains on Home until the master selects a game or until the player navigates away.

## Avatar Data

Avatar images live under `assets/avatars`.

Required behavior:

- The app loads available avatar filenames into memory.
- Player session storage stores the selected avatar filename.
- If no player avatar has been selected yet, the app uses a default player avatar.
- The master uses a hard-coded master avatar.

Session storage contains:

- Current user name.
- Current user role.
- Selected avatar filename for player users.

## Game Selection and Communication

V1.0 has a shared game-selection model for future games.

Required behavior:

- Available games are represented in memory.
- `Word Scramble` is the first available game.
- The Game Master can select the active game.
- Selecting a game updates server-side active game state.
- Selecting a game sends a SignalR message to connected players.
- Player browsers navigate to the selected game's player page after receiving the SignalR message.
- The Game Master navigates to the selected game's master dashboard.
- Each game must define its own master dashboard and its own player page.

No database, message broker, or external real-time service is used.

## Word Scramble V1.0

### Master Dashboard

The Word Scramble master dashboard must be safe to show on a shared screen. It must not display the active sentence while players are solving it.

Required UI:

- Start button.
- Restart button.
- Finish button.
- Timer.
- Sortable submission/results grid.

Submission/results grid columns:

- Player name.
- Avatar.
- Submission duration.
- Current sentence score.
- Total score.

Sorting:

- All displayed columns are sortable.
- Default sort is by player name.

### Player Page

The Word Scramble player page stays functionally as-is for V1.0.

Players continue to:

- See round status and timer.
- Click shuffled words.
- Build a collected sentence.
- Submit their result.
- See submitted state after submission.

### Sentence Flow

- Start begins the first sentence if no sentence has started yet.
- Start begins the next sentence after results are shown.
- Restart resets Word Scramble from the first sentence and clears cumulative game score.
- Finish ends the current sentence immediately.
- After Finish, results remain visible until the master clicks Start again.

### Scoring

Correctness is calculated by word position using exact word tokens.

When a sentence finishes:

- Submitted players are scored from their submitted sentence.
- Unsubmitted players are scored from their current partial collected sentence.
- The correct sentence is shown after scoring.
- Each player's current sentence score is shown.
- Each player's total score is incremented by the number of correct words for that sentence.

Total score is cumulative across sentences and equals the sum of correct words.

## Data and State

All V1.0 app state remains in memory except browser session storage.

Server in-memory state includes:

- Registered players.
- Available games.
- Active game.
- Active Word Scramble sentence index.
- Current Word Scramble round state.
- Player per-sentence state.
- Player current sentence scores.
- Player cumulative total scores.

Browser session storage includes:

- Current user's name.
- Current user's role.
- Player avatar filename.

Restarting the server clears registered players, active game state, rounds, sentence progress, and scores.

## Tech Stack

- .NET / ASP.NET Core Blazor Web App.
- Blazor `InteractiveServer` render mode.
- MudBlazor for web components and styles.
- Explicit SignalR hub for game-selection messages.
- Solution remains `HaveFun`.
- Implementation code remains under `src`.
- `HaveFun.Web` contains UI, startup, SignalR hub, session storage integration, game-specific master dashboards, game-specific player pages, and static assets.
- `HaveFun.Core` contains reusable models and in-memory services.

## Implementation Stages

### Stage 1: Home and Avatar Foundation

Build `/home` as the shared post-registration landing page and add avatar support.

Included:

- Redirect both roles to `/home` after registration.
- Show master Home with shared LAN URL, QR code, and joined players with avatars.
- Show player Home with selected avatar, avatar selection, and waiting message.
- Load avatar filenames from `assets/avatars`.
- Store player avatar filename in session storage.

Done when:

- Master and players land on `/home`.
- Master Home shows the LAN URL, QR code, and joined players.
- Player Home can select/change avatar.

### Stage 2: Multi-Game Shell and SignalR Navigation

Add the shared game-selection model and real-time navigation layer.

Included:

- Represent available games in memory, starting with `Word Scramble`.
- Show game entries in the role-aware menu.
- Let only the master select games.
- Add an explicit SignalR hub for game-selection messages.
- Navigate master to the selected game's master dashboard.
- Navigate players to the selected game's player page when SignalR message is received.

Done when:

- Master selection of `Word Scramble` moves the master and all connected players to the correct role-specific game pages.
- Players can see game entries but cannot manually select them.

### Stage 3: Word Scramble Game Page Split

Move Word Scramble into game-specific master and player pages.

Included:

- Create a Word Scramble master dashboard.
- Create or relocate the Word Scramble player page.
- Keep player gameplay functionally as-is.
- Remove Word Scramble-specific round controls from any generic dashboard.
- Keep role/session guards on both game pages.

Done when:

- Word Scramble has its own master dashboard and player page.
- The master dashboard is share-safe and does not reveal the active sentence while players are solving it.

### Stage 4: Word Scramble Master Controls

Implement the V1.0 master-controlled sentence flow.

Included:

- Add Start, Restart, and Finish controls to the Word Scramble master dashboard.
- Add timer display.
- Start begins the first or next sentence.
- Restart resets from the first sentence and clears cumulative game score.
- Finish ends the current sentence immediately.
- Results remain visible after Finish until master clicks Start.

Done when:

- Master can run repeated Word Scramble sentences without restarting the app.
- Finish no longer waits for timeout.

### Stage 5: Word Scramble Scoring and Totals

Add per-sentence scoring, partial scoring on Finish, and cumulative total score.

Included:

- Score submitted players from submitted sentence.
- Score unsubmitted players from current partial collected sentence.
- Show correct sentence only after the sentence is finished.
- Track current sentence score and cumulative total score.
- Add sortable results grid with player name, avatar, submission duration, current sentence score, and total score.
- Default grid sort is player name.

Done when:

- Finish scores every registered player.
- Total score increments by correct word count across sentences.
- All result columns are sortable.

### Stage 6: V1 Polish and Verification

Clean up end-to-end behavior and verify V1.0 acceptance criteria.

Included:

- Confirm route guards, empty states, waiting states, and error messages.
- Confirm QR code is master-only.
- Confirm avatar fallback behavior.
- Confirm server restart clears in-memory state.
- Confirm no database, auth, cloud, or persistent leaderboard was added.
- Build and manually verify multi-tab master/player flows.

Done when:

- V1.0 acceptance criteria pass end-to-end on one host with multiple browser tabs or LAN devices.

## Acceptance Criteria

- Registration redirects both roles to `/home`.
- Master Home shows the shared LAN URL and joined players with avatars.
- Master Home shows a QR code for the shared LAN URL.
- Player Home shows avatar selection and a waiting-for-game-start message.
- Player avatar filename persists in browser session storage.
- Menu shows `Word Scramble` for both roles.
- Master can select `Word Scramble`; players cannot manually select it.
- Selecting `Word Scramble` sends players to the game page with SignalR.
- Word Scramble has its own master dashboard and player page.
- Word Scramble master dashboard hides the active sentence while the sentence is being solved.
- Master can Start, Restart, and Finish Word Scramble.
- Finish scores submitted and unsubmitted players using current partial player state.
- Results show correct sentence, player avatar, player name, submission duration, current sentence score, and total score.
- Results grid defaults to player name sorting and supports sorting all displayed columns.
- Total score increments by correct word count across sentences.
- After results, master clicks Start to begin the next sentence.
- No database, cloud service, accounts, passwords, or authentication are added.

## Future Ideas

- Add more games.
- Add browser-created custom sentences.
- Add undo for Word Scramble word selection.
- Add persistent game history.
- Add teams.
- Add optional internet hosting or tunneling.
