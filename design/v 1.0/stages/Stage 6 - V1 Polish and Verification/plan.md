# Stage 6 Plan: V1 Polish and Verification

## Objective

Verify and polish the complete V1.0 flow.

## Scope

Included:

- Confirm route guards for master, player, and unregistered users.
- Confirm every game has exactly two role-specific pages, using the `{GameName}.Master.razor` and `{GameName}.Player.razor` naming convention.
- Confirm empty, waiting, active, finished, and error states are clear.
- Confirm QR code appears only on Master Home.
- Confirm avatar fallback behavior.
- Confirm server restart clears in-memory state.
- Confirm no database, auth, cloud service, or persistent leaderboard was added.
- Verify multi-tab and LAN-device flows.

Not included:

- New gameplay features.
- Additional games.
- Persistent storage.

## Done Criteria

- All V1.0 acceptance criteria pass.
- The app supports master plus multiple player tabs/devices.
- Word Scramble works across multiple sentences.
- Documentation and UI wording are consistent with V1.0.

## Test Plan

- Build from `src`.
- Run the app on the LAN bind address.
- Join as master and two players.
- Select avatars for players.
- Confirm Master Home shows LAN URL, QR code, and players.
- Select Word Scramble and confirm SignalR navigation.
- Confirm Word Scramble routes land on `WordScramble.Master.razor` for master and `WordScramble.Player.razor` for players.
- Play multiple sentences, including Finish before all players submit.
- Confirm scoring, totals, sorting, and restart behavior.
