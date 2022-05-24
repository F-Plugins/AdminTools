using AdminTools.Models;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenMod.API.Eventing;
using OpenMod.Unturned.Players.Chat.Events;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminTools.Events
{
    public class UnturnedPlayerChattingEventListener : IEventListener<UnturnedPlayerChattingEvent>
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<UnturnedPlayerChattingEvent> _logger;

        public UnturnedPlayerChattingEventListener(IConfiguration configuration, ILogger<UnturnedPlayerChattingEvent> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task HandleEventAsync(object? sender, UnturnedPlayerChattingEvent @event)
        {
            List<WebCommand> config = _configuration.GetSection("Web:WebCommands").Get<List<WebCommand>>();
            WebCommand find = config.FirstOrDefault(x => x.CommandName != null && x.CommandName.ToLower() == @event.Message.Remove(0, 1));
            if (find != null && find.Enabled)
            {
                if (find.RequestDescription == null || find.RequestURL == null)
                {
                    _logger.LogWarning("The web request on the join event has an error");
                    return;
                }
                await UniTask.SwitchToMainThread();
                @event.Player.Player.sendBrowserRequest(find.RequestDescription, find.RequestURL);
                await UniTask.SwitchToThreadPool();
                @event.IsCancelled = true;
            }
        }
    }
}
