using OpenMod.API.Ioc;
using OpenMod.Unturned.Users;
using System.Threading.Tasks;

namespace AdminTools.API
{
    [Service]
    public interface IFlyService
    {
        Task<bool> AddToFlyMode(UnturnedUser user);

        Task<bool> RemoveFromFlyMode(UnturnedUser user);
    }
}
