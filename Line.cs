using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Primitives2D
{
    public class Line
    {
        public Vector2 point1, point2;

        public Line(Vector2 point1, Vector2 point2)
        {
            this.point1 = point1;
            this.point2 = point2;
        }
    }
}
