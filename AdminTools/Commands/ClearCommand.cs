using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Commands;
using OpenMod.Unturned.Users;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdminTools.Commands
{
    [Command("clear")]
    [CommandDescription("A command to clear items or vehicles")]
    [CommandSyntax("<i|items|ev|emptyvehicles> [range]")]
    [CommandActor(typeof(UnturnedUser))]
    public class ClearCommand : UnturnedCommand
    {
        private readonly IStringLocalizer _localizer;

        public ClearCommand(IServiceProvider serviceProvider, IStringLocalizer stringLocalizer) : base(serviceProvider)
        {
            _localizer = stringLocalizer;
        }

        protected override async UniTask OnExecuteAsync()
        {
            UnturnedUser player = (UnturnedUser)Context.Actor;

            if (Context.Parameters.TryGet<string>(0, out string? value))
            {
                await UniTask.SwitchToMainThread();
                if (value == "i" || value == "items")
                {
                    if (Context.Parameters.TryGet<float>(1, out float itemsRange))
                    {
                        ItemManager.ServerClearItemsInSphere(player.Player.Player.transform.position, itemsRange);
                    }
                    else
                    {
                        ItemManager.askClearAllItems();
                    }
                    await player.PrintMessageAsync(_localizer["Commands:Clear:Items"]);
                }
                else if (value == "ev" || value == "emptyvehicles")
                {
                    if (Context.Parameters.TryGet<float>(1, out float vehicleRange))
                    {
                        List<InteractableVehicle> vehicles = new List<InteractableVehicle>();
                        VehicleManager.getVehiclesInRadius(player.Player.Player.transform.position, vehicleRange, vehicles);
                        foreach (InteractableVehicle vehicle in vehicles.Where(x => x.isEmpty).ToList())
                        {
                            VehicleManager.askVehicleDestroy(vehicle);
                        }
                    }
                    else
                    {
                        foreach (InteractableVehicle vehicle in VehicleManager.vehicles.Where(x => x.isEmpty).ToList())
                        {
                            VehicleManager.askVehicleDestroy(vehicle);
                        }
                    }
                    await player.PrintMessageAsync(_localizer["Commands:Clear:Vehicles"]);
                }
                else
                {
                    throw new CommandWrongUsageException(_localizer["Commands:Clear:Error"]);
                }
                await UniTask.SwitchToThreadPool();
            }
            else
            {
                throw new CommandWrongUsageException(_localizer["Commands:Clear:Error"]);
            }
        }
    }
}
