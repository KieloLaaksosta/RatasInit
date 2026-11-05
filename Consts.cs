using Microsoft.Xna.Framework;

public static class Consts
{
    public const int WorldDrawSize = 360;
    public const int WorldMargins = 90;

    public const int SubStep = 4;

    public static readonly Rectangle WorldRect = new(0, 0, WorldDrawSize, WorldDrawSize);
}