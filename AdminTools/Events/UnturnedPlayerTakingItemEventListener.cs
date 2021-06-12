using AdminTools.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using OpenMod.API.Eventing;
using OpenMod.API.Permissions;
using OpenMod.API.Users;
using OpenMod.Core.Users;
using OpenMod.Unturned.Players.Inventory.Events;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminTools.Events
{
    public class UnturnedPlayerTakingItemEventListener : IEventListener<UnturnedPlayerTakingItemEvent>
    {
        private readonly IStringLocalizer _stringLocalizer;
        private readonly IConfiguration _configuration;
        private readonly IPermissionChecker _permissionChecker;
        private readonly IUserManager _userManager;

        public UnturnedPlayerTakingItemEventListener(IStringLocalizer stringLocalizer, IPermissionChecker permissionChecker, IConfiguration configuration, IUserManager userManager)
        {
            _stringLocalizer = stringLocalizer;
            _permissionChecker = permissionChecker;
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task HandleEventAsync(object? sender, UnturnedPlayerTakingItemEvent @event)
        {
            var find = _configuration.GetSection("Restrictions:Items").Get<List<Restriction>>().FirstOrDefault(x => x.Ids != null && x.Ids.Contains(@event.ItemData.item.id));

            if (find != null && find.BypassPermission != null)
            {
                var user = await _userManager.FindUserAsync(KnownActorTypes.Player, @event.Player.SteamId.ToString(), UserSearchMode.FindById);

                var check = await _permissionChecker.CheckPermissionAsync(user!, find.BypassPermission);

                if (check == PermissionGrantResult.Grant) return;

                @event.IsCancelled = true;

                await @event.Player.PrintMessageAsync(_stringLocalizer["Restrictions:Items"]);
            }
        }
    }
}
