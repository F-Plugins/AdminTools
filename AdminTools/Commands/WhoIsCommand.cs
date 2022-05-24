using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OpenMod.API.Users;
using OpenMod.Core.Commands;
using OpenMod.Core.Users;
using OpenMod.Unturned.Commands;
using OpenMod.Unturned.Users;
using SDG.Unturned;
using System;
using UnityEngine;

namespace AdminTools.Commands
{
    [Command("whois")]
    [CommandDescription("A command know the owner of something you are looking at")]
    [CommandActor(typeof(UnturnedUser))]
    public class WhoIsCommand : UnturnedCommand
    {
        private readonly IUserDataStore _userData;

        private readonly IStringLocalizer _localizer;

        public WhoIsCommand(IServiceProvider serviceProvider, IStringLocalizer stringLocalizer, IUserDataStore userDataStore) : base(serviceProvider)
        {
            _userData = userDataStore;
            _localizer = stringLocalizer;
        }

        protected override async UniTask OnExecuteAsync()
        {
            UnturnedUser player = (UnturnedUser)Context.Actor;

            await UniTask.SwitchToMainThread();

            Vector3 position = player.Player.Player.look.aim.position;
            Vector3 forward = player.Player.Player.look.aim.forward;
            int itemID = (int)ERayMask.BARRICADE | (int)ERayMask.VEHICLE | (int)ERayMask.STRUCTURE;

            if (Physics.Raycast(position, forward, out RaycastHit hit, 5f, itemID))
            {

                Transform transform = hit.transform;

                InteractableVehicle vehicle = transform.GetComponent<InteractableVehicle>();
                if (vehicle != null)
                {
                    String? user = "Unknown";
                    if (vehicle.isLocked)
                    {
                        UserData? data = await _userData.GetUserDataAsync(vehicle.lockedOwner.ToString(), KnownActorTypes.Player);
                        if (data == null) throw new CommandWrongUsageException(_localizer["Commands:WhoIs:Error"]);
                        user = data.LastDisplayName;
                    }
                    await player.PrintMessageAsync(_localizer["Commands:WhoIs:Success", new
                    {
                        Name = vehicle.asset.name.Replace("_", " "),
                        Id = vehicle.id,
                        Health = vehicle.health,
                        Player = user,
                        Owner = vehicle.lockedOwner,
                        Group = vehicle.lockedGroup
                    }]);
                    return;
                }

                InteractableBed bed = transform.GetComponent<InteractableBed>();
                InteractableDoorHinge door = transform.GetComponent<InteractableDoorHinge>();
                if (door != null) transform = transform.parent.parent;
                BarricadeDrop barricadeDrop = BarricadeManager.FindBarricadeByRootTransform(transform);
                if (barricadeDrop != null)
                {
                    BarricadeData barricade = barricadeDrop.GetServersideData();
                    UserData? claim = null;
                    if (bed != null) claim = await _userData.GetUserDataAsync(bed.owner.ToString(), KnownActorTypes.Player);
                    UserData? data = await _userData.GetUserDataAsync(barricade.owner.ToString(), KnownActorTypes.Player);
                    if (data == null) throw new CommandWrongUsageException(_localizer["Commands:WhoIs:Error"]);
                    ushort maxHealth = barricade.barricade.asset.health;
                    if (claim == null)
                    {
                        await player.PrintMessageAsync(_localizer["Commands:WhoIs:Success", new
                        {
                            Name = barricade.barricade.asset.name.Replace("_", " "),
                            Id = barricade.barricade.asset.id,
                            Health = barricade.barricade.health + "/" + maxHealth,
                            Player = data.LastDisplayName,
                            Owner = barricade.owner,
                            Group = barricade.group
                        }]);
                    }
                    else
                    {
                        await player.PrintMessageAsync(_localizer["Commands:WhoIs:SuccessBed", new
                        {
                            Name = barricade.barricade.asset.name.Replace("_", " "),
                            Id = barricade.barricade.asset.id,
                            Health = barricade.barricade.health + "/" + maxHealth,
                            Player = data.LastDisplayName,
                            Owner = barricade.owner,
                            Group = barricade.group,
                            Claim = claim.LastDisplayName
                        }]);
                    }
                    return;
                }

                StructureDrop structureDrop = StructureManager.FindStructureByRootTransform(transform);
                if (structureDrop != null)
                {
                    StructureData structure = structureDrop.GetServersideData();
                    UserData? data = await _userData.GetUserDataAsync(structure.owner.ToString(), KnownActorTypes.Player);
                    if (data == null) throw new CommandWrongUsageException(_localizer["Commands:WhoIs:Error"]);
                    ushort maxHealth = structure.structure.asset.health;
                    await player.PrintMessageAsync(_localizer["Commands:WhoIs:Success", new
                    {
                        Name = structure.structure.asset.name.Replace("_", " "),
                        Id = structure.structure.asset.id,
                        Health = structure.structure.health + "/" + maxHealth,
                        Player = data.LastDisplayName,
                        Owner = structure.owner,
                        Group = structure.group
                    }]);
                    return;
                }

                throw new CommandWrongUsageException(_localizer["Commands:WhoIs:NotLooking"]);
            }
            else
            {
                throw new CommandWrongUsageException(_localizer["Commands:WhoIs:NotLooking"]);
            }
        }
    }
}
