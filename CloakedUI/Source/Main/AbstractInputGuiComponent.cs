using System.Collections.Generic;
using System.Linq;
using Clkd.Assets;
using ClkdUI.Assets;
using ClkdUI.Assets.Interfaces;
using ClkdUI.Assets.SubComponents;
using ClkdUI.SubComponents;
using Microsoft.Xna.Framework;

namespace ClkdUI.Main
{
    public abstract class AbstractInputGuiComponent : AbstractGuiComponent
    {
        public GuiInput Input { get; private set; }
        private bool _inputInitialized = false;
        private HoverComponentState _hover;
        public HoverComponentState Hover
        {
            get => (HoverComponentState)_hover.Uncollapse();
        }

        protected AbstractInputGuiComponent()
        {
        }

        protected override AbstractGuiComponentState SetState()
        {
            Input = new GuiInput(this);
            _hover = new HoverComponentState();
            InternalState = base.SetState().SetBefore(Hover);
            return InternalState;
        }

        internal override void UpdateInternal(GameTime gameTime)
        {
            if (!_inputInitialized && _positionInitialized)
            {
                GetRootPane().GuiInputManager.AddGuiInput(Input);
                _inputInitialized = true;
            }
            base.UpdateInternal(gameTime);
        }

        public void Focus()
        {
            FocusInternal();
        }

        // Because some internal classes need to override this 
        // method, it is virtual and internal. A proxy method is then
        // provided to expost unfocusing to external use.
        internal virtual void UnfocusInternal()
        {
            Input.Focused = false;
        }

        public void Unfocus()
        {
            UnfocusInternal();
        }

        internal void Remove()
        {
            if (_inputInitialized)
            {
                GetRootPane().GuiInputManager.RemoveGuiInput(Input);
            }
        }

        // Because some internal classes need to override this 
        // method, it is virtual and internal. A proxy method is then
        // provided to expost unfocusing to external use.
        internal virtual void FocusInternal()
        {
            List<AbstractGuiComponent> focused = GetHierarchy();
            IEnumerable<AbstractGuiComponent> unfocused = GetRootPane().GetChildren().Except(focused);
            foreach (AbstractGuiComponent guiComponent in unfocused)
            {
                if (guiComponent is AbstractInputGuiComponent temp) temp.Input.Focused = false;
            }

            foreach (AbstractGuiComponent guiComponent in focused)
            {
                if (guiComponent is AbstractInputGuiComponent temp) temp.Input.Focused = true;
            }
        }
    }
}