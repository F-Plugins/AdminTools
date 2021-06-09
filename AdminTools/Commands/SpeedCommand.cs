using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OpenMod.Unturned.Commands;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenMod.API.Users;

namespace AdminTools.Commands
{
    [Command("speed")]
    [CommandDescription("A command set the speed of a player")]
    [CommandSyntax("/speed <speed> | /speed <playerName> <speed>")]
    [CommandActor(typeof(UnturnedUser))]
    public class SpeedCommand : UnturnedCommand
    {
        private readonly IStringLocalizer _stringLocalizer;

        public SpeedCommand(IServiceProvider serviceProvider, IStringLocalizer stringLocalizer) : base(serviceProvider)
        {
            _stringLocalizer = stringLocalizer;
        }

        protected override async UniTask OnExecuteAsync()
        {
            var player = (UnturnedUser)Context.Actor;

            switch (Context.Parameters.Length)
            {
                case 1:

                    if(Context.Parameters.TryGet<float>(0, out float value))
                    {
                        await UniTask.SwitchToMainThread();
                        player.Player.Player.movement.sendPluginSpeedMultiplier(value);
                        await UniTask.SwitchToThreadPool();
                        await player.PrintMessageAsync(_stringLocalizer["Commands:Speed:Sucess", new
                        {
                            Name = player.DisplayName,
                            Speed = value
                        }]);
                    }
                    else
                    {
                        throw new CommandWrongUsageException(_stringLocalizer["Commands:Speed:Error"]);
                    }

                    break;
                case 2:
                    
                    if(Context.Parameters.TryGet<IUser>(0, out IUser? user))
                    {
                        if(user != null)
                        {
                            if (Context.Parameters.TryGet<float>(1, out float speed))
                            {
                                var find = (UnturnedUser)user;
                                await UniTask.SwitchToMainThread();
                                find.Player.Player.movement.sendPluginSpeedMultiplier(speed);
                                await UniTask.SwitchToThreadPool();
                                await player.PrintMessageAsync(_stringLocalizer["Commands:Speed:Sucess", new
                                {
                                    Name = user.DisplayName,
                                    Speed = speed
                                }]);
                            }
                            else
                            {
                                throw new CommandWrongUsageException(_stringLocalizer["Commands:Speed:Speed"]);
                            }
                        }
                    }
                    else
                    {
                        throw new CommandWrongUsageException(_stringLocalizer["Commands:Speed:Invalid"]);
                    }

                    break;
                default:
                    throw new CommandWrongUsageException(_stringLocalizer["Commands:Speed:Error"]);
            }
        }
    }
}
