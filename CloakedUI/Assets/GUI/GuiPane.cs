using System.Collections.Generic;
using Clkd.Assets;
using Clkd.GUI.Interfaces;
using Clkd.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Clkd.GUI
{
    public class GuiPane : AbstractGuiComponent
    {
        private GuiContainer RootContainer { get; set; }
        public Vector2 Position { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }

        public GuiPane(GuiContainer rootContainer, GraphicsDevice graphicsDevice, Vector2 position)
        {
            GuiCoordinate = BuildGuiCoordinate(graphicsDevice.Viewport.Bounds, position);
            GraphicsDevice = graphicsDevice;
            Position = position;
            RootContainer = rootContainer;
        }

        public override List<Renderable> GetRenderables(RenderableCoordinate? renderableCoordinate = null)
        {
            return RootContainer.GetRenderables();
        }

        public override void Update(GameTime gameTime)
        {
            RootContainer.Update(gameTime);
        }

        private GuiCoordinate BuildGuiCoordinate(Rectangle bounds, Vector2 position)
        {
            return new GuiCoordinate(bounds.X, bounds.Y, bounds.Width, bounds.Height, position.X, position.Y, this);
        }

    }
}