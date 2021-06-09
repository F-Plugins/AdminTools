using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OpenMod.API.Commands;
using OpenMod.API.Users;
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
    [Command("salvage")]
    [CommandDescription("A command to set the salvage time of a player")]
    [CommandSyntax("/salvage <time> | /salvage <playerName> <time>")]
    [CommandActor(typeof(UnturnedUser))]
    public class SalvageCommand : UnturnedCommand
    {
        private readonly IStringLocalizer _stringLocalizer;

        public SalvageCommand(IServiceProvider serviceProvider, IStringLocalizer stringLocalizer) : base(serviceProvider)
        {
            _stringLocalizer = stringLocalizer;
        }

        protected override async UniTask OnExecuteAsync()
        {
            var player = (UnturnedUser)Context.Actor;

            switch (Context.Parameters.Length)
            {
                case 1:

                    if (Context.Parameters.TryGet<float>(0, out float value))
                    {
                        await UniTask.SwitchToMainThread();
                        player.Player.Player.interact.sendSalvageTimeOverride(value);
                        await UniTask.SwitchToThreadPool();
                        await player.PrintMessageAsync(_stringLocalizer["Commands:Salvage:Success:1", new
                        {
                            Time = value
                        }]);
                    }
                    else
                    {
                        throw new UserFriendlyException(_stringLocalizer["Commands:Salvage:Error:3"]);
                    }

                    break;
                case 2:

                    if (Context.Parameters.TryGet<IUser>(0, out IUser? user))
                    {
                        var ass = (user as UnturnedUser)!;

                        if (Context.Parameters.TryGet<float>(1, out float value2))
                        {
                            await UniTask.SwitchToMainThread();
                            ass.Player.Player.interact.sendSalvageTimeOverride(value2);
                            await UniTask.SwitchToThreadPool();
                            await player.PrintMessageAsync(_stringLocalizer["Commands:Salvage:Success:2", new
                            {
                                Name = ass.DisplayName,
                                Time = value2
                            }]);
                        }
                        else
                        {
                            throw new UserFriendlyException(_stringLocalizer["Commands:Salvage:Error:3"]);
                        }
                    }
                    else
                    {
                        throw new UserFriendlyException(_stringLocalizer["Commands:Salvage:Error:2"]);
                    }

                    break;
                default:
                    throw new CommandWrongUsageException(_stringLocalizer["Commands:Salvage:Error:1"]);
            }
        }
    }
}
