using Celeste.Mod.NerdHelper.Entities;

namespace Celeste.Mod.NerdHelper.Module
{
    public class NerdHelperModule : EverestModule

    {
        public override void Load()
        {
            Logger.SetLogLevel("NerdHelper", LogLevel.Info);

            DashThroughSpikes.Load();
        }

        public override void Unload()
        {
            DashThroughSpikes.Unload();
        }

        
    }
}
