using AdminTools.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using OpenMod.API.Eventing;
using OpenMod.API.Permissions;
using OpenMod.API.Users;
using OpenMod.Core.Users;
using OpenMod.Unturned.Vehicles.Events;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminTools.Events
{
    public class UnturnedPlayerEnteringVehicleEventListener : IEventListener<UnturnedPlayerEnteringVehicleEvent>
    {
        private readonly IStringLocalizer _localizer;
        private readonly IPermissionChecker _checker;
        private readonly IConfiguration _configuration;
        private readonly IUserManager _manager;

        public UnturnedPlayerEnteringVehicleEventListener(IStringLocalizer stringLocalizer, IPermissionChecker permissionChecker, IConfiguration configuration, IUserManager userManager)
        {
            _localizer = stringLocalizer;
            _checker = permissionChecker;
            _configuration = configuration;
            _manager = userManager;
        }

        public async Task HandleEventAsync(object? sender, UnturnedPlayerEnteringVehicleEvent @event)
        {
            Restriction find = _configuration.GetSection("Restrictions:Vehicles").Get<List<Restriction>>().FirstOrDefault(x => x.Ids != null && x.Ids.Contains(@event.Vehicle.Vehicle.id));
            if (find != null && find.BypassPermission != null)
            {
                IUser? user = await _manager.FindUserAsync(KnownActorTypes.Player, @event.Player.SteamId.ToString(), UserSearchMode.FindById);
                PermissionGrantResult check = await _checker.CheckPermissionAsync(user!, find.BypassPermission);
                if (check == PermissionGrantResult.Grant) return;
                @event.IsCancelled = true;
                await @event.Player.PrintMessageAsync(_localizer["Restrictions:Vehicles"]);
            }
        }
    }
}
