using Celeste.Mod.NerdHelper.Entities;
using NerdHelper.Code.Components;

namespace Celeste.Mod.NerdHelper.Module {
    public class NerdHelperModule : EverestModule {
        public override void Load() {
            Logger.SetLogLevel("NerdHelper", LogLevel.Info);

            DashThroughSpikes.Load();
            JumpListener.Load();
        }

        public override void Unload() {
            DashThroughSpikes.Unload();
            JumpListener.Unload();
        }
    }
}