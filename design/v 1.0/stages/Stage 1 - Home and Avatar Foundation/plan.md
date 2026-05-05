# Stage 1 Plan: Home and Avatar Foundation

## Objective

Build the V1.0 post-registration Home experience and avatar foundation for both roles.

## Scope

Included:

- Redirect successful registration to `/home`.
- Build role-aware Home behavior for master and players.
- Show shared LAN URL and QR code only on Master Home.
- Show joined players with avatars on Master Home.
- Add player avatar selection on Player Home.
- Load avatar filenames from `assets/avatars`.
- Store selected player avatar filename in session storage with name and role.
- Use a hard-coded master avatar.

Not included:

- SignalR game switching.
- Game-specific page split.
- Word Scramble scoring changes.

## Done Criteria

- Both roles land on `/home` after registration.
- Master Home shows LAN URL, QR code, and joined players with avatars.
- Player Home shows selected avatar, avatar selection, and waiting message.
- Player avatar persists for the browser session.
- QR code is not shown to players.

## Test Plan

- Build from `src`.
- Join as master and confirm `/home` opens.
- Join as player and confirm `/home` opens.
- Change player avatar and refresh the tab; confirm the selected avatar remains.
- Confirm Master Home shows joined player avatars.
- Confirm QR code encodes the same shared LAN URL shown as text.
