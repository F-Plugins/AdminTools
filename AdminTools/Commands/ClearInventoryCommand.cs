using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OpenMod.API.Commands;
using OpenMod.API.Users;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Commands;
using OpenMod.Unturned.Users;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminTools.Commands
{
    [Command("clearinventory")]
    [CommandDescription("A command to wipe players inventory")]
    [CommandSyntax("/ci | /ci <playerName>")]
    [CommandActor(typeof(UnturnedUser))]
    [CommandAlias("ci")]
    [CommandAlias("wipeinventory")]
    public class ClearInventoryCommand : UnturnedCommand
    {
        private readonly IStringLocalizer _stringLocalizer;

        public ClearInventoryCommand(IServiceProvider serviceProvider, IStringLocalizer stringLocalizer) : base(serviceProvider)
        {
            _stringLocalizer = stringLocalizer;
        }

        protected override async UniTask OnExecuteAsync()
        {
            var player = (UnturnedUser)Context.Actor;
            
            switch (Context.Parameters.Length) 
            {
                case 0:
                    await UniTask.SwitchToMainThread();
                    ClearPlayerInventory(player.Player.Player);
                    await UniTask.SwitchToThreadPool();
                    await player.PrintMessageAsync(_stringLocalizer["Commands:ClearInventory:Success:1"]);
                    break;
                case 1:

                    if (Context.Parameters.TryGet<IUser>(0, out IUser? user))
                    {
                        var ass = (user as UnturnedUser)!;

                        await UniTask.SwitchToMainThread();
                        ClearPlayerInventory(ass.Player.Player);
                        await UniTask.SwitchToThreadPool();

                        await player.PrintMessageAsync(_stringLocalizer["Commands:ClearInventory:Success:2", new
                        {
                            Name = ass.DisplayName
                        }]);
                    }
                    else
                    {
                        throw new UserFriendlyException(_stringLocalizer["Commands:ClearInventory:Error:2"]);
                    }

                    break;
                default:
                    throw new CommandWrongUsageException(_stringLocalizer["Commands:ClearInventory:Error:1"]);
            }
        }

        private void ClearPlayerInventory(Player player)
        {
            for (byte i = 0; i < PlayerInventory.PAGES - 2; i++)
            {
                var count = player.inventory.getItemCount(i);

                for (byte k = 0; k < count; k++)
                {
                    player.inventory.removeItem(i, k);
                }
            }
            
            void Remove()
            {
                player.inventory.removeItem(2, 0);
            }

            player.clothing.ReceiveWearBackpack(0, 0, new byte[0]);
            Remove();
            player.clothing.ReceiveWearGlasses(0, 0, new byte[0]);
            Remove();
            player.clothing.ReceiveWearHat(0, 0, new byte[0]);
            Remove();
            player.clothing.ReceiveWearMask(0, 0, new byte[0]);
            Remove();
            player.clothing.ReceiveWearPants(0, 0, new byte[0]);
            Remove();
            player.clothing.ReceiveWearShirt(0, 0, new byte[0]);
            Remove();
            player.clothing.ReceiveWearVest(0, 0, new byte[0]);
            Remove();
        }
    }
}
