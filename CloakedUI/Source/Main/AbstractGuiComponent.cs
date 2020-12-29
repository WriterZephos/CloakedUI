using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Clkd.Assets;
using Clkd.Main;
using ClkdUI.Assets;
using ClkdUI.Exceptions;
using ClkdUI.Main;

namespace ClkdUI.Main
{
    public abstract class AbstractGuiComponent : AbstractComponent
    {
        public Guid Guid { get; set; }
        public GuiCoordinate GuiCoordinate { get; set; }
        private bool _positionInitialized = false;
        private Lazy<GuiInput> _guiInput;
        private bool _inputInitialized = false;
        public GuiInput Input
        {
            get
            {
                return _guiInput.Value;
            }
        }
        public float LeftMargin { get; set; }
        public float RightMargin { get; set; }
        public float TopMargin { get; set; }
        public float BottomMargin { get; set; }
        public float LeftPadding { get; set; }
        public float RightPadding { get; set; }
        public float TopPadding { get; set; }
        public float BottomPadding { get; set; }
        public bool HasRelativeWidth { get; private set; }
        private float _width;
        public float Width
        {
            get => _width;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Width", "Width must be greater than zero.");
                }
                _width = value;
                if (value <= 1)
                {
                    HasRelativeWidth = true;
                }
                else
                {
                    HasRelativeWidth = false;
                }
            }
        }
        public bool HasRelativeHeight { get; private set; }
        private float _height;
        public float Height
        {
            get => _height;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Height", "Height must be greater than zero.");
                }
                _height = value;
                if (value <= 1)
                {
                    HasRelativeHeight = true;
                }
                else
                {
                    HasRelativeHeight = false;
                }
            }
        }
        public int ZIndex
        {
            get => GuiCoordinate.ZIndex;
            set => GuiCoordinate.ZIndex = value;
        }
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
                if (_inputInitialized)
                {
                    Input.Focused = value;
                }
            }
        }

        internal float RealWidth
        {
            get => GuiCoordinate.Dimensions.X;
        }
        internal float RealHeight
        {
            get => GuiCoordinate.Dimensions.Y;
        }

        public AbstractGuiComponent() : base(canGetRenderables: true, canUpdate: true)
        {
            GuiCoordinate = new GuiCoordinate(child: this);
            _guiInput = new Lazy<GuiInput>(() => { return new GuiInput(this); });
            Guid = Guid.NewGuid();
        }

        public abstract override List<Renderable> GetRenderables(RenderableCoordinate? renderableCoordinate = null);


        public abstract override void Update(GameTime gameTime);
        internal virtual void UpdateInternal(GameTime gameTime)
        {
            if (_guiInput.IsValueCreated && !_inputInitialized && _positionInitialized)
            {
                GetRootPane().GuiInputManager.AddGuiInput(Input);
                _inputInitialized = true;
            }
            Update(gameTime);
        }

        // Because GuiContainer needs to override this method, it is
        // virtual and internal. A proxy method is then provided
        // to expost unfocusing to external use.
        internal virtual void UnfocusInternal()
        {
            Focused = false;
        }

        public void Unfocus()
        {
            UnfocusInternal();
        }

        public virtual void Focus()
        {
            Focused = true;
            foreach (AbstractGuiComponent guiComponent in GetAncestors())
            {
                guiComponent.Focused = true;
            }
        }

        internal virtual void UpdatePosition(GuiContainer parent, Vector2 offsets)
        {
            if (!_positionInitialized) _positionInitialized = true;
            GuiCoordinate.UpdateCoordinate(
                parent: parent,
                offsets: offsets);
        }

        internal virtual void UpdatePosition(GuiCoordinate guiCoordinate, Vector2 offsets)
        {
            if (!_positionInitialized) _positionInitialized = true;
            GuiCoordinate.UpdateCoordinate(
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
            GuiCoordinate currentCoordinate = GuiCoordinate;
            while (currentCoordinate.Parent != null)
            {
                currentCoordinate = currentCoordinate.Parent.GuiCoordinate;
            }

            if (currentCoordinate.Child is GuiPane gp)
            {
                return gp;
            }

            throw new OrphanedGuiComponentException("Could not reach a GuiPane instance by traversing up the hierarchy.");
        }

        private List<AbstractGuiComponent> GetAncestors()
        {
            List<AbstractGuiComponent> components = new List<AbstractGuiComponent>();
            components.Add(this);
            if (!_positionInitialized)
            {
                GuiCoordinate currentCoordinate = GuiCoordinate;
                while (currentCoordinate.Parent != null)
                {
                    components.Add(currentCoordinate.Parent);
                    currentCoordinate = currentCoordinate.Parent.GuiCoordinate;
                }
            }
            return components;
        }
    }
}