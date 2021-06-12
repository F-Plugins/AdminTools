using AdminTools.API;
using AdminTools.Models;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using OpenMod.API.Permissions;
using OpenMod.API.Plugins;
using OpenMod.Unturned.Plugins;
using SDG.Unturned;
using System;
using System.Collections.Generic;

[assembly: PluginMetadata("Feli.AdminTools", DisplayName = "AdminTools", Author = "Feli", Website = "fplugins.com")]

namespace AdminTools
{
    public class AdminTools : OpenModUnturnedPlugin
    {
        private readonly ILogger<AdminTools> _logger;
        private readonly IConfiguration _configuration;
        private readonly IBroadcastService _broadcastService;
        private readonly IPermissionRegistry _permissionRegistry;

        public AdminTools(
            ILogger<AdminTools> logger,
            IConfiguration configuration,
            IBroadcastService broadcastService,
            IPermissionRegistry permissionRegistry,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _logger = logger;
            _configuration = configuration;
            _broadcastService = broadcastService;
            _permissionRegistry = permissionRegistry;
        }

        protected override UniTask OnLoadAsync()
        {
            if (_configuration.GetSection("Broadcast:ServiceEnabled").Get<bool>())
            {
                _broadcastService.StartBroadcastAsync();
            }

            foreach(var restrict in _configuration.GetSection("Restrictions:Items").Get<List<Restriction>>())
            {
                if(restrict.BypassPermission != null)
                {
                    _permissionRegistry.RegisterPermission(this, restrict.BypassPermission, "A bypass permission for restrictions");
                }
            }

            foreach (var restrict in _configuration.GetSection("Restrictions:Vehicles").Get<List<Restriction>>())
            {
                if (restrict.BypassPermission != null)
                {
                    _permissionRegistry.RegisterPermission(this, restrict.BypassPermission, "A bypass permission for restrictions");
                }
            }

            _logger.LogInformation("Admin Tools plugin made by Feli");
            _logger.LogInformation("Discord: https://discord.gg/4FF2548");
            return UniTask.CompletedTask;
        }

        protected override UniTask OnUnloadAsync()
        {
            _logger.LogInformation("Admin Tools plugin made by Feli");
            _logger.LogInformation("Discord: https://discord.gg/4FF2548");
            return UniTask.CompletedTask;
        }
    }
}
