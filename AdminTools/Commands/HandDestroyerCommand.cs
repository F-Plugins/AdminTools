using AdminTools.API;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Commands;
using OpenMod.Unturned.Users;
using System;

namespace AdminTools.Commands
{
    [Command("handdestroyer")]
    [CommandAlias("hdestroyer")]
    [CommandDescription("A command to toggle the destroyer mode")]
    [CommandActor(typeof(UnturnedUser))]
    public class HandDestroyerCommand : UnturnedCommand
    {
        private readonly IHandDestroyerService _service;
        private readonly IStringLocalizer _localizer;

        public HandDestroyerCommand(IServiceProvider serviceProvider, IStringLocalizer stringLocalizer, IHandDestroyerService handDestroyerService) : base(serviceProvider)
        {
            _service = handDestroyerService;
            _localizer = stringLocalizer;
        }

        protected override async UniTask OnExecuteAsync()
        {
            UnturnedUser player = (UnturnedUser)Context.Actor;
            if (_service.IsOnDestroyerMode(player))
            {
                await _service.RemoveFromDestroyerMode(player);
                await player.PrintMessageAsync(_localizer["Commands:HandDestroyer:TurnOff"]);
            }
            else
            {
                await _service.AddToDestroyerMode(player);
                await player.PrintMessageAsync(_localizer["Commands:HandDestroyer:TurnOn"]);
            }
        }
    }
}
