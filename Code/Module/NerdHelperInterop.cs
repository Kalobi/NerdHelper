using Celeste.Mod.NerdHelper.Utils;
using Microsoft.Xna.Framework;
using Monocle;
using MonoMod.ModInterop;

namespace Celeste.Mod.NerdHelper.Module;

[ModExportName("NerdHelper")]
public class NerdHelperInterop {
    public void CutsceneShake(float time) {
        (Engine.Scene as Level)?.CutsceneShake(time);
    }

    public void CutsceneDirectionalShake(Vector2 dir, float time) {
        (Engine.Scene as Level)?.CutsceneDirectionalShake(dir, time);
    }
}