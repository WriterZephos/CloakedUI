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

        public static void CurveAndBlurrEdgesOutward(Texture2D texture, int radius, float edgeBlurr = 1f)
        {
            int maxRadius = Math.Min(texture.Width, texture.Height) / 2;
            int finalRadius = radius <= maxRadius ? radius : maxRadius;
            if (radius > 0)
            {
                Point rectSize = new Point(radius, radius);
                Vector2 topLeftOrigin = new Vector2(radius, radius);
                Point topLeftRectPosition = new Point(0, 0);
                BlurrCornerOutward(texture, radius, new Rectangle(topLeftRectPosition, rectSize), topLeftOrigin, edgeBlurr);

                Vector2 topRightOrigin = new Vector2(-1, radius);
                Point topRightRectPosition = new Point(texture.Width - radius, 0);
                BlurrCornerOutward(texture, radius, new Rectangle(topRightRectPosition, rectSize), topRightOrigin, edgeBlurr);

                Vector2 bottomLeftOrigin = new Vector2(radius, -1);
                Point bottomLeftRectPosition = new Point(0, texture.Height - radius);
                BlurrCornerOutward(texture, radius, new Rectangle(bottomLeftRectPosition, rectSize), bottomLeftOrigin, edgeBlurr);

                Vector2 bottomRightOrigin = new Vector2(-1, -1);
                Point bottomRightRectPosition = new Point(texture.Width - radius, texture.Height - radius);
                BlurrCornerOutward(texture, radius, new Rectangle(bottomRightRectPosition, rectSize), bottomRightOrigin, edgeBlurr);
            }

            Point horizontalEdgeRectSize = new Point(texture.Width - (radius * 2), (int)edgeBlurr);
            Point topRectPosition = new Point(radius, 0);
            BlurrEdgeOutward(texture, new Rectangle(topRectPosition, horizontalEdgeRectSize), edgeBlurr, (x, y, quadrant) => ((float)y) / quadrant.Height);

            Point bottomRectPosition = new Point(radius, (int)(texture.Height - edgeBlurr));
            BlurrEdgeOutward(texture, new Rectangle(bottomRectPosition, horizontalEdgeRectSize), edgeBlurr, (x, y, quadrant) => ((float)quadrant.Height - 1 - y) / quadrant.Height);

            Point verticalEdgeRectSize = new Point((int)edgeBlurr, texture.Height - (radius * 2));
            Point leftRectPosition = new Point(0, radius);
            BlurrEdgeOutward(texture, new Rectangle(leftRectPosition, verticalEdgeRectSize), edgeBlurr, (x, y, quadrant) => ((float)x) / quadrant.Width);

            Point rightRectPosition = new Point((int)(texture.Width - edgeBlurr), radius);
            BlurrEdgeOutward(texture, new Rectangle(rightRectPosition, verticalEdgeRectSize), edgeBlurr, (x, y, quadrant) => ((float)quadrant.Width - 1 - x) / quadrant.Width);
        }

        public static void CurveAndBlurrEdgesInnward(Texture2D texture, int radius, float offset, float edgeBlurr = 1f)
        {
            int maxRadius = Math.Min(texture.Width, texture.Height) / 2;
            int finalRadius = radius <= maxRadius ? radius : maxRadius;
            if (radius > 0)
            {
                Point rectSize = new Point(radius, radius);
                Vector2 topLeftOrigin = new Vector2(radius, radius);
                Point topLeftRectPosition = new Point(0, 0);
                BlurrCornerInnward(texture, radius, new Rectangle(topLeftRectPosition, rectSize), topLeftOrigin, edgeBlurr, offset);

                Vector2 topRightOrigin = new Vector2(-1, radius);
                Point topRightRectPosition = new Point(texture.Width - radius, 0);
                BlurrCornerInnward(texture, radius, new Rectangle(topRightRectPosition, rectSize), topRightOrigin, edgeBlurr, offset);

                Vector2 bottomLeftOrigin = new Vector2(radius, -1);
                Point bottomLeftRectPosition = new Point(0, texture.Height - radius);
                BlurrCornerInnward(texture, radius, new Rectangle(bottomLeftRectPosition, rectSize), bottomLeftOrigin, edgeBlurr, offset);

                Vector2 bottomRightOrigin = new Vector2(-1, -1);
                Point bottomRightRectPosition = new Point(texture.Width - radius, texture.Height - radius);
                BlurrCornerInnward(texture, radius, new Rectangle(bottomRightRectPosition, rectSize), bottomRightOrigin, edgeBlurr, offset);
            }

            Point horizontalEdgeRectSize = new Point(texture.Width - (radius * 2), (int)edgeBlurr);
            Point topRectPosition = new Point(radius, (int)offset);
            BlurrEdgeInnward(texture, new Rectangle(topRectPosition, horizontalEdgeRectSize), edgeBlurr, (x, y, quadrant) => ((float)y) / quadrant.Height);

            Point bottomRectPosition = new Point(radius, (int)(texture.Height - edgeBlurr - offset));
            BlurrEdgeInnward(texture, new Rectangle(bottomRectPosition, horizontalEdgeRectSize), edgeBlurr, (x, y, quadrant) => ((float)quadrant.Height - 1 - y) / quadrant.Height);

            Point verticalEdgeRectSize = new Point((int)edgeBlurr, texture.Height - (radius * 2));
            Point leftRectPosition = new Point((int)offset, radius);
            BlurrEdgeInnward(texture, new Rectangle(leftRectPosition, verticalEdgeRectSize), edgeBlurr, (x, y, quadrant) => ((float)x) / quadrant.Width);

            Point rightRectPosition = new Point((int)(texture.Width - edgeBlurr - offset), radius);
            BlurrEdgeInnward(texture, new Rectangle(rightRectPosition, verticalEdgeRectSize), edgeBlurr, (x, y, quadrant) => ((float)quadrant.Width - 1 - x) / quadrant.Width);
        }

        private static void BlurrCornerOutward(Texture2D texture, int radius, Rectangle quadrant, Vector2 origin, float edgeBlurr)
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

        private static void BlurrCornerInnward(Texture2D texture, int radius, Rectangle quadrant, Vector2 origin, float edgeBlurr, float offset)
        {
            Color[] data = new Color[radius * radius];
            texture.GetData(0, quadrant, data, 0, radius * radius);
            for (int y = radius - 1; y >= 0; y--)
            {
                for (int x = 0; x < radius; x++)
                {
                    Vector2 currentPixel = new Vector2(x, y);
                    float distance = Vector2.Distance(currentPixel, origin);
                    if (distance < radius - offset)
                    {
                        if (distance > radius - offset - edgeBlurr)
                        {
                            float multiplier = 1f - (radius - distance - offset) / edgeBlurr;
                            data[y * radius + x].A = (byte)(byte.MaxValue * multiplier);
                        }
                        else
                        {
                            data[y * radius + x].A = 0;
                        }
                    }
                }
            }
            texture.SetData<Color>(0, quadrant, data, 0, radius * radius);
        }

        private static void BlurrEdgeOutward(Texture2D texture, Rectangle quadrant, float edgeBlurr, Func<float, float, Rectangle, float> multiplierFunction)
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

        private static void BlurrEdgeInnward(Texture2D texture, Rectangle quadrant, float edgeBlurr, Func<float, float, Rectangle, float> multiplierFunction)
        {
            if (quadrant.Width == 0 || quadrant.Height == 0) return;
            Color[] data = new Color[quadrant.Width * quadrant.Height];
            texture.GetData(0, quadrant, data, 0, quadrant.Width * quadrant.Height);
            for (int y = quadrant.Height - 1; y >= 0; y--)
            {
                for (int x = 0; x < quadrant.Width; x++)
                {
                    float multiplier = 1f - multiplierFunction(x, y, quadrant);
                    byte alpha = (byte)(byte.MaxValue * multiplier);
                    data[y * quadrant.Width + x].A = data[y * quadrant.Width + x].A < alpha ? data[y * quadrant.Width + x].A : alpha;
                }
            }
            texture.SetData<Color>(0, quadrant, data, 0, quadrant.Width * quadrant.Height);
        }

        public static void DrawBorder(Texture2D texture, int radius, Color color, int width, float edgeBlurr = 1f, float innerEdgeBlurr = 1f)
        {
            int maxRadius = Math.Min(texture.Width, texture.Height) / 2;
            int finalRadius = radius <= maxRadius ? radius : maxRadius;
            if (radius > 0)
            {
                float fullWidth = edgeBlurr + width + innerEdgeBlurr;
                Point rectSize = new Point(radius, radius);
                Vector2 topLeftOrigin = new Vector2(radius, radius);
                Point topLeftRectPosition = new Point(0, 0);
                DrawCornerBorder(texture, radius, new Rectangle(topLeftRectPosition, rectSize), topLeftOrigin, fullWidth, color);

                Vector2 topRightOrigin = new Vector2(-1, radius);
                Point topRightRectPosition = new Point(texture.Width - radius, 0);
                DrawCornerBorder(texture, radius, new Rectangle(topRightRectPosition, rectSize), topRightOrigin, fullWidth, color);

                Vector2 bottomLeftOrigin = new Vector2(radius, -1);
                Point bottomLeftRectPosition = new Point(0, texture.Height - radius);
                DrawCornerBorder(texture, radius, new Rectangle(bottomLeftRectPosition, rectSize), bottomLeftOrigin, fullWidth, color);

                Vector2 bottomRightOrigin = new Vector2(-1, -1);
                Point bottomRightRectPosition = new Point(texture.Width - radius, texture.Height - radius);
                DrawCornerBorder(texture, radius, new Rectangle(bottomRightRectPosition, rectSize), bottomRightOrigin, fullWidth, color);
            }

            Point horizontalEdgeRectSize = new Point(texture.Width - (radius * 2), (int)(edgeBlurr + width + innerEdgeBlurr));
            Point topRectPosition = new Point(radius, 0);
            DrawEdgeBorder(
                texture,
                new Rectangle(topRectPosition, horizontalEdgeRectSize),
                color);

            Point bottomRectPosition = new Point(radius, (int)(texture.Height - edgeBlurr - width - innerEdgeBlurr));
            DrawEdgeBorder(
                texture,
                new Rectangle(bottomRectPosition, horizontalEdgeRectSize),
                color);

            Point verticalEdgeRectSize = new Point((int)(edgeBlurr + width + innerEdgeBlurr), texture.Height - (radius * 2));
            Point leftRectPosition = new Point(0, radius);
            DrawEdgeBorder(
                texture,
                new Rectangle(leftRectPosition, verticalEdgeRectSize),
                color);

            Point rightRectPosition = new Point((int)(texture.Width - edgeBlurr - width - innerEdgeBlurr), radius);
            DrawEdgeBorder(
                texture,
                new Rectangle(rightRectPosition, verticalEdgeRectSize),
                color);

            CurveAndBlurrEdgesOutward(texture, radius, edgeBlurr);
            CurveAndBlurrEdgesInnward(texture, radius, edgeBlurr + width, innerEdgeBlurr);
        }

        private static void DrawCornerBorder(Texture2D texture, int radius, Rectangle quadrant, Vector2 origin, float fullWidth, Color color)
        {
            Color[] data = new Color[radius * radius];
            texture.GetData(0, quadrant, data, 0, radius * radius);
            for (int y = radius - 1; y >= 0; y--)
            {
                for (int x = 0; x < radius; x++)
                {
                    Vector2 currentPixel = new Vector2(x, y);
                    float distance = Vector2.Distance(currentPixel, origin);
                    if (distance < radius)
                    {
                        if (distance > radius - fullWidth)
                        {
                            data[y * radius + x] = color;
                        }
                    }
                }
            }
            texture.SetData<Color>(0, quadrant, data, 0, radius * radius);
        }

        private static void DrawEdgeBorder(
            Texture2D texture,
            Rectangle quadrant,
            Color color
        )
        {
            if (quadrant.Width == 0 || quadrant.Height == 0) return;
            Color[] data = new Color[quadrant.Width * quadrant.Height];
            texture.GetData(0, quadrant, data, 0, quadrant.Width * quadrant.Height);
            for (int y = quadrant.Height - 1; y >= 0; y--)
            {
                for (int x = quadrant.Width - 1; x >= 0; x--)
                {
                    data[y * quadrant.Width + x] = color;
                }
            }
            texture.SetData<Color>(0, quadrant, data, 0, quadrant.Width * quadrant.Height);
        }
    }
}