using System.Collections.Generic;
using Clkd.Assets;
using ClkdUI.Assets.Interfaces;
using ClkdUI.Main;
using ClkdUI.Support;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ClkdUI.Assets.SubComponents
{
    enum BorderStyle
    {
        Solid,
        Dashed,
        Dotted,
        Double,
        Inset,
        Outset,
        Groove,
        Ridge
    }
    public class Border
    {
        BorderStyle Style { get; set; }
        public int Width { get; set; }
        public float InnerEdgeBlurr { get; set; }
        private Color _color;
        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                if (BorderTexture != null) BorderTexture.Dispose();
                BorderTexture = null;
            }
        }
        public float TotalWidth<T>(T guiComponent) where T : IBorderComponent
        {
            return guiComponent.Edges.EdgeBlurr + Width + InnerEdgeBlurr;
        }

        public Rectangle InnerRectangle<T>(T guiComponent) where T : AbstractGuiComponent, IBorderComponent
        {
            return new Rectangle(
                (int)(guiComponent.Coordinate.ActualPosition.X + (TotalWidth(guiComponent) * 2)),
                (int)(guiComponent.Coordinate.ActualPosition.Y + (TotalWidth(guiComponent) * 2)),
                (int)(guiComponent.Coordinate.ActualDimensions.X - (TotalWidth(guiComponent) * 2)),
                (int)(guiComponent.Coordinate.ActualDimensions.Y - (TotalWidth(guiComponent) * 2))
            );
        }
        private Texture2D BorderTexture { get; set; }
        public Sprite BorderSprite { get; set; }
        internal List<Renderable> GetRenderables<T>(T guiComponent) where T : AbstractGuiComponent, IBorderComponent
        {
            RecreateBorderSprite(guiComponent);
            List<Renderable> renderables = new List<Renderable>();
            if (BorderSprite != null) renderables.AddRange(BorderSprite.GetRenderables(guiComponent.Coordinate.GetRenderableCoordinate(1)));
            return renderables;
        }

        private void RecreateBorderSprite<T>(T guiComponent) where T : AbstractGuiComponent, IBorderComponent
        {
            int width = (int)guiComponent.Coordinate.ActualDimensions.X;
            int height = (int)guiComponent.Coordinate.ActualDimensions.Y;
            if (Color != null)
            {
                if (BorderTexture == null ||
                     BorderTexture.Width != width ||
                     BorderTexture.Height != height)
                {
                    GenerateBorderSprite(height, width, guiComponent);
                }
            }
        }

        private void GenerateBorderSprite<T>(int height, int width, T guiComponent) where T : AbstractGuiComponent, IBorderComponent
        {
            if (BorderTexture != null)
            {
                BorderTexture.Dispose();
            }
            BorderTexture = Utilities.GetEmptyTexture(width, height, Color.Transparent);
            Utilities.DrawBorder(BorderTexture, guiComponent.Edges.Radius, Color, Width, guiComponent.Edges.EdgeBlurr, InnerEdgeBlurr);
            BorderSprite = new Sprite(BorderTexture, 0, 0, width, height);
            BorderSprite.BatchStrategy = "generatedTexture";
        }

    }
}