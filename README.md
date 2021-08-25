# HollowKnight.BenchwarpMod

Mod which creates a clickable menu on the pause screen to alter your current bench respawn point. Use warp button (or save and quit) after selection to travel to new bench. By default, only benches visited after installing the mod can be destinations.

Options
- Warp Only: removes bench selection menu
- Unlock All: enables all benches on the selection menu
- Show Room Name: displays the name of the current room from the game code in the bottom left
- Use Room Names: bench menu uses room names from the game code instead of descriptive names
- Enable Deploy: adds a menu to place a custom bench point
- Always Toggle All: all bench menus are displayed by default
- Doorwarp: replaces bench menu with a menu for selecting scene transitions (area=>room=>door). 
	- While active, the Warp button can only be used to warp to the selected transition, and will not warp to respawn.
	- The Flip button is used to change selection to the (vanilla) target of the currently selected transition.
- Enable Hotkeys: lets you warp to benches by typing a short key sequence rather than clicking the menu

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
	- No Dark Rooms or Dream Rooms: prevents use of deploy in Dream World rooms, and if the player does not have lantern, prevents use of deploy in rooms which are dark
	- [Obsolete] Reduce Preloads: when the game starts up, only the selected style is preloaded. To use other styles, the player must restart the game.
		- As of version 3.0, Reduce Preloads is no longer a setting, and only one bench is preloaded by default.
	- No preloads: no bench styles are preloaded. The deploy button instead will create a respawn marker. To deploy benches, the player must change the setting and restart the game.
	- Apply Style To All: applies the selected styles to base game benches. Requires a scene reload to take effect. Not all benches are supported.

Bench Hotkeys
- Each of the following combinations is equivalent to clicking the corresponding bench button, and then the Warp button in Bench Warp mode. In other words, a code for an unselectable bench will result in warping to the current respawn. Hotkeys only perform bench warps; this holds even if the current setting is set to Door Warp mode.
- **NM**: **N**ailmaster **M**ato
- **DM**: **D**irt**M**outh
- **FC**: **F**orgotten **C**rossroads Stag
- **FS**: **F**orgotten Crossroads Hot **S**prings
- **SM**: Ancestral (**S**haman) **M**ound
- **SA**: **SA**lubra
- **BE**: **B**lack **E**gg
- **GW**: **G**reenpath **W**aterfall
- **SS**: **S**tone **S**anctuary
- **GT**: **G**reenpath **T**oll
- **GP**: **G**reen**P**ath Stag
- **LU**: **L**ake of **U**nn
- **NS**: **N**ailmaster **S**heo
- **TA**: **T**eacher's **A**rchives
- **QS**: **Q**ueen's **S**tation
- **LE**: **L**eg **E**ater
- **BR**: **BR**etta
- **MV**: **M**antis **V**illage
- **CQ**: **C**ity **Q**uirrel
- **CT**: **C**ity **T**oll
- **CS**: **C**ity **S**torerooms
- **WS**: **W**atcher **S**pire
- **KS**: **K**ing's **S**tation
- **PH**: **P**leasure **H**ouse
- **WW**: **W**ater**W**ays
- **GA**: **G**odhome **A**trium
- **GR**: **G**odhome **R**oof
- **HG**: **H**all of **G**ods
- **DS**: **D**eepnest Hot **S**prings
- **FT**: **F**ailed **T**ramway
- **BD**: **B**east's **D**en
- **BT**: **B**asin **T**oll
- **HS**: **H**idden **S**tation
- **NO**: **N**ailmaster **O**ro
- **EC**: **E**dge **C**amp
- **CF**: **C**olosseum of **F**ools
- **BB**: Hive (**b**ee-**b**ee)
- **PD**: **P**eak **D**ark Room
- **CG**: **C**rystal **G**uardian
- **RG**: **R**esting **G**rounds Stag
- **GM**: **G**rey **M**ourner
- **QC**: **Q**ueen's Gardens **C**ornifer
- **QT**: **Q**ueen's Gardens **T**oll
- **QG**: **Q**ueen's **G**ardens Stag
- **PE**: **P**alace **E**ntrance
- **PA**: **P**alace **A**trium
- **PB**: **P**alace **B**alcony
- **UT**: **U**pper **T**ram
- **LT**: **L**ower **T**ram
- **LB**: **L**ast **B**ench (equivalent to just clicking Warp)
- **SB**: **S**tart **B**ench (equivalent to clicking Set Start, then Warp)
- **WD**: **W**arp to **D**eployed Bench (equivalent to clicking Set in the Deploy submenu, then Warp)
- **TM**: **T**oggle **M**enu (equivalent to clicking Show Menu in the Benchwarp submenu in the Mods menu)
- **DW**: **D**oor **W**arp (equivalent to clicking Door Warp in the Settings menu)

Each hotkey can be remapped by modifying the "hotkeyOverrides" dictionary in the
mod's global settings file (Benchwarp.GlobalSettings.json, in your saves folder). This dictionary
should map the default bindings to their replacements. For example, `"hotkeyOverrides": {"BB": "HV"}` remaps the Hive bench to use the hotkey "HV".
Custom hotkeys must be composed of exactly two uppercase letters. 

Credits
homothety - main developer
PimpasPimpinela - hotkey support!
Serena - developed the UI, as part of DebugMod
Serena and 5FiftySix6 - developed the Warp method, as part of this mod
The Embraced One - originally created the menu assets, as part of DebugMod

Benchwarp is licensed under LGPL. Source code and license can be found at https://github.com/homothetyhk/HollowKnight.BenchwarpMod/