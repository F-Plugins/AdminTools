using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OpenMod.API.Users;
using OpenMod.Core.Commands;
using OpenMod.Core.Users;
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
    [Command("storage")]
    [CommandDescription("A command to open locked storages")]
    [CommandActor(typeof(UnturnedUser))]
    public class StorageCommand : UnturnedCommand
    {
        private readonly IStringLocalizer _stringLocalizer;

        public StorageCommand(IStringLocalizer stringLocalizer, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _stringLocalizer = stringLocalizer;
        }

        protected async override UniTask OnExecuteAsync()
        {
            var player = (UnturnedUser)Context.Actor;

            await UniTask.SwitchToMainThread();
            if (Physics.Raycast(player.Player.Player.look.aim.position, player.Player.Player.look.aim.forward, out RaycastHit hit, 15f, (int)ERayMask.BARRICADE))
            {
                var storage = hit.transform.GetComponent<InteractableStorage>();

                if(storage != null)
                {
                    storage.isOpen = true;
                    storage.opener = player.Player.Player;
                    player.Player.Player.inventory.isStoring = true;
                    player.Player.Player.inventory.storage = storage;
                    player.Player.Player.inventory.updateItems(7, storage.items);
                    player.Player.Player.inventory.sendStorage();
                }
            }
            else
            {
                await player.PrintMessageAsync(_stringLocalizer["Commands:Storage:NotFound"]);
            }
            await UniTask.SwitchToThreadPool();
        }
    }
}
