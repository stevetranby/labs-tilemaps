using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ST
{
	public class Map
	{
		public bool UseAstarPathfinding = false;
		public bool UseSimple2Dastar = false;

		// TODO: MapLayer[] mapLayers; // for 3+ layers
		// TODO: should be accessed through getLayer(string:name)
		public MapLayer floorLayer;
		public MapLayer wallLayer;
		public int cols = 24;
		public int rows = 24;
		public float tileHeightStep = 2.0f;
		public int maxTileSteps = 4;
		public int[,] heightMap;
		private GameObject parentNode;

		// grandberg astar
		public int[,] heightMapGranbergAstar;
		public GameObject[,] astarWallCubes;
		public GameObject astarShipNode; // point to AI.Pathfinder gameobject

		// simple 2d astar
		public AStar.Graph graph;

		public Map ()
		{
		}

		public void initFloor (Texture2D tileset, Size tileSize, Size textureTileSize, bool showGround)
		{
			floorLayer = new MapLayer (this, "floor", tileset, tileSize, textureTileSize, showGround);
			floorLayer.zOffset = -0.25f;
		}

		// TODO: refactor into MapLayer
		public void initWalls (Texture2D tileset, Size tileSize, Size textureTileSize, bool showGround)
		{
			wallLayer = new MapLayer (this, "wall", tileset, tileSize, textureTileSize, showGround);
			floorLayer.zOffset = 0.5f;
		}

		public GameObject createFloorTile (TileCoord coord, int tileIndex)
		{
			return floorLayer.createTile (this.parentNode, coord, tileIndex);
		}

		public GameObject createWallTile (TileCoord coord, int tileIndex)
		{
			return wallLayer.createTile (this.parentNode, coord, tileIndex);
		}

		public void generateMap (GameObject layer)
		{
			// make sure to generate a random seed, but SAVE the seed for any replay or regeneration
			Random.seed = 234234;

			this.parentNode = layer;

			heightMap = new int[cols, rows];

			for (int r = 0; r < rows; r++) {
				for (int c = 0; c < cols; c++) {
					int tileIndex = Random.Range (0, 4);

					int h = Random.Range (0, 4);
					heightMap [c, r] = h;

					TileCoord coord = new TileCoord (c, r);
					floorLayer.createTile (parentNode, coord, tileIndex);

					refreshTilesAt (coord);

					// add in astar pathfinding
					if (UseAstarPathfinding) {
						if (UseSimple2Dastar) {
							if (null == graph) {
								graph = new AStar.Graph ();
							}
						
							// for r,c create tile nodes
							// TODO: shouldn't use strings, but that's what the lib uses (should use byte/int map instead)
							string nodename = GetNodeKey(coord);
							graph.AddNode (nodename, null, c, r);

							// NOTE: not allowed here: needs nodes setup before creating edges
//							// Edges 
//							// for r,c create edges to <= 4 neighbors
//							// cost is 1 unless cliff or whatever, maybe use height differential
//							int cost = 1;
//							if (r > 0)
//								graph.AddUndirectedEdge (nodename, "tile_" + (r - 1) + "_" + c, cost);
//							if (r < rows - 1)
//								graph.AddUndirectedEdge (nodename, "tile_" + (r + 1) + "_" + c, cost);
//							if (c > 0)
//								graph.AddUndirectedEdge (nodename, "tile_" + r + "_" + (c - 1), cost);
//							if (c < cols - 1)
//								graph.AddUndirectedEdge (nodename, "tile_" + r + "_" + (c + 1), cost);
						} else {
							// TODO: generate 3d node in side map
						}
					}
				}
			}

			// need to add edges after nodes are created to keep things simple
			for (int r = 0; r < rows; r++) {
				for (int c = 0; c < cols; c++) {
					if (UseAstarPathfinding) {
						if (UseSimple2Dastar) {
							if (null == graph) {
								graph = new AStar.Graph ();
							}
					
							// for r,c create tile nodes
							// TODO: shouldn't use strings, but that's what the lib uses (should use byte/int map instead)
							string nodename = GetNodeKey(new TileCoord(c,r));
					
							// Edges - TODO: check if this works, since letting all neighbors have cost of 1 we can add them here
							// for r,c create edges to <= 4 neighbors
							// cost is 1 unless cliff or whatever, maybe use height differential
							int cost = 1;

							// NOTE: should only need down and right edges since up and left are already added when using undirectedEdge
//							if (r > 0)
//								AddEdge(graph, nodename, GetNodeKey(new TileCoord(c, r-1)), cost);
							if (r < rows - 1)
								AddEdge(graph, nodename, GetNodeKey(new TileCoord(c, r+1)), cost);
//							if (c > 0)
//								AddEdge(graph, nodename, GetNodeKey(new TileCoord(c-1, r)), cost);
							if (c < cols - 1)
								AddEdge(graph, nodename, GetNodeKey(new TileCoord(c+1, r)), cost);
						} else {
							// TODO: generate 3d node in side map
						}
		
					}
				}
			}
		}

		public void AddEdge(AStar.Graph graph, string nodekey1, string nodekey2, int cost)
		{
			// NOTE: undirect edge means adding directed edges from node1->node2 and node2->node1
			graph.AddUndirectedEdge (nodekey1, nodekey2, cost);
			//Debug.Log("adding edges: " + node1 + " to " + node2 + ", with cost " + cost);
		}

		public void AddAllNeighbors(AStar.Graph graph, TileCoord coord)
		{
			string nodekey = GetNodeKey(coord);
			AStar.Node node = graph.Nodes[nodekey];
			int c = node.X;
			int r = node.Y;

			List<AStar.Node> nodesToAdd = new List<AStar.Node>();
			// check up
			{
				string nodekeyCheck = GetNodeKey(new TileCoord(c, r-1));
				AStar.Node n = graph.Nodes[nodekeyCheck];
				if(null != n) {
					// now check and make sure not collision
					if(! hasWall(new TileCoord(c, r))) {
						// add edge
						nodesToAdd.Add(n);
					}
				}
			}

			node.AddNeighborsToNodes(nodesToAdd);
		}

		public string GetNodeKey(TileCoord coord)
		{
			return "tile_" + coord.r + "_" + coord.c;
		}

		public void RemoveEdges(AStar.Graph graph, TileCoord coord)
		{
			string nodekey = GetNodeKey(coord);
			AStar.Node node = graph.Nodes[nodekey];
			node.RemoveNeighbors();
		}

		public Sprite floorSpriteForTileIndex (int tileIndex)
		{
			if (null != wallLayer)
				return floorLayer.spriteCreateForTileIndex (tileIndex);
			return null;
		}

		public Sprite wallSpriteForTileIndex (int tileIndex)
		{
			if (null != wallLayer)
				return wallLayer.spriteCreateForTileIndex (tileIndex);
			return null;
		}

		public void changeFloor (TileCoord coord, int tileIndex)
		{
			floorLayer.changeTileIndex (this.parentNode, coord, tileIndex);
		}

		public void changeWall (TileCoord coord, int tileIndex)
		{
			if (UseAstarPathfinding) {
				if (UseSimple2Dastar) {
					if (tileIndex >= 0) {
						RemoveEdges (graph, coord);
					} else {
						AddAllNeighbors (graph, coord);
					}
				} else {
					// aron granberg's astar
					if (tileIndex < 0) {
						//remove
						if (null != astarWallCubes) {
							GameObject wallCube = astarWallCubes [coord.c, coord.r];
							if (null != wallCube) {
//							wallCube.transform.parent = null;
								GameObject.Destroy (wallCube);
								astarWallCubes [coord.c, coord.r] = null;
							}
						}

					} else {
						if (null == astarWallCubes) {
							astarWallCubes = new GameObject[cols, rows];
						}

						GameObject wallCube = astarWallCubes [coord.c, coord.r];
						if (null == wallCube) {
							GameObject go = GameObject.CreatePrimitive (PrimitiveType.Cube);						
							go.transform.parent = astarShipNode.transform;
							go.transform.localPosition = new Vector3 (coord.r, 0f, coord.c);

							// need to make sure that cubes overlap the grid to force collisions with scan()
							// - scale of (1,1,1) I believe leaves open "cracks" in the collision grid 
							go.transform.localScale = new Vector3(1.1f,1.1f,1.1f);
							// also make sure to set as a collision object for the AI Pathfinder
							go.layer = LayerMask.NameToLayer("Obstacles");

							go.renderer.material.color = new Color (1.0f, 0f, 0f);
							go.collider.isTrigger = false;
							astarWallCubes [coord.c, coord.r] = go;
							Debug.Log ("adding wall cube @ " + go.transform.localPosition);

							// rescan??
							this.astarShipNode.GetComponent<AstarPath>().Scan();
						}
					}
				}
			}

			wallLayer.changeTileIndex (this.parentNode, coord, tileIndex);
		}

		public void changeTileColor (TileCoord coord, Color color)
		{
			floorLayer.changeTileColor (coord, color);
			wallLayer.changeTileColor (coord, color);
		}

		public float heightForTile (TileCoord coord)
		{
			if (! validTileCoord (coord))
				return 0;

			return (float)(heightMap [coord.c, coord.r]) * tileHeightStep;
		}

		public void raiseTile (TileCoord coord)
		{
			if (! validTileCoord (coord))
				return;
			int h = heightMap [coord.c, coord.r];
			h++;
			heightMap [coord.c, coord.r] = h > maxTileSteps ? maxTileSteps : h;
			refreshTilesAt (coord);
			Debug.Log ("raising tile @ " + coord + " to height = " + heightMap [coord.c, coord.r]);
		}

		public void lowerTile (TileCoord coord)
		{
			if (! validTileCoord (coord))
				return;
			int h = heightMap [coord.c, coord.r];
			h--;
			heightMap [coord.c, coord.r] = h < 0 ? 0 : h;
			refreshTilesAt (coord);
			
			Debug.Log ("lowering tile @ " + coord + " to height = " + heightMap [coord.c, coord.r]);
		}

		public void refreshTilesAt (TileCoord coord)
		{
			floorLayer.refreshTile (coord);
			wallLayer.refreshTile (coord);
		}

		public bool validTileCoord (TileCoord coord)
		{
			if (coord.c < 0 || coord.c > cols - 1 || coord.r < 0 || coord.r > rows - 1)
				return false;
			return true;
		}

		// default to floor layer for map referenced tile<-->pos
		public Vector3 posForTile (TileCoord coord)
		{
			return floorLayer.posForTile (coord);
		}

		// float version for when don't want to treat tiles on unit grid
		// default to floor layer for map referenced tile<-->pos
		public Vector3 posForTile (Vector2 coordFloat)
		{
			return floorLayer.posForTile (coordFloat);
		}
		
		// calculating the tile coordinates from world location
		public TileCoord tileForPos (Vector2 pos)
		{
			return floorLayer.tileForPos (pos);
		}

		public bool hasWall (TileCoord coord)
		{
			return wallLayer.tileExists (coord);
		}

	}
}