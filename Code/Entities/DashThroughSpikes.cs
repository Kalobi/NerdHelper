using System;
using System.Reflection;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.NerdHelper.Entities;

[CustomEntity(
    "NerdHelper/DashThroughSpikesUp = LoadUp",
    "NerdHelper/DashThroughSpikesDown = LoadDown",
    "NerdHelper/DashThroughSpikesLeft = LoadLeft",
    "NerdHelper/DashThroughSpikesRight = LoadRight"
)]
[TrackedAs(typeof(Spikes))]
public class DashThroughSpikes : Spikes {
    private readonly Action<Player> origOnCollide;

    private void OnPlayer(Player player) {
        if (((player.StateMachine.State == Player.StDash
              || player.DashAttacking && player.StateMachine.State != Player.StRedDash
              || player.StateMachine.State == Player.StDreamDash
              || player.StateMachine.State == Player.StRedDash && red)
             && (!zeroSpeedOnly || player.Speed.Equals(Vector2.Zero))
             && CheckDir(player.DashDir))
            ^ invert) {
            return;
        }
        origOnCollide(player);
    }

    private bool CheckDir(Vector2 dashDir) {
        if (Direction is Directions.Up or Directions.Down) {
            return dashDir.X != 0 && dashDir.Y == 0 && along
                   || dashDir.X == 0 && dashDir.Y != 0 && into
                   || dashDir.X != 0 && dashDir.Y != 0 && diag
                   || dashDir.Equals(Vector2.Zero);
        }
        return dashDir.X != 0 && dashDir.Y == 0 && into
               || dashDir.X == 0 && dashDir.Y != 0 && along
               || dashDir.X != 0 && dashDir.Y != 0 && diag
               || dashDir.Equals(Vector2.Zero);
    }

    private bool red;
    private bool invert;
    private bool zeroSpeedOnly;
    private bool into;
    private bool along;
    private bool diag;

    public static Entity LoadUp(Level level, LevelData levelData, Vector2 offset, EntityData entityData) {
        return new DashThroughSpikes(entityData, offset, Directions.Up);
    }

    public static Entity LoadDown(Level level, LevelData levelData, Vector2 offset, EntityData entityData) {
        return new DashThroughSpikes(entityData, offset, Directions.Down);
    }

    public static Entity LoadLeft(Level level, LevelData levelData, Vector2 offset, EntityData entityData) {
        return new DashThroughSpikes(entityData, offset, Directions.Left);
    }

    public static Entity LoadRight(Level level, LevelData levelData, Vector2 offset, EntityData entityData) {
        return new DashThroughSpikes(entityData, offset, Directions.Right);
    }

    private static readonly FieldInfo spikesOverrideType = typeof(Spikes).GetField("overrideType", BindingFlags.Instance | BindingFlags.NonPublic);

    public DashThroughSpikes(EntityData data, Vector2 offset, Directions dir) : base(data, offset, dir) {
        red = data.Bool("red_boosters_count_as_dash", true);
        invert = data.Bool("invert", false);
        zeroSpeedOnly = data.Bool("zero_speed_only", false);
        into = data.Bool("into", true);
        along = data.Bool("along", true);
        diag = data.Bool("diag", true);
        string texturePath = data.Attr("type", "Kalobi/NerdHelper/dashthroughspike");
        if (texturePath.Length == 0) {
            texturePath = "Kalobi/NerdHelper/dashthroughspike";
        }
        spikesOverrideType.SetValue(this, texturePath);
        var collider = Get<PlayerCollider>();
        origOnCollide = collider.OnCollide;
        collider.OnCollide = OnPlayer;
    }
}