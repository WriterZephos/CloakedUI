using System.Collections.Generic;
using System.Linq;
using Clkd.Assets;
using Clkd.GUI.Interfaces;
using Clkd.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Clkd.GUI
{
    public class GuiContainer : AbstractGuiComponent
    {
        public AbstractGuiLayout Layout { get; set; }

        GuiContainer(GuiCoordinate guiCoordinate, AbstractGuiLayout layout) : base(guiCoordinate)
        {
            Layout = layout;
        }

        public override List<Renderable> GetRenderables(RenderableCoordinate? renderableCoordinate = null)
        {
            return Layout
                .Select(child => child.GetRenderables())
                .Where((list) => list != null).Aggregate(
                    new List<Renderable>(),
                    (finalList, list) =>
                    {
                        finalList.AddRange(list);
                        return finalList;
                    }
                );
        }

        public override void Update(GameTime gameTime)
        {
            if (Layout.Dirty)
            {
                Layout.RecalculateChildren(this);
            }

            foreach (AbstractGuiComponent c in Layout)
            {
                c.Update(gameTime);
            }
        }
    }
}