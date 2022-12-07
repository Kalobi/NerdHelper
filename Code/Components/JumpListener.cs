using System;
using Monocle;

namespace NerdHelper.Code.Components {
    [Tracked]
    public class JumpListener : Component {
        public static void Load() {
            On.Celeste.Player.Jump += PlayerOnJump;
            On.Celeste.Player.WallJump += PlayerOnWallJump;
            On.Celeste.Player.SuperWallJump += PlayerOnSuperWallJump;
            On.Celeste.Player.SuperJump += PlayerOnSuperJump;
        }

        public static void Unload() {
            On.Celeste.Player.Jump -= PlayerOnJump;
            On.Celeste.Player.WallJump -= PlayerOnWallJump;
            On.Celeste.Player.SuperWallJump -= PlayerOnSuperWallJump;
            On.Celeste.Player.SuperJump -= PlayerOnSuperJump;
        }

        private static void PlayerOnSuperJump(On.Celeste.Player.orig_SuperJump orig, Celeste.Player self) {
            var ducked = self.Ducking;
            orig(self);
            InvokeJumpListeners(self.Scene, ducked ? JumpTypes.HYPER : JumpTypes.SUPER);
        }

        private static void PlayerOnSuperWallJump(On.Celeste.Player.orig_SuperWallJump orig, Celeste.Player self, int dir) {
            orig(self, dir);
            InvokeJumpListeners(self.Scene, JumpTypes.WALLBOUNCE);
        }

        private static void PlayerOnWallJump(On.Celeste.Player.orig_WallJump orig, Celeste.Player self, int dir) {
            orig(self, dir);
            InvokeJumpListeners(self.Scene, JumpTypes.WALL_JUMP);
        }

        private static void PlayerOnJump(On.Celeste.Player.orig_Jump orig, Celeste.Player self, bool particles, bool playsfx) {
            orig(self, particles, playsfx);
            InvokeJumpListeners(self.Scene, JumpTypes.GROUND_JUMP);
        }

        [Flags]
        public enum JumpTypes {
            GROUND_JUMP = 1,
            WALL_JUMP = 2,
            WALLBOUNCE = 4,
            SUPER = 8,
            HYPER = 16
        }

        public Action<JumpTypes> OnJump;

        public JumpListener(Action<JumpTypes> onJump) : base(false, false) {
            OnJump = onJump;
        }

        public static void InvokeJumpListeners(Scene scene, JumpTypes type) {
            foreach (JumpListener jumpListener in scene.Tracker.GetComponents<JumpListener>()) {
                jumpListener.OnJump?.Invoke(type);
            }
        }
    }
}