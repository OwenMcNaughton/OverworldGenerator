using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Voronoi2;
using Primitives2D;

namespace OverworldGenerator
{
    public class MapGenerator
    {
        public enum VoroType { RANDOM, RELAXED, PERTURBED };
        public VoroType vtype;

        public enum IslandType { RADIAL, PERLIN };
        public IslandType itype;

        List<Line> lines;
        List<Circle> circles;
        int points = 500, width, height;
        Voronoi voro;

        static double ISLAND_FACTOR = 1.07;
        double startAngle, dipWidth, dipAngle;
        int bumps;

        PerlinMatrix pmat;

        public MapGenerator(int width, int height, int points, VoroType vtype, IslandType itype)
        {
            this.width = width;
            this.height = height;
            this.points = points;
            this.vtype = vtype;
            this.itype = itype;

            lines = new List<Line>();
            circles = new List<Circle>();

            Gen();
        }

        public void Gen()
        {
            switch(vtype)
            {
                case VoroType.RANDOM: VoroRandom(); break;
                case VoroType.RELAXED: VoroRelaxed(); break;
                case VoroType.PERTURBED: VoroPerturbed(); break;
            }

            Islandify();
            MakeGraphics();
        }

        public void MakeGraphics()
        {
            List<GraphEdge> edges = voro.GetEdges();
            lines = new List<Line>();
            foreach (GraphEdge g in edges)
            {
                Vector2 v1 = new Vector2((float)g.x1, (float)g.y1);
                Vector2 v2 = new Vector2((float)g.x2, (float)g.y2);
                lines.Add(new Line(v1, v2));
            }

            Site[] sites = voro.GetSites();
            circles = new List<Circle>();
            foreach (Site s in sites)
            {
                Circle c = new Circle(new Vector2((float)s.coord.x, (float)s.coord.y));
                if (s.water)
                {
                    c.color = Color.LightSeaGreen;
                }
                else
                {
                    c.color = Color.CornflowerBlue;
                }
                circles.Add(c);
            }
        }

        public void VoroRandom()
        {
            Random gen = new Random();

            double[] xvalues = new double[points];
            double[] yvalues = new double[points];

            PointGenerator.GenerateRandom(width, height, gen.Next(), points, xvalues, yvalues);

            Voronoi voro = new Voronoi(0);
            voro.generateVoronoi(xvalues, yvalues, 0, width, 0, height);
        }

        public void VoroRelaxed()
        {
            Random gen = new Random();

            PointGenerator.GenerateRandom(width, height, 3, points);
            PointGenerator.GenerateRelaxed(width, height, 3, points);

            double[] xvalues = PointGenerator.GetX();
            double[] yvalues = PointGenerator.GetY();

            voro = new Voronoi(1);
            voro.generateVoronoi(xvalues, yvalues, 0, width, 0, height);
        }

        public void VoroPerturbed()
        {
            Random gen = new Random();

            PointGenerator.GeneratePeturbed(width, height, gen.Next(), points);

            double[] xvalues = PointGenerator.GetX();
            double[] yvalues = PointGenerator.GetY();

            voro = new Voronoi(0);
            voro.generateVoronoi(xvalues, yvalues, 0, width, 0, height);
            voro.Regionify();
        }

        private void Islandify()
        {
            Random gen = new Random();
            switch(itype)
            { 
                case IslandType.RADIAL:
                    bumps = gen.Next(6) + 1;
                    startAngle = gen.NextDouble() * 2 * Math.PI;
                    dipAngle = gen.NextDouble() * 2 * Math.PI;
                    dipWidth = gen.NextDouble() * 0.5 + 0.2;
                    break;
                case IslandType.PERLIN:
                    pmat = Perlin.CreateOctave(width, height, gen.NextDouble(), 0.1, 1, 1);
                    break;
            }

            Site[] sites = voro.GetSites();
            for(int i = 0; i != sites.Length; i++)
            {
                Voronoi2.Point p = new Voronoi2.Point(2 * (sites[i].coord.x / width - 0.5), 
                                                        2 * (sites[i].coord.y / height - 0.5));
                switch(itype)
                {
                    case IslandType.PERLIN:
                        sites[i].water = !PerlinInside(p);
                        break;
                    case IslandType.RADIAL:
                        sites[i].water = !RadialInside(p);
                        break;
                }
                voro.SetSite(i, sites[i]);
            }
        }

        public Boolean RadialInside(Voronoi2.Point p) 
        {
            double angle = Math.Atan2(p.y, p.x);
            double length = 0.6 * (Math.Max(Math.Abs(p.x), Math.Abs(p.y)) + p.Length());

            double r1 = 0.5 + 0.40*Math.Sin(startAngle + bumps*angle + Math.Cos((bumps+3)*angle));
            double r2 = 0.7 - 0.20*Math.Sin(startAngle + bumps*angle - Math.Sin((bumps+2)*angle));
            if (Math.Abs(angle - dipAngle) < dipWidth
                || Math.Abs(angle - dipAngle + 2*Math.PI) < dipWidth
                || Math.Abs(angle - dipAngle - 2*Math.PI) < dipWidth) {
                r1 = r2 = 0.2;
            }
            return (length < r1 || (length > r1*ISLAND_FACTOR && length < r2));
        }

        public Boolean PerlinInside(Voronoi2.Point p)
        { 
            double d = pmat.Get((int)p.x, (int)p.y);
            double l = p.Length();
            return d > l * l + 0.3;
        }

        public Color[] MapToColor()
        {
            Color[] pixels = new Color[width * height];
            Site[] s = voro.GetSites();
            List<Site> sites = new List<Site>(s);
            for(int i = 0; i != width; i++)
            {
                for (int j = 0; j != height; j++)
                {
                    int idx = GetCell(sites, new Vector2(i, j));
                    if (sites[idx].water)
                    {
                        pixels[j * width + i] = Color.LightSkyBlue;
                    }
                    else
                    {
                        pixels[j * width + i] = Color.LightSeaGreen;
                    }
                }
            }
            return pixels;
        }

        public int GetCell(List<Site> sites, Vector2 v)
        {
            double minDist = Double.MaxValue;
            int idx = -1;
            for (int i = 0; i != sites.Count(); i++)
            {
                double dist = Dist(sites[i].coord, v);
                if (dist < minDist)
                {
                    minDist = dist;
                    idx = i;
                }
            }
            return idx;
        }

        public double Dist(Voronoi2.Point p, Vector2 v)
        {
            return Math.Sqrt((p.x - v.X) * (p.x - v.X) + (p.y - v.Y) * (p.y - v.Y));
        }

        public Texture2D GetImage(GraphicsDevice graphics)
        {
            Texture2D tex = new Texture2D(graphics, width, height);
            Color[] pixels = MapToColor();
            tex.SetData(pixels);
            return tex;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Line l in lines)
            {
                l.Draw(spriteBatch);
            }

            foreach (Circle c in circles)
            {
                c.Draw(spriteBatch);
            }
        }
    }
}
