using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Primitives2D
{
    public class Circle
    {
        public Vector2 center;
        public float radius;
        public int sides;
        public Color color = Color.Red;

        public static int DEFAULT_SIDES = 10;
        public static float DEFAULT_RADIUS = 3;

        public Circle(Vector2 center, float radius, int sides)
        {
            this.center = center;
            this.radius = radius;
            this.sides = sides;
        }

        public Circle(float x, float y, float radius, int sides)
        {
            this.center = new Vector2(x, y);
            this.radius = radius;
            this.sides = sides;
        }

        public Circle(Vector2 center, float radius)
        {
            this.center = center;
            this.radius = radius;
            this.sides = DEFAULT_SIDES;
        }

        public Circle(float x, float y, float radius)
        {
            this.center = new Vector2(x, y);
            this.radius = radius;
            this.sides = DEFAULT_SIDES;
        }

        public Circle(Vector2 center, int sides)
        {
            this.center = center;
            this.radius = DEFAULT_RADIUS;
            this.sides = sides;
        }

        public Circle(float x, float y, int sides)
        {
            this.center = new Vector2(x, y);
            this.radius = DEFAULT_RADIUS;
            this.sides = sides;
        }

        public Circle(Vector2 center)
        {
            this.center = center;
            this.radius = DEFAULT_RADIUS;
            this.sides = DEFAULT_SIDES;
        }

        public Circle(float x, float y)
        {
            this.center = new Vector2(x, y);
            this.radius = DEFAULT_RADIUS;
            this.sides = DEFAULT_SIDES;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawCircle(center, radius, sides, color, 1);
        }
    }
}
