using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Voronoi2
{
    public class PointGenerator
    {
        static int NUM_LLOYD_RELAXATIONS = 5;
        static double[] _xvalues, _yvalues;
        static Voronoi voro;

        public static double[] GetX()
        {
            return _xvalues;
        }

        public static double[] GetY()
        {
            return _yvalues;
        }

        static public void GenerateRandom(int width, int height, int seed, int numPoints)
        {
            _xvalues = new double[numPoints];
            _yvalues = new double[numPoints];
            Random gen = new Random();
            for (int i = 0; i < numPoints; i++) {
                _xvalues[i] = gen.NextDouble() * (width-20) + 10;
                _yvalues[i] = gen.NextDouble() * (height-20) + 10;
            }
            voro = new Voronoi(10);
            voro.generateVoronoi(_xvalues, _yvalues, 0, width, 0, height);
            voro.Regionify();
        }

        static public void GenerateRandom(int width, int height, int seed, int numPoints, double[] xvalues, double[] yvalues)
        {
            Random gen = new Random();
            for (int i = 0; i < numPoints; i++)
            {
                xvalues[i] = gen.NextDouble() * (width - 20) + 10;
                yvalues[i] = gen.NextDouble() * (height - 20) + 10;
            }
        }

        static public void GenerateRelaxed(int width, int height, int seed, int numPoints)
        {
            for (int i = 0; i < NUM_LLOYD_RELAXATIONS; i++)
            {
                Site[] sites = voro.GetSites();

                for (int j = 0; j != sites.Length; j++)
                {
                    List<Vector2> region = voro.GetRegion(j);
                    int count = 0;
                    foreach (Vector2 v in region)
                    {
                        if (v.X > 0 || v.X < width || v.Y > 0 || v.Y < height)
                        {
                            _xvalues[j] += v.X;
                            _yvalues[j] += v.Y;
                            count++;
                        }
                    }
                    _xvalues[j] /= count;
                    _yvalues[j] /= count;
                }
            }
        }

        static public void GeneratePeturbed(int width, int height, int seed, int numPoints)
        {
            _xvalues = new double[numPoints];
            _yvalues = new double[numPoints];
            Random gen = new Random();

            int sq = (int)Math.Sqrt(numPoints);
            double spacing = (width*height) / sq;
            int count = 0;
            for (int i = 0; i != sq; i++)
            {
                for (int j = 0; j != sq; j++)
                {
                    _xvalues[i * sq + j] = (i * spacing + spacing / 2) + gen.Next(14) - 7;
                    _yvalues[i * sq + j] = (j * spacing + spacing / 2) + gen.Next(14) - 7;
                    count++;
                }
            }
            while(count < numPoints)
            {
                _xvalues[count] = gen.Next(500);
                _yvalues[count++] = gen.Next(500);
            }
        }
    }
}
