<h1 align="center">AdminTools</h1>
<h3 align="center">A plugin with some utilities for your admins</h3>

## Installation
```bash
openmod install Feli.AdminTools
```
Or you can download it from [releases](https://github.com/F-Plugins/AdminTools/releases).

## Plans
- Add more commands
- Add some options to make moderation better

## Discord
- Discord: https://discord.gg/4FF2548

## Commands
- A command to teleports you up a certain distance  
  syntax: `/ascend [distance]`  
  id: `AdminTools.Commands.AscendCommand`  
- A command to clear items or vehicles  
  syntax: `/clear <i|items|ev|emptyvehicles> [range]`  
  id: `AdminTools.Commands.ClearCommand`  
- A command to wipe players inventory  
  syntax: `/clearinventory [player]`  
  id: `AdminTools.Commands.ClearInventoryCommand`  
- A command to open/close a door  
  syntax: `/door`  
  id: `AdminTools.Commands.DoorCommand`  
- A command to toggle the destroyer mode  
  syntax: `/handdestroyer`  
  id: `AdminTools.Commands.HandDestroyerCommand`  
- A command to unlock all skills to max  
  syntax: `/maxskills [player|all]`  
  id: `AdminTools.Commands.MaxSkillsCommand`  
- A command to redirect a player to another server  
  syntax: `/redirect <playerName> <ip> <port> [force] [password]`  
  id: `AdminTools.Commands.RedirectCommand`  
- A command to refuel the object or current vehicle  
  syntax: `/refuel`  
  id: `AdminTools.Commands.RedirectCommand`  
- A command to repair the object or current vehicle  
  syntax: `/repair [i|inventory]`  
  id: `AdminTools.Commands.RepairCommand`  
- A command to set the salvage time of a player  
  syntax: `/salvage [player] <time>`  
  id: `AdminTools.Commands.SalvageCommand`  
- A command to set the balance of a player  
  syntax: `/setbalance [player] <balance>`  
  id: `AdminTools.Commands.SetBalanceCommand`  
- A command set the speed of a player  
  syntax: `/speed [player] <speed>`  
  id: `AdminTools.Commands.SpeedCommand`  
- A command to open locked storages  
  syntax: `/storage`  
  id: `AdminTools.Commands.StorageCommand`  
- A command know the owner of something you are looking at  
  syntax: `/whois`  
  id: `AdminTools.Commands.WhoIsCommand`  

## Config
- Web:OnJoin  
  Will be called when the player joins the server  
- Web:WebCommands  
  Will be called when the player writes on the command  
- Broadcast  
  Send messages at intervals  
- Restrictions:Items  
  Restrict players to use that items  
- Restrictions:Vehicles  
  Restrict players to use that vehicles  

## Permissions
- `Feli.AdminTools:Commands.ascend`  
  Grants access to the `AdminTools.Commands.Ascend` command.  
- `Feli.AdminTools:Commands.clear`  
  Grants access to the `AdminTools.Commands.ClearCommand` command.  
- `Feli.AdminTools:Commands.clearinventory`  
  Grants access to the `AdminTools.Commands.ClearInventoryCommand` command.  
- `Feli.AdminTools:Commands.door`  
  Grants access to the `AdminTools.Commands.DoorCommand` command.  
- `Feli.AdminTools:Commands.handdestroyer`  
  Grants access to the `AdminTools.Commands.HandDestroyerCommand` command.  
- `Feli.AdminTools:Commands.maxskills`  
  Grants access to the `AdminTools.Commands.MaxSkillsCommand` command.  
- `Feli.AdminTools:Commands.redirect`  
  Grants access to the `AdminTools.Commands.RedirectCommand` command.  
- `Feli.AdminTools:Commands.refuel`  
  Grants access to the `AdminTools.Commands.RefuelCommand` command.  
- `Feli.AdminTools:Commands.repair`  
  Grants access to the `AdminTools.Commands.RepairCommand` command.  
- `Feli.AdminTools:Commands.salvage`  
  Grants access to the `AdminTools.Commands.SalvageCommand` command.  
- `Feli.AdminTools:Commands.setbalance`  
  Grants access to the `AdminTools.Commands.SetBalanceCommand` command.  
- `Feli.AdminTools:Commands.speed`  
  Grants access to the `AdminTools.Commands.SpeedCommand` command.  
- `Feli.AdminTools:Commands.storage`  
  Grants access to the `AdminTools.Commands.StorageCommand` command.  
- `Feli.AdminTools:Commands.whois`  
  Grants access to the `AdminTools.Commands.WhoIsCommand` command.  

## Alternative
- [NewEssentials](https://github.com/Kr4ken-9/NewEssentials) by Kr4ken-9
