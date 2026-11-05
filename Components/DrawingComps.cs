using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RatasInit.Components;

public record struct CBatch(SpriteBatch Batch, float Depth);
public record struct CSprite(Texture2D Texture, Rectangle Sprite, Color Modulation);
public record struct CTargetRectangle(Rectangle Rect);