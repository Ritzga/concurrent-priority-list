using System.Collections;
using System.Collections.Generic;

namespace World.Structure
{
    public class Edge
    {
        public readonly Vertex startVertex;
        public readonly Vertex endVertex;
        public readonly int ID;

        public Edge(Vertex startVertex, Vertex endVertex)
        {
            this.startVertex = startVertex;
            this.endVertex = endVertex;
            ID = startVertex.ID * endVertex.ID;
        }

        public override bool Equals(object obj)
        {
            var edge = obj as Edge;
            return edge != null && (ID == edge.ID);
        }

        public static bool operator ==(Edge edge, Edge edge2)
        {
            return (!(edge is null) && !(edge2 is null)) && (edge.ID == edge2.ID);
        }

        public static bool operator !=(Edge edge, Edge edge2)
        {
            return !(edge == edge2);
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(this);
        }
    }
}