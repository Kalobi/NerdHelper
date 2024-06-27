using System;
using Celeste.Mod.NerdHelper.Utils;
using MonoMod.ModInterop;
using NerdHelper.Code.Components;

namespace Celeste.Mod.NerdHelper.Module;

public class NerdHelperModule : EverestModule {
    
    public static NerdHelperModule Instance { get; private set; }
    public override Type SettingsType => typeof(NerdHelperModuleSettings);
    public static NerdHelperModuleSettings Settings => (NerdHelperModuleSettings) Instance._Settings;

    public NerdHelperModule() {
        Instance = this;
    }
    public override void Load() {
        Logger.SetLogLevel("NerdHelper", LogLevel.Info);
            
        JumpListener.Load();
        CutsceneScreenshake.Load();
        
        typeof(NerdHelperInterop).ModInterop();
    }

    public override void Unload() {
        JumpListener.Unload();
        CutsceneScreenshake.Unload();
    }
}