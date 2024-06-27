using System;
using System.Diagnostics;
using Celeste.Mod.NerdHelper.Module;
using Microsoft.Xna.Framework;
using Monocle;
using MonoMod.Cil;

namespace Celeste.Mod.NerdHelper.Utils;

public static class CutsceneScreenshake {
    public enum CutsceneScreenshakeModes {
        MatchVanilla,
        AlwaysOff,
        Always50,
        Always100
    }

    [Tracked]
    private class CutsceneShakeMarker : Entity {
        public CutsceneShakeMarker(float time) {
            Add(new Alarm {
                Duration = time,
                Mode = Alarm.AlarmMode.Oneshot,
                OnComplete = RemoveSelf
            });
        }
    }
    
    public static void CutsceneShake(this Level level, float time = 0.3f) {
        if (NerdHelperModule.Settings.CutsceneScreenshake == CutsceneScreenshakeModes.MatchVanilla && Settings.Instance.ScreenShake == ScreenshakeAmount.Off
            || NerdHelperModule.Settings.CutsceneScreenshake == CutsceneScreenshakeModes.AlwaysOff) {
            return;
        }
        level.Add(new CutsceneShakeMarker(time));
        level.shakeDirection = Vector2.Zero;
        level.shakeTimer = Math.Max(level.shakeTimer, time);
    }
    
    public static void CutsceneDirectionalShake(this Level level, Vector2 dir, float time = 0.3f) {
        if (NerdHelperModule.Settings.CutsceneScreenshake == CutsceneScreenshakeModes.MatchVanilla && Settings.Instance.ScreenShake == ScreenshakeAmount.Off
            || NerdHelperModule.Settings.CutsceneScreenshake == CutsceneScreenshakeModes.AlwaysOff) {
            return;
        }
        level.Add(new CutsceneShakeMarker(time));
        level.shakeDirection = dir.SafeNormalize();
        level.lastDirectionalShake = 0;
        level.shakeTimer = Math.Max(level.shakeTimer, time);
    }

    private static ScreenshakeAmount ModScreenshakeStrength(ScreenshakeAmount orig, Level level) {
        if (level.Tracker.GetEntity<CutsceneShakeMarker>() == null) {
            return orig;
        }
        return NerdHelperModule.Settings.CutsceneScreenshake switch {
            CutsceneScreenshakeModes.MatchVanilla or CutsceneScreenshakeModes.AlwaysOff => orig,
            CutsceneScreenshakeModes.Always50 => ScreenshakeAmount.Half,
            CutsceneScreenshakeModes.Always100 => ScreenshakeAmount.On,
            _ => throw new UnreachableException()
        };
    }

    public static void Load() {
        IL.Celeste.Level.Update += LevelOnUpdate;
    }

    public static void Unload() {
        IL.Celeste.Level.Update -= LevelOnUpdate;
    }

    private static void LevelOnUpdate(ILContext il) {
        ILCursor cursor = new(il);
        while (cursor.TryGotoNext(MoveType.After, instr => instr.MatchLdfld<Settings>(nameof(Settings.ScreenShake)))) {
            cursor.EmitLdarg0();
            cursor.EmitDelegate(ModScreenshakeStrength);
        }
    }
}