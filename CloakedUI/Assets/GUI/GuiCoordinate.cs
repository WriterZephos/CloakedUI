using Clkd.Assets;
using Clkd.Main;
using Microsoft.Xna.Framework;

namespace Clkd.GUI
{
    public class GuiCoordinate
    {
        public float ParentRealX { get; set; }
        public float ParentRealY { get; set; }
        public float ParentRealWidth { get; set; }
        public float ParentRealHeight { get; set; }
        public float XOffset { get; set; }
        public float YOffset { get; set; }

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
            UpdateCoordinate(
                parentRealX: parentRealX,
                parentRealY: parentRealY,
                parentRealWidth: parentRealWidth,
                parentRealHeight: parentRealHeight,
                xOffset: xOffset,
                yOffset: yOffset);
            Child = child;
        }

        public GuiCoordinate(GuiCoordinate parentCoordinate, float xOffset, float yOffset, AbstractGuiComponent child)
        {
            Child = child;
            UpdateCoordinate(
                parentRealX: parentCoordinate.RealX,
                parentRealY: parentCoordinate.RealY,
                parentRealWidth: parentCoordinate.RealWidth,
                parentRealHeight: ParentRealHeight = RealHeight,
                xOffset: xOffset,
                yOffset: yOffset);
        }

        public RenderableCoordinate GetRenerableCoordinate()
        {
            return new RenderableCoordinate((int)RealX, (int)RealY, 0, (int)RealWidth, (int)RealHeight);
        }

        public void UpdateCoordinate(float parentRealX, float parentRealY, float parentRealWidth, float parentRealHeight, float xOffset, float yOffset)
        {
            ParentRealX = parentRealX;
            ParentRealY = parentRealY;
            ParentRealWidth = parentRealWidth;
            ParentRealHeight = parentRealHeight;
            XOffset = xOffset;
            YOffset = yOffset;
        }

        public void UpdateCoordinate(GuiCoordinate parentCoordinate, float xOffset, float yOffset)
        {
            UpdateCoordinate(
                parentRealX: parentCoordinate.RealX,
                parentRealY: parentCoordinate.RealY,
                parentRealWidth: parentCoordinate.RealWidth,
                parentRealHeight: ParentRealHeight = parentCoordinate.RealHeight,
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