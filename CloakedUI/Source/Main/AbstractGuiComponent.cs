using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Clkd.Assets;
using Clkd.Main;
using ClkdUI.Assets;
using ClkdUI.Exceptions;
using System.Linq;
using ClkdUI.Assets.SubComponents;

namespace ClkdUI.Main
{
    public abstract class AbstractGuiComponent : AbstractComponent
    {
        public GuiCoordinate Coordinate { get; set; }
        protected bool _positionInitialized = false;
        private bool _inputInitialized = false;
        public GuiInput Input { get; }
        public GuiDirectionalVector4 Margin { get; }
        public GuiDirectionalVector4 Padding { get; }
        public GuiDimensions Dimensions { get; set; } = new GuiDimensions();

        public bool Focused
        {
            get
            {
                if (_inputInitialized)
                {
                    return Input.Focused;
                }
                return false;
            }
            internal set
            {

                Input.Focused = value;

            }
        }

        public AbstractGuiComponent() : base(canGetRenderables: true, canUpdate: true)
        {
            Coordinate = new GuiCoordinate(child: this);
            Margin = new GuiDirectionalVector4();
            Padding = new GuiDirectionalVector4();
            Dimensions = new GuiDimensions();
            Input = new GuiInput(this);
        }

        public abstract override List<Renderable> GetRenderables(RenderableCoordinate? renderableCoordinate = null);

        public abstract override void Update(GameTime gameTime);
        internal virtual void UpdateInternal(GameTime gameTime)
        {
            if (!_inputInitialized && _positionInitialized)
            {
                GetRootPane().GuiInputManager.AddGuiInput(Input);
                _inputInitialized = true;
            }
            Update(gameTime);
        }

        // Because some internal classes need to override this 
        // method, it is virtual and internal. A proxy method is then
        // provided to expost unfocusing to external use.
        internal virtual void UnfocusInternal()
        {
            Focused = false;
        }

        public void Unfocus()
        {
            UnfocusInternal();
        }

        // Because some internal classes need to override this 
        // method, it is virtual and internal. A proxy method is then
        // provided to expost unfocusing to external use.
        internal virtual void FocusInternal()
        {
            List<AbstractGuiComponent> focused = GetHierarchy();
            IEnumerable<AbstractGuiComponent> unfocused = GetRootPane().GetFocusedInternal().Except(focused);
            foreach (AbstractGuiComponent guiComponent in unfocused)
            {
                guiComponent.Focused = false;
            }

            foreach (AbstractGuiComponent guiComponent in focused)
            {
                guiComponent.Focused = true;
            }
        }

        public void Focus()
        {
            FocusInternal();
        }

        public List<AbstractGuiComponent> GetHierarchy()
        {
            List<AbstractGuiComponent> components = new List<AbstractGuiComponent>();
            components.Add(this);
            if (!_positionInitialized)
            {
                GuiCoordinate currentCoordinate = Coordinate;
                while (currentCoordinate.Parent != null)
                {
                    components.Add(currentCoordinate.Parent);
                    currentCoordinate = currentCoordinate.Parent.Coordinate;
                }
            }
            return components;
        }

        internal virtual void UpdatePosition(AbstractGuiComponent parent, Vector2 offsets)
        {
            if (!_positionInitialized) _positionInitialized = true;
            Coordinate.UpdateCoordinate(
                parent: parent,
                offsets: offsets);
        }

        internal virtual void UpdatePosition(GuiCoordinate guiCoordinate, Vector2 offsets)
        {
            if (!_positionInitialized) _positionInitialized = true;
            Coordinate.UpdateCoordinate(
                guiCoordinate: guiCoordinate,
                offsets: offsets);
        }

        internal void Remove()
        {
            if (_inputInitialized)
            {
                GetRootPane().GuiInputManager.RemoveGuiInput(Input);
            }
        }

        private GuiPane GetRootPane()
        {
            if (!_positionInitialized) throw new OrphanedGuiComponentException("This component has not been positioned by a parent, and therefore has no reference to a parent.");
            GuiCoordinate currentCoordinate = Coordinate;
            while (currentCoordinate.Parent != null)
            {
                currentCoordinate = currentCoordinate.Parent.Coordinate;
            }

            if (currentCoordinate.Child is GuiPane gp)
            {
                return gp;
            }

            throw new OrphanedGuiComponentException("Could not reach a GuiPane instance by traversing up the hierarchy.");
        }
    }
}