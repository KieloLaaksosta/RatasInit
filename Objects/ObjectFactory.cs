using Arch.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RatasInit.Components;
using RatasInit.Managers;

namespace RatasInit.Objects;

public static class ObjectFactory
{
    public static Entity CreateShrapnel(World world, SpriteBatch batch, Vector2 pos, Vector2 vel)
    {
        var entity = world.Create(
            new CPosition(pos),
            new CMass(0.01f),
            new CVelocity(vel),
            new CForce(),

            new CBatch(batch, 0f),
            new CSprite(AssetManager.Pixel, new Rectangle(0, 0, 1, 1), Color.White)
        );
        return entity;
    }

    public static Entity CreateStaticPlanet(World world, SpriteBatch batch, float mass)
    {
        var entity = world.Create(
            new CPosition(Consts.WorldCentre),
            new CMass(mass),

            new CGravitySource(),

            new CBatch(batch, 0f),
            new CSprite(AssetManager.Pixel, new Rectangle(0, 0, 1, 1), Color.Red)
        );
        return entity;
    }
}
