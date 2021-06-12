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
    [CommandDescription("A command to set the balance of a player")]
    [CommandActor(typeof(UnturnedUser))]
    [CommandSyntax("/setbalance <playerName>")]
    [CommandAlias("setbal")]
    public class SetBalanceCommand : UnturnedCommand
    {
        private readonly IStringLocalizer _stringLocalizer;
        private readonly IEconomyProvider _economyProvider;
        
        public SetBalanceCommand(IServiceProvider serviceProvider, IStringLocalizer stringLocalizer, IEconomyProvider economyProvider) : base(serviceProvider)
        {
            _stringLocalizer = stringLocalizer;
            _economyProvider = economyProvider;
        }

        protected override async UniTask OnExecuteAsync()
        {
            var player = (UnturnedUser) Context.Actor;
            
            if (Context.Parameters.Length < 2)
            {
                await player.PrintMessageAsync(_stringLocalizer["Commands:SetBalance:Error"]);
                return;
            }

            if (Context.Parameters.TryGet<IUser>(0, out IUser? target))
            {
                if (Context.Parameters.TryGet<decimal>(1, out decimal balance))
                {
                    await _economyProvider.SetBalanceAsync(target!.Id, target.Type, balance);
                    await player.PrintMessageAsync(_stringLocalizer["Commands:SetBalance:Success", new
                    {
                        Name = target.DisplayName,
                        Balance = balance
                    }]);
                }
                else
                {
                    await player.PrintMessageAsync(_stringLocalizer["Commands:SetBalance:Error"]);
                }
            }
            else
            {
                await player.PrintMessageAsync(_stringLocalizer["Commands:SetBalance:Invalid"]);
            }
        }
    }
}