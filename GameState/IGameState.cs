using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RatasInit.GameStates;

public interface IGameState : IDisposable
{
    void Load(GameWindow window, GraphicsDevice gd, GraphicsDeviceManager gdManager);
    void Update(GameTime time);
    void Draw(); 
}
