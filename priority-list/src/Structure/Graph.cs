using System;
using System.Collections.Generic;
using System.Numerics;

namespace World.Structure
{
    public class Graph
    {
        public Dictionary<int, Edge> edges;
        public Dictionary<string, Vertex> vertices;
        public List<int> ids;
        public Dictionary<int, string> vertexType;
        private int next = 1;

        public Graph()
        {
            //hard coded for tests
            ids = new List<int> {2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127, 
                131, 137, 139, 149, 151, 157, 163, 167, 173, 179, 181, 191, 193, 197, 199, 211, 223, 227, 229, 233, 239, 241, 251, 257, 263, 269, 271, 277, 281,
                283, 293, 307, 311, 313, 317, 331, 337, 347, 349, 353, 359, 367, 373, 379, 383, 389, 397, 401, 409, 419, 421, 431, 433, 439, 443, 449, 457, 461, 
                463, 467, 479, 487, 491, 499, 503, 509, 521, 523, 541, 547, 557, 563, 569, 571, 577, 587, 593, 599, 601, 607, 613, 617, 619, 631, 641, 643, 647, 
                653, 659, 661, 673, 677, 683, 691, 701, 709, 719, 727, 733, 739, 743, 751, 757, 761, 769, 773, 787, 797, 809, 811, 821, 823, 827, 829, 839, 853, 
                857, 859, 863, 877, 881, 883, 887, 907, 911, 919, 929, 937, 941, 947, 953, 967, 971, 977, 983, 991, 997 };
            edges = new Dictionary<int, Edge>();
            vertices = new Dictionary<string, Vertex>();
        }

        public void AddEdgesToGraphAndToVertices(params Edge[] edges)
        {
            foreach (var edge in edges)
            {
                if (edge != null && AddEdgeToGraph(edge))
                {
                    AddEdgeToVertices(edge);
                }
            }
        }

        public void AddEdgesToGraph(params Edge[] edges)
        {
            foreach (var edge in edges)
            {
                if (edge != null)
                {
                    AddEdgeToGraph(edge);
                }
            }
        }

        public static void AddEdgeToVertices(Edge edge)
        {
            edge.startVertex.assignedEdges.Add(edge);
            edge.endVertex.assignedEdges.Add(edge);
        }

        public bool AddEdgeToGraph(Edge edge)
        {
            if (edges.ContainsKey(edge.startVertex.ID * edge.endVertex.ID))
            {
                return false;
            }
            edges.Add(edge.startVertex.ID * edge.endVertex.ID, edge);
            return true;
        }

        public Edge GetEdge(int ID)
        {
            try
            {
                return edges[ID];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

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