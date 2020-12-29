using System;
using Clkd.Assets;
using ClkdUI.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ClkdUI.Assets
{

    public delegate void FocusEventHandler(AbstractGuiComponent sender);
    public delegate void MouseEventHandler(AbstractGuiComponent sender, MouseStatus e);
    public delegate void KeyEventHandler(AbstractGuiComponent sender, KeyStatus e);
    public delegate void TextInputEventHandler(AbstractGuiComponent sender, TextInputEventArgs args);

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
                    _focused = value;
                    if (value && OnFocus != null) OnFocus(Subject);
                    else if (OnUnFocus != null) OnUnFocus(Subject);
                }
            }
        }
        public event FocusEventHandler OnFocus;
        public event FocusEventHandler OnUnFocus;
        public event FocusEventHandler OnIndirectFocus;
        public event FocusEventHandler OnIndirectUnFocus;
        public event KeyEventHandler OnKeyPressed;
        public event KeyEventHandler OnKeyHeld;
        public event KeyEventHandler OnKeyReleased;
        public event TextInputEventHandler OnTextEntered;
        public event MouseEventHandler OnMouseClicked;
        public event MouseEventHandler OnMouseHeld;
        public event MouseEventHandler OnMouseReleased;
        public event MouseEventHandler OnMouseEnter;
        public event MouseEventHandler OnMouseHover;
        public event MouseEventHandler OnMouseExit;

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

        internal bool IsHovered(MouseStateWrapper state)
        {
            if (state == null) return false;
            return Subject.GuiCoordinate.Bounds.Contains(state.X, state.Y);
        }

        internal void PublishOnKeyPressed(KeyStatus status)
        {
            if (OnKeyPressed != null) OnKeyPressed(Subject, status);
        }

        internal void PublishOnTextEntered(object sender, TextInputEventArgs textInputEventArgs)
        {
            if (OnTextEntered != null) OnTextEntered(Subject, textInputEventArgs);
        }

        internal void PublishOnKeyHeld(KeyStatus status)
        {
            if (OnKeyHeld != null) OnKeyHeld(Subject, status);
        }

        internal void PublishOnKeyReleased(KeyStatus status)
        {
            if (OnKeyReleased != null) OnKeyReleased(Subject, status);
        }

        internal void PublishOnMouseEnter(MouseStatus mouseStatus)
        {
            if (OnMouseEnter != null) OnMouseEnter(Subject, mouseStatus);
        }

        internal void PublishOnMouseHover(MouseStatus mouseStatus)
        {
            if (OnMouseHover != null) OnMouseHover(Subject, mouseStatus);
        }

        internal void PublishOnMouseExit(MouseStatus mouseStatus)
        {
            if (OnMouseExit != null) OnMouseExit(Subject, mouseStatus);
        }

        internal void PublishOnMouseClicked(MouseStatus mouseStatus)
        {
            if (OnMouseClicked != null) OnMouseClicked(Subject, mouseStatus); ;
        }

        internal void PublishOnMouseHeld(MouseStatus mouseStatus)
        {
            if (OnMouseHeld != null) OnMouseHeld(Subject, mouseStatus);
        }

        internal void PublishOnMouseReleased(MouseStatus mouseStatus)
        {
            if (OnMouseReleased != null) OnMouseReleased(Subject, mouseStatus);
        }
    }
}