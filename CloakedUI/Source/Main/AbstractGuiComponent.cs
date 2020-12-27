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
        public float RealWidth
        {
            get => GuiCoordinate.RealWidth;
        }
        public float RealHeight
        {
            get => GuiCoordinate.RealHeight;
        }

        public AbstractGuiComponent() : base(canGetRenderables: true, canUpdate: true)
        {
            GuiCoordinate = new GuiCoordinate(0, 0, 0, 0, 0, 0, this);
            Guid = Guid.NewGuid();
        }

        public abstract override List<Renderable> GetRenderables(RenderableCoordinate? renderableCoordinate = null);

        public abstract override void Update(GameTime gameTime);

        public virtual void UpdatePosition(GuiContainer parent, float xOffset, float yOffset)
        {
            GuiCoordinate.UpdateCoordinate(
                parent: parent,
                xOffset: xOffset,
                yOffset: yOffset);
        }

        public virtual void UpdatePosition(GuiCoordinate guiCoordinate, float xOffset, float yOffset)
        {
            GuiCoordinate.UpdateCoordinate(
                guiCoordinate: guiCoordinate,
                xOffset: xOffset,
                yOffset: yOffset);
        }

        private GuiPane GetRootPane()
        {
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