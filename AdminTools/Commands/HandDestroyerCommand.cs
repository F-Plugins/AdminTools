using AdminTools.API;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Commands;
using OpenMod.Unturned.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminTools.Commands
{
    [Command("handdestroyer")]
    [CommandDescription("A command to toggle the destroyer mode")]
    [CommandSyntax("/hdestroyer <on|off>")]
    [CommandAlias("hdestroyer")]
    [CommandActor(typeof(UnturnedUser))]
    public class HandDestroyerCommand : UnturnedCommand
    {
        private readonly IStringLocalizer _stringLocalizer;
        private readonly IHandDestroyerService _handDestroyerService;

        public HandDestroyerCommand(IServiceProvider serviceProvider, IStringLocalizer stringLocalizer, IHandDestroyerService handDestroyerService) : base(serviceProvider)
        {
            _handDestroyerService = handDestroyerService;
            _stringLocalizer = stringLocalizer;
        }

        protected override async UniTask OnExecuteAsync()
        {
            var player = (UnturnedUser)Context.Actor;

            if (Context.Parameters.Length < 1)
            {
                throw new CommandWrongUsageException(_stringLocalizer["Commands:HandDestroyer:Error"]);
            }

            if (Context.Parameters.TryGet<string>(0, out string? value))
            {
                if (value != null)
                {
                    switch (value)
                    {
                        case "on":
                            if (await _handDestroyerService.AddToDestroyerMode(player))
                            {
                                await player.PrintMessageAsync(_stringLocalizer["Commands:HandDestroyer:On:Success"]);
                            }
                            else
                            {
                                await player.PrintMessageAsync(_stringLocalizer["Commands:HandDestroyer:On:Already"]);
                            }
                            break;
                        case "off":
                            if (await _handDestroyerService.RemoveFromDestroyerMode(player))
                            {
                                await player.PrintMessageAsync(_stringLocalizer["Commands:HandDestroyer:Off:Success"]);
                            }
                            else
                            {
                                await player.PrintMessageAsync(_stringLocalizer["Commands:HandDestroyer:Off:Already"]);
                            }
                            break;
                        default:
                            throw new CommandWrongUsageException(_stringLocalizer["Commands:HandDestroyer:Error"]);
                    }
                }
                else
                {
                    throw new CommandWrongUsageException(_stringLocalizer["Commands:HandDestroyer:Error"]);
                }
            }
            else
            {
                throw new CommandWrongUsageException(_stringLocalizer["Commands:Fly:Error"]);
            }
        }
    }
}
