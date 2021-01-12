using System.Collections.Generic;
using Clkd.Assets;
using Clkd.Main;
using ClkdUI.Assets.Interfaces;
using ClkdUI.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public enum HorizontalAlignment
{
    Center,
    Left,
    Right
}

public enum VerticalAlignment
{
    Center,
    Top,
    Bottom
}
namespace ClkdUI.Assets.SubComponents
{
    public class Text : ICollapsableState
    {
        private string _font;
        public string Font
        {
            get => _font ?? CollapseTo?.Font ?? null;
            set
            {
                if (CollapseTo != null) CollapseTo.Font = value;
                else _font = value;
            }
        }
        private Color? _color;
        public Color Color
        {
            get => _color ?? CollapseTo?.Color ?? Color.Black;
            set
            {
                if (CollapseTo != null) CollapseTo.Color = value;
                else _color = value;
            }
        }
        private HorizontalAlignment? _horizontalAlignment;
        public HorizontalAlignment HorizontalAlignment
        {
            get => _horizontalAlignment ?? CollapseTo?.HorizontalAlignment ?? HorizontalAlignment.Center;
            set
            {
                if (CollapseTo != null) CollapseTo.HorizontalAlignment = value;
                else _horizontalAlignment = value;
            }
        }
        private VerticalAlignment? _verticalAlignment;
        public VerticalAlignment VerticalAlignment
        {
            get => _verticalAlignment ?? CollapseTo?.VerticalAlignment ?? VerticalAlignment.Center;
            set
            {
                if (CollapseTo != null) CollapseTo.VerticalAlignment = value;
                else _verticalAlignment = value;
            }
        }
        private string _textValue;
        public string TextValue
        {
            get => _textValue ?? CollapseTo?._textValue ?? "";
            set
            {
                if (CollapseTo != null) CollapseTo._textValue = value;
                else _textValue = value;
            }
        }
        private Text CollapseTo { get; set; }

        public List<IRenderable> GetRenderables<T>(T guiComponent, Vector3 internalOffsets) where T : AbstractGuiComponent
        {
            SpriteFont font = Cloaked.TextureManager.GetSpriteFont(Font);
            if (font == null) return null;
            Vector2 size = font.MeasureString(TextValue);
            Point pos = guiComponent.Coordinate.ActualBounds.Center;

            switch (HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    pos.X = (int)(guiComponent.Coordinate.ActualBounds.Left + (int)guiComponent.Padding.Left + internalOffsets.X);
                    break;
                case HorizontalAlignment.Right:
                    pos.X = (int)(guiComponent.Coordinate.ActualBounds.Right - size.X - guiComponent.Padding.Right - internalOffsets.X);
                    break;
                case HorizontalAlignment.Center:
                    pos.X -= (int)(size.X / 2);
                    break;
            }

            switch (VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    pos.Y = (int)(guiComponent.Coordinate.ActualBounds.Top + (int)guiComponent.Padding.Top + internalOffsets.Y);
                    break;
                case VerticalAlignment.Bottom:
                    pos.Y = (int)(guiComponent.Coordinate.ActualBounds.Bottom - size.Y - guiComponent.Padding.Bottom - internalOffsets.Y);
                    break;
                case VerticalAlignment.Center:
                    pos.Y -= (int)(size.Y / 2);
                    break;
            }

            RenderableCoordinate coord = guiComponent.Coordinate.GetRenderableCoordinate((int)internalOffsets.Z);
            coord.X = pos.X;
            coord.Y = pos.Y;
            StringRenderable renderable = new StringRenderable(TextValue, Font, Color, coord);
            renderable.BatchStrategy = "generatedTexture";
            return new List<IRenderable>() { renderable };
        }

        public ICollapsableState Collapse(ICollapsableState other)
        {
            if (other is Text temp) CollapseTo = temp;
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