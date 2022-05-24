using AdminTools.API;
using AdminTools.Models;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenMod.API.Permissions;
using OpenMod.API.Plugins;
using OpenMod.Unturned.Plugins;
using System;
using System.Collections.Generic;

[assembly: PluginMetadata("Feli.AdminTools", DisplayName = "AdminTools", Author = "Feli", Website = "fplugins.com", Description = "A plugin with some utilities for your admins")]

namespace AdminTools
{
    public class AdminTools : OpenModUnturnedPlugin
    {
        private readonly ILogger<AdminTools> _logger;
        private readonly IConfiguration _configuration;
        private readonly IBroadcastService _broadcast;
        private readonly IPermissionRegistry _permission;

        public AdminTools(
            ILogger<AdminTools> logger,
            IConfiguration configuration,
            IBroadcastService broadcastService,
            IPermissionRegistry permissionRegistry,
            IServiceProvider serviceProvider
        ) : base(serviceProvider)
        {
            _logger = logger;
            _configuration = configuration;
            _broadcast = broadcastService;
            _permission = permissionRegistry;
        }

        protected override UniTask OnLoadAsync()
        {
            if (_configuration.GetSection("Broadcast:ServiceEnabled").Get<bool>())
            {
                _broadcast.StartBroadcastAsync();
            }

            foreach (Restriction restrict in _configuration.GetSection("Restrictions:Items").Get<List<Restriction>>())
            {
                if (restrict.BypassPermission != null)
                {
                    _permission.RegisterPermission(this, restrict.BypassPermission, "A bypass permission for restrictions");
                }
            }

            foreach (Restriction restrict in _configuration.GetSection("Restrictions:Vehicles").Get<List<Restriction>>())
            {
                if (restrict.BypassPermission != null)
                {
                    _permission.RegisterPermission(this, restrict.BypassPermission, "A bypass permission for restrictions");
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
