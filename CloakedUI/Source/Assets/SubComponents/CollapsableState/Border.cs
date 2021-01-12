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
    public class Border : ICollapsableState
    {
        private BorderStyle? _style;
        BorderStyle Style
        {
            get => _style ?? CollapseTo?.Style ?? BorderStyle.Solid;
            set
            {
                if (CollapseTo != null) CollapseTo.Style = value;
                else _style = value;
            }
        }
        private int? _width;
        public int Width
        {
            get => _width ?? CollapseTo?.Width ?? 0;
            set
            {
                if (CollapseTo != null) CollapseTo.Width = value;
                else _width = value;
            }
        }
        private Color? _color;
        public Color Color
        {
            get => _color ?? CollapseTo?.Color ?? Color.Black;
            set
            {
                if (CollapseTo != null) CollapseTo.Color = value;
                else
                {
                    _color = value;
                    if (BorderTexture != null) BorderTexture.Dispose();
                    BorderTexture = null;
                }
            }
        }

        private Border CollapseTo { get; set; }
        public float TotalWidth<T>(T guiComponent) where T : AbstractInputGuiComponent
        {
            return guiComponent.Edges.EdgeBlurr + Width + guiComponent.Edges.InnerEdgeBlurr;
        }

        public Rectangle InnerRectangle<T>(T guiComponent) where T : AbstractInputGuiComponent
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
        internal List<IRenderable> GetRenderables<T>(T guiComponent, Vector3 internalOffsets) where T : AbstractInputGuiComponent
        {
            RenderableCoordinate coordinate = guiComponent.Coordinate.GetRenderableCoordinate((int)internalOffsets.Z);
            RecreateBorderSprite(guiComponent);
            List<IRenderable> renderables = new List<IRenderable>();
            if (BorderSprite != null) renderables.Condense(BorderSprite.GetRenderables(coordinate));
            return renderables;
        }

        private void RecreateBorderSprite<T>(T guiComponent) where T : AbstractInputGuiComponent
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

        private void GenerateBorderSprite<T>(int height, int width, T guiComponent) where T : AbstractInputGuiComponent
        {
            if (BorderTexture != null)
            {
                BorderTexture.Dispose();
            }
            if (width == 0 || height == 0) return;
            BorderTexture = Utilities.GetEmptyTexture(width, height, Color.Transparent);
            Utilities.DrawBorder(BorderTexture, guiComponent.Edges.Radius, Color, Width, guiComponent.Edges.EdgeBlurr, guiComponent.Edges.InnerEdgeBlurr);
            BorderSprite = new Sprite(BorderTexture, 0, 0, width, height);
            BorderSprite.BatchStrategy = "generatedTexture";
        }

        public ICollapsableState Collapse(ICollapsableState other)
        {
            if (other is Border temp) CollapseTo = temp;
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