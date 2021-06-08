using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OpenMod.API.Persistence;
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
        private readonly IStringLocalizer _stringLocalizer;
        private readonly IUserDataStore _userDataStore;

        public WhoIsCommand(IServiceProvider serviceProvider, IStringLocalizer stringLocalizer, IUserDataStore userDataStore) : base(serviceProvider)
        {
            _userDataStore = userDataStore;
            _stringLocalizer = stringLocalizer;
        }

        protected override async UniTask OnExecuteAsync()
        {
            var player = (UnturnedUser)Context.Actor;

            await UniTask.SwitchToMainThread();
            if (Physics.Raycast(player.Player.Player.look.aim.position, player.Player.Player.look.aim.forward, out RaycastHit hit, 15f, (int)ERayMask.BARRICADE | (int)ERayMask.VEHICLE | (int)ERayMask.STRUCTURE))
            {
                var vehicle = hit.transform.GetComponent<InteractableVehicle>();

                if(vehicle != null)
                {
                    var data = await _userDataStore.GetUserDataAsync(vehicle.lockedOwner.ToString(), KnownActorTypes.Player);

                    if (data == null) throw new CommandWrongUsageException(_stringLocalizer["Commands:WhoIs:Error"]);

                    await player.PrintMessageAsync(_stringLocalizer["Commands:WhoIs:Success", new
                    {
                        Name = vehicle.asset.name.Replace("_", " "),
                        Id = vehicle.id,
                        Player = data.LastDisplayName,
                        Owner = vehicle.lockedOwner,
                        Group = vehicle.lockedGroup
                    }]);
                    return;
                }

                if(BarricadeManager.tryGetInfo(hit.transform, out byte x, out byte y, out ushort plant, out ushort index, out BarricadeRegion region))
                {
                    var barricade = region.barricades[index];

                    var data = await _userDataStore.GetUserDataAsync(barricade.owner.ToString(), KnownActorTypes.Player);

                    if (data == null) throw new CommandWrongUsageException(_stringLocalizer["Commands:WhoIs:Error"]);

                    await player.PrintMessageAsync(_stringLocalizer["Commands:WhoIs:Success", new
                    {
                        Name = barricade.barricade.asset.name.Replace("_", " "),
                        Id = barricade.barricade.id,
                        Player = data.LastDisplayName,
                        Owner = barricade.owner,
                        Group = barricade.group
                    }]);
                    return;
                }

                if (StructureManager.tryGetInfo(hit.transform, out byte x1, out byte y1, out ushort index1, out StructureRegion region1))
                {
                    var structure = region1.structures[index1];

                    var data = await _userDataStore.GetUserDataAsync(structure.owner.ToString(), KnownActorTypes.Player);

                    if (data == null) throw new CommandWrongUsageException(_stringLocalizer["Commands:WhoIs:Error"]);

                    await player.PrintMessageAsync(_stringLocalizer["Commands:WhoIs:Success", new
                    {
                        Name = structure.structure.asset.name.Replace("_", " "),
                        Id = structure.structure.id,
                        Player = data.LastDisplayName,
                        Owner = structure.owner,
                        Group = structure.group
                    }]);
                    return;
                }

                throw new CommandWrongUsageException(_stringLocalizer["Commands:WhoIs:NotLooking"]);
            }
            else
            {
                throw new CommandWrongUsageException(_stringLocalizer["Commands:WhoIs:NotLooking"]);
            }
        }
    }
}
