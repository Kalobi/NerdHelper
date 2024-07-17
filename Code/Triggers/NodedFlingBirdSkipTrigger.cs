using System;
using Celeste.Mod.Entities;
using Celeste.Mod.NerdHelper.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.NerdHelper.Triggers;

[CustomEntity("NerdHelper/NodedFlingBirdSkipTrigger")]
public class NodedFlingBirdSkipTrigger(EntityData data, Vector2 offset) : Trigger(data, offset) {

    private int birdID = data.Int("birdID", -1);
    private int skipToNode = data.Int("skipToNode");
    private bool takeDirectRoute = data.Bool("takeDirectRoute", true);
    private NodedFlingBird bird;

    public override void Awake(Scene scene) {
        base.Awake(scene);
        foreach (NodedFlingBird b in scene.Tracker.GetEntities<NodedFlingBird>()) {
            if (b.eid.ID == birdID) {
                bird = b;
                return;
            }
        }
        throw new ArgumentException($"Noded Fling Bird Skip Trigger at {Position} could not find Noded Fling Bird with id {birdID}");
    }

    public override void OnEnter(Player player) {
        bird.TriggerSkipToNode(skipToNode, takeDirectRoute);
    }
}