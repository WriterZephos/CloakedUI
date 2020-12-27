using System.Collections.Generic;
using System.Linq;
using Clkd.Assets;
using ClkdUI.Layouts;
using ClkdUI.Main;
using Microsoft.Xna.Framework;

namespace ClkdUI.Assets
{
    public class GuiContainer : AbstractGuiComponent
    {
        public AbstractGuiLayout Layout { get; set; }

        public GuiContainer(AbstractGuiLayout layout)
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
                RecalculateChildren();
            }

            foreach (AbstractGuiComponent c in Layout)
            {
                if (c != null) c.UpdateInternal(gameTime);
            }
        }

        public T GetLayout<T>() where T : AbstractGuiLayout
        {
            return Layout as T;
        }

        public void RecalculateChildren()
        {
            Layout.RecalculateChildren(this);
        }
    }
}