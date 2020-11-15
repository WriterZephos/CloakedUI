using System.Collections.Generic;
using Clkd.Assets;
using Microsoft.Xna.Framework;

namespace Clkd.GUI
{
    public class ColoredRectangle : AbstractGuiComponent
    {
        public Color Color { get; set; }
        public Sprite Sprite { get; set; }
        public ColoredRectangle(float width, float height, Color color) : base(null)
        {
            Width = width;
            Height = height;
            Color = color;
            Sprite = new Sprite(color);
        }
        public override List<Renderable> GetRenderables(RenderableCoordinate? renderableCoordinate = null)
        {
            return Sprite.GetRenderables(GuiCoordinate.GetRenderableCoordinate());
        }

        public override void Update(GameTime gameTime)
        {
            return;
        }
    }
}