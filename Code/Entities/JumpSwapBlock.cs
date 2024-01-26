using System;
using System.Reflection;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using NerdHelper.Code.Components;

namespace Celeste.Mod.NerdHelper.Entities;

[CustomEntity("NerdHelper/JumpSwapBlock")]
public class JumpSwapBlock : SwapBlock {
    private JumpListener.JumpTypes jumpTypes;

    public JumpSwapBlock(EntityData data, Vector2 offset) : base(data, offset) {
        foreach (JumpListener.JumpTypes type in Enum.GetValues(typeof(JumpListener.JumpTypes))) {
            if (data.Bool(type.ToString().ToLower(), true)) {
                jumpTypes |= type;
            }
        }
        Remove(Get<DashListener>());
        Add(new JumpListener(OnJump));
    }

    private static readonly MethodInfo swapBlockOnDash = typeof(SwapBlock).GetMethod("OnDash", BindingFlags.Instance | BindingFlags.NonPublic);

    private void OnJump(JumpListener.JumpTypes type) {
        if (jumpTypes.HasFlag(type)) {
            swapBlockOnDash.Invoke(this, new object[] {Vector2.Zero});
        }
    }
}