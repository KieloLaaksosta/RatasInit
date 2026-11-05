using System.Runtime.CompilerServices;
using Arch.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RatasInit.Components;

namespace RatasInit.Systems;

public static class DrawingSystem
{
    private static readonly QueryDescription _recQuery = new QueryDescription()
        .WithAll<CBatch, CSprite, CTargetRectangle>();

    public static void Draw(World world)
    {
        DrawRects(world);
    }

    private static void DrawRects(World world)
    {
        var chunks = world.Query(in _recQuery);
        foreach (ref var chunk in chunks.GetChunkIterator())
        {
            var refs = chunk.GetFirst<CBatch, CSprite, CTargetRectangle>();
            foreach (var i in chunk)
            {
                ref var batch = ref Unsafe.Add(ref refs.t0, i);
                ref var sprite = ref Unsafe.Add(ref refs.t1, i);
                ref var targetRect = ref Unsafe.Add(ref refs.t2, i);

                batch.Batch.Draw(
                    sprite.Texture,
                    targetRect.Rect,
                    sprite.Sprite,
                    sprite.Modulation,
                    0f,
                    Vector2.Zero,
                    SpriteEffects.None,
                    batch.Depth
                );
            }
        }
    }
}