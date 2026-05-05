# Stage 3 Plan: Word Scramble Game Page Split

## Objective

Move Word Scramble into game-owned role-specific pages.

## Scope

Included:

- Create a Word Scramble master dashboard.
- Create or relocate the Word Scramble player page.
- Keep the player page functionally as-is.
- Move Word Scramble controls out of any generic dashboard.
- Keep role and session guards on game pages.
- Ensure the master dashboard is safe to share and does not show the active sentence while players are solving it.

Not included:

- Start/Restart/Finish behavior changes.
- Cumulative scoring.
- New game types.

## Done Criteria

- Word Scramble owns its master dashboard.
- Word Scramble owns its player page.
- Master and player routes are separate.
- Player gameplay still works as before.

## Test Plan

- Build from `src`.
- Navigate as master to Word Scramble master dashboard.
- Navigate as player to Word Scramble player page.
- Confirm normal players cannot open the master dashboard.
- Confirm the master dashboard does not reveal the active sentence during play.
