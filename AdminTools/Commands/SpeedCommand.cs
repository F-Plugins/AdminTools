using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OpenMod.Unturned.Commands;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Users;
using System;
using OpenMod.API.Users;

namespace AdminTools.Commands
{
    [Command("speed")]
    [CommandDescription("A command set the speed of a player")]
    [CommandSyntax("[player] <speed>")]
    [CommandActor(typeof(UnturnedUser))]
    public class SpeedCommand : UnturnedCommand
    {
        private readonly IStringLocalizer _localizer;

        public SpeedCommand(IServiceProvider serviceProvider, IStringLocalizer stringLocalizer) : base(serviceProvider)
        {
            _localizer = stringLocalizer;
        }

        protected override async UniTask OnExecuteAsync()
        {
            UnturnedUser player = (UnturnedUser)Context.Actor;

            switch (Context.Parameters.Length)
            {
                case 1:
                    if (Context.Parameters.TryGet<float>(0, out float speed1))
                    {
                        await UniTask.SwitchToMainThread();
                        player.Player.Player.movement.sendPluginSpeedMultiplier(speed1);
                        await UniTask.SwitchToThreadPool();
                        await player.PrintMessageAsync(_localizer["Commands:Speed:Success", new
                        {
                            Name = player.DisplayName,
                            Speed = speed1
                        }]);
                    }
                    else
                    {
                        throw new CommandWrongUsageException(_localizer["Commands:Speed:Error"]);
                    }
                    break;
                case 2:
                    if (Context.Parameters.TryGet<IUser>(0, out IUser? iuser))
                    {
                        if (Context.Parameters.TryGet<float>(1, out float speed2))
                        {
                            UnturnedUser user = (UnturnedUser)iuser!;
                            await UniTask.SwitchToMainThread();
                            user.Player.Player.movement.sendPluginSpeedMultiplier(speed2);
                            await UniTask.SwitchToThreadPool();
                            await player.PrintMessageAsync(_localizer["Commands:Speed:Success", new
                            {
                                Name = user.DisplayName,
                                Speed = speed2
                            }]);
                        }
                        else
                        {
                            throw new CommandWrongUsageException(_localizer["Commands:Speed:Speed"]);
                        }
                    }
                    else
                    {
                        throw new CommandWrongUsageException(_localizer["Commands:Speed:Invalid"]);
                    }
                    break;
                default:
                    throw new CommandWrongUsageException(_localizer["Commands:Speed:Error"]);
            }
        }
    }
}
