using System.Runtime.CompilerServices;
using Arch.Core;
using Microsoft.Xna.Framework;
using RatasInit.Components;

namespace RatasInit.Systems;

public static class NewtonianPhysicSystem
{
    private static readonly QueryDescription _forceQuery = new QueryDescription()
        .WithAll<CPosition, CVelocity, CForce, CMass>();

    private static readonly QueryDescription _velocityQuery = new QueryDescription()
        .WithAll<CPosition, CVelocity>()
        .WithNone<CForce>();

    public static void Update(World world, float dt)
    {
        var forceChunks = world.Query(in _forceQuery);
        foreach (ref var chunk in forceChunks.GetChunkIterator())
        {
            var references = chunk.GetFirst<CPosition, CVelocity, CForce, CMass>();

            foreach (int i in chunk)
            {
                ref var pos = ref Unsafe.Add(ref references.t0, i);
                ref var vel = ref Unsafe.Add(ref references.t1, i);
                ref var force = ref Unsafe.Add(ref references.t2, i);
                ref var mass = ref Unsafe.Add(ref references.t3, i);

                Vector2 currentAccel = mass.InvMass * force.Force;
                Vector2 deltaAccel = currentAccel - force.PreviousAcceleration;

                pos.Pos += dt * (vel.Vel + dt * (currentAccel / 2f + deltaAccel / 6f)   );
                vel.Vel += dt * (currentAccel + (deltaAccel * 0.5f));

                force.PreviousAcceleration = currentAccel;
                force.Force = Vector2.Zero;
            }
        }

        var velocityChunks = world.Query(in _velocityQuery);
        foreach (ref var chunk in velocityChunks.GetChunkIterator())
        {
            var references = chunk.GetFirst<CPosition, CVelocity>();

            foreach (int i in chunk)
            {
                ref var pos = ref Unsafe.Add(ref references.t0, i);
                ref var vel = ref Unsafe.Add(ref references.t1, i);

                pos.Pos += vel.Vel * dt;
            }
        }
    }
}