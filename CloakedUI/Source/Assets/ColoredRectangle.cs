using System.Collections.Generic;
using Clkd.Assets;
using Clkd.Assets.SubComponents;
using ClkdUI.Assets.Interfaces;
using ClkdUI.Assets.SubComponents;
using ClkdUI.Main;
using Microsoft.Xna.Framework;

namespace ClkdUI.Assets
{
    //TODO: Extract a parent class that is just a rect, 
    //then add support for custom textures and rename this class or make a similar class
    //called TexturedRectangle
    public class ColoredRectangle : AbstractGuiComponent, IBackgroundComponent, IBorderComponent
    {
        public int Radius { get; set; }
        public Background Background { get; set; }
        public Border Border { get; set; }
        public float EdgeBlurr { get; set; }

        public ColoredRectangle(float width, float height, Color color) : base()
        {
            Width = width;
            Height = height;
            Background = new Background();
            Background.Color = color;
        }
        public override List<Renderable> GetRenderables(RenderableCoordinate? renderableCoordinate = null)
        {
            return Background.GetRenderables(this);
        }

        public override void Update(GameTime gameTime)
        {
            return;
        }
    }
}