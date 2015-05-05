using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace OverworldGenerator
{
    public class PerlinMatrix : Matrix
    {
        private int thresh;

        public PerlinMatrix(int cols, int rows, int thresh)
            : base(cols, rows)
        {
            this.thresh = thresh;
        }

        public double Min()
        {
            double min = Double.MaxValue;
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; ++y)
                {
                    if (Get(x, y) < min)
                    {
                        min = Get(x, y);
                    }
                }
            }
            return min;
        }

        public double Max()
        {
            double max = Double.MinValue;
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; ++y)
                {
                    if (Get(x, y) > max)
                    {
                        max = Get(x, y);
                    }
                }
            }
            return max;
        }

        private Color[] DoubleToColor()
        {
            double min = Min();
            double max = Max();
            double steps = 255.0 / (max - min);
            int t = 255 / thresh;

            Color[] pixels = new Color[rows * cols];
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    int q = (int)((Math.Abs(min - Get(x, y))) * steps);
                    int level = (q / t) * t;
                    pixels[y * cols + x] = new Color(level, level, level, 255);
                }
            }
            return pixels;
        }

        public Texture2D GetImage(GraphicsDevice graphics)
        {
            Texture2D tex = new Texture2D(graphics, cols, rows);
            Color[] pixels = DoubleToColor();
            tex.SetData(pixels);
            return tex;
        }
    }
}
