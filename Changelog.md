# Changelog

## [1.5.1.2] - 10.08.2022

https://github.com/Psenix/OverlayTimer/commit/16559b589b0e853fa967d0b84fac476913fb45a5

- Auto updates crashes solved

## [1.5.1.1] - 10.08.2022

https://github.com/Psenix/OverlayTimer/commit/4395b1b5b482ce16e1686673954b76bc60185b94

- Auto updates are now available (will always prompt the user to let everyone decide by themselves)
- Feature to delete own public speedruns was added
- Changelogs are now available in the program under settings
- Fixed a bug that did not save user preferences
- When retrieving public data, a loading info was added and the code for fetching the data is now started as a seperated thread to avoid blocking the GUI
- A new button to the discord server was added to the main page
- Fixed a bug where "The Legend of Zelda: Breath of the wild" did not save locally due to unallowed characters in the path
- Time of a speedrun will not get saved properly, previously the AM/PM information was lost
- Current version gets shown in the settings

## [1.5.0.0] - 08.08.2022

https://github.com/Psenix/OverlayTimer/commit/bf7001ca3425bfed16403771915cf70edcb132d6

- When approving runs, adapting the time will be easier interpreted (eg. 5:100 gets changed to 5:2 ( <= zeros weren't manually added). Previously this would result in 2 milliseconds, now 200 milliseconds.
- Improvments with saving the files in a config file
- Settings page is now scrollable
- Option to disable and enable Discord Rich Presence
- Files get saved neater (in a sub-directory)
- Program now asks the user before canceling a speedrun
- RAM Optimizations/cleanups
- The escape key can now be used to navigate back

## [1.4.0.0] - 06.08.2022

https://github.com/Psenix/OverlayTimer/commit/19b05777df14ee9fc96f1c7e7075386f004e93d5

- Local runs are now deleteable
- Support for Discord Rich Presence was added
- Also some changes with the API were adapted since variables got renamed for less confusion.

## [1.3.0.0] - 05.08.2022

https://github.com/Psenix/OverlayTimer/commit/6cea21323eaa7948415b35bce84875ede8b4e1e9

- Added an icon
- Username checking (no duplicates allowed)
- Video link checking (no duplicates allowed)
- Timer more accurate and also lags will not stop the timer
- Speedruns get sorted by oldest => newest for moderators

## [1.2.0.0] - 01.08.2022

https://github.com/Psenix/OverlayTimer/commit/b599a4a133d88e0c6a0d4e82f8fc88948354f3ac

- Rules for each game get displayed before starting a timer
- Token gets checked every time in case it gets invalid
- X and Y Coordinates of the timer window position saves and loads automatically
- After stopping timer the submit page will be topmost
- For certain tasks threads were added to avoid blocking the GUI

## [1.1.0.0] - 28.07.2022

https://github.com/Psenix/OverlayTimer/commit/baca2109f537ac4b09e18468592c0c0ab0e623b9

- Added a moderator page
- Only unverified speedruns get displayed to the leaderboard
- Verifying and deleting an entry (only with a token/password)
- Checks the count of a list to avoid crashing when a category is empty
- Also a little bit of clean up in the code to make it better readable and also less lines.

## [1.0.0.0] - 16.07.2022

https://github.com/Psenix/OverlayTimer/commit/9b1f5b15e243fae63d74843eff50a297be0dc19e

- Added modern message boxes for cleaner UI
- Public leaderboard ListView support
- Local leaderboard ListView support
- Clicking on a list item will open the saved video link
- Support for resizing EVERY page and window properly with it's content
- New elements on submit page (cancel button and the scored time)
- Massive design update/improvement (including Leaderboard design, submit page design, buttons design, and ability to resize controls properly with the window by additionally adding ViewBoxes to the grid)
- Bug and crash fixes
- Back buttons for easier navigation between pages
- Merged FullEntry and Entry class for simplification
- Displaying when something is loading (e.g., something still needs to get fetched from the server)
- Added visually appealing time formatting
- Options and leaderboard will get reset when page shows again after close
- Added smoother scrolling by handling scrolling in the .cs instead of XAML
- Moved material design themes to the pages separately instead of in App.xaml to avoid conflicts
- Valid names check before starting timer
- Improved loading times between switching pages and windows
- Checking if a speed run entry request got denied by the server
- Application will properly (instead of still running partially in the background) shutdown when user closes a running timer
- Properly storing local data
- Local speed runs now contain all the data as public ones (also GUID, for being able to identify a certain entry)
- Code clean up
- Bug fixes and preparations for future implementations for API Backend
- Some optimization for less heavy RAM usage.
