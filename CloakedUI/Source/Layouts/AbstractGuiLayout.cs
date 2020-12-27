using System.Collections;
using System.Collections.Generic;
using ClkdUI.Assets;
using ClkdUI.Main;

namespace ClkdUI.Layouts
{
    public abstract class AbstractGuiLayout : IEnumerable<AbstractGuiComponent>
    {
        internal bool Dirty { get; set; }
        internal abstract void RecalculateChildren(GuiContainer parent);
        public abstract IEnumerator<AbstractGuiComponent> GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}