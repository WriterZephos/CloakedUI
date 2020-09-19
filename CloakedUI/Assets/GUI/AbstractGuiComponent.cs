using System;
using System.Collections.Generic;
using Clkd.Assets;
using Clkd.GUI.Layout;
using Clkd.Main;
using Microsoft.Xna.Framework;

namespace Clkd.GUI
{
    public abstract class AbstractGuiComponent : AbstractComponent
    {
        public GuiCoordinate GuiCoordinate { get; set; }
        public AbstractGuiComponent(GuiCoordinate guiCoordinate = null) : base(canGetRenderables: true, canUpdate: true)
        {
            GuiCoordinate = guiCoordinate != null ? guiCoordinate : new GuiCoordinate(0, 0, 0, 0, 0, 0, this);
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
                if (value < 1)
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
                if (value < 1)
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
        public abstract override List<Renderable> GetRenderables(RenderableCoordinate? renderableCoordinate = null);

        public abstract override void Update(GameTime gameTime);
    }
}