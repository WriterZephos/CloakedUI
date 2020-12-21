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

        public GuiContainer(AbstractGuiLayout layout) : base()
        {
            Layout = layout;
        }

        public override List<Renderable> GetRenderables(RenderableCoordinate? renderableCoordinate = null)
        {
            return Layout
                .Where((component) => component != null)
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
                if (c != null) c.Update(gameTime);
            }
        }

        public T GetLayout<T>() where T : AbstractGuiLayout
        {
            return Layout as T;
        }
    }
}