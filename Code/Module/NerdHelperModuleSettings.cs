using Celeste.Mod.NerdHelper.Utils;

namespace Celeste.Mod.NerdHelper.Module;

public class NerdHelperModuleSettings : EverestModuleSettings {
    [SettingName("NerdHelperCutsceneScreenshakeSetting")]
    [SettingSubText("NerdHelperCutsceneScreenshakeDescription")]
    public CutsceneScreenshake.CutsceneScreenshakeModes CutsceneScreenshake { get; set; }
}