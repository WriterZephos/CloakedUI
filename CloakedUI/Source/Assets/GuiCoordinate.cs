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
        public Vector2 ActualPosition { get; set; }
        public int ZIndex { get; set; }
        internal Vector2 ActualDimensions { get; set; }
        public Rectangle ActualBounds { get; private set; }
        internal AbstractGuiComponent Parent { get; set; }
        internal AbstractGuiComponent Child { get; set; }

        internal GuiCoordinate(
            AbstractGuiComponent child,
            Vector2 parentPosition = default(Vector2),
            Vector2 parentDimensions = default(Vector2),
            Vector2 offsets = default(Vector2),
            int? zIndex = null)
        {
            Parent = null;
            Child = child;
            UpdateCoordinateValues(
                parentPosition: parentPosition,
                parentDimensions: parentDimensions,
                offsets: offsets,
                zIndex: zIndex);
        }

        internal void UpdateCoordinate(GuiCoordinate guiCoordinate, Vector2 offsets, int? zIndex = null)
        {
            Parent = null;
            UpdateCoordinateValues(
                parentPosition: guiCoordinate.ActualPosition,
                parentDimensions: guiCoordinate.ActualDimensions,
                offsets: offsets,
                zIndex: zIndex);
        }

        internal void UpdateCoordinate(AbstractGuiComponent parent, Vector2 offsets, int? zIndex = null)
        {
            Parent = parent;
            UpdateCoordinateValues(
                parentPosition: parent.Coordinate.ActualPosition,
                parentDimensions: parent.Coordinate.ActualDimensions,
                offsets: offsets,
                zIndex: zIndex);
        }

        internal RenderableCoordinate GetRenderableCoordinate(int zOffset = 0)
        {
            return new RenderableCoordinate(
                x: (int)ActualPosition.X,
                y: (int)ActualPosition.Y,
                z: ZIndex + zOffset,
                width: (int)ActualDimensions.X,
                height: (int)ActualDimensions.Y,
                isOffset: false);
        }

        private void UpdateCoordinateValues(Vector2 parentPosition, Vector2 parentDimensions, Vector2 offsets, int? zIndex)
        {
            ParentPosition = parentPosition;
            ParentDimensions = parentDimensions;
            Offsets = offsets;
            ActualPosition = new Vector2(CalculateRealX(), CalculateRealY());
            ZIndex = zIndex ?? Parent?.Coordinate.ZIndex + 25 ?? CloakedGUIConfig.BaseZIndex;
            ActualDimensions = new Vector2(CalculateRealWidth(), CalculateRealHeight());
            ActualBounds = new Rectangle((int)ActualPosition.X, (int)ActualPosition.Y, (int)ActualDimensions.X, (int)ActualDimensions.Y);
        }

        private float CalculateRealWidth()
        {
            return Child.Dimensions.HasRelativeWidth ? ParentDimensions.X * Child.Dimensions.X : Child.Dimensions.X;
        }

        private float CalculateRealHeight()
        {
            return Child.Dimensions.HasRelativeHeight ? ParentDimensions.Y * Child.Dimensions.Y : Child.Dimensions.Y;
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