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
    [Command("storage")]
    [CommandDescription("A command to open locked storages")]
    [CommandActor(typeof(UnturnedUser))]
    public class StorageCommand : UnturnedCommand
    {
        private readonly IStringLocalizer _localizer;

        public StorageCommand(IStringLocalizer stringLocalizer, IServiceProvider serviceProvider) : base(serviceProvider)
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
                InteractableStorage storage = hit.transform.GetComponent<InteractableStorage>();
                if (storage != null)
                {
                    storage.isOpen = true;
                    storage.opener = player.Player.Player;
                    player.Player.Player.inventory.isStoring = true;
                    player.Player.Player.inventory.storage = storage;
                    player.Player.Player.inventory.updateItems(7, storage.items);
                    player.Player.Player.inventory.sendStorage();
                }
                else
                {
                    throw new CommandWrongUsageException(_localizer["Commands:Storage:NotFound"]);
                }
            }
            else
            {
                throw new CommandWrongUsageException(_localizer["Commands:Storage:NotFound"]);
            }
            await UniTask.SwitchToThreadPool();
        }
    }
}
