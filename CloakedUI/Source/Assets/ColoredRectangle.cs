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
    public class ColoredRectangle : AbstractInputGuiComponent
    {


        public ColoredRectangle(float width, float height, Color color)
        {
            Dimensions.X = width;
            Dimensions.Y = height;
            Background.Color = color;
        }
        public override List<IRenderable> GetRenderables(RenderableCoordinate? renderableCoordinate = null)
        {
            List<IRenderable> renderables = new List<IRenderable>();
            Vector3 internalOffsets = new Vector3(0, 0, 0);
            if (Border != null)
            {
                internalOffsets.Z = 2;
                renderables.Condense(Border.GetRenderables(this, internalOffsets));
                internalOffsets.X = Edges.EdgeBlurr;
                internalOffsets.Y = Edges.EdgeBlurr;
            }

            if (Background != null)
            {
                internalOffsets.Z = 1;
                renderables.Condense(Background.GetRenderables(this, internalOffsets, Border == null));
            }

            if (Text != null)
            {
                internalOffsets.Z = 3;
                if (Border != null)
                {
                    internalOffsets.X = Edges.EdgeBlurr + Edges.InnerEdgeBlurr + Border.Width;
                    internalOffsets.Y = Edges.EdgeBlurr + Edges.InnerEdgeBlurr + Border.Width;
                }
                renderables.Condense(Text.GetRenderables(this, internalOffsets));
            }

            return renderables;
        }

        public override void Update(GameTime gameTime)
        {
            return;
        }
    }
}