using System;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Commands;
using OpenMod.Unturned.Users;
using UnityEngine;

namespace AdminTools.Commands
{
    [Command("ascend")]
    [CommandAlias("up")]
    [CommandDescription("A command to teleports you up a certain distance")]
    [CommandSyntax("<distance>")]
    [CommandActor(typeof(UnturnedUser))]
    public class DescendCommand : UnturnedCommand
    {
        public DescendCommand(IServiceProvider serviceProvider, IStringLocalizer stringLocalizer) : base(serviceProvider)
        {
        }

        protected override async UniTask OnExecuteAsync()
        {
            UnturnedUser user = (UnturnedUser)Context.Actor;
            float distance = await Context.Parameters.GetAsync<float>(0, 0);
            Vector3 position = user.Player.Player.transform.position;
            position.y += distance;
            user.Player.Player.teleportToLocationUnsafe(position, user.Player.Player.transform.eulerAngles.y);
        }
    }
}
