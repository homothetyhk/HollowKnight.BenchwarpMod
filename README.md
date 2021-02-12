# HollowKnight.BenchwarpMod

Mod which creates a clickable menu on the pause screen to alter your current bench respawn point. Use warp button (or save and quit) after selection to travel to new bench. By default, only benches visited after installing the mod can be destinations.

Options
- Warp Only: removes bench selection menu
- Unlock All: enables all benches on the selection menu
- Show Room Name: displays the name of the current room from the game code in the bottom left
- Use Room Names: bench menu uses room names from the game code instead of descriptive names
- Enable Deploy: adds a menu to place a custom bench point
- Always Toggle All: all bench menus are displayed by default
- Doorwarp (beta): replaces bench menu with a menu for selecting scene transitions (area=>room=>door). 
	- While active, the Warp button can only be used to warp to the selected transition, and will not warp to respawn.
	- The Flip button is used to change selection to the (vanilla) target of the currently selected transition.

Deploy
- Deploy button: places a bench at the current location
- Set: alters your respawn/warp point to be your currently deployed bench
- Style: select your preferred bench sprite
- Options:
	- Cooldown: adds a 300s cooldown to the deploy button
	- Noninteractive: deployed bench cannot be interacted with, except via warping (e.g. preventing use during boss fights)
	- No Mid-Air Deploy: the game must recognize the player as grounded in order to deploy
	- No Dark Rooms or Dream Rooms: prevents use of deploy in Dream World rooms, and if the player does not have lantern, prevents use of deploy in rooms which are dark
	- Reduce Preloads: when the game starts up, only the selected style is preloaded. To use other styles, the player must restart the game.
	- No preloads: no bench styles are preloaded. The deploy button instead will create a respawn marker. To deploy benches, the player must change the setting and restart.

Credits
Serena - developed the UI, as part of DebugMod
Serena and 5FiftySix6 - developed the Warp method, as part of this mod
The Embraced One - originally created the menu assets, as part of DebugMod
