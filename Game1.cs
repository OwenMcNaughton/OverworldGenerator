using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Voronoi2;
using Primitives2D;

namespace OverworldGenerator
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        List<Line> lines;
        List<Circle> circles;
        int points = 500;
        Voronoi voro;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 500;
            graphics.PreferredBackBufferHeight = 500;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";

            //VoroRandom();
            VoroRelaxed();
            //VoroPerturbed();
        }

        public void VoroRandom() {
            Random gen = new Random();

            double[] xvalues = new double[points];
            double[] yvalues = new double[points];

            PointGenerator.GenerateRandom(500, gen.Next(), points, xvalues, yvalues);

            Voronoi voro = new Voronoi(0);
            voro.generateVoronoi(xvalues, yvalues, 0, 500, 0, 500);

            List<GraphEdge> edges = voro.GetEdges();
            lines = new List<Line>();
            foreach(GraphEdge g in edges)
            {
                Vector2 v1 = new Vector2((float)g.x1, (float)g.y1);
                Vector2 v2 = new Vector2((float)g.x2, (float)g.y2);
                lines.Add(new Line(v1, v2));
            }

            Site[] sites = voro.GetSites();
            circles = new List<Circle>();
            foreach (Site s in sites)
            {
                Vector2 v = new Vector2((float)s.coord.x, (float)s.coord.y);
                circles.Add(new Circle(v));
            }
        }

        public void VoroRelaxed()
        {
            Random gen = new Random();

            PointGenerator.GenerateRandom(500, 3, points);
            PointGenerator.GenerateRelaxed(500, 3, points);

            double[] xvalues = PointGenerator.GetX();
            double[] yvalues = PointGenerator.GetY();

            voro = new Voronoi(1);
            voro.generateVoronoi(xvalues, yvalues, 0, 500, 0, 500);

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
                Vector2 v = new Vector2((float)s.coord.x, (float)s.coord.y);
                circles.Add(new Circle(v));
            }
        }

        public void VoroPerturbed()
        {
            Random gen = new Random();

            PointGenerator.GeneratePeturbed(500, gen.Next(), points);

            double[] xvalues = PointGenerator.GetX();
            double[] yvalues = PointGenerator.GetY();

            voro = new Voronoi(0);
            voro.generateVoronoi(xvalues, yvalues, 0, 500, 0, 500);
            voro.Regionify();

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
                Circle c = new Circle((float)s.coord.x, (float)s.coord.y);
                c.color = Color.CornflowerBlue;
                circles.Add(c);
            }

        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            foreach(Line l in lines) 
            {
                spriteBatch.DrawLine(l.point1, l.point2, Color.White, 2);
            }

            foreach (Circle c in circles)
            {
                //spriteBatch.DrawCircle(c.center, c.radius, c.sides, c.color, 1);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
