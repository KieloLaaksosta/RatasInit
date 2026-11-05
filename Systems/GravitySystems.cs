using System;
using System.Runtime.CompilerServices;
using Arch.Core;
using Microsoft.Xna.Framework;
using RatasInit.Components;

namespace RatasInit.Systems;

public static class GravitySystem
{
    private const float EPSILON = 100f;

    private static readonly QueryDescription _staticSourceQuery = new QueryDescription()
        .WithAll<CPosition, CMass, CGravitySource>()
        .WithNone<CForce>();

    private static readonly QueryDescription _targetQuery = new QueryDescription()
        .WithAll<CPosition, CMass, CForce>();

    public static void Update(World world, float G)
    {
        ApplyGravity(world.Query(in _staticSourceQuery), world.Query(in _targetQuery), G);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Vector2 GetForce(ref CPosition p1, ref CMass m1, ref CPosition p2, ref CMass m2, float G)
    {
        Vector2 delta = p1.Pos - p2.Pos;
        float distanceSquared = delta.LengthSquared();

        return G * m1.Mass * m2.Mass / (distanceSquared * MathF.Sqrt(distanceSquared) + EPSILON) * delta;
    }

    private static void ApplyGravity(Query sourceQuery, Query targetQuery, float G)
    {
        foreach (ref var sourceChunk in sourceQuery.GetChunkIterator())
        {
            var sourceReferences = sourceChunk.GetFirst<CPosition, CMass>();

            foreach (int sourceIdx in sourceChunk)
            {
                ref var sourcePos = ref Unsafe.Add(ref sourceReferences.t0, sourceIdx);
                ref var sourceMass = ref Unsafe.Add(ref sourceReferences.t1, sourceIdx);

                foreach (ref var targetChunk in targetQuery.GetChunkIterator())
                {
                    var targetReferences = targetChunk.GetFirst<CPosition, CMass, CForce>();

                    foreach (int targetIdx in targetChunk)
                    {
                        ref var targetPos = ref Unsafe.Add(ref targetReferences.t0, targetIdx);
                        ref var targetMass = ref Unsafe.Add(ref targetReferences.t1, targetIdx);
                        ref var targetForce = ref Unsafe.Add(ref targetReferences.t2, targetIdx);

                        targetForce.Force += GetForce(ref sourcePos, ref sourceMass, ref targetPos, ref targetMass, G);
                    }
                }
            }
        }
    }
}