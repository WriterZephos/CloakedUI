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
        internal void UpdateInternal(GameTime gameTime)
        {
            if (_guiInput.IsValueCreated && !_inputInitialized && _positionInitialized)
            {
                GetRootPane().GuiInputManager.AddGuiInput(Input);
                _inputInitialized = true;
            }
            Update(gameTime);
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
    }
}