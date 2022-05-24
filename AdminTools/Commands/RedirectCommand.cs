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
    [CommandSyntax("<playerName> <ip> <port> [force] [password]")]
    public class RedirectCommand : UnturnedCommand
    {
        private readonly IStringLocalizer _localizer;

        public RedirectCommand(IServiceProvider serviceProvider, IStringLocalizer stringLocalizer) : base(serviceProvider)
        {
            _localizer = stringLocalizer;
        }

        protected override async UniTask OnExecuteAsync()
        {
            if (Context.Parameters.Length < 3)
            {
                await Context.Actor.PrintMessageAsync(_localizer["Commands:Redirect:Error"]);
                return;
            }

            if (Context.Parameters.TryGet<IUser>(0, out IUser? iuser))
            {
                UnturnedUser user = (UnturnedUser)iuser!;
                bool validIP = Context.Parameters.TryGet<string>(1, out string? ip);
                bool validPort = ushort.TryParse(await Context.Parameters.GetAsync<string>(2), out ushort port);
                if (validIP && validPort)
                {
                    Context.Parameters.TryGet<bool>(3, out bool isForce);
                    Context.Parameters.TryGet<string>(4, out string? password);
                    user.Player.Player.sendRelayToServer(Parser.getUInt32FromIP(ip!), port, password ?? "", !isForce);
                    await Context.Actor.PrintMessageAsync(_localizer["Commands:Redirect:Success"]);
                }
                else
                {
                    throw new CommandWrongUsageException(_localizer["Commands:Redirect:Error"]);
                }
            }
            else
            {
                throw new CommandWrongUsageException(_localizer["Commands:Redirect:NotFound"]);
            }
        }
    }
}
