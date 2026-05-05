# Stage 3 Plan: Word Scramble Game Page Split

## Objective

Move Word Scramble into game-owned role-specific Razor pages.

## Scope

Included:

- Create `WordScramble.Master.razor` for the Word Scramble master dashboard.
- Create or relocate the player gameplay page to `WordScramble.Player.razor`.
- Keep the player page functionally as-is.
- Move Word Scramble controls out of any generic dashboard.
- Keep role and session guards on game pages.
- Ensure the master dashboard is safe to share and does not show the active sentence while players are solving it.

Not included:

- Start/Restart/Finish behavior changes.
- Cumulative scoring.
- New game types.

## Done Criteria

- Word Scramble owns `WordScramble.Master.razor`.
- Word Scramble owns `WordScramble.Player.razor`.
- Master and player routes are separate.
- Player gameplay still works as before.

## Test Plan

- Build from `src`.
- Navigate as master to `WordScramble.Master.razor`.
- Navigate as player to `WordScramble.Player.razor`.
- Confirm normal players cannot open the master dashboard.
- Confirm the master dashboard does not reveal the active sentence during play.
