using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OpenMod.API.Commands;
using OpenMod.API.Users;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Commands;
using OpenMod.Unturned.Users;
using SDG.Unturned;
using System;

namespace AdminTools.Commands
{
    [Command("clearinventory")]
    [CommandAlias("ci")]
    [CommandAlias("wipeinventory")]
    [CommandDescription("A command to wipe players inventory")]
    [CommandSyntax("[player]")]
    [CommandActor(typeof(UnturnedUser))]
    public class ClearInventoryCommand : UnturnedCommand
    {
        private readonly IStringLocalizer _localizer;

        public ClearInventoryCommand(IServiceProvider serviceProvider, IStringLocalizer stringLocalizer) : base(serviceProvider)
        {
            _localizer = stringLocalizer;
        }

        protected override async UniTask OnExecuteAsync()
        {
            UnturnedUser player = (UnturnedUser)Context.Actor;

            switch (Context.Parameters.Length)
            {
                case 0:
                    ClearPlayerInventory(player.Player.Player);
                    await player.PrintMessageAsync(_localizer["Commands:ClearInventory:Success"]);
                    break;
                case 1:
                    if (Context.Parameters.TryGet<IUser>(0, out IUser? iuser))
                    {
                        UnturnedUser user = (iuser as UnturnedUser)!;
                        ClearPlayerInventory(user.Player.Player);
                        await player.PrintMessageAsync(_localizer["Commands:ClearInventory:SuccessNamed", new
                        {
                            Name = user.DisplayName
                        }]);
                    }
                    else
                    {
                        throw new UserFriendlyException(_localizer["Commands:ClearInventory:Invalid"]);
                    }
                    break;
                default:
                    throw new CommandWrongUsageException(_localizer["Commands:ClearInventory:Error"]);
            }
        }

        async private void ClearPlayerInventory(Player player)
        {
            await UniTask.SwitchToMainThread();

            for (byte i = 0; i < PlayerInventory.PAGES - 2; i++)
            {
                int count = player.inventory.getItemCount(i);
                for (byte k = 0; k < count; k++)
                {
                    player.inventory.removeItem(i, k);
                }
            }

            Guid Int2Guid(int value)
            {
                byte[] bytes = new byte[16];
                BitConverter.GetBytes(value).CopyTo(bytes, 0);
                return new Guid(bytes);
            }

            void Remove() => player.inventory.removeItem(2, 0);

            player.clothing.ReceiveWearBackpack(Int2Guid(0), 0, new byte[0]);
            Remove();
            player.clothing.ReceiveWearGlasses(Int2Guid(0), 0, new byte[0]);
            Remove();
            player.clothing.ReceiveWearHat(Int2Guid(0), 0, new byte[0]);
            Remove();
            player.clothing.ReceiveWearMask(Int2Guid(0), 0, new byte[0]);
            Remove();
            player.clothing.ReceiveWearPants(Int2Guid(0), 0, new byte[0]);
            Remove();
            player.clothing.ReceiveWearShirt(Int2Guid(0), 0, new byte[0]);
            Remove();
            player.clothing.ReceiveWearVest(Int2Guid(0), 0, new byte[0]);
            Remove();

            await UniTask.SwitchToThreadPool();
        }
    }
}
