using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Clkd.Assets;
using Clkd.Main;
using ClkdUI.Assets;
using System.Linq;
using Clkd.Managers;

namespace ClkdUI.Main
{
    public class GuiPane : AbstractGuiComponent
    {
        public GuiContainer RootContainer { get; set; }
        public GuiInputManager GuiInputManager { get; private set; }
        public Vector2 Position
        {
            get => Coordinate.Offsets;
            set
            {
                Coordinate.Offsets = value;
            }
        }
        public bool Initialized { get; private set; }

        public GuiPane(GuiContainer rootContainer, Vector2 position = default(Vector2))
        {
            Position = position;
            RootContainer = rootContainer;
            RenderableManager.BatchStrategies.Add("generatedTexture", new GeneratedTextureBatchStrategy());
        }

        public override List<Renderable> GetRenderables(RenderableCoordinate? renderableCoordinate = null)
        {
            if (!Initialized) return null;
            return RootContainer.GetRenderables();
        }

        public override void Update(GameTime gameTime)
        {
            if (!Initialized) return;
            RootContainer.UpdateInternal(gameTime);
        }

        public void Initialize(GameContext gameContext)
        {
            SetRootGuiCoordinate();
            RootContainer.RecalculateChildren();
            GuiInputManager = new GuiInputManager(gameContext);
            Initialized = true;
        }

        internal IEnumerable<AbstractGuiComponent> GetFocusedInternal()
        {
            return RootContainer.Layout.Where((guiComponent) => { return guiComponent?.Input.Focused ?? false; });
        }

        public List<AbstractGuiComponent> GetFocused()
        {
            return GetFocusedInternal().ToList();
        }

        internal override sealed void UnfocusInternal()
        {
            base.UnfocusInternal();
            RootContainer.Unfocus();
        }

        private void SetRootGuiCoordinate()
        {
            Coordinate = BuildGuiCoordinate(Cloaked.GraphicsDeviceManager.GraphicsDevice.Viewport.Bounds, Position);
            RootContainer.Dimensions.X = Coordinate.ActualDimensions.X;
            RootContainer.Dimensions.Y = Coordinate.ActualDimensions.Y;
            RootContainer.UpdatePosition(
                parent: this,
                offsets: this.Coordinate.Offsets);
        }

        private GuiCoordinate BuildGuiCoordinate(Rectangle bounds, Vector2 position)
        {
            this.Dimensions.X = bounds.Width;
            this.Dimensions.Y = bounds.Height;
            return new GuiCoordinate(
                parentPosition: new Vector2(bounds.X, bounds.Y),
                parentDimensions: new Vector2(bounds.Width, bounds.Height),
                offsets: new Vector2(position.X, position.Y),
                child: this);
        }
    }
}