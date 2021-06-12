using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Commands;
using OpenMod.Unturned.Users;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdminTools.Commands
{
    [Command("door")]
    [CommandDescription("A command to open/close a door")]
    [CommandActor(typeof(UnturnedUser))]
    public class DoorCommand : UnturnedCommand
    {
        private readonly IStringLocalizer _stringLocalizer;

        public DoorCommand(IServiceProvider serviceProvider, IStringLocalizer stringLocalizer) : base(serviceProvider)
        {
            _stringLocalizer = stringLocalizer;
        }

        protected override async UniTask OnExecuteAsync()
        {
            var player = (UnturnedUser)Context.Actor;

            await UniTask.SwitchToMainThread();
            if (Physics.Raycast(player.Player.Player.look.aim.position, player.Player.Player.look.aim.forward, out RaycastHit hit, 15f, (int)ERayMask.BARRICADE))
            {
                var door = hit.transform.GetComponent<InteractableDoorHinge>();

                if (door != null)
                {
                    BarricadeManager.ServerSetDoorOpen(door.door, !door.door.isOpen);
                }
                else
                {
                    throw new CommandWrongUsageException(_stringLocalizer["Commands:Door:NotFound"]);
                }
            }
            else
            {
                throw new CommandWrongUsageException(_stringLocalizer["Commands:Door:NotFound"]);
            }
            await UniTask.SwitchToThreadPool();
        }
    }
}
