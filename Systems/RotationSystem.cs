using System.Runtime.CompilerServices;
using Arch.Core;
using RatasInit.Components;

namespace RatasInit.Systems;

public static class RotationSystem
{
    private static readonly QueryDescription _torqueQuery = new QueryDescription()
        .WithAll<CRotation, CAngularVelocity, CTorque, CInertia>();

    private static readonly QueryDescription _angularQuery = new QueryDescription()
        .WithAll<CRotation, CAngularVelocity>()
        .WithNone<CTorque>();

    public static void Update(World world, float dt)
    {
        var torqueChunks = world.Query(in _torqueQuery);
        foreach (ref var chunk in torqueChunks.GetChunkIterator())
        {
            var refs = chunk.GetFirst<CRotation, CAngularVelocity, CTorque, CInertia>();
            foreach (var i in chunk)
            {
                ref var rot = ref Unsafe.Add(ref refs.t0, i);
                ref var angVel = ref Unsafe.Add(ref refs.t1, i);
                ref var torque = ref Unsafe.Add(ref refs.t2, i);
                ref var inertia = ref Unsafe.Add(ref refs.t3, i);

                float accel = inertia.InvInertia * torque.Torq;
                angVel.AngVel += accel * dt;
                rot.Rot += angVel.AngVel * dt;

                torque.Torq = 0;
            }
        }

        var angularChunks = world.Query(in _angularQuery);
        foreach (ref var chunk in angularChunks.GetChunkIterator())
        {
            var refs = chunk.GetFirst<CRotation, CAngularVelocity>();
            foreach (var i in chunk)
            {
                ref var rot = ref Unsafe.Add(ref refs.t0, i);
                ref var angVel = ref Unsafe.Add(ref refs.t1, i);
                rot.Rot += angVel.AngVel * dt;
            }
        }
    }
}