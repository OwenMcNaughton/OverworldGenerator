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
        static int NUM_LLOYD_RELAXATIONS = 2;
        static double[] _xvalues, _yvalues;

        static public void GenerateRandom(int size, int seed, int numPoints)
        {
            _xvalues = new double[numPoints];
            _yvalues = new double[numPoints];
            Random gen = new Random();
            for (int i = 0; i < numPoints; i++) {
                _xvalues[i] = gen.NextDouble() * (size-20) + 10;
                _yvalues[i] = gen.NextDouble() * (size-20) + 10;
            }
        }

        static public void GenerateRandom(int size, int seed, int numPoints, double[] xvalues, double[] yvalues)
        {
            Random gen = new Random();
            for (int i = 0; i < numPoints; i++)
            {
                xvalues[i] = gen.NextDouble() * (size - 20) + 10;
                yvalues[i] = gen.NextDouble() * (size - 20) + 10;
            }
        }

        static public void GenerateRelaxed(int size, int seed, int numPoints)
        {
            Random gen = new Random();
            GenerateRandom(size, gen.Next(), numPoints);
            for (int i = 0; i < NUM_LLOYD_RELAXATIONS; i++)
            {
                Voronoi v = new Voronoi(10);
                v.generateVoronoi(_xvalues, _yvalues, 0, size, 0, size);

            }

            //return points;
        }
    
    }
}
