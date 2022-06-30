using System.Reflection;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.NerdHelper.Entities
{
    [CustomEntity(
        "NerdHelper/DashThroughSpikesUp = LoadUp",
        "NerdHelper/DashThroughSpikesDown = LoadDown",
        "NerdHelper/DashThroughSpikesLeft = LoadLeft",
        "NerdHelper/DashThroughSpikesRight = LoadRight"
    )]
    [TrackedAs(typeof(Spikes))]
    public class DashThroughSpikes : Spikes
    {

        public static void Load()
        {
            On.Celeste.Spikes.OnCollide += OnCollide;
        }

        public static void Unload()
        {
            On.Celeste.Spikes.OnCollide -= OnCollide;
        }

        private static void OnCollide(On.Celeste.Spikes.orig_OnCollide orig, Spikes self, Player player)
        {
            if (self is DashThroughSpikes spike)
            {
                if (((player.StateMachine.State == Player.StDash
                      || (player.DashAttacking && player.StateMachine.State != Player.StRedDash)
                      || player.StateMachine.State == Player.StDreamDash
                      || (player.StateMachine.State == Player.StRedDash && spike.red))
                     && (!spike.zeroSpeedOnly || player.Speed.Equals(Vector2.Zero))
                     && spike.CheckDir(player.DashDir))
                    ^ spike.invert)
                {
                    return;
                }
            }
            orig(self, player);
        }

        private bool CheckDir(Vector2 dashDir)
        {
            if (Direction == Directions.Up || Direction == Directions.Down)
            {
                return (dashDir.X != 0 && dashDir.Y == 0 && along)
                       || (dashDir.X == 0 && dashDir.Y != 0 && into)
                       || (dashDir.X != 0 && dashDir.Y != 0 && diag)
                       || dashDir.Equals(Vector2.Zero);
            }
            return (dashDir.X != 0 && dashDir.Y == 0 && into)
                   || (dashDir.X == 0 && dashDir.Y != 0 && along)
                   || (dashDir.X != 0 && dashDir.Y != 0 && diag)
                   || dashDir.Equals(Vector2.Zero);
        }

        private bool red;
        private bool invert;
        private bool zeroSpeedOnly;
        private bool into;
        private bool along;
        private bool diag;

        public static Entity LoadUp(Level level, LevelData levelData, Vector2 offset, EntityData entityData) => new DashThroughSpikes(entityData, offset, Directions.Up);

        public static Entity LoadDown(Level level, LevelData levelData, Vector2 offset, EntityData entityData) => new DashThroughSpikes(entityData, offset, Directions.Down);

        public static Entity LoadLeft(Level level, LevelData levelData, Vector2 offset, EntityData entityData) => new DashThroughSpikes(entityData, offset, Directions.Left);

        public static Entity LoadRight(Level level, LevelData levelData, Vector2 offset, EntityData entityData) => new DashThroughSpikes(entityData, offset, Directions.Right);

        private readonly FieldInfo spikesOverrideType = typeof(Spikes).GetField("overrideType", BindingFlags.Instance | BindingFlags.NonPublic);
        public DashThroughSpikes(EntityData data, Vector2 offset, Directions dir) : base(data, offset, dir)
        {
            red = data.Bool("red_boosters_count_as_dash", true);
            invert = data.Bool("invert", false);
            zeroSpeedOnly = data.Bool("zero_speed_only", false);
            into = data.Bool("into", true);
            along = data.Bool("along", true);
            diag = data.Bool("diag", true);
            string texturePath = data.Attr("type", "Kalobi/NerdHelper/dashthroughspike");
            if (texturePath.Length == 0)
            {
                texturePath = "Kalobi/NerdHelper/dashthroughspike";
            }
            spikesOverrideType.SetValue(this, texturePath);
        }
    }
}
