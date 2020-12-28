using Clkd.Assets;
using Clkd.Main;
using ClkdUI.Main;
using Microsoft.Xna.Framework;

namespace ClkdUI.Assets
{
    //TODO: add support for limiting the size of a component and setting overflow behavior
    public class GuiCoordinate
    {
        internal Vector2 ParentPosition { get; set; }
        internal Vector2 ParentDimensions { get; set; }
        internal Vector2 Offsets { get; set; }
        internal Vector2 MaxDimensions { get; set; }
        internal float MaxWidth { get; set; }
        internal float MaxHeight { get; set; }
        public Vector2 Position { get; set; }
        internal Vector2 Dimensions { get; set; }
        public Rectangle Bounds { get; private set; }
        internal GuiContainer Parent { get; set; }
        internal AbstractGuiComponent Child { get; set; }

        internal GuiCoordinate(
            AbstractGuiComponent child,
            Vector2 parentPosition = default(Vector2),
            Vector2 parentDimensions = default(Vector2),
            Vector2 offsets = default(Vector2))
        {
            Parent = null;
            Child = child;
            UpdateCoordinateValues(
                parentPosition: parentPosition,
                parentDimensions: parentDimensions,
                offsets: offsets);
            Child = child;
        }

        internal void UpdateCoordinate(GuiCoordinate guiCoordinate, Vector2 offsets)
        {
            Parent = null;
            UpdateCoordinateValues(
                parentPosition: guiCoordinate.Position,
                parentDimensions: guiCoordinate.Dimensions,
                offsets: offsets);
        }

        internal void UpdateCoordinate(GuiContainer parent, Vector2 offsets)
        {
            Parent = parent;
            UpdateCoordinateValues(
                parentPosition: parent.GuiCoordinate.Position,
                parentDimensions: parent.GuiCoordinate.Dimensions,
                offsets: offsets);
        }

        internal RenderableCoordinate GetRenderableCoordinate()
        {
            return new RenderableCoordinate(
                x: (int)Position.X,
                y: (int)Position.Y,
                z: 1,
                width: (int)Dimensions.X,
                height: (int)Dimensions.Y,
                isOffset: false);
        }

        private void UpdateCoordinateValues(Vector2 parentPosition, Vector2 parentDimensions, Vector2 offsets)
        {
            ParentPosition = parentPosition;
            ParentDimensions = parentDimensions;
            Offsets = offsets;
            Position = new Vector2(CalculateRealX(), CalculateRealY());
            Dimensions = new Vector2(CalculateRealWidth(), CalculateRealHeight());
            Bounds = new Rectangle((int)Position.X, (int)Position.Y, (int)Dimensions.X, (int)Dimensions.Y);
        }

        private float CalculateRealWidth()
        {
            return Child.HasRelativeWidth ? ParentDimensions.X * Child.Width : Child.Width;
        }

        private float CalculateRealHeight()
        {
            return Child.HasRelativeHeight ? ParentDimensions.Y * Child.Height : Child.Height;
        }

        private float CalculateRealY()
        {
            return (ParentPosition + Offsets).Y;
        }

        private float CalculateRealX()
        {
            return (ParentPosition + Offsets).X;
        }
    }
}