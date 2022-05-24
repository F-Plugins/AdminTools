using AdminTools.API;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OpenMod.API.Ioc;
using OpenMod.Unturned.Players;
using OpenMod.Unturned.Users;
using SDG.Unturned;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace AdminTools.Services
{
    [PluginServiceImplementation(Lifetime = Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton)]
    public class HandDestroyerService : IHandDestroyerService
    {
        private List<string> _players;
        private readonly IStringLocalizer _localizer;

        public HandDestroyerService(IStringLocalizer stringLocalizer)
        {
            _players = new List<string>();
            _localizer = stringLocalizer;
        }

        public bool IsOnDestroyerMode(UnturnedUser user)
        {
            return _players.Contains(user.Id);
        }

        public Task<bool> AddToDestroyerMode(UnturnedUser user)
        {
            if (_players.Contains(user.Id))
            {
                return Task.FromResult(result: false);
            }
            _players.Add(user.Id);
            return Task.FromResult(result: true);

        }

        public Task<bool> RemoveFromDestroyerMode(UnturnedUser user)
        {
            if (_players.Contains(user.Id))
            {
                _players.Remove(user.Id);
                return Task.FromResult(result: true);
            }
            return Task.FromResult(result: false);
        }

        public async Task Destroy(UnturnedPlayer user)
        {
            if (!_players.Contains(user.SteamId.ToString())) return;

            await UniTask.SwitchToMainThread();

            Vector3 position = user.Player.look.aim.position;
            Vector3 forward = user.Player.look.aim.forward;
            int isItems = (int)ERayMask.VEHICLE | (int)ERayMask.BARRICADE | (int)ERayMask.STRUCTURE | (int)ERayMask.RESOURCE;
            if (Physics.Raycast(position, forward, out RaycastHit hit, 5f, isItems))
            {
                Transform transform = hit.transform;

                InteractableVehicle vehicle = transform.GetComponent<InteractableVehicle>();
                if (vehicle != null)
                {
                    VehicleManager.askVehicleDestroy(vehicle);
                    await user.PrintMessageAsync(_localizer["Commands:HandDestroyer:Destroy", new
                    {
                        Name = vehicle.asset.name.Replace("_", " ")
                    }]);
                    return;
                }

                InteractableDoorHinge door = transform.GetComponent<InteractableDoorHinge>();
                if (door != null) transform = transform.parent.parent;
                BarricadeDrop barricadeDrop = BarricadeManager.FindBarricadeByRootTransform(transform);
                if (barricadeDrop != null)
                {
                    BarricadeManager.tryGetRegion(barricadeDrop.model, out byte barricade_x, out byte barricade_y, out ushort barricade_plant, out _);
                    BarricadeManager.destroyBarricade(barricadeDrop, barricade_x, barricade_y, barricade_plant);
                    await user.PrintMessageAsync(_localizer["Commands:HandDestroyer:Destroy", new
                    {
                        Name = barricadeDrop.asset.name.Replace("_", " ")
                    }]);
                    return;
                }

                StructureDrop structureDrop = StructureManager.FindStructureByRootTransform(transform);
                if (structureDrop != null)
                {
                    StructureData structureData = structureDrop.GetServersideData();
                    Regions.tryGetCoordinate(structureData.point, out byte structure_x, out byte structure_y);
                    StructureManager.destroyStructure(structureDrop, structure_x, structure_y, Vector3.zero);
                    await user.PrintMessageAsync(_localizer["Commands:HandDestroyer:Destroy", new
                    {
                        Name = structureDrop.asset.name.Replace("_", " ")
                    }]);
                    return;
                }

                if (ResourceManager.tryGetRegion(transform, out byte x2, out byte y2, out ushort index2))
                {
                    ResourceManager.ServerSetResourceDead(x2, y2, index2, transform.position);
                }
            }

            await UniTask.SwitchToThreadPool();
        }
    }
}
