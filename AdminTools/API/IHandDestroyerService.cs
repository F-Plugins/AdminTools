using OpenMod.API.Ioc;
using OpenMod.Unturned.Players;
using OpenMod.Unturned.Users;
using System.Threading.Tasks;

namespace AdminTools.API
{
    [Service]
    public interface IHandDestroyerService
    {
        bool IsOnDestroyerMode(UnturnedUser user);

        Task<bool> AddToDestroyerMode(UnturnedUser user);

        Task<bool> RemoveFromDestroyerMode(UnturnedUser user);

        Task Destroy(UnturnedPlayer user);
    }
}
