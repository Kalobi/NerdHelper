using System;
using Celeste.Mod.NerdHelper.Entities;
using Celeste.Mod.NerdHelper.Utils;
using MonoMod.ModInterop;
using NerdHelper.Code.Components;

namespace Celeste.Mod.NerdHelper.Module;

public class NerdHelperModule : EverestModule {
    
    public static NerdHelperModule Instance { get; private set; }
    public override Type SettingsType => typeof(NerdHelperModuleSettings);
    public static NerdHelperModuleSettings Settings => (NerdHelperModuleSettings) Instance._Settings;

    [ModImportName("CommunalHelper.DashStates")]
    internal class CommunalHelperInterop {
        public static Func<int> GetDreamTunnelDashState;
        public static Func<bool> IsSeekerDashAttacking;
    }

    public NerdHelperModule() {
        Instance = this;
    }
    public override void Load() {
        Logger.SetLogLevel("NerdHelper", LogLevel.Info);
            
        JumpListener.Load();
        CutsceneScreenshake.Load();
        NodedFlingBird.Load();
        
        typeof(NerdHelperInterop).ModInterop();
        typeof(CommunalHelperInterop).ModInterop();
    }

    public override void Unload() {
        JumpListener.Unload();
        CutsceneScreenshake.Unload();
        NodedFlingBird.Unload();
    }
}