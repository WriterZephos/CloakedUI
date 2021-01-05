using System;
using System.Collections.Generic;
using Clkd.Assets;
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
        private Lazy<Edges> _edges = new Lazy<Edges>();
        public Edges Edges
        {
            get => _edges.Value;
        }
        public Background Background { get; set; }
        public Border Border { get; set; }

        public ColoredRectangle(float width, float height, Color color) : base()
        {
            Dimensions.X = width;
            Dimensions.Y = height;
            //Background = new Background();
            //Border = new Border();
            // Background.Color = color;
        }
        public override List<Renderable> GetRenderables(RenderableCoordinate? renderableCoordinate = null)
        {
            List<Renderable> renderables = new List<Renderable>();
            Vector3 internalOffsets = new Vector3(0, 0, 1);
            if (Border != null)
            {
                renderables.AddRange(Border.GetRenderables(this));
                internalOffsets.X = Edges.EdgeBlurr + Border.Width + Edges.InnerEdgeBlurr;
            }

            if (Background != null)
            {
                renderables.AddRange(Background.GetRenderables(this, internalOffsets, Border != null));
            }

            return renderables;
        }

        public override void Update(GameTime gameTime)
        {
            return;
        }
    }
}