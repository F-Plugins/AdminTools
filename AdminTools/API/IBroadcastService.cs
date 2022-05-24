using Cysharp.Threading.Tasks;
using OpenMod.API.Ioc;

namespace AdminTools.API
{
    [Service]
    public interface IBroadcastService
    {
        UniTask StartBroadcastAsync();
    }
}
