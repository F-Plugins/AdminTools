using AdminTools.API;
using Cysharp.Threading.Tasks;
using OpenMod.API.Ioc;
using OpenMod.Unturned.Players;
using OpenMod.Unturned.Users;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdminTools.Services
{
    [PluginServiceImplementation(Lifetime = Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton)]
    public class HandDestroyerService : IHandDestroyerService
    {
        private List<string> _playerIds;

        public HandDestroyerService()
        {
            _playerIds = new List<string>();
        }

        public Task<bool> AddToDestroyerMode(UnturnedUser user)
        {
            if (!_playerIds.Contains(user.Id))
            {
                _playerIds.Add(user.Id);
                return Task.FromResult(result: true);
            }

            return Task.FromResult(result: false);
        }

        public async Task Destoy(UnturnedPlayer user)
        {
            await UniTask.SwitchToMainThread();
            if (Physics.Raycast(user.Player.look.aim.position, user.Player.look.aim.forward, out RaycastHit hit, 15f, (int)ERayMask.BARRICADE | (int)ERayMask.VEHICLE | (int)ERayMask.STRUCTURE))
            {
                var vehicle = hit.transform.GetComponent<InteractableVehicle>();

                if (vehicle != null)
                {
                    VehicleManager.askVehicleDestroy(vehicle);
                    return;
                }

                if (BarricadeManager.tryGetInfo(hit.transform, out byte x, out byte y, out ushort plant, out ushort index, out BarricadeRegion region))
                {
                    var barricade = region.barricades[index];

                    BarricadeManager.destroyBarricade(region, x, y, plant, index);

                    return;
                }

                if (StructureManager.tryGetInfo(hit.transform, out byte x1, out byte y1, out ushort index1, out StructureRegion region1))
                {
                    var structure = region1.structures[index1];

                    StructureManager.destroyStructure(region1, x1, y1, index1, structure.point);                   
                };
            }
            await UniTask.SwitchToThreadPool();
        }

        public Task<bool> RemoveFromDestroyerMode(UnturnedUser user)
        {
            if (_playerIds.Contains(user.Id))
            {
                _playerIds.Remove(user.Id);
                return Task.FromResult(result: true);
            }

            return Task.FromResult(result: false);
        }
    }
}
