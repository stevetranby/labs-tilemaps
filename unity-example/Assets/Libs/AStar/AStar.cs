using System;
using System.Collections.Generic;
using System.Linq;

// http://www.leniel.net/2009/06/astar-pathfinding-search-in-csharp.html
// http://blogs.msdn.com/ericlippert/archive/tags/AStar/default.aspx
// http://blogs.vertigo.com/personal/rtaylor/Blog/Lists/Posts/Post.aspx?ID=4

namespace AStar
{
    class AStar
    {
//        static void Main(string[] args)
//        {
//            do
//            {
//                // Creating the Graph...
//                Graph graph = new Graph();
//			
//				// fill graph (refactor into tile map creation)
//                FillGraphWithGridMap(graph);
//
//                // Prints on screen the cities that you can choose as Start and Destination.
//                foreach(Node n in graph.Nodes.Cast<Node>().OrderBy(n => n.Key))
//                {
//                    Console.WriteLine(n.Key);
//                }
//
//				string startCity = "tile_1_1";
//
//				string destinationCity = "tile_1_2";
//
//                Node start = graph.Nodes[startCity];
//
//                Node destination = graph.Nodes[destinationCity];
//
//                // Function which tells us the exact distance between two neighbours.
//                Func<Node, Node, double> distance =
//					(node1, node2) => node1.Neighbors.Cast<EdgeToNeighbor>().Single(
//						etn => etn.Neighbor.Key == node2.Key).Cost;
//
//                // Estimation/Heuristic function (Manhattan distance)
//                // It tells us the estimated distance between the last node on a proposed path and the destination node.
//                Func<Node, double> manhattanEstimation = n => Math.Abs(n.X - destination.X) + Math.Abs(n.Y - destination.Y);
//			
//				Path<Node> shortestPath = FindPath(start, destination, distance, manhattanEstimation);
//
//                Console.WriteLine("\nThis is the shortest path based on the A* Search Algorithm:\n");
//
//                // Prints the shortest path.
//                foreach(Path<Node> path in shortestPath.Reverse())
//                {
//                    if(path.PreviousSteps != null)
//                    {
//                        Console.WriteLine(string.Format("From {0, -15}  to  {1, -15} -> Total cost = {2:#.###} {3}",
//                                          path.PreviousSteps.LastStep.Key, path.LastStep.Key, path.TotalCost));
//                    }
//                }
//
//                Console.Write("\nDo you wanna try A* Search again? Yes or No? ");
//            }
//            while(Console.ReadLine().ToLower() == "yes");
//        }
//
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="graph"></param>
//        private static void FillGraphWithGridMap(Graph graph)
//        {
//			// for r,c create tile nodes
//			//graph.AddNode("tile_r_c", null, 68, -61);
//
//            // Edges
//			// for r,c create edges to <= 4 neighbors
//			// cost is 1 unless cliff or whatever, maybe use height differential
//			graph.AddUndirectedEdge("tile_1_1", "tile_0_1", 1);
//			graph.AddUndirectedEdge("tile_1_1", "tile_2_1", 1);
//			graph.AddUndirectedEdge("tile_1_1", "tile_1_0", 1);
//			graph.AddUndirectedEdge("tile_1_1", "tile_1_2", 1);
//		}

//
//        /// <summary>
//        /// This is the method responsible for finding the shortest path between a Start and Destination cities using the A*
//        /// search algorithm.
//        /// </summary>
//        /// <typeparam name="TNode">The Node type</typeparam>
//        /// <param name="start">Start city</param>
//        /// <param name="destination">Destination city</param>
//        /// <param name="distance">Function which tells us the exact distance between two neighbours.</param>
//        /// <param name="estimate">Function which tells us the estimated distance between the last node on a proposed path and the
//        /// destination node.</param>
//        /// <returns></returns>
//        static public Path<TNode> FindPath<TNode>(
//            TNode start,
//            TNode destination,
//            Func<TNode, TNode, double> distance,
//            Func<TNode, double> estimate) where TNode : IHasNeighbours<TNode>
//        {
//            var closed = new HashSet<TNode>();
//
//            var queue = new PriorityQueue<double, Path<TNode>>();
//
//            queue.Enqueue(0, new Path<TNode>(start));
//
//            while(!queue.IsEmpty)
//            {
//                var path = queue.Dequeue();
//
//                if(closed.Contains(path.LastStep))
//                    continue;
//
//                if(path.LastStep.Equals(destination))
//                    return path;
//
//                closed.Add(path.LastStep);
//
//                foreach(TNode n in path.LastStep.Neighbours)
//                {
//                    double d = distance(path.LastStep, n);
//
//                    var newPath = path.AddStep(n, d);
//
//                    queue.Enqueue(newPath.TotalCost + estimate(n), newPath);
//                }
//            }
//
//            return null;
//        }
    }

    sealed partial class Node : IHasNeighbours<Node>
    {
        public IEnumerable<Node> Neighbours
        {
            get
            {
                List<Node> nodes = new List<Node>();

                foreach(EdgeToNeighbor etn in Neighbors)
                {
                    nodes.Add(etn.Neighbor);
                }

                return nodes;
            }
        }
    }
}
