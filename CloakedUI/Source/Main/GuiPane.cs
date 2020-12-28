using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Clkd.Assets;
using Clkd.Main;
using ClkdUI.Assets;

namespace ClkdUI.Main
{
    public class GuiPane : AbstractGuiComponent
    {
        public GuiContainer RootContainer { get; set; }
        private Vector2 _position;
        public Vector2 Position
        {
            get => GuiCoordinate.Offsets;
            set
            {
                GuiCoordinate.Offsets = value;
            }
        }
        public bool Initialized { get; private set; }

        public GuiPane(GuiContainer rootContainer, Vector2 position = default(Vector2))
        {
            Position = position;
            RootContainer = rootContainer;
        }

        public override List<Renderable> GetRenderables(RenderableCoordinate? renderableCoordinate = null)
        {
            if (!Initialized) return null;
            return RootContainer.GetRenderables();
        }

        public override void Update(GameTime gameTime)
        {
            if (!Initialized) return;
            RootContainer.Update(gameTime);
        }

        public void Initialize()
        {
            GuiCoordinate = BuildGuiCoordinate(Cloaked.GraphicsDeviceManager.GraphicsDevice.Viewport.Bounds, Position);
            SetRootGuiCoordinate();
            Initialized = true;
        }

        private void SetRootGuiCoordinate()
        {
            RootContainer.UpdatePosition(GuiCoordinate, 0f, 0f);
        }

        private GuiCoordinate BuildGuiCoordinate(Rectangle bounds, Vector2 position)
        {
            return new GuiCoordinate(bounds.X, bounds.Y, bounds.Width, bounds.Height, position.X, position.Y, this);
        }
    }
}