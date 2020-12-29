using System;
using ClkdUI.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ClkdUI.Assets
{

    public delegate void MouseEventHandler(object sender, EventArgs e);

    public class GuiInput : IComparable<GuiInput>
    {
        internal AbstractGuiComponent Subject { get; set; }
        private bool _focused;
        internal bool Focused
        {
            get => _focused;
            set
            {
                if (value != _focused)
                {
                    // TODO: Trigger onfocus and onunfocus events here.
                }
                _focused = value;
            }
        }
        public event MouseEventHandler OnClick;
        public event MouseEventHandler OnMouseEnter;
        public event MouseEventHandler OnMouseHover;

        internal GuiInput(AbstractGuiComponent subject)
        {
            Subject = subject;
        }

        public int CompareTo(GuiInput other)
        {
            if (other == null) return -1;

            // This GuiInput will go before the other in event handling order, so higher values
            // get to respond to events first.
            if (Subject.ZIndex > other.Subject.ZIndex) return -1;

            // This GuiInput will follow the other in event handling order, so higher values
            // get to respond to events first.
            if (Subject.ZIndex < other.Subject.ZIndex) return 1;

            return 0;
        }
    }
}