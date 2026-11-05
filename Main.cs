using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RatasInit.GameStates;
using RatasInit.Managers;

namespace RatasInit;

public class Main : Game
{
    private GraphicsDeviceManager _graphics;
    private IGameState _gameState;

    public Main()
    {
        _graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width,
            PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height,

        };
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();

        _gameState = new Battle();
        _gameState.Load(Window, GraphicsDevice, _graphics);
    }

    protected override void LoadContent()
    {
        AssetManager.Load(Content, GraphicsDevice);
        ScreenManager.Initialize(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _gameState.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        _gameState.Draw();
        ScreenManager.DrawToBackBuffer();

        base.Draw(gameTime);
    }
}
