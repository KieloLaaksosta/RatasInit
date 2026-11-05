using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RatasInit.Managers;

public static class AssetManager
{
    public static Texture2D Pixel { get; private set; }

    public static void Load(ContentManager content, GraphicsDevice gd)
    {
        Pixel = new Texture2D(gd, 1, 1);
        Pixel.SetData([Color.White]);   
    }
}
