using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Clkd.Main;
using System;

namespace ClkdUI.Support
{
    public static class Utilities
    {

        public static Texture2D GetEmptyTexture(int width, int height, Color color = default(Color))
        {
            Texture2D texture = new Texture2D(Cloaked.GraphicsDeviceManager.GraphicsDevice, width, height);
            Color[] data = new Color[width * height];
            for (int i = 0; i < width * height; i++) data[i] = color;
            texture.SetData(data);
            return texture;
        }
        public static void DecorateTextureEdges(Texture2D texture, int radius, float edgeBlurr = 1f)
        {
            if (radius > 0)
            {
                Point rectSize = new Point(radius, radius);
                Vector2 topLeftOrigin = new Vector2(radius, radius);
                Point topLeftRectPosition = new Point(0, 0);
                RoundCorner(texture, radius, new Rectangle(topLeftRectPosition, rectSize), topLeftOrigin, edgeBlurr);

                Vector2 topRightOrigin = new Vector2(-1, radius);
                Point topRightRectPosition = new Point(texture.Width - radius, 0);
                RoundCorner(texture, radius, new Rectangle(topRightRectPosition, rectSize), topRightOrigin, edgeBlurr);

                Vector2 bottomLeftOrigin = new Vector2(radius, -1);
                Point bottomLeftRectPosition = new Point(0, texture.Height - radius);
                RoundCorner(texture, radius, new Rectangle(bottomLeftRectPosition, rectSize), bottomLeftOrigin, edgeBlurr);

                Vector2 bottomRightOrigin = new Vector2(-1, -1);
                Point bottomRightRectPosition = new Point(texture.Width - radius, texture.Height - radius);
                RoundCorner(texture, radius, new Rectangle(bottomRightRectPosition, rectSize), bottomRightOrigin, edgeBlurr);
            }

            Point horizontalEdgeRectSize = new Point(texture.Width - (radius * 2), (int)edgeBlurr);
            Point topRectPosition = new Point(radius, 0);
            SmoothEdge(texture, new Rectangle(topRectPosition, horizontalEdgeRectSize), edgeBlurr, (x, y, quadrant) => ((float)y) / quadrant.Height);

            Point bottomRectPosition = new Point(radius, (int)(texture.Height - edgeBlurr));
            SmoothEdge(texture, new Rectangle(bottomRectPosition, horizontalEdgeRectSize), edgeBlurr, (x, y, quadrant) => ((float)quadrant.Height - 1 - y) / quadrant.Height);

            Point verticalEdgeRectSize = new Point((int)edgeBlurr, texture.Height - (radius * 2));
            Point leftRectPosition = new Point(0, radius);
            SmoothEdge(texture, new Rectangle(leftRectPosition, verticalEdgeRectSize), edgeBlurr, (x, y, quadrant) => ((float)x) / quadrant.Width);

            Point rightRectPosition = new Point((int)(texture.Width - edgeBlurr), radius);
            SmoothEdge(texture, new Rectangle(rightRectPosition, verticalEdgeRectSize), edgeBlurr, (x, y, quadrant) => ((float)quadrant.Width - 1 - x) / quadrant.Width);
        }

        private static void RoundCorner(Texture2D texture, int radius, Rectangle quadrant, Vector2 origin, float edgeBlurr)
        {
            Color[] data = new Color[radius * radius];
            texture.GetData(0, quadrant, data, 0, radius * radius);
            for (int y = radius - 1; y >= 0; y--)
            {
                for (int x = 0; x < radius; x++)
                {
                    Vector2 currentPixel = new Vector2(x, y);
                    float distance = Vector2.Distance(currentPixel, origin);
                    if (distance >= radius)
                    {
                        data[y * radius + x].A = 0;
                    }
                    else if (distance > radius - edgeBlurr)
                    {
                        float multiplier = (radius - distance) / edgeBlurr;
                        data[y * radius + x].A = (byte)(byte.MaxValue * multiplier);
                    }
                }
            }
            texture.SetData<Color>(0, quadrant, data, 0, radius * radius);
        }

        private static void SmoothEdge(Texture2D texture, Rectangle quadrant, float edgeBlurr, Func<float, float, Rectangle, float> multiplierFunction)
        {
            if (quadrant.Width == 0 || quadrant.Height == 0) return;
            Color[] data = new Color[quadrant.Width * quadrant.Height];
            texture.GetData(0, quadrant, data, 0, quadrant.Width * quadrant.Height);
            for (int y = quadrant.Height - 1; y >= 0; y--)
            {
                for (int x = 0; x < quadrant.Width; x++)
                {
                    float multiplier = multiplierFunction(x, y, quadrant);
                    byte alpha = (byte)(byte.MaxValue * multiplier);
                    data[y * quadrant.Width + x].A = data[y * quadrant.Width + x].A < alpha ? data[y * quadrant.Width + x].A : alpha;
                }
            }
            texture.SetData<Color>(0, quadrant, data, 0, quadrant.Width * quadrant.Height);
        }
    }
}