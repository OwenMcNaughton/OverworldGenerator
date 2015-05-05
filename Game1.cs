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
        int points = 100;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 500;
            graphics.PreferredBackBufferHeight = 500;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";

            Random gen = new Random();

            double[] xvalues = new double[points];
            double[] yvalues = new double[points];

            PointGenerator.GenerateRandom(500, gen.Next(), points, xvalues, yvalues);

            Voronoi voro = new Voronoi(10);
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            foreach(Line l in lines) 
            {
                spriteBatch.DrawLine(l.point1, l.point2, Color.Aqua);
            }

            foreach (Circle c in circles)
            {
                spriteBatch.DrawCircle(c.center, c.radius, c.sides, Color.Red);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
