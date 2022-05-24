using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Commands;
using OpenMod.Unturned.Users;
using SDG.Unturned;
using System;
using UnityEngine;

namespace AdminTools.Commands
{
    [Command("door")]
    [CommandDescription("A command to open/close a door")]
    [CommandActor(typeof(UnturnedUser))]
    public class DoorCommand : UnturnedCommand
    {
        private readonly IStringLocalizer _localizer;

        public DoorCommand(IServiceProvider serviceProvider, IStringLocalizer stringLocalizer) : base(serviceProvider)
        {
            _localizer = stringLocalizer;
        }

        protected override async UniTask OnExecuteAsync()
        {
            UnturnedUser player = (UnturnedUser)Context.Actor;

            await UniTask.SwitchToMainThread();

            Vector3 position = player.Player.Player.look.aim.position;
            Vector3 forward = player.Player.Player.look.aim.forward;
            if (Physics.Raycast(position, forward, out RaycastHit hit, 5f, (int)ERayMask.BARRICADE))
            {
                InteractableDoorHinge door = hit.transform.GetComponent<InteractableDoorHinge>();
                if (door != null)
                {
                    BarricadeManager.ServerSetDoorOpen(door.door, !door.door.isOpen);
                }
                else
                {
                    throw new CommandWrongUsageException(_localizer["Commands:Door:NotFound"]);
                }
            }
            else
            {
                throw new CommandWrongUsageException(_localizer["Commands:Door:NotFound"]);
            }
            await UniTask.SwitchToThreadPool();
        }
    }
}
