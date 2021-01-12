using System.Collections.Generic;
using Clkd.Assets;
using ClkdUI.Assets.Interfaces;
using ClkdUI.Main;
using ClkdUI.Support;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ClkdUI.Assets.SubComponents
{
    public class Background : ICollapsableState
    {
        private Color? _color;
        public Color Color
        {
            get => _color ?? CollapseTo?.Color ?? Color.White;
            set
            {
                if (CollapseTo != null) CollapseTo.Color = value;
                else
                {
                    _color = value;
                    if (ColorTexture != null) ColorTexture.Dispose();
                    ColorTexture = null;
                }
            }
        }
        private Texture2D _colorTexture;
        public Texture2D ColorTexture
        {
            get => _colorTexture ?? CollapseTo?.ColorTexture ?? null;
            set
            {
                if (CollapseTo != null) CollapseTo.ColorTexture = value;
                else
                {
                    _colorTexture = value;
                }
            }
        }
        private bool _textureProcessed = true;
        public Texture2D Texture
        {
            get => Sprite?.SpriteCoordinate.Texture ?? CollapseTo?.Texture ?? null;
            set
            {
                if (CollapseTo != null) CollapseTo.Texture = value;
                else
                {
                    _textureProcessed = false;
                    _sprite = new Sprite(value, 0, 0, value.Width, value.Height);
                }
            }
        }
        private Sprite ColorSprite { get; set; }
        private Sprite _sprite;
        public Sprite Sprite
        {
            get => _sprite ?? CollapseTo?.Sprite ?? null;
            set
            {
                if (CollapseTo != null) CollapseTo.Sprite = value;
                else
                {
                    _textureProcessed = false;
                    _sprite = value;
                }
            }
        }
        private Background CollapseTo;
        internal List<IRenderable> GetRenderables<T>(T guiComponent, Vector3 internalOffsets, bool blurrEdges) where T : AbstractInputGuiComponent
        {
            RenderableCoordinate coordinate = guiComponent.Coordinate.GetRenderableCoordinate((int)internalOffsets.Z);
            coordinate.X += (int)(internalOffsets.X);
            coordinate.Y += (int)(internalOffsets.Y);
            coordinate.Width -= (int)(2 * (internalOffsets.X));
            coordinate.Height -= (int)(2 * (internalOffsets.Y));
            float edgeBlurr = blurrEdges ? guiComponent.Edges.EdgeBlurr : 0f;

            RecreateColorSprite(guiComponent, edgeBlurr);
            ProcessTexture(guiComponent, edgeBlurr);

            List<IRenderable> renderables = new List<IRenderable>();
            if (ColorSprite != null) renderables.Condense(ColorSprite.GetRenderables(coordinate));
            if (Sprite != null) renderables.Condense(Sprite.GetRenderables(coordinate));

            return renderables;
        }

        public void SetTexture(Texture2D texture, int x, int y, int width, int height)
        {
            Sprite = new Sprite(texture, x, y, width, height);
        }

        private void RecreateColorSprite<T>(T guiComponent, float edgeBlurr) where T : AbstractInputGuiComponent
        {
            int width = (int)guiComponent.Coordinate.ActualDimensions.X;
            int height = (int)guiComponent.Coordinate.ActualDimensions.Y;
            if (_color != null)
            {
                if (_colorTexture == null ||
                     _colorTexture.Width != width ||
                     _colorTexture.Height != height)
                {
                    GenerateColorSprite(height, width, guiComponent, edgeBlurr);
                }
            }
        }

        private void GenerateColorSprite<T>(int height, int width, T guiComponent, float edgeBlurr) where T : AbstractInputGuiComponent
        {
            if (_colorTexture != null)
            {
                _colorTexture.Dispose();
            }
            if (width == 0 || height == 0) return;
            _colorTexture = Utilities.GetEmptyTexture(width, height, Color);
            Utilities.CurveAndBlurrEdgesOutward(_colorTexture, guiComponent.Edges.Radius, edgeBlurr);
            ColorSprite = new Sprite(ColorTexture, 0, 0, width, height);
            ColorSprite.BatchStrategy = "generatedTexture";
        }

        // Note: This processes Textures in their original dimensions. Maybe change this behavior later? 
        private void ProcessTexture<T>(T guiComponent, float edgeBlurr) where T : AbstractGuiComponent
        {
            if (!_textureProcessed && _sprite?.SpriteCoordinate.Texture != null)
            {
                Texture2D newTexture = Utilities.GetEmptyTexture(Texture.Width, Texture.Height);
                Color[] data = new Color[Texture.Width * Texture.Height];
                Texture.GetData(data);
                newTexture.SetData(data);
                Utilities.CurveAndBlurrEdgesOutward(newTexture, guiComponent.Edges.Radius, edgeBlurr);
                _sprite = new Sprite(newTexture, _sprite.SpriteCoordinate.X, _sprite.SpriteCoordinate.Y, _sprite.SpriteCoordinate.Width, _sprite.SpriteCoordinate.Height);
                _sprite.BatchStrategy = "basic";
                _textureProcessed = true;
            }
        }

        public ICollapsableState Collapse(ICollapsableState other)
        {
            if (other is Background temp) CollapseTo = temp;
            else CollapseTo = null;
            return this;
        }

        public ICollapsableState UnCollapse()
        {
            CollapseTo = null;
            return this;
        }
    }
}