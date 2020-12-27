using System;
using ClkdUI.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ClkdUI.Assets
{

    public delegate void MouseEventHandler(object sender, EventArgs e);

    public class GuiInput
    {
        public AbstractGuiComponent Subject { get; set; }
        public event MouseEventHandler OnClick;
        public event MouseEventHandler OnMouseEnter;
        public event MouseEventHandler OnMouseHover;

        public GuiInput(AbstractGuiComponent subject)
        {
            Subject = subject;
        }
    }
}