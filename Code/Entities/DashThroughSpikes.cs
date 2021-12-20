using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;
using MonoMod.Utils;

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
                Logger.Log(LogLevel.Warn,"NerdHelper/DashThroughSpikes", spike.zeroSpeedOnly.ToString());
                Logger.Log(LogLevel.Warn, "NerdHelper/DashThroughSpikes", "cond: " + (((player.StateMachine.State == Player.StDash
                                                                          || player.StateMachine.State == Player.StDreamDash
                                                                          || (player.StateMachine.State == Player.StRedDash && spike.red))
                                                                         && (!spike.zeroSpeedOnly || player.Speed.Equals(Vector2.Zero)))
                                          ).ToString());
                if (((player.StateMachine.State == Player.StDash
                    || player.StateMachine.State == Player.StDreamDash
                    || (player.StateMachine.State == Player.StRedDash && spike.red))
                    /*&& (!spike.zeroSpeedOnly || player.Speed.Equals(Vector2.Zero))*/)
                    /*^ spike.invert*/)
                {
                    return;
                }
            }
            orig(self, player);
        }

        private bool red;
        private bool invert;
        private bool zeroSpeedOnly;

        public static Entity LoadUp(Level level, LevelData levelData, Vector2 offset, EntityData entityData) => new DashThroughSpikes(entityData, offset, Directions.Up);

        public static Entity LoadDown(Level level, LevelData levelData, Vector2 offset, EntityData entityData) => new DashThroughSpikes(entityData, offset, Directions.Down);

        public static Entity LoadLeft(Level level, LevelData levelData, Vector2 offset, EntityData entityData) => new DashThroughSpikes(entityData, offset, Directions.Left);

        public static Entity LoadRight(Level level, LevelData levelData, Vector2 offset, EntityData entityData) => new DashThroughSpikes(entityData, offset, Directions.Right);

        public DashThroughSpikes(EntityData data, Vector2 offset, Directions dir) : base(data, offset, dir)
        {
            red = data.Bool("red_boosters_count_as_dash", true);
            invert = data.Bool("invert", false);
            zeroSpeedOnly = data.Bool("zero_speed_only", false);
            string texturePath = data.Attr("type", "Kalobi/NerdHelper/dashthroughspike");
            if (texturePath.Length == 0)
            {
                texturePath = "Kalobi/NerdHelper/dashthroughspike";
            }
            new DynamicData(typeof(Spikes),this).Set("overrideType", texturePath);
        }
    }
}
