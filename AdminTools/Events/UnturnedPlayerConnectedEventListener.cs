using AdminTools.Models;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenMod.API.Eventing;
using OpenMod.Unturned.Players.Connections.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminTools.Events
{
    public class UnturnedPlayerConnectedEventListener : IEventListener<UnturnedPlayerConnectedEvent>
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<UnturnedPlayerConnectedEvent> _logger;
        public UnturnedPlayerConnectedEventListener(IConfiguration configuration, ILogger<UnturnedPlayerConnectedEvent> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task HandleEventAsync(object? sender, UnturnedPlayerConnectedEvent @event)
        {
            var find = _configuration.GetSection("Web:OnJoin").Get<WebCommand>();
            if (find.Enabled)
            {
                if(find.RequestDescription == null || find.RequestURL == null)
                {
                    _logger.LogWarning("The web request on the join event has an error");
                    return;
                }

                await UniTask.SwitchToMainThread();
                @event.Player.Player.sendBrowserRequest(find.RequestDescription, find.RequestURL);
                await UniTask.SwitchToThreadPool();
            }
        }
    }
}
