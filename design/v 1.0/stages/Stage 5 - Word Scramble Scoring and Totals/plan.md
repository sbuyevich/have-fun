# Stage 5 Plan: Word Scramble Scoring and Totals

## Objective

Add V1.0 Word Scramble scoring, partial scoring on Finish, and cumulative totals.

## Scope

Included:

- Score submitted players from submitted sentences.
- Score unsubmitted players from current partial collected sentences.
- Show the correct sentence only after the sentence is finished.
- Track current sentence score.
- Track cumulative total score as the sum of correct words across sentences.
- Add sortable results grid with player name, avatar, submission duration, current sentence score, and total score.
- Default results sort is player name.

Not included:

- Persistent leaderboard.
- Rank-based scoring.
- Fuzzy matching or punctuation normalization.

## Done Criteria

- Finish scores every registered player.
- Partial collected sentences are scored for unsubmitted players.
- Current sentence score is visible after finish.
- Total score increments across sentences.
- All result grid columns are sortable.

## Test Plan

- Build from `src`.
- Run a sentence with one submitted player and one unsubmitted player.
- Click Finish and confirm both players receive a score.
- Confirm partial answers score by exact word position.
- Start another sentence and confirm total score increments.
- Confirm default sort is player name and all columns can sort.
