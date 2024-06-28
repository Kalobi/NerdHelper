using Celeste.Mod.Entities;
using Celeste.Mod.NerdHelper.Utils;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.NerdHelper.Triggers;

[CustomEntity("NerdHelper/CutsceneShakeTrigger")]
public class CutsceneShakeTrigger(EntityData data, Vector2 offset, EntityID id) : Trigger(data, offset) {
    private float time = data.Float("time");
    private Vector2 direction = new(data.Float("xDirection"), data.Float("yDirection"));
    private TriggerFrequency freq = data.Enum<TriggerFrequency>("frequency");

    public override void OnEnter(Player player) {
        Level level = (Scene as Level)!;
        level.CutsceneDirectionalShake(direction, time);
        if (freq == TriggerFrequency.OnEachEntry) {
            return;
        }
        RemoveSelf();
        if (freq == TriggerFrequency.OncePerSession) {
            level.Session.DoNotLoad.Add(id);
        }
    }
}