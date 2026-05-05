# App Changes

App should support multi games. 
Each game should have core functionality for communication with players.

## Changes for v 1.0

### Layout changes
- Move `Hi,.. ` on center of top bar 

### Home page 
- Post registration leads to Home page.
- Master Home page shows 
  - Please enter following URL in you browser bar with Shared LAN URL
  - Register players with avatar
- Player Home page show player Avatar and message like `Wait for Game Start`

### Game choices
Menu has multi games names. 
For ex, current implemented game is `Word Scramble`. 
Master and Player has `Word Scramble` in menu but only Master can select it. 
Each player switches to selected game page.

### `Word Scramble` game changes
Game page for Master can be shared therefore do not show sentence, only 
 - Start, Restart and Finish buttons
 - Timer
 - Submission grid with player names, Avatars and time duration for submit. Make grid sortable.
Player page keep as is for now.

### Avatar feature
Player can option to select avatar after registration. Avatar images are in assets/avatars folder. They are loaded in memory and persisted in player session storage as file name.






