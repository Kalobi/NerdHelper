using Celeste.Mod.Entities;
using Celeste.Mod.NerdHelper.Utils;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.NerdHelper.Triggers;

[CustomEntity("NerdHelper/CutsceneShakeTrigger")]
public class CutsceneShakeTrigger(EntityData data, Vector2 offset) : Trigger(data, offset) {
    private float time = data.Float("time");
    private Vector2 direction = new(data.Float("xDirection"), data.Float("yDirection"));

    public override void OnEnter(Player player) {
        (Scene as Level).CutsceneDirectionalShake(direction, time);
    }
}