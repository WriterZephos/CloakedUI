using Clkd.Assets;
using Clkd.Main;
using Microsoft.Xna.Framework;

namespace Clkd.GUI
{
    //TODO: add support for limiting the size of a component and setting overflow behavior
    public class GuiCoordinate
    {
        public float ParentRealX { get; set; }
        public float ParentRealY { get; set; }
        public float ParentRealWidth { get; set; }
        public float ParentRealHeight { get; set; }
        public float XOffset { get; set; }
        public float YOffset { get; set; }
        public float MaxWidth { get; set; }
        public float MaxHeight { get; set; }
        public float RealX
        {
            get => CalculateRealX();
        }

        public float RealY
        {
            get => CalculateRealY();
        }

        public float RealWidth
        {
            get => CalculateRealWidth();
        }

        public float RealHeight
        {
            get => CalculateRealHeight();
        }

        public GuiContainer Parent { get; set; }
        public AbstractGuiComponent Child { get; set; }

        public GuiCoordinate(
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

        public RenderableCoordinate GetRenerableCoordinate()
        {
            return new RenderableCoordinate((int)RealX, (int)RealY, 0, (int)RealWidth, (int)RealHeight);
        }

        public void UpdateCoordinate(float parentRealX, float parentRealY, float parentRealWidth, float parentRealHeight, float xOffset, float yOffset)
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

        public void UpdateCoordinate(GuiCoordinate guiCoordinate, float xOffset, float yOffset)
        {
            Parent = null;
            UpdateCoordinateValues(
                parentRealX: guiCoordinate.RealX,
                parentRealY: guiCoordinate.RealY,
                parentRealWidth: guiCoordinate.RealWidth,
                parentRealHeight: guiCoordinate.RealHeight,
                xOffset: xOffset,
                yOffset: yOffset);
        }

        private void UpdateCoordinateValues(float parentRealX, float parentRealY, float parentRealWidth, float parentRealHeight, float xOffset, float yOffset)
        {
            ParentRealX = parentRealX;
            ParentRealY = parentRealY;
            ParentRealWidth = parentRealWidth;
            ParentRealHeight = parentRealHeight;
            XOffset = xOffset;
            YOffset = yOffset;
        }

        public void UpdateCoordinate(GuiContainer parent, float xOffset, float yOffset)
        {
            Parent = parent;
            UpdateCoordinateValues(
                parentRealX: parent.GuiCoordinate.RealX,
                parentRealY: parent.GuiCoordinate.RealY,
                parentRealWidth: parent.GuiCoordinate.RealWidth,
                parentRealHeight: parent.GuiCoordinate.RealHeight,
                xOffset: xOffset,
                yOffset: yOffset);
        }

        public RenderableCoordinate GetRenderableCoordinate()
        {
            return new RenderableCoordinate(
                x: (int)CalculateRealX(),
                y: (int)CalculateRealY(),
                z: 1,
                width: (int)CalculateRealWidth(),
                height: (int)CalculateRealHeight(),
                isOffset: false);
        }

        private float CalculateRealWidth()
        {
            return Child.HasRelativeWidth ? ParentRealWidth * Child.Width : Child.Width;
        }

        private float CalculateRealHeight()
        {
            return Child.HasRelativeHeight ? ParentRealHeight * Child.Height : Child.Height;
        }

        private float CalculateRealY()
        {
            return ParentRealY + YOffset;
        }

        private float CalculateRealX()
        {
            return ParentRealX + XOffset;
        }
    }
}