using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OpenMod.API.Users;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Commands;
using OpenMod.Unturned.Users;
using SDG.Unturned;
using System;

namespace AdminTools.Commands
{
    [Command("redirect")]
    [CommandAlias("redirectplayer")]
    [CommandDescription("A command to redirect a player to another server")]
    [CommandSyntax("/redirect <playerName> <ip> <port> <dontForce>")]
    public class RedirectCommand : UnturnedCommand
    {
        private readonly IStringLocalizer _stringLocalizer;
        public RedirectCommand(IServiceProvider serviceProvider, IStringLocalizer stringLocalizer) : base(serviceProvider)
        {
            _stringLocalizer = stringLocalizer;
        }

        protected override async UniTask OnExecuteAsync()
        {
            if(Context.Parameters.Length < 4)
            {
                await Context.Actor.PrintMessageAsync(_stringLocalizer["Commands:Redirect:Error:1"]);
                return;
            }

            if (Context.Parameters.TryGet<IUser>(0, out IUser? user))
            {
                var uUser = (UnturnedUser)user!;

                await Context.Actor.PrintMessageAsync(_stringLocalizer["Commands:Redirect:Success"]);

                if (Context.Parameters.TryGet<string>(1, out string? ip))
                {
                    if (ushort.TryParse(await Context.Parameters.GetAsync<string>(2), out ushort port))
                    {
                        if (bool.TryParse(await Context.Parameters.GetAsync<string>(3), out bool value))
                        {
                            await Context.Actor.PrintMessageAsync(_stringLocalizer["Commands:Redirect:Success"]);
                            uUser.Player.Player.sendRelayToServer(Parser.getUInt32FromIP(ip!), port, "", value);
                        }
                        else
                        {
                            throw new CommandWrongUsageException(_stringLocalizer["Commands:Redirect:Error:1"]);
                        }
                    }
                    else
                    {
                        throw new CommandWrongUsageException(_stringLocalizer["Commands:Redirect:Error:1"]);
                    }
                }
                else
                {
                    throw new CommandWrongUsageException(_stringLocalizer["Commands:Redirect:Error:1"]);
                }
            }
            else
            {
                throw new CommandWrongUsageException(_stringLocalizer["Commands:Redirect:Error:2"]);
            }
        }
    }
}
