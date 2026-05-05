# Stage 2 Plan: Multi-Game Shell and SignalR Navigation

## Objective

Add the shared game-selection shell and SignalR navigation layer for V1.0 multi-game support.

## Scope

Included:

- Add an in-memory available-games model with `Word Scramble` as the first game.
- Include role-specific route metadata for each game: one master route and one player route.
- Add server-side active game state.
- Add role-aware game menu entries.
- Let only the master select game entries.
- Add an explicit SignalR hub for game-selection messages.
- Navigate the master to the selected game's `{GameName}.Master.razor` page.
- Navigate players to the selected game's `{GameName}.Player.razor` page after receiving the SignalR message.

Not included:

- More games beyond Word Scramble.
- Database-backed game registry.
- Word Scramble scoring changes.

## Done Criteria

- Master can select `Word Scramble` from the menu.
- Players see `Word Scramble` but cannot manually select it.
- Selecting `Word Scramble` navigates master to `WordScramble.Master.razor`.
- Connected players navigate to `WordScramble.Player.razor` through SignalR.
- The available-games model points Word Scramble at the role-specific pages later owned by `WordScramble.Master.razor` and `WordScramble.Player.razor`.

## Test Plan

- Build from `src`.
- Open one master tab and at least one player tab.
- Select `Word Scramble` as master.
- Confirm the master lands on `WordScramble.Master.razor`.
- Confirm player tabs automatically land on `WordScramble.Player.razor`.
