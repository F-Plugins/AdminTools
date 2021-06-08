using AdminTools.API;
using AdminTools.Models;
using Microsoft.Extensions.Configuration;
using OpenMod.API.Ioc;
using OpenMod.API.Permissions;
using OpenMod.API.Users;
using OpenMod.Core.Helpers;
using OpenMod.Core.Users;
using OpenMod.Unturned.Users;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminTools.Services
{
    [PluginServiceImplementation(Lifetime = Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton)]
    public class FlyService : IFlyService, IDisposable
    {
        private List<FlyData> _flyPlayers;

        private readonly IPermissionChecker _permissionChecker;
        private readonly IUserManager _userManager;
        private readonly IConfiguration _configuration;

        public FlyService(IPermissionChecker permissionChecker, IUserManager userManager, IConfiguration configuration)
        {
            _permissionChecker = permissionChecker;
            _userManager = userManager;
            _configuration = configuration;
            _flyPlayers = new List<FlyData>();
            PlayerInput.onPluginKeyTick += OnTick;
        }

        private void OnTick(Player player, uint simulation, byte key, bool state)
        {
            if (_flyPlayers.Any(x => x.UserId == player.channel.owner.playerID.steamID.ToString()))
            {
                AsyncHelper.RunSync(async () =>
                {
                    var user = await _userManager.FindUserAsync(KnownActorTypes.Player, player.channel.owner.playerID.steamID.ToString(), UserSearchMode.FindById);
                    if(user != null)
                    {
                        var check = await _permissionChecker.CheckPermissionAsync(user, "commands.fly");
                        var find = _flyPlayers.FirstOrDefault(x => x.UserId == player.channel.owner.playerID.steamID.ToString());
                        if (check == PermissionGrantResult.Grant)
                        {
                            if(key == 4 && state)
                            {
                                player.movement.sendPluginGravityMultiplier(1);
                            }
                            else if(key == 3 && state && !find.IsChanging)
                            {
                                find.IsChanging = true;
                                player.movement.sendPluginGravityMultiplier(-0.2f);
                            }
                            if(key == 3 && !state && find.IsChanging)
                            {
                                player.movement.sendPluginGravityMultiplier(0);
                                find.IsChanging = false;
                            }
                        }
                    }
                });
            }
        }

        public Task<bool> AddToFlyMode(UnturnedUser user)
        {
            if (!_flyPlayers.Any(x => x.UserId == user.Id))
            {
                _flyPlayers.Add(new FlyData
                {
                    UserId = user.Id,
                    IsChanging = false
                });
                return Task.FromResult(result: true);
            }

            return Task.FromResult(result: false);
        }

        public Task<bool> RemoveFromFlyMode(UnturnedUser user)
        {
            if (_flyPlayers.Any(x => x.UserId == user.Id))
            {
                _flyPlayers.Remove(_flyPlayers.FirstOrDefault(x => x.UserId == user.Id));
                return Task.FromResult(result: true);
            }

            return Task.FromResult(result: false);
        }

        public void Dispose()
        {
            _flyPlayers.Clear();
            PlayerInput.onPluginKeyTick -= OnTick;
        }
    }
}
