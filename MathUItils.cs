using System;
using Microsoft.Xna.Framework;

namespace RatasInit;

public static class MathUtils
{
    public static Vector2 DirToVec2(float direction)
    {
        (float sin, float cos) = MathF.SinCos(direction);
        return new Vector2(cos, sin);
    }
}
