using System.Collections.Generic;
using System.Numerics;

namespace World.Structure
{
    public class Vertex
    {
        public Vertex(Vector2 worldPositon, int ID)
        {
            worldPosition = worldPositon;
            this.ID = ID;
        }

        public List<Edge> assignedEdges = new List<Edge>();
        public Vector2 worldPosition;
        public int ID;

        public override bool Equals(object obj)
        {
            return obj is Vertex vertex && worldPosition.Equals(vertex.worldPosition);
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(this);
        }

        public override string ToString()
        {
            return "Vertex on pos: " + worldPosition.ToString();
        }

        public static bool operator ==(Vertex vertex, Vertex vertex2)
        {
            return (!(vertex is null) && !(vertex2 is null)) && vertex.ID == vertex2.ID;
        }

        public static bool operator !=(Vertex vertex, Vertex vertex2)
        {
            return !(vertex == vertex2);
        }
    }
}