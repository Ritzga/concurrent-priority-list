using System;
using System.Collections.Generic;
using System.Numerics;

namespace World.Structure
{
    public class Graph
    {
        public static int ManhattanDistance(Vector2 from, Vector2 to)
        {
            int fromX = Convert.ToInt32(from.X);
            int fromY = Convert.ToInt32(from.Y);
            int toX = Convert.ToInt32(to.X);
            int toY = Convert.ToInt32(to.Y);

            return Math.Abs(fromX - fromY) + Math.Abs(toX - toY);
        }
    }
}
