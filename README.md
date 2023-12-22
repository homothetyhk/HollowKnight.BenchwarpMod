# HollowKnight.BenchwarpMod

Mod which creates a clickable menu on the pause screen to alter your current bench respawn point. Use warp button (or save and quit) after selection to travel to new bench. By default, only benches visited after installing the mod can be destinations.

Benchwarp fixes a bug where a the player can softlock by interacting with a bench before an area title popup occurs. As a side-effect, benches are interactable during the area title popup with Benchwarp.

Options
- Warp Only: removes bench selection menu
- Unlock All: enables all benches on the selection menu
- Show Room Name: displays the name of the current room from the game code in the bottom left
- Max Displayed Room Names: if `Show Room Name` is enabled, show up to this many additional loaded rooms
- Use Room Names: bench menu uses room names from the game code instead of descriptive names
- Enable Deploy: adds a menu to place a custom bench point
- Always Toggle All: all bench menus are displayed by default
- Doorwarp: replaces bench menu with a menu for selecting scene transitions (area=>room=>door). 
	- While active, the Warp button can only be used to warp to the selected transition, and will not warp to respawn.
	- The Flip button is used to change selection to the (vanilla) target of the currently selected transition.
- Enable Hotkeys: lets you warp to benches by typing a short key sequence rather than clicking the menu
- Override Localization: leaves bench names & UI in English when the game is in Chinese

Deploy
- Deploy button: places a bench at the current location
- Set: alters your respawn/warp point to be your currently deployed bench
- Near Style: select your preferred bench sprite which fades in when the hero is near the bench
- Far Style: select your preferred bench sprite which shows when the hero is away from the bench
	- For safety, styles cannot be changed while benched.
- Options:
	- Cooldown: adds a 300s cooldown to the deploy button
	- Noninteractive: deployed bench cannot be interacted with, except via warping (e.g. preventing use during boss fights)
	- No Mid-Air Deploy: the game must recognize the player as grounded in order to deploy
	- Allow Deploy in Unsafe Rooms: prevents use of deploy in unsafe areas, including Dream World rooms and Colosseum trials.
	  - This setting can be accessed from the mod menu, but should not be enabled in normal use. Placing a bench in an unsafe room can make it impossible to load into the bench after a menu/death/warp. See the section below for instructions on how to recover a save file in such circumstances.
	- [Obsolete] Reduce Preloads: when the game starts up, only the selected style is preloaded. To use other styles, the player must restart the game.
		- As of version 3.0, Reduce Preloads is no longer a setting, and only one bench is preloaded by default.
	- No preloads: no bench styles are preloaded. The deploy button instead will create a respawn marker. To deploy benches, the player must change the setting and restart the game.
	- Apply Style To All: applies the selected styles to base game benches. Requires a scene reload to take effect. Not all benches are supported.

Bench Hotkeys
- A small 2-digit number is displayed above each bench. Typing that number while paused is equivalent to clicking the corresponding bench button, and then the Warp button in Bench Warp mode. In other words, a code for an unselectable bench will result in warping to the current respawn. Hotkeys only perform bench warps; this holds even if the current setting is set to Door Warp mode.

There are also several convenient commands available by entering certain letter combinations:
- **LB**: **L**ast **B**ench (equivalent to just clicking Warp)
- **SB**: **S**tart **B**ench (equivalent to clicking Set Start, then Warp)
- **WD**: **W**arp to **D**eployed Bench (equivalent to clicking Set in the Deploy submenu, then Warp)
- **TM**: **T**oggle **M**enu (equivalent to clicking Show Menu in the Benchwarp submenu in the Mods menu)
- **DW**: **D**oor **W**arp (equivalent to clicking Door Warp in the Settings menu)
- **DB**: **D**eploy **B**ench (equivalent to clicking Deploy in the Deploy submenu)

In older versions of Benchwarp, hotkeys were implemented through entering a 2 letter sequence associated with the name of the bench. That feature can be reactivated by enabling the Legacy Hotkeys setting in the mod menu, and toggling the mod or restarting the game.

Hotkeys can be remapped by modifying the "hotkeyOverrides" dictionary in the
mod's global settings file (Benchwarp.GlobalSettings.json, in your saves folder). This dictionary
should map the default bindings to their replacements. For example, `"hotkeyOverrides": {"SB": "WS"}` remaps the Start Bench command to use the hotkey "WS".
Custom hotkeys must be composed of exactly two uppercase letters. 

Some rooms may not be safe to place a deployed bench, including dream areas, Godhome arenas, and Colosseum trials. If you are unable to load into a deployed bench, you can recover through the following steps:
- Disable Benchwarp using the mod menu. That is, navigate to Options->Mods->Benchwarp Options->Benchwarp and set to Off.
- Reload the save file. You should now be at your most recent permanent respawn point.
- Reenable Benchwarp using the mod menu.
- Use the deploy menu to destroy the old deployed bench, or to deploy a new bench in a safe location.

Credits
homothety - main developer
PimpasPimpinela - hotkey support!
Serena - developed the UI, as part of DebugMod
Serena and 5FiftySix6 - developed the Warp method, as part of this mod
The Embraced One - originally created the menu assets, as part of DebugMod

Benchwarp is licensed under LGPL. Source code and license can be found at https://github.com/homothetyhk/HollowKnight.BenchwarpMod/