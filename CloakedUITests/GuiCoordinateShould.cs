using Microsoft.Xna.Framework;

using Clkd.Assets;

using Xunit;
using ClkdUI.Assets;

namespace CloakedUITests
{
    public class GuiCoordinateShould
    {
        [Theory]
        [InlineData(50f, 100f, 50f)]
        [InlineData(0.5f, 100f, 50f)]
        [InlineData(0.33f, 100f, 33f)]
        [InlineData(0.25f, 100f, 25f)]
        [InlineData(0.155f, 100f, 15.5f)]
        [InlineData(0.155f, 555f, 86.025f)]
        [InlineData(1f, 555f, 555f)]
        public void CalculateRealWidth(float widthProperty, float parentWidth, float expectedWidth)
        {
            ColoredRectangle rect = new ColoredRectangle(widthProperty, 50f, Color.Aqua);
            GuiCoordinate sut = new GuiCoordinate(
                child: rect,
                parentPosition: new Vector2(0f, 0f),
                parentDimensions: new Vector2(parentWidth, 100f),
                offsets: new Vector2(0f, 0f));
            Assert.Equal(sut.Dimensions.X, expectedWidth);
        }

        [Theory]
        [InlineData(50f, 100f, 50f)]
        [InlineData(0.5f, 100f, 50f)]
        [InlineData(0.33f, 100f, 33f)]
        [InlineData(0.25f, 100f, 25f)]
        [InlineData(0.155f, 100f, 15.5f)]
        [InlineData(0.155f, 555f, 86.025f)]
        [InlineData(1f, 555f, 555f)]
        public void CalculateRealHeight(float heightProperty, float parentHeight, float expectedHeight)
        {
            ColoredRectangle rect = new ColoredRectangle(50f, heightProperty, Color.Aqua);
            GuiCoordinate sut = new GuiCoordinate(
                child: rect,
                parentPosition: new Vector2(0f, 0f),
                parentDimensions: new Vector2(100f, parentHeight));
            Assert.Equal(sut.Dimensions.Y, expectedHeight);
        }

        [Theory]
        [InlineData(20f, 20f, 40f)]
        [InlineData(75f, 25f, 100f)]
        [InlineData(99.5f, 30f, 129.5f)]
        public void CalculateRealX(float parentRealX, float xOffset, float expectedRealX)
        {
            ColoredRectangle rect = new ColoredRectangle(50f, 50f, Color.Aqua);
            GuiCoordinate sut = new GuiCoordinate(
                child: rect,
                parentPosition: new Vector2(parentRealX, 0f),
                parentDimensions: new Vector2(100f, 100f),
                offsets: new Vector2(xOffset, 0f));
            Assert.Equal(sut.Position.X, expectedRealX);
        }

        [Theory]
        [InlineData(20f, 20f, 40f)]
        [InlineData(75f, 25f, 100f)]
        [InlineData(99.5f, 30f, 129.5f)]
        public void CalculateRealY(float parentRealY, float yOffset, float expectedRealY)
        {
            ColoredRectangle rect = new ColoredRectangle(50f, 50f, Color.Aqua);
            GuiCoordinate sut = new GuiCoordinate(
                child: rect,
                parentPosition: new Vector2(0f, parentRealY),
                parentDimensions: new Vector2(100f, 100f),
                offsets: new Vector2(0f, yOffset));
            Assert.Equal(sut.Position.Y, expectedRealY);
        }

        [Theory]
        [InlineData(.5f, .5f, 0f, 0f, 100f, 100f, 25f, 25f, 25f, 25f, 50f, 50f)]
        [InlineData(80f, .5f, 0f, 0f, 100f, 100f, 25f, 25f, 25f, 25f, 80f, 50f)]
        public void GetCorrectRenderableCoordinate(
            float widthProperty,
            float heightProperty,
            float parentRealX,
            float parentRealY,
            float parentWidth,
            float parentHeight,
            float xOffset,
            float yOffset,
            float expectedX,
            float expectedY,
            float expectedWidth,
            float expectedHeight
        )
        {
            ColoredRectangle rect = new ColoredRectangle(widthProperty, heightProperty, Color.Aqua);
            GuiCoordinate sut = new GuiCoordinate(
                child: rect,
                parentPosition: new Vector2(parentRealX, parentRealY),
                parentDimensions: new Vector2(parentWidth, parentHeight),
                offsets: new Vector2(xOffset, yOffset));
            RenderableCoordinate coord = sut.GetRenderableCoordinate();
            Assert.Equal(coord.X, expectedX);
            Assert.Equal(coord.Y, expectedY);
            Assert.Equal(coord.X, expectedX);
            Assert.Equal(coord.Width, expectedWidth);
            Assert.Equal(coord.Height, expectedHeight);
        }
    }
}
