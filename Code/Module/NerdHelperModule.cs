using NerdHelper.Code.Components;

namespace Celeste.Mod.NerdHelper.Module {
    public class NerdHelperModule : EverestModule {
        public override void Load() {
            Logger.SetLogLevel("NerdHelper", LogLevel.Info);
            
            JumpListener.Load();
        }

        public override void Unload() {
            JumpListener.Unload();
        }
    }
}