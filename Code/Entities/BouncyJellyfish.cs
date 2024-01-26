using System.Reflection;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.NerdHelper.Entities;

[CustomEntity("NerdHelper/BouncyJellyfish")]
public class BouncyJellyfish : Glider {
    private float cannotHitTimer;

    public BouncyJellyfish(EntityData data, Vector2 offset) : base(data, offset) {
        Add(new PlayerCollider(OnPlayer, new Hitbox(30f, 8f, -14f, -19f)));
    }

    public override void Update() {
        base.Update();
        cannotHitTimer -= Engine.DeltaTime;
    }

    private static readonly FieldInfo gliderSprite = typeof(Glider).GetField("sprite", BindingFlags.Instance | BindingFlags.NonPublic);

    private void OnPlayer(Player player) {
        if (cannotHitTimer <= 0 && player.Speed.Y >= 0) {
            player.Bounce(Top);
            Speed += new Vector2(0, 50f);
            (gliderSprite.GetValue(this) as Sprite).Scale = new Vector2(1.2f, 0.8f);
            Audio.Play("event:/new_content/game/10_farewell/puffer_boop", Position);
        }
        cannotHitTimer = 0.1f;
    }
}