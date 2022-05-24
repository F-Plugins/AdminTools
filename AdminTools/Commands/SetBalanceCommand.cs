using System;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OpenMod.API.Users;
using OpenMod.Core.Commands;
using OpenMod.Extensions.Economy.Abstractions;
using OpenMod.Unturned.Commands;
using OpenMod.Unturned.Users;

namespace AdminTools.Commands
{
    [Command("setbalance")]
    [CommandAlias("setbal")]
    [CommandDescription("A command to set the balance of a player")]
    [CommandSyntax("[player] <balance>")]
    [CommandActor(typeof(UnturnedUser))]
    public class SetBalanceCommand : UnturnedCommand
    {
        private readonly IEconomyProvider _economy;
        private readonly IStringLocalizer _localizer;

        public SetBalanceCommand(IServiceProvider serviceProvider, IStringLocalizer stringLocalizer, IEconomyProvider economyProvider) : base(serviceProvider)
        {
            _economy = economyProvider;
            _localizer = stringLocalizer;
        }

        protected override async UniTask OnExecuteAsync()
        {
            UnturnedUser player = (UnturnedUser)Context.Actor;

            switch (Context.Parameters.Length)
            {
                case 1:
                    if (Context.Parameters.TryGet<decimal>(0, out decimal balance1))
                    {
                        await _economy.SetBalanceAsync(player.Id, player.Type, balance1);
                        await player.PrintMessageAsync(_localizer["Commands:SetBalance:Success", new
                        {
                            Name = "your",
                            Balance = balance1
                        }]);
                    }
                    else
                    {
                        await player.PrintMessageAsync(_localizer["Commands:SetBalance:Error"]);
                    }
                    break;
                case 2:
                    if (Context.Parameters.TryGet<IUser>(0, out IUser? target))
                    {
                        if (Context.Parameters.TryGet<decimal>(1, out decimal balance2))
                        {
                            await _economy.SetBalanceAsync(target!.Id, target.Type, balance2);
                            await player.PrintMessageAsync(_localizer["Commands:SetBalance:Success", new
                            {
                                Name = target.DisplayName,
                                Balance = balance2
                            }]);
                        }
                        else
                        {
                            await player.PrintMessageAsync(_localizer["Commands:SetBalance:Error"]);
                        }
                        break;
                    }
                    else
                    {
                        await player.PrintMessageAsync(_localizer["Commands:SetBalance:Invalid"]);
                    }
                    break;
                default:
                    await player.PrintMessageAsync(_localizer["Commands:SetBalance:Error"]);
                    return;
            }
        }
    }
}
