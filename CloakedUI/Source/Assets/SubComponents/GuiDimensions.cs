using System;
using Microsoft.Xna.Framework;

namespace ClkdUI.Assets.SubComponents
{
    public class GuiDimensions
    {
        private Vector2 Dimensions;
        public bool HasRelativeWidth { get; private set; }
        public float X
        {
            get => Dimensions.X;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Width", "Width must be greater than zero.");
                }
                Dimensions.X = value;
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
        public float Y
        {
            get => Dimensions.Y;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Height", "Height must be greater than zero.");
                }
                Dimensions.Y = value;
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
    }
}