using Microsoft.Xna.Framework;

namespace RatasInit.Components;

public record struct CPosition(Vector2 Pos);
public record struct CVelocity(Vector2 Vel);
public record struct CForce(Vector2 Force, Vector2 PreviousAcceleration);
public record struct CMass
{
    private float _mass;
    public float Mass
    {
        readonly get => _mass;
        set
        {
            _mass = value;
            InvMass = value != 0 ? 1f / value : 0f;
        }
    }

    public float InvMass { get; private set; }

    public CMass(float mass)
    {
        _mass = mass;
        InvMass = mass != 0 ? 1f / mass : 0f;
    }
}

public record struct CGravitySource();

public record struct CRotation(float Rot);
public record struct CAngularVelocity(float AngVel);
public record struct CTorque(float Torq);
public record struct CInertia(float InvInertia);