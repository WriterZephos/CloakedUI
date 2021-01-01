using System.Collections.Generic;
using Clkd.Assets;
using Clkd.Main;
using ClkdUI.Assets.Interfaces;
using ClkdUI.Main;
using ClkdUI.Support;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ClkdUI.Assets.SubComponents
{
    public class Background
    {
        public Color Color { get; set; }
        public Texture2D ColorTexture { get; set; }
        public Sprite ColorSprite { get; set; }
        public Sprite Sprite { get; set; }
        internal List<Renderable> GetRenderables<T>(T guiComponent) where T : AbstractGuiComponent, IBackgroundComponent
        {
            RecreateColorSprite(guiComponent);

            List<Renderable> renderables = new List<Renderable>();
            if (ColorSprite != null) renderables.AddRange(ColorSprite.GetRenderables(guiComponent.GuiCoordinate.GetRenderableCoordinate()));
            if (Sprite != null) renderables.AddRange(Sprite.GetRenderables(guiComponent.GuiCoordinate.GetRenderableCoordinate()));

            return renderables;
        }

        private void RecreateColorSprite<T>(T guiComponent) where T : AbstractGuiComponent, IBackgroundComponent
        {
            int width = (int)guiComponent.GuiCoordinate.Dimensions.X;
            int height = (int)guiComponent.GuiCoordinate.Dimensions.Y;
            if (Color != null)
            {
                if (ColorTexture == null ||
                     ColorTexture.Width != width ||
                     ColorTexture.Height != height)
                {
                    GenerateColorSprite(height, width, guiComponent);
                }
            }
        }

        private void GenerateColorSprite<T>(int height, int width, T guiComponent) where T : AbstractGuiComponent, IBackgroundComponent
        {
            if (ColorTexture != null)
            {
                ColorTexture.Dispose();
            }
            ColorTexture = Utilities.GetEmptyTexture(width, height, Color);
            Utilities.DecorateTextureEdges(ColorTexture, guiComponent.Radius, guiComponent.EdgeBlurr);
            ColorSprite = new Sprite(ColorTexture, 0, 0, width, height);
        }
    }
}