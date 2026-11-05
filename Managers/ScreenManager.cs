using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RatasInit.Managers;

public static class ScreenManager
{
    public const int FinalTargetSizeX = 540;
    public const int FinalTargetSizeY = 360;

    public static GraphicsDevice Gd {get; private set;}
    public static RenderTarget2D FinalTarget { get; private set; }

    private static SpriteBatch _batch;

    public static void Initialize(GraphicsDevice gd)
    {
        FinalTarget = new RenderTarget2D(gd, FinalTargetSizeX, FinalTargetSizeY);
        _batch = new SpriteBatch(gd);
        Gd = gd;
    }

    public static void DrawToBackBuffer()
    {
        Gd.SetRenderTarget(null);
        Gd.Clear(Color.Black);

        float screenScaleX = (float)Gd.Viewport.Width / FinalTargetSizeX;
        float screenScaleY = (float)Gd.Viewport.Height / FinalTargetSizeY;
        float drawScale = Math.Min(screenScaleX, screenScaleY);

        int presentWidth = (int)(FinalTargetSizeX * drawScale);
        int presentHeight = (int)(FinalTargetSizeY * drawScale);

        int x = (Gd.Viewport.Width - presentWidth) / 2;
        int y = (Gd.Viewport.Height - presentHeight) / 2;
        Rectangle destinationRectangle = new(x, y, presentWidth, presentHeight);

        _batch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointClamp);
        _batch.Draw(FinalTarget, destinationRectangle, Color.White);
        _batch.End();
    }
}