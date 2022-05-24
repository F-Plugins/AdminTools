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
    [Command("refuel")]
    [CommandDescription("A command to refuel the object or vehicle")]
    [CommandActor(typeof(UnturnedUser))]
    public class RefuelCommand : UnturnedCommand
    {
        private readonly IStringLocalizer _localizer;

        public RefuelCommand(IServiceProvider serviceProvider, IStringLocalizer stringLocalizer) : base(serviceProvider)
        {
            _localizer = stringLocalizer;
        }

        protected override async UniTask OnExecuteAsync()
        {
            UnturnedUser user = (UnturnedUser)Context.Actor;

            await UniTask.SwitchToMainThread();

            InteractableVehicle vehicle = user.Player.Player.movement.getVehicle();
            if (vehicle != null)
            {
                vehicle.askFillFuel(vehicle.asset.fuel);
                await user.PrintMessageAsync(_localizer["Commands:Refuel:Success", new
                {
                    Name = vehicle.asset.name.Replace("_", " "),
                }]);
            }

            Vector3 origin = user.Player.Player.look.aim.position;
            Vector3 direction = user.Player.Player.look.aim.forward;
            if (Physics.Raycast(origin, direction, out RaycastHit hit, 5f))
            {
                vehicle = hit.transform.GetComponent<InteractableVehicle>();
                if (vehicle != null)
                {
                    vehicle.askFillFuel(vehicle.asset.fuel);
                    await user.PrintMessageAsync(_localizer["Commands:Refuel:Success", new
                    {
                        Name = vehicle.asset.name.Replace("_", " "),
                    }]);
                }

                Interactable interactable = hit.transform.GetComponent<Interactable>();
                if (interactable == null) return;

                if (interactable is InteractableGenerator generator)
                {
                    BarricadeManager.sendFuel(interactable.transform, generator.capacity);
                    await user.PrintMessageAsync(_localizer["Commands:Refuel:Success", new
                    {
                        Name = generator.name.Replace("_", " "),
                    }]);
                }

                if (interactable is InteractableOil oil)
                {
                    BarricadeManager.sendOil(interactable.transform, oil.capacity);
                    await user.PrintMessageAsync(_localizer["Commands:Refuel:Success", new
                    {
                        Name = oil.name.Replace("_", " "),
                    }]);
                }

                if (interactable is InteractableTank { source: ETankSource.FUEL } tank1)
                {
                    tank1.ServerSetAmount(tank1.capacity);
                    await user.PrintMessageAsync(_localizer["Commands:Refuel:Success", new
                    {
                        Name = tank1.name.Replace("_", " "),
                    }]);
                }

                if (interactable is InteractableTank { source: ETankSource.WATER } tank2)
                {
                    tank2.ServerSetAmount(tank2.capacity);
                    await user.PrintMessageAsync(_localizer["Commands:Refuel:Success", new
                    {
                        Name = tank2.name.Replace("_", " "),
                    }]);
                }

                if (interactable is InteractableObjectResource { objectAsset: { interactability: EObjectInteractability.FUEL } } resource1)
                {
                    ObjectManager.updateObjectResource(interactable.transform, resource1.capacity, true);
                    await user.PrintMessageAsync(_localizer["Commands:Refuel:Success", new
                    {
                        Name = resource1.name.Replace("_", " "),
                    }]);
                }

                if (interactable is InteractableObjectResource { objectAsset: { interactability: EObjectInteractability.WATER } } resource2)
                {
                    ObjectManager.updateObjectResource(interactable.transform, resource2.capacity, true);
                    await user.PrintMessageAsync(_localizer["Commands:Refuel:Success", new
                    {
                        Name = resource2.name.Replace("_", " "),
                    }]);
                }
            }

            await UniTask.SwitchToThreadPool();
        }
    }
}
