using System;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Commands;
using OpenMod.Unturned.Users;
using SDG.Unturned;
using UnityEngine;

namespace AdminTools.Commands
{
    [Command("repair")]
    [CommandDescription("A command to repair the object or vehicle")]
    [CommandSyntax("[i|inventory]")]
    [CommandActor(typeof(UnturnedUser))]
    public class RepairCommand : UnturnedCommand
    {
        private readonly IStringLocalizer _localizer;

        public RepairCommand(IServiceProvider serviceProvider, IStringLocalizer stringLocalizer) : base(serviceProvider)
        {
            _localizer = stringLocalizer;
        }

        protected override async UniTask OnExecuteAsync()
        {
            UnturnedUser user = (UnturnedUser)Context.Actor;

            await UniTask.SwitchToMainThread();

            switch (Context.Parameters.Length)
            {
                case 0:
                    InteractableVehicle vehicle = user.Player.Player.movement.getVehicle();
                    if (vehicle == null)
                    {
                        Vector3 origin = user.Player.Player.look.aim.position;
                        Vector3 direction = user.Player.Player.look.aim.forward;
                        if (Physics.Raycast(origin, direction, out RaycastHit hit, 5f, (int)ERayMask.VEHICLE))
                        {
                            vehicle = hit.transform.GetComponent<InteractableVehicle>();
                            if (vehicle == null) return;
                            vehicle.askRepair(vehicle.asset.health);
                            await user.PrintMessageAsync(_localizer["Commands:Repair:Success", new
                            {
                                Name = vehicle.asset.name.Replace("_", " "),
                            }]);
                        }
                    }
                    else
                    {
                        vehicle.askRepair(vehicle.asset.health);
                        await user.PrintMessageAsync(_localizer["Commands:Repair:Success", new
                        {
                            Name = vehicle.asset.name.Replace("_", " "),
                        }]);
                    }
                    break;
                case 1:
                    string? value = await Context.Parameters.GetAsync<string>(0);
                    if (value == "i" || value == "inventory")
                    {
                        PlayerInventory inventory = user.Player.Player.inventory;
                        foreach (Items items in inventory.items)
                        {
                            if (items == null) continue;
                            foreach (ItemJar jar in items.items)
                            {
                                inventory.sendUpdateQuality(items.page, jar.x, jar.y, 100);
                            }
                        }
                        await user.PrintMessageAsync(_localizer["Commands:Repair:Success", new
                        {
                            Name = "inventory",
                        }]);
                    }
                    break;
                default: return;
            }

            await UniTask.SwitchToThreadPool();
        }
    }
}
