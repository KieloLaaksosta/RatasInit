using Arch.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RatasInit.Components;
using RatasInit.Managers;
using RatasInit.Systems;

namespace RatasInit.GameStates;

public sealed class Battle : IGameState
{
    public World World { get; private set; }
    private GraphicsDevice _gd;

    private RenderTarget2D _gameWorld;
    private SpriteBatch _gameWorldBatch;

    public void Dispose()
    {
        World.Dispose();
        _gameWorldBatch.Dispose();
        _gameWorld.Dispose();
    }

    public void Load(GameWindow window, GraphicsDevice gd, GraphicsDeviceManager gdManager)
    {
        World = World.Create();
        _gd = gd;

        _gameWorldBatch = new SpriteBatch(gd);
        _gameWorld = new RenderTarget2D(gd, Consts.WorldDrawSize, Consts.WorldDrawSize);

        World.Create(new CBatch(_gameWorldBatch, 0f), new CSprite(AssetManager.Pixel, new Rectangle(0, 0, 1, 1), Color.White), new CTargetRectangle(new Rectangle(50, 50, 100, 100)));
    }

    public void Update(GameTime time)
    {
        float dt = (float)time.ElapsedGameTime.TotalSeconds;
        for (int i = 0; i < Consts.SubStep; i++)
            NewtonianPhysicSystem.Update(World, dt / Consts.SubStep);
    }

    public void Draw()
    {
        _gd.SetRenderTarget(_gameWorld);
        _gameWorldBatch.Begin(samplerState: SamplerState.PointClamp);

        DrawingSystem.Draw(World);

        _gameWorldBatch.End();

        _gd.SetRenderTarget(ScreenManager.FinalTarget);
        _gameWorldBatch.Begin(samplerState: SamplerState.PointClamp);
        _gameWorldBatch.Draw(_gameWorld, new Rectangle(Consts.WorldMargins, 0, Consts.WorldDrawSize, Consts.WorldDrawSize), Color.White);
        _gameWorldBatch.End();
    }
}
