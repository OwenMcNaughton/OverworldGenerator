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
        static double[] xvalues, yvalues;

        static public void GenerateRandom(int size, int seed, int numPoints)
        {
            xvalues = new double[numPoints];
            yvalues = new double[numPoints];
            Random gen = new Random();
            for (int i = 0; i < numPoints; i++) {
                xvalues[i] = gen.NextDouble() * (size-20) + 10;
                yvalues[i] = gen.NextDouble() * (size-20) + 10;
            }
        }

        static public void GenerateRelaxed(int size, int seed, int numPoints)
        {
            Random gen = new Random();
            GenerateRandom(500, gen.Next(), 100);
            for (int i = 0; i < NUM_LLOYD_RELAXATIONS; i++)
            {
                Voronoi v = new Voronoi(10);
                v.generateVoronoi(xvalues, yvalues, 0, size, 0, size);

            }

            //return points;
        }
    
    }
}
