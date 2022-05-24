using AdminTools.API;
using AdminTools.Models;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OpenMod.API.Ioc;
using OpenMod.Core.Helpers;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdminTools.Services
{
    [PluginServiceImplementation(Lifetime = Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton)]
    public class BroadcastService : IBroadcastService, IDisposable
    {
        private bool _enabled = false;
        private int _index = 0;
        private readonly IConfiguration _configuration;

        public BroadcastService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public UniTask StartBroadcastAsync()
        {
            _index = 0;
            if (Level.isLoaded)
            {
                _enabled = true;
                AsyncHelper.Schedule("Feli.AdminTools.Broadcast", () => Broadcast().AsTask());
            }

            Level.onLevelLoaded += OnLoaded;
            return UniTask.CompletedTask;
        }

        private void OnLoaded(int level)
        {
            if (level == 2)
            {
                _enabled = true;
                AsyncHelper.Schedule("Feli.AdminTools.Broadcast", () => Broadcast().AsTask());
            }
        }

        private async UniTask Broadcast()
        {
            while (_enabled)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_configuration.GetSection("Broadcast:Timer").Get<double>()));

                BroadcastMessage? message = null;
                List<BroadcastMessage> list = _configuration.GetSection("Broadcast:Messages").Get<List<BroadcastMessage>>();

                if (_configuration.GetSection("Broadcast:RandomOrder").Get<bool>())
                {
                    message = list[new Random().Next(0, list.Count)];
                }
                else
                {
                    if (_index >= list.Count()) _index = 0;
                    message = list[_index];
                    _index++;
                }

                if (message != null && message.ImageURL != null && message.Message != null)
                {
                    await UniTask.SwitchToMainThread();
                    ChatManager.serverSendMessage(message.Message.Replace("{", "<").Replace("}", ">"), UnityEngine.Color.white, null, null, EChatMode.GLOBAL, message.ImageURL, true);
                    await UniTask.SwitchToThreadPool();
                }
            }
        }

        public void Dispose()
        {
            _enabled = false;
            Level.onLevelLoaded -= OnLoaded;
        }
    }
}
