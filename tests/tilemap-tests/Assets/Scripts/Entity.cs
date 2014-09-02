using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AStar;

namespace ST
{
    public class Entity
    {
        // public for A* unity plugin
        public TileCoord curTile;
        private Map map;
        private GameObject gameObject;
        private Vector2 basePos;
        private TileCoord curWaypoint;
        private List<TileCoord> waypoints;
        private TileCoord goalTile;
        private Direction curDir;

        // feet location offset
        private float feetOffset = 28.0f;
        private float speed = 0.0f;
        private float speedMax = 60.0f;
        private float zOffset = -0.75f;
        private Animator anim;

        public Entity (Map m)
        {
            this.map = m;
            this.curTile = null;
            this.curWaypoint = null;
            this.goalTile = null;
            this.waypoints = new List<TileCoord> ();
        }

        // Use this for initialization
        public void Start (GameObject go)
        {
            this.gameObject = go;
            setTile (new TileCoord (5, 10));

            anim = go.GetComponent<Animator> ();
        }

        public enum Direction
        {
            None,
            NorthEast,
            NorthWest,
            SouthEast,
            SouthWest,
        }

        public Vector3 vectorForDirection (Direction dir)
        {
            switch (dir) {
            case Direction.NorthEast:
                return new Vector3 (1f, 0.5f);
            case Direction.NorthWest:
                return new Vector3 (-1f, 0.5f);
            case Direction.SouthEast:
                return new Vector3 (1f, -0.5f);
            case Direction.SouthWest:
                return new Vector3 (-1f, 0.5f);
            }
            return new Vector3 (0, 0, 0);
        }

        // currently hacking around to get the height to work well
        public void setPosition (Vector3 p)
        {
            basePos = new Vector2 (p.x, p.y);

            var z = map.wallLayer.zOrderForTile (curTile) + zOffset;
            var h = map.heightForTile (curTile);
            //Debug.Log("h = " + h + " @ " + curTile);

            Vector3 pos = new Vector3 (p.x, p.y + h + feetOffset, z);
            gameObject.transform.position = pos;
        }

        public void updateFlip ()
        {
            // TODO: only change if different
            Vector3 scale = gameObject.transform.localScale;
            
            bool flip = scale.x <= 0 && (curDir == Direction.NorthEast || curDir == Direction.SouthEast);
            flip = flip || (scale.x >= 0 && (curDir == Direction.NorthWest || curDir == Direction.SouthWest));
            if (flip)
                scale.x *= -1;
            gameObject.transform.localScale = scale;
        }

        public void updateAnim ()
        {
            // NOTE: can't wait for dir change as current code is written or never move into idle
//          if (oldDir != curDir) {
            string animName = null;

            switch (curDir) {
            case Direction.NorthEast:
            case Direction.NorthWest:
                if (speed > 0)
                    animName = "entity-walk-ne";
                else
                    animName = "entity-idle-ne";
                break;
            case Direction.SouthEast:
            case Direction.SouthWest:
                if (speed > 0)
                    animName = "entity-walk-se";
                else
                    animName = "entity-idle-se";
                break;
            }

            if (null != animName) {
                int hash = Animator.StringToHash (animName);
                anim.CrossFade (hash, 0.1f);
                anim.Play (hash);
            }
//          }
        }

        public void update ()
        {
            Vector2 curPos = basePos;

            if (null != curWaypoint || null != goalTile) {

                TileCoord nextTile = curWaypoint != null ? curWaypoint : goalTile != null ? goalTile : null;
                if (null != nextTile) {
                    var pos3 = map.posForTile (nextTile);
                    Vector2 pos = new Vector2 (pos3.x, pos3.y);

                    var delta = pos - curPos;

                    // TODO: should change direction only when getting next tile
                    // direction based on position change 
                    if (delta.y > 0) {
                        curDir = delta.x < 0 ? Direction.NorthWest : Direction.NorthEast;
                    } else {    
                        curDir = delta.x < 0 ? Direction.SouthWest : Direction.SouthEast;
                    }

                    speed = speedMax;
                    var vel = delta.normalized * speed * UnityEngine.Time.deltaTime;
                    vel = new Vector2 (vel.x, vel.y);
                    //Debug.Log ("velocity = " + velocity);
                    curPos += vel;

                    var epsilon = 1E-04f;
                    var dist = Vector3.Distance (curPos, pos);

                    if (dist < epsilon || dist <= 2.5f * vel.magnitude) {
                        //map.changeTileIndex (nextTile, 1);
                        map.changeTileColor (nextTile, new Color (1f, 1f, 1f));
                        curPos = map.posForTile (nextTile);
                        curTile = nextTile;

                        if (null != curWaypoint) {
                            Debug.Log ("finished waypoint");
                            if (waypoints.Count () > 0) {
                                curWaypoint = waypoints [0];
                                waypoints.RemoveAt (0);
                            } else {
                                curWaypoint = null;
                            }
                        } else {
                            Debug.Log ("finished goal");
                            goalTile = null;
                        }
                    } else {
                        // check if we've moved to a new tile
                        var coord = map.tileForPos (curPos);
                        if (curTile == null || coord.c != curTile.c || coord.r != curTile.r) {
                            if (map.hasWall (coord)) {
                                // reset (move back)
                                curPos = basePos;
                                resetWaypoints ();
                            } else {
                                curTile = coord;
                            }
                        }
                    }
                }
            } else {
                speed = 0.0f;
            }

            setPosition (new Vector3 (curPos.x, curPos.y));

            updateFlip ();
            updateAnim ();

        }

        public void resetWaypoints ()
        {
            if (null != curWaypoint)
                map.changeTileColor (curWaypoint, new Color (1f, 1f, 1f));
            if (null != goalTile)
                map.changeTileColor (goalTile, new Color (1f, 1f, 1f));
            
            curWaypoint = null;
            goalTile = null;
            waypoints.Clear ();
        }

        // Simple Astar pathfinding
        public void setGoalTileSimpleAStar (TileCoord coord)
        {
            string startCity = "tile_" + curTile.r + "_" + curTile.c;
            string destinationCity = "tile_" + goalTile.r + "_" + goalTile.c;
        
            AStar.Node start = map.graph.Nodes [startCity];
            AStar.Node destination = map.graph.Nodes [destinationCity];

            // TODO: move this outside this method or cache it
            // Function which tells us the exact distance between two neighbours.
            Func<AStar.Node, AStar.Node, double> distanceFunc = (node1, node2) => {
                AdjacencyList neighbors = node1.Neighbors;
                var etns = neighbors.Cast<AStar.EdgeToNeighbor> ();
                var edge = etns.Single (etn => etn.Neighbor.Key == node2.Key);
                double cost = edge.Cost;
                return cost;
            };
            
            // Estimation/Heuristic function (Manhattan distance)
            // It tells us the estimated distance between the last node on a proposed path and the destination node.
            Func<AStar.Node, double> manhattanEstimation = n => Math.Abs (n.X - destination.X) + Math.Abs (n.Y - destination.Y);

            Debug.Log ("starting find path from " + startCity + " to " + destinationCity);
            AStar.Path<AStar.Node> shortestPath = FindPath (start, destination, distanceFunc, manhattanEstimation);
            Debug.Log ("found path");

            // DEBUG LOG
            // Prints the shortest path.
            Debug.Log ("\nThis is the shortest path based on the A* Search Algorithm:\n");
            foreach (AStar.Path<AStar.Node> path in shortestPath.Reverse()) {
//              if (path.PreviousSteps != null) {
//                  Debug.Log (string.Format ("From {0, -15}  to  {1, -15} -> Total cost = {2:#.###}",
//                                                  path.PreviousSteps.LastStep.Key, path.LastStep.Key, path.TotalCost));
//              }

                int c = path.LastStep.X;
                int r = path.LastStep.Y;

                Debug.Log ("adding waypoint " + r + ", " + c);
                waypoints.Add (new TileCoord (c, r, -1));
            }
        }

        // Cheap "seek" pathfinding
        public void setGoalTileSeek (TileCoord coord)
        {
            int dc = goalTile.c - curTile.c;
            int dr = goalTile.r - curTile.r;

            curDir = Direction.None;
            if (Mathf.Abs (dc) > Mathf.Abs (dr)) {
                // move along c-axis
                curDir = dc < 0 ? Direction.NorthWest : Direction.SouthEast;
                waypoints.Add (new TileCoord (goalTile.c, curTile.r, -1));
            } else {    
                // move along r-axis
                curDir = dr < 0 ? Direction.SouthWest : Direction.NorthEast;
                waypoints.Add (new TileCoord (curTile.c, goalTile.r, -1));
            }
        }

        public void setGoalTile (TileCoord coord)
        {
            if (! map.validTileCoord (coord))
                return;

            waypoints.Clear ();

            var pos = basePos;
            curTile = map.tileForPos (pos);
            goalTile = coord;

            if (map.UseAstarPathfinding) {
                if (map.UseSimple2Dastar) {
                    setGoalTileSimpleAStar (coord);
                } else {
                    // use the actual A* plugin
                    
                }
            } else {
                setGoalTileSeek (coord);
            }

            curWaypoint = waypoints [0];
            map.changeTileColor (curTile, new Color (1f, 0.3f, 0.3f));
            map.changeTileColor (curWaypoint, new Color (0.3f, 0.3f, 1f));
        }

        public void setTile (TileCoord coord)
        {
//          if (null != curTile)
//              map.changeTileIndex (curTile, 1);
            curTile = coord;
//          map.changeTileIndex (curTile, 2);

            var xy = map.posForTile (curTile);
            setPosition (new Vector3 (xy.x, xy.y));
        }



        /// <summary>
        /// This is the method responsible for finding the shortest path between a Start and Destination cities using the A*
        /// search algorithm.
        /// </summary>
        /// <typeparam name="TNode">The Node type</typeparam>
        /// <param name="start">Start city</param>
        /// <param name="destination">Destination city</param>
        /// <param name="distance">Function which tells us the exact distance between two neighbours.</param>
        /// <param name="estimate">Function which tells us the estimated distance between the last node on a proposed path and the
        /// destination node.</param>
        /// <returns></returns>
        static public AStar.Path<TNode> FindPath<TNode> (
            TNode start,
            TNode destination,
            Func<TNode, TNode, double> distance,
            Func<TNode, double> estimate) where TNode : AStar.IHasNeighbours<TNode>
        {
            var closed = new HashSet<TNode> ();
            
            var queue = new AStar.PriorityQueue<double, Path<TNode>> ();
            
            queue.Enqueue (0, new AStar.Path<TNode> (start));
            
            while (!queue.IsEmpty) {
                var path = queue.Dequeue ();
                
                if (closed.Contains (path.LastStep))
                    continue;
                
                if (path.LastStep.Equals (destination))
                    return path;
                
                closed.Add (path.LastStep);
                
                foreach (TNode n in path.LastStep.Neighbours) {
                    double d = distance (path.LastStep, n);
                    
                    var newPath = path.AddStep (n, d);
                    
                    queue.Enqueue (newPath.TotalCost + estimate (n), newPath);
                }
            }
            
            return null;
        }

//      sealed partial class Node : AStar.IHasNeighbours<Node>
//      {
//          public IEnumerable<Node> Neighbours {
//              get {
//                  List<Node> nodes = new List<Node> ();
//                  
//                  foreach (AStar.EdgeToNeighbor etn in Neighbors) {
//                      nodes.Add (etn.Neighbor);
//                  }
//                  
//                  return nodes;
//              }
//          }
//      }
    }
}
