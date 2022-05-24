using AdminTools.API;
using OpenMod.API.Eventing;
using OpenMod.API.Permissions;
using OpenMod.API.Users;
using OpenMod.Core.Users;
using OpenMod.Unturned.Players.Movement.Events;
using System.Threading.Tasks;

namespace AdminTools.Events
{
    public class UnturnedPlayerGestureUpdatedEventListener : IEventListener<UnturnedPlayerGestureUpdatedEvent>
    {
        private readonly IPermissionChecker _checker;
        private readonly IHandDestroyerService _service;
        private readonly IUserManager _manager;

        public UnturnedPlayerGestureUpdatedEventListener(IPermissionChecker permissionChecker, IHandDestroyerService handDestroyerService, IUserManager userManager)
        {
            _checker = permissionChecker;
            _service = handDestroyerService;
            _manager = userManager;
        }

        public async Task HandleEventAsync(object? sender, UnturnedPlayerGestureUpdatedEvent @event)
        {
            if (@event.Gesture == SDG.Unturned.EPlayerGesture.PUNCH_LEFT || @event.Gesture == SDG.Unturned.EPlayerGesture.PUNCH_RIGHT)
            {
                IUser? user = await _manager.FindUserAsync(KnownActorTypes.Player, @event.Player.SteamId.ToString(), UserSearchMode.FindById);
                if (user == null) return;
                PermissionGrantResult permission = await _checker.CheckPermissionAsync(user, "commands.handdestroyer");
                if (permission == PermissionGrantResult.Grant) await _service.Destroy(@event.Player);
            }
        }
    }
}
