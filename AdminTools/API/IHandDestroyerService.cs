using OpenMod.API.Ioc;
using OpenMod.Unturned.Players;
using OpenMod.Unturned.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminTools.API
{
    [Service]
    public interface IHandDestroyerService
    {
        Task<bool> AddToDestroyerMode(UnturnedUser user);

        Task<bool> RemoveFromDestroyerMode(UnturnedUser user);

        Task Destoy(UnturnedPlayer user);
    }
}
