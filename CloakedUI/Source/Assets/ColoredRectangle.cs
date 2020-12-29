using System.Collections.Generic;
using Clkd.Assets;
using ClkdUI.Main;
using Microsoft.Xna.Framework;

namespace ClkdUI.Assets
{
    //TODO: Extract a parent class that is just a rect, 
    //then add support for custom textures and rename this class or make a similar class
    //called TexturedRectangle
    public class ColoredRectangle : AbstractGuiComponent
    {
        private Color _color;
        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                Sprite = new Sprite(_color);
            }
        }
        public Sprite Sprite { get; set; }
        public ColoredRectangle(float width, float height, Color color) : base()
        {
            Width = width;
            Height = height;
            Color = color;
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