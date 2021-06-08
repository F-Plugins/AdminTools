using AdminTools.API;
using Microsoft.Extensions.Localization;
using OpenMod.API.Eventing;
using OpenMod.API.Permissions;
using OpenMod.API.Users;
using OpenMod.Core.Users;
using OpenMod.Unturned.Players.Movement.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminTools.Events
{
    public class UnturnedPlayerGestureUpdatedEventListener : IEventListener<UnturnedPlayerGestureUpdatedEvent>
    {
        private readonly IPermissionChecker _permissionChecker;
        private readonly IHandDestroyerService _handDestroyerService;
        private readonly IUserManager _userManager;

        public UnturnedPlayerGestureUpdatedEventListener(IPermissionChecker permissionChecker, IHandDestroyerService handDestroyerService, IUserManager userManager)
        {
            _permissionChecker = permissionChecker;
            _userManager = userManager;
            _handDestroyerService = handDestroyerService;
        }

        public async Task HandleEventAsync(object? sender, UnturnedPlayerGestureUpdatedEvent @event)
        {
            if (@event.Gesture == SDG.Unturned.EPlayerGesture.PUNCH_LEFT|| @event.Gesture == SDG.Unturned.EPlayerGesture.PUNCH_RIGHT)
            {
                var user = await _userManager.FindUserAsync(KnownActorTypes.Player, @event.Player.SteamId.ToString(), UserSearchMode.FindById);

                if(user != null)
                {
                    var check = await _permissionChecker.CheckPermissionAsync(user, "commands.handdestroyer");

                    if (check == PermissionGrantResult.Grant)
                    {
                        await _handDestroyerService.Destoy(@event.Player);
                    }
                }
            }
        }
    }
}
