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
    [Command("fly")]
    [CommandDescription("A command to toggle the fly mode")]
    [CommandSyntax("/fly <on|off>")]
    [CommandActor(typeof(UnturnedUser))]
    public class FlyCommand : UnturnedCommand
    {
        private readonly IFlyService _flyService;
        private readonly IStringLocalizer _stringLocalizer;

        public FlyCommand(IServiceProvider serviceProvider, IFlyService flyService, IStringLocalizer stringLocalizer) : base(serviceProvider)
        {
            _flyService = flyService;
            _stringLocalizer = stringLocalizer;
        }

        protected async override UniTask OnExecuteAsync()
        {
            var player = (UnturnedUser)Context.Actor;

            if(Context.Parameters.Length < 1)
            {
                throw new CommandWrongUsageException(_stringLocalizer["Commands:Fly:Error"]);
            }
            
            if(Context.Parameters.TryGet<string>(0, out string? value))
            {
                if(value != null)
                {
                    switch (value)
                    {
                        case "on":
                            if (await _flyService.AddToFlyMode(player))
                            {
                                await player.PrintMessageAsync(_stringLocalizer["Commands:Fly:On:Success"]);
                            }
                            else
                            {
                                await player.PrintMessageAsync(_stringLocalizer["Commands:Fly:On:Already"]);
                            }
                            break;
                        case "off":
                            if (await _flyService.RemoveFromFlyMode(player))
                            {
                                await player.PrintMessageAsync(_stringLocalizer["Commands:Fly:Off:Sucess"]);
                            }
                            else
                            {
                                await player.PrintMessageAsync(_stringLocalizer["Commands:Fly:Off:Already"]);
                            }
                            break;
                        default:
                            throw new CommandWrongUsageException(_stringLocalizer["Commands:Fly:Error"]);
                    }
                }
                else
                {
                    throw new CommandWrongUsageException(_stringLocalizer["Commands:Fly:Error"]);
                }
            }
            else
            {
                throw new CommandWrongUsageException(_stringLocalizer["Commands:Fly:Error"]);
            }
        }
    }
}
