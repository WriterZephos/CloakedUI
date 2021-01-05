using System.Collections.Generic;
using Clkd.Assets;
using ClkdUI.Assets.Interfaces;
using ClkdUI.Main;
using ClkdUI.Support;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ClkdUI.Assets.SubComponents
{
    public class Background
    {
        private Color _color;
        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                if (ColorTexture != null) ColorTexture.Dispose();
                ColorTexture = null;
            }
        }
        public Texture2D ColorTexture { get; private set; }
        private bool _textureProcessed = false;
        public Texture2D Texture
        {
            get => Sprite?.SpriteCoordinate.Texture;
            set
            {
                _textureProcessed = false;
                _sprite = new Sprite(value, 0, 0, value.Width, value.Height);
            }
        }
        private Sprite ColorSprite { get; set; }
        private Sprite _sprite;
        public Sprite Sprite
        {
            get => _sprite;
            set
            {
                _textureProcessed = false;
                _sprite = value;
            }
        }
        internal List<Renderable> GetRenderables<T>(T guiComponent, Vector3 internalOffsets, bool blurrEdges) where T : AbstractGuiComponent, IBackgroundComponent
        {
            RenderableCoordinate coordinate = guiComponent.Coordinate.GetRenderableCoordinate((int)internalOffsets.Z);
            coordinate.X += (int)(internalOffsets.X);
            coordinate.Y += (int)(internalOffsets.Y);
            coordinate.Width -= (int)(2 * (internalOffsets.X));
            coordinate.Height -= (int)(2 * (internalOffsets.Y));
            float edgeBlurr = blurrEdges ? guiComponent.Edges.EdgeBlurr : 0f;

            RecreateColorSprite(guiComponent, edgeBlurr);
            ProcessTexture(guiComponent, edgeBlurr);

            List<Renderable> renderables = new List<Renderable>();
            if (ColorSprite != null) renderables.AddRange(ColorSprite.GetRenderables(coordinate));
            if (Sprite != null) renderables.AddRange(Sprite.GetRenderables(coordinate));

            return renderables;
        }

        public void SetTexture(Texture2D texture, int x, int y, int width, int height)
        {
            Sprite = new Sprite("steam", 1, 0, 2398, 2398);
        }

        private void RecreateColorSprite<T>(T guiComponent, float edgeBlurr) where T : AbstractGuiComponent, IBackgroundComponent
        {
            int width = (int)guiComponent.Coordinate.ActualDimensions.X;
            int height = (int)guiComponent.Coordinate.ActualDimensions.Y;
            if (Color != null)
            {
                if (ColorTexture == null ||
                     ColorTexture.Width != width ||
                     ColorTexture.Height != height)
                {
                    GenerateColorSprite(height, width, guiComponent, edgeBlurr);
                }
            }
        }

        private void GenerateColorSprite<T>(int height, int width, T guiComponent, float edgeBlurr) where T : AbstractGuiComponent, IBackgroundComponent
        {
            if (ColorTexture != null)
            {
                ColorTexture.Dispose();
            }
            ColorTexture = Utilities.GetEmptyTexture(width, height, Color);
            Utilities.CurveAndBlurrEdgesOutward(ColorTexture, guiComponent.Edges.Radius, edgeBlurr);
            ColorSprite = new Sprite(ColorTexture, 0, 0, width, height);
            ColorSprite.BatchStrategy = "generatedTexture";
        }

        private void ProcessTexture<T>(T guiComponent, float edgeBlurr) where T : AbstractGuiComponent, IBackgroundComponent
        {
            if (!_textureProcessed && Texture != null)
            {
                Texture2D newTexture = Utilities.GetEmptyTexture(Texture.Width, Texture.Height);
                Color[] data = new Color[Texture.Width * Texture.Height];
                Texture.GetData(data);
                newTexture.SetData(data);
                Utilities.CurveAndBlurrEdgesOutward(newTexture, guiComponent.Edges.Radius, edgeBlurr);
                Sprite = new Sprite(newTexture, Sprite.SpriteCoordinate.X, Sprite.SpriteCoordinate.Y, Sprite.SpriteCoordinate.Width, Sprite.SpriteCoordinate.Height);
                Sprite.BatchStrategy = "basic";
                _textureProcessed = true;
            }
        }
    }
}