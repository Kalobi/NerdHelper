using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.NerdHelper.Entities;

[CustomEntity("NerdHelper/NodedFlingBird")]
public class NodedFlingBird : Entity {
    
    private enum States {
        Wait,
        Fling,
        Move,
        WaitForLightningClear,
        Leaving
    }
    
    private static readonly Color trailColor = Calc.HexToColor("639bff");
    private Sprite sprite;
    private States state;
    private Vector2 flingSpeed;
    private Vector2 flingTargetSpeed;
    private float flingAccel;
    private Vector2 spriteOffset = new Vector2(0f, 8f);
    private SoundSource moveSfx;
    
    private List<Vector2> positions;
    private HashSet<int> leftFlingingNodes;
    private int nodeIndex;
    private bool doCameraZoom;
    private bool waitForLightning;
    private Facings facing;
    
    public NodedFlingBird(EntityData data, Vector2 offset) : base(data.Position + offset) {
        positions = [..data.NodesWithPosition(offset)];
        leftFlingingNodes = [];
        string rawLeftNodes = data.Attr("leftFlingingNodes");
        if (!string.IsNullOrWhiteSpace(rawLeftNodes)) {
            foreach (string s in data.Attr("leftFlingingNodes").Split(',')) {
                if (!int.TryParse(s.Trim(), out int i) || i < 0 || i >= positions.Count) {
                    throw new ArgumentException($"{s} is not a valid node for Noded Fling Bird at {Position}");
                }
                leftFlingingNodes.Add(i);
            }
        }
        doCameraZoom = data.Bool("doCameraZoom", true);
        waitForLightning = data.Bool("waitForLightning");
        
        Depth = -1;
        Add(sprite = GFX.SpriteBank.Create("bird"));
        sprite.Play("hover");
        // waiting bird faces in opposite direction of throw
        facing = leftFlingingNodes.Contains(nodeIndex) ? Facings.Right : Facings.Left;
        sprite.Scale.X = (float) facing;
        sprite.Position = spriteOffset;
        sprite.OnFrameChange = _ => BirdNPC.FlapSfxCheck(sprite);
        Collider = new Circle(16f);
        Add(new PlayerCollider(OnPlayer));
        Add(moveSfx = new SoundSource());
        Add(new TransitionListener { OnOut = t => sprite.Color = Color.White * (1f - Calc.Map(t, 0f, 0.4f))});
    }

    public override void Update() {
        base.Update();
        if (state != States.Wait) {
            sprite.Position = Calc.Approach(sprite.Position, spriteOffset, 32f * Engine.DeltaTime);
        }
        switch (state) {
            case States.Wait:
                // bird is visually attracted to player 
                if (Scene.Tracker.GetEntity<Player>() is Player player) {
                    Vector2 toPlayer = player.Center - Position;
                    float pullStrength = Calc.ClampedMap(toPlayer.Length(), 16f, 64f, 12f, 0f);
                    Vector2 dir = toPlayer.SafeNormalize();
                    sprite.Position = Calc.Approach(sprite.Position, spriteOffset + dir * pullStrength, 32f * Engine.DeltaTime);
                }
                break;
            case States.Fling:
                if (flingAccel > 0f) {
                    flingSpeed = Calc.Approach(flingSpeed, flingTargetSpeed, flingAccel * Engine.DeltaTime);
                }
                Position += flingSpeed * Engine.DeltaTime;
                break;
            case States.WaitForLightningClear:
                if (!waitForLightning || X > (Scene as Level)!.Bounds.Right || Scene.Entities.FindFirst<Lightning>() == null) {
                    state = States.Leaving;
                    Add(new Coroutine(LeaveRoutine()));
                }
                break;
            case States.Move:
            case States.Leaving:
                break;
            default:
                throw new UnreachableException();
        }
        sprite.Scale.X = float.CopySign(sprite.Scale.X, (float) facing);
    }

    private void Skip() {
        state = States.Move;
        Add(new Coroutine(MoveRoutine()));
    }

    private void OnPlayer(Player player) {
        // taken from Player.DoFlingBird
        if (state == States.Wait && !player.Dead && player.StateMachine.State != Player.StFlingBird && player.StateMachine.State != StNodedFlingBird) {
            player.Get<NodedFlingBirdPlayerStateComponent>().CurrentBird = this;
            player.StateMachine.State = StNodedFlingBird;
            player.Drop();
            // taken from FlingBird.OnPlayer
            flingSpeed = new Vector2(player.Speed.X * 0.4f, 120f);
            flingTargetSpeed = Vector2.Zero;
            flingAccel = 1000f;
            player.Speed = Vector2.Zero;
            state = States.Fling;
            Add(new Coroutine(DoFlingRoutine(player)));
            Audio.Play(SFX.game_10_bird_throw, Center);
        }
    }

    private IEnumerator DoFlingRoutine(Player player) {
        Level level = (Scene as Level)!;
        if (doCameraZoom) {
            Vector2 cameraPosition = level.Camera.Position;
            Vector2 screenSpaceFocusPoint = player.Position - cameraPosition;
            screenSpaceFocusPoint.X = Calc.Clamp(screenSpaceFocusPoint.X, 145f, 215f);
            screenSpaceFocusPoint.Y = Calc.Clamp(screenSpaceFocusPoint.Y, 85f, 95f);
            Add(new Coroutine(level.ZoomTo(screenSpaceFocusPoint, 1.1f, 0.2f)));
        }
        Engine.TimeRate = 0.8f;
        Input.Rumble(RumbleStrength.Light, RumbleLength.Medium);
        while (flingSpeed != Vector2.Zero)
        {
            yield return null;
        }
        sprite.Play("throw");
        facing = leftFlingingNodes.Contains(nodeIndex) ? Facings.Left : Facings.Right;
        flingSpeed = new Vector2(-140f * (float)facing, 140f);
        flingTargetSpeed = Vector2.Zero;
        flingAccel = 1400f;
        yield return 0.1f;
        Celeste.Freeze(0.05f);
        Vector2 directionalFlingSpeed = FlingBird.FlingSpeed;
        directionalFlingSpeed.X *= (float) facing;
        flingTargetSpeed = directionalFlingSpeed;
        flingAccel = 6000f;
        yield return 0.1f;
        Input.Rumble(RumbleStrength.Strong, RumbleLength.Medium);
        Engine.TimeRate = 1f;
        level.Shake();
        if (doCameraZoom) {
            Add(new Coroutine(level.ZoomBack(0.1f)));
        }
        // from Player.FinishFlingBird
        player.StateMachine.State = Player.StNormal;
        player.AutoJump = true;
        player.forceMoveX = (int) facing;
        player.forceMoveXTimer = 0.2f;
        player.Speed = directionalFlingSpeed;
        player.varJumpTimer = 0.2f;
        player.varJumpSpeed = player.Speed.Y;
        player.launched = true;
        
        flingTargetSpeed = Vector2.Zero;
        flingAccel = 4000f;
        yield return 0.3f;
        Add(new Coroutine(MoveRoutine()));
    }

    private IEnumerator MoveRoutine() {
        state = States.Move;
        sprite.Play("fly");
        moveSfx.Play(SFX.game_10_bird_relocate);
        nodeIndex++;
        bool atEnding = nodeIndex >= positions.Count;
        if (!atEnding) {
            facing = positions[nodeIndex].X >= positions[nodeIndex - 1].X ? Facings.Right : Facings.Left;
            yield return MoveOnCurve(Position, (Position + positions[nodeIndex]) / 2, positions[nodeIndex]);
        }
        sprite.Rotation = 0f;
        sprite.Scale = Vector2.One;
        if (atEnding) {
            sprite.Play("hoverStressed");
            state = States.WaitForLightningClear;
            yield break;
        }
        sprite.Play("hover");
        // waiting bird faces in opposite direction of throw
        facing = leftFlingingNodes.Contains(nodeIndex) ? Facings.Right : Facings.Left;
        state = States.Wait;
    }

    private IEnumerator LeaveRoutine() {
        sprite.Play("fly");
        Vector2 target = new Vector2((facing == Facings.Right ? (Scene as Level)!.Bounds.Right : (Scene as Level)!.Bounds.Left) + 32, Y);
        yield return MoveOnCurve(Position, (Position + target) * 0.5f - Vector2.UnitY * 12f, target);
        RemoveSelf();
    }

    private IEnumerator MoveOnCurve(Vector2 from, Vector2 anchor, Vector2 to) {
        SimpleCurve curve = new SimpleCurve(from, to, anchor);
        float duration = curve.GetLengthParametric(32) / 500f;
        Vector2 was = from;
        for (float t = 0.016f; t <= 1f; t += Engine.DeltaTime / duration)
        {
            Position = curve.GetPoint(t).Floor();
            /*
            sprite.Rotation = Calc.Angle(curve.GetPoint(Math.Max(0f, t - 0.05f)), curve.GetPoint(Math.Min(1f, t + 0.05f)));
            sprite.Scale.X = 1.25f;
            sprite.Scale.Y = 0.7f;
            */
            if ((was - Position).Length() > 32f)
            {
                TrailManager.Add(this, trailColor, 1f);
                was = Position;
            }
            yield return null;
        }
        Position = to;
    }

    #region Player State
    
    internal static void Load() {
        Everest.Events.Player.OnRegisterStates += RegisterNodedFlingBirdState;
        On.Celeste.Player.DoFlingBird += PlayerOnDoFlingBird;
    }

    internal static void Unload() {
        Everest.Events.Player.OnRegisterStates -= RegisterNodedFlingBirdState;
        On.Celeste.Player.DoFlingBird -= PlayerOnDoFlingBird;
    }
    
    private static int StNodedFlingBird;

    private static bool PlayerOnDoFlingBird(On.Celeste.Player.orig_DoFlingBird orig, Player self, FlingBird bird) {
        return self.StateMachine.State != StNodedFlingBird && orig(self, bird);
    }
    
    private class NodedFlingBirdPlayerStateComponent() : Component(false, false) {
        public NodedFlingBird CurrentBird;
    }

    private static void RegisterNodedFlingBirdState(Player player) {
        StNodedFlingBird = player.AddState("NerdHelper_StNodedFlingBird", NodedFlingBirdUpdate, null, NodedFlingBirdBegin);
        player.Add(new NodedFlingBirdPlayerStateComponent());
    }

    private static void NodedFlingBirdBegin(Player player) {
        player.RefillDash();
        player.RefillStamina();
    }

    private static int NodedFlingBirdUpdate(Player player) {
        NodedFlingBird bird = player.Get<NodedFlingBirdPlayerStateComponent>().CurrentBird;
        player.MoveTowardsX(bird.X, 250f * Engine.DeltaTime);
        player.MoveTowardsY(bird.Y + 8f + player.Collider.Height, 250f * Engine.DeltaTime);
        return StNodedFlingBird;
    }

    #endregion
}