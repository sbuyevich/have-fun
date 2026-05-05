# Stage 4 Plan: Word Scramble Master Controls

## Objective

Implement the V1.0 Word Scramble master-controlled sentence flow.

## Scope

Included:

- Add Start, Restart, and Finish controls to the Word Scramble master dashboard.
- Add timer display to the master dashboard.
- Start begins the first sentence when no sentence has started.
- Start begins the next sentence after results are shown.
- Restart resets the game from the first sentence and clears cumulative score.
- Finish ends the current sentence immediately without waiting for timeout.
- Keep results visible after Finish until master clicks Start.

Not included:

- Final results grid scoring columns.
- Avatar display in results.
- More games.

## Done Criteria

- Master can start the first sentence.
- Master can finish a sentence immediately.
- Master can start the next sentence after results are shown.
- Master can restart from the first sentence.
- Timer is visible to the master.

## Test Plan

- Build from `src`.
- Start Word Scramble from the master dashboard.
- Confirm timer begins.
- Click Finish and confirm the sentence ends immediately.
- Click Start again and confirm the next sentence begins.
- Click Restart and confirm the game returns to the first sentence with cleared totals.
