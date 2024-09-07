using System;
using Celeste.Mod.Entities;
using Celeste.Mod.NerdHelper.Module;
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
    private const string defaultTexture = "Kalobi/NerdHelper/dashthroughspike";
    
    private readonly Action<Player> origOnCollide;

    private void OnPlayer(Player player) {
        if (((player.StateMachine.State == Player.StDash
              || player.DashAttacking && player.StateMachine.State != Player.StRedDash
              || player.StateMachine.State == Player.StDreamDash
              || (NerdHelperModule.CommunalHelperInterop.GetDreamTunnelDashState != null
                  && player.StateMachine.State == NerdHelperModule.CommunalHelperInterop.GetDreamTunnelDashState())
              || (NerdHelperModule.CommunalHelperInterop.IsSeekerDashAttacking?.Invoke() ?? false)
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

    public DashThroughSpikes(EntityData data, Vector2 offset, Directions dir) : base(data, offset, dir) {
        red = data.Bool("red_boosters_count_as_dash", true);
        invert = data.Bool("invert", false);
        zeroSpeedOnly = data.Bool("zero_speed_only", false);
        into = data.Bool("into", true);
        along = data.Bool("along", true);
        diag = data.Bool("diag", true);
        
        string texturePath = data.Attr("type", defaultTexture);
        if (texturePath.Length == 0) {
            texturePath = defaultTexture;
        }
        overrideType = texturePath;
        
        origOnCollide = pc.OnCollide;
        pc.OnCollide = OnPlayer;
    }
}