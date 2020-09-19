using System.Collections;
using System.Collections.Generic;

namespace Clkd.GUI.Interfaces
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