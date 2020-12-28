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
            float parentRealX,
            float parentRealY,
            float parentRealWidth,
            float parentRealHeight,
            float xOffset,
            float yOffset,
            AbstractGuiComponent child)
        {
            Parent = null;
            Child = child;
            UpdateCoordinateValues(
                parentRealX: parentRealX,
                parentRealY: parentRealY,
                parentRealWidth: parentRealWidth,
                parentRealHeight: parentRealHeight,
                xOffset: xOffset,
                yOffset: yOffset);
            Child = child;
        }

        internal RenderableCoordinate GetRenerableCoordinate()
        {
            return new RenderableCoordinate((int)Position.X, (int)Position.Y, 0, (int)Dimensions.X, (int)Dimensions.Y);
        }

        internal void UpdateCoordinate(float parentRealX, float parentRealY, float parentRealWidth, float parentRealHeight, float xOffset, float yOffset)
        {
            Parent = null;
            UpdateCoordinateValues(
                parentRealX: parentRealX,
                parentRealY: parentRealY,
                parentRealWidth: parentRealWidth,
                parentRealHeight: parentRealHeight,
                xOffset: xOffset,
                yOffset: yOffset);
        }

        internal void UpdateCoordinate(GuiCoordinate guiCoordinate, float xOffset, float yOffset)
        {
            Parent = null;
            UpdateCoordinateValues(
                parentRealX: guiCoordinate.Position.X,
                parentRealY: guiCoordinate.Position.Y,
                parentRealWidth: guiCoordinate.Dimensions.X,
                parentRealHeight: guiCoordinate.Dimensions.Y,
                xOffset: xOffset,
                yOffset: yOffset);
        }



        internal void UpdateCoordinate(GuiContainer parent, float xOffset, float yOffset)
        {
            Parent = parent;
            UpdateCoordinateValues(
                parentRealX: parent.GuiCoordinate.Position.X,
                parentRealY: parent.GuiCoordinate.Position.Y,
                parentRealWidth: parent.GuiCoordinate.Dimensions.X,
                parentRealHeight: parent.GuiCoordinate.Dimensions.Y,
                xOffset: xOffset,
                yOffset: yOffset);
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

        private void UpdateCoordinateValues(float parentRealX, float parentRealY, float parentRealWidth, float parentRealHeight, float xOffset, float yOffset)
        {
            UpdateCoordinateValues(
                parentPosition: new Vector2(parentRealX, parentRealY),
                parentDimensions: new Vector2(parentRealWidth, parentRealHeight),
                offsets: new Vector2(xOffset, yOffset));
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