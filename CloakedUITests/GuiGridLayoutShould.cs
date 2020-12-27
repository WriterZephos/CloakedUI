using ClkdUI.Assets;
using ClkdUI.Layouts;
using Microsoft.Xna.Framework;
using Xunit;

namespace CloakedUITests
{
    public class GuiGridLayoutShould
    {
        private GuiGridLayout layout;
        public GuiGridLayoutShould()
        {
            layout = new GuiGridLayout(10, 10);
            var nestedLayout = new GuiGridLayout(10, 10);
            ColoredRectangle rect = new ColoredRectangle(50f, 50f, Color.Aqua);
            var nested = new GuiContainer(nestedLayout);
            nested.GetLayout<GuiGridLayout>().AddComponent(0, 0, rect);
            layout.AddComponent(0, 0, nested);
        }

        [Fact]
        public void AddComponentCorrectly()
        {
            ColoredRectangle rect = new ColoredRectangle(50f, 50f, Color.Aqua);
            layout.AddComponent(1, 0, rect);
            Assert.Equal(layout.Components[1, 0], rect);
        }

        public void GetComponentCorrectly()
        {

        }

        public void GetEnumerator()
        {

        }

        public void RecalculateChildrenCorrectly()
        {

        }

        public void GetLayout()
        {

        }

        public void BeDirtyAfterHorizontalGutterChange()
        {

        }

        public void BeDirtyAfterVerticalGutterChange()
        {

        }

        public void BeDirtyAfterAddComponent()
        {

        }
    }
}