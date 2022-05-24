using System;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OpenMod.API.Users;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Commands;
using OpenMod.Unturned.Users;
using SDG.Unturned;

namespace AdminTools.Commands
{
    [Command("maxskills")]
    [CommandDescription("A command to unlock all skills to max")]
    [CommandSyntax("[player|all]")]
    [CommandActor(typeof(UnturnedUser))]
    public class MaxSkillsCommand : UnturnedCommand
    {
        private readonly IStringLocalizer _localizer;

        public MaxSkillsCommand(IServiceProvider serviceProvider, IStringLocalizer stringLocalizer) : base(serviceProvider)
        {
            _localizer = stringLocalizer;
        }

        protected override async UniTask OnExecuteAsync()
        {
            UnturnedUser user = (UnturnedUser)Context.Actor;
            await UniTask.SwitchToMainThread();
            switch (Context.Parameters.Length)
            {
                case 0:
                    user.Player.Player.skills.ServerUnlockAllSkills();
                    await user.PrintMessageAsync(_localizer["Commands:MaxSkills:Success", new
                    {
                        Name = "your"
                    }]);
                    break;
                case 1:
                    string player = await Context.Parameters.GetAsync<string>(0);
                    if (player == "all")
                    {
                        foreach (SteamPlayer client in Provider.clients)
                        {
                            client.player.skills.ServerUnlockAllSkills();
                        }
                        await user.PrintMessageAsync(_localizer["Commands:MaxSkills:Success", new
                        {
                            Name = "all players"
                        }]);
                    }
                    else if (Context.Parameters.TryGet<IUser>(0, out IUser? iuser))
                    {
                        (iuser as UnturnedUser)!.Player.Player.skills.ServerUnlockAllSkills();
                        await iuser.PrintMessageAsync(_localizer["Commands:MaxSkills:Success", new
                        {
                            Name = "your"
                        }]);
                    }
                    else
                    {
                        throw new CommandWrongUsageException(Context);
                    }
                    break;
                default:
                    throw new CommandWrongUsageException(Context);
            }
            await UniTask.SwitchToThreadPool();
        }
    }
}
