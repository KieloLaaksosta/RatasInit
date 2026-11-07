using Arch.Core;
using Arch.Core.Extensions;
using Microsoft.Xna.Framework;
using RatasInit.Components;
using RatasInit.Managers;

namespace RatasInit.Objects;

public static class Player
{
    public static void Update(Entity player, float dt)
    {
        HandleForwardMovement(player);
        HandleRotation(player);
    }

    private static void HandleForwardMovement(Entity player)
    {
        bool forwardHeld = InputManager.IsHeld(InputCode.Forward);
        bool backwardHeld = InputManager.IsHeld(InputCode.Backward);

        int dir = (forwardHeld ? 1 : 0) + (backwardHeld ? -1 : 0);

        if (dir == 0)
            return;

        ref var force = ref player.Get<CForce>();
        ref var rotation = ref player.Get<CRotation>();
        ref var cPlayer = ref player.Get<CPlayer>();

        float thrust = dir > 0 ? cPlayer.ThrustForward : -cPlayer.ThrustBackward;
        Vector2 forward = MathUtils.DirToVec2(rotation.Rot);

        force.Force += thrust * forward;
    }

    private static void HandleRotation(Entity player)
    {
        bool cwHeld = InputManager.IsHeld(InputCode.Clockwise);
        bool ccwHeld = InputManager.IsHeld(InputCode.CounterClockwise);

        float dir = (cwHeld ? -1f : 0f) + (ccwHeld ? 1f : 0f);

        if (dir == 0)
        {
            ref var angular = ref player.Get<CAngularVelocity>();
            angular.AngVel = 0;
            return;
        }

        ref var torque = ref player.Get<CTorque>();
        ref var cPlayer = ref player.Get<CPlayer>();

        torque.Torq += dir * cPlayer.Torque;
    }
}