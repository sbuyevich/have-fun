# Stage 2 Plan: Multi-Game Shell and SignalR Navigation

## Objective

Add the shared game-selection shell and SignalR navigation layer for V1.0 multi-game support.

## Scope

Included:

- Add an in-memory available-games model with `Word Scramble` as the first game.
- Add server-side active game state.
- Add role-aware game menu entries.
- Let only the master select game entries.
- Add an explicit SignalR hub for game-selection messages.
- Navigate the master to the selected game's master dashboard.
- Navigate players to the selected game's player page after receiving the SignalR message.

Not included:

- More games beyond Word Scramble.
- Database-backed game registry.
- Word Scramble scoring changes.

## Done Criteria

- Master can select `Word Scramble` from the menu.
- Players see `Word Scramble` but cannot manually select it.
- Selecting `Word Scramble` navigates master to its master dashboard.
- Connected players navigate to its player page through SignalR.

## Test Plan

- Build from `src`.
- Open one master tab and at least one player tab.
- Select `Word Scramble` as master.
- Confirm the master lands on the Word Scramble master dashboard.
- Confirm player tabs automatically land on the Word Scramble player page.
