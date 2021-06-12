using Cysharp.Threading.Tasks;
using OpenMod.API.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminTools.API
{
    [Service]
    public interface IBroadcastService
    {
        UniTask StartBroadcastAsync();
    }
}
