using System;
using ClkdUI.Assets.Interfaces;
using Microsoft.Xna.Framework;

namespace ClkdUI.Assets.SubComponents
{
    public class GuiDimensions : ICollapsableState
    {
        public bool HasRelativeWidth { get; private set; }
        private float? _x;
        public float X
        {
            get => _x ?? _collapseTo?.X ?? 0f;
            set
            {
                if (_collapseTo == null)
                {
                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException("Width", "Width must be greater than zero.");
                    }
                    else if (value <= 1)
                    {
                        HasRelativeWidth = true;
                    }
                    else
                    {
                        HasRelativeWidth = false;
                    }
                    _x = value;
                }
                else
                {
                    _collapseTo.X = value;
                }
            }
        }
        public bool HasRelativeHeight { get; private set; }
        private float? _y;
        public float Y
        {
            get => _y ?? _collapseTo?.Y ?? 0f;
            set
            {
                if (_collapseTo == null)
                {
                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException("Height", "Height must be greater than zero.");
                    }
                    else if (value <= 1)
                    {
                        HasRelativeHeight = true;
                    }
                    else
                    {
                        HasRelativeHeight = false;
                    }
                    _y = value;
                }
                else
                {
                    _collapseTo.Y = value;
                }
            }
        }

        private GuiDimensions _collapseTo;

        public ICollapsableState Collapse(ICollapsableState other)
        {
            if (other is GuiDimensions temp) _collapseTo = temp;
            else _collapseTo = null;
            return this;
        }

        public ICollapsableState UnCollapse()
        {
            _collapseTo = null;
            return this;
        }
    }
}