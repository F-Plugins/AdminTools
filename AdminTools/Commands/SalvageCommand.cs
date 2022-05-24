using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OpenMod.API.Commands;
using OpenMod.API.Users;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Commands;
using OpenMod.Unturned.Users;
using System;

namespace AdminTools.Commands
{
    [Command("salvage")]
    [CommandDescription("A command to set the salvage time of a player")]
    [CommandSyntax("[player] <time>")]
    [CommandActor(typeof(UnturnedUser))]
    public class SalvageCommand : UnturnedCommand
    {
        private readonly IStringLocalizer _localizer;

        public SalvageCommand(IServiceProvider serviceProvider, IStringLocalizer stringLocalizer) : base(serviceProvider)
        {
            _localizer = stringLocalizer;
        }

        protected override async UniTask OnExecuteAsync()
        {
            UnturnedUser player = (UnturnedUser)Context.Actor;

            switch (Context.Parameters.Length)
            {
                case 1:
                    if (Context.Parameters.TryGet<float>(0, out float time1))
                    {
                        await UniTask.SwitchToMainThread();
                        player.Player.Player.interact.sendSalvageTimeOverride(time1);
                        await UniTask.SwitchToThreadPool();
                        await player.PrintMessageAsync(_localizer["Commands:Salvage:Success", new
                        {
                            Name = "your",
                            Time = time1
                        }]);
                    }
                    else
                    {
                        throw new UserFriendlyException(_localizer["Commands:Salvage:Error"]);
                    }
                    break;
                case 2:
                    if (Context.Parameters.TryGet<IUser>(0, out IUser? iuser))
                    {
                        UnturnedUser user = (UnturnedUser)iuser!;
                        if (Context.Parameters.TryGet<float>(1, out float time2))
                        {
                            await UniTask.SwitchToMainThread();
                            user.Player.Player.interact.sendSalvageTimeOverride(time2);
                            await UniTask.SwitchToThreadPool();
                            await player.PrintMessageAsync(_localizer["Commands:Salvage:Success", new
                            {
                                Name = user.DisplayName,
                                Time = time2
                            }]);
                        }
                        else
                        {
                            throw new UserFriendlyException(_localizer["Commands:Salvage:Error"]);
                        }
                    }
                    else
                    {
                        throw new UserFriendlyException(_localizer["Commands:Salvage:Invalid"]);
                    }
                    break;
                default:
                    throw new CommandWrongUsageException(_localizer["Commands:Salvage:Error"]);
            }
        }
    }
}
