using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStarGrid : MonoBehaviour {
	//Grid parameters:
	//collidableMap is the TileMap containing collidable tiles
	//gridWorldSize is the size of the grid in world units centered in the middle of the grid
	//resolution is the size of each A* node in world units
	//allowDiagonals determines whether diagonal nodes are returned as neighbor
	public Tilemap collidableMap;
	public Vector2 gridWorldSize;
	public float resolution;
	public bool allowDiagonals;

	//For debugging purposes, shows the A* grid
	public bool showGrid;

	//The node map
	Node[,] nodes;

	//gridX, gridY: Size of grid in resolution units in x and y
	//offset is resolution / 2
	private int gridX, gridY;
	private float offset;

	void Start () {
		//set gridX, gridY, offset
		gridX = Mathf.RoundToInt(gridWorldSize.x / resolution);
		gridY = Mathf.RoundToInt(gridWorldSize.y / resolution);

		offset = resolution / 2;

		//start building the grid
		BuildGrid();
	}

	void BuildGrid() {
		//create the map array
		nodes = new Node[gridX, gridY];
		//Bottom left corner of bottom left tile
		Vector3 startPos = transform.position - new Vector3(gridWorldSize.x / 2, gridWorldSize.y / 2, 0);
		//iterate across the map space
		for (int x = 0; x < gridX; x++) {
			for (int y = 0; y < gridY; y++) {
				//check the middle of each node
				Vector3 checkPos = startPos + new Vector3(x * resolution + offset, y * resolution + offset, 0);
				bool isSolid = false;

				//if there is a collidable tile there, then mark the node as solid
				if (collidableMap.HasTile(collidableMap.WorldToCell(checkPos))) {
					isSolid = true;
				}

				//update the node map
				nodes[x, y] = new Node(isSolid, x, y);
			}
		}
	}

	public List<Node> GetNeighboringNodes(Node node) {
		//return value, contains a list of neighboring nodes
		List<Node> neighborList = new List<Node>();

		int x = node.posX;
		int y = node.posY;

		//edge checks
		bool left = x != 0;
		bool right = x != gridX - 1;
		bool down = y != 0;
		bool up = y != gridY - 1;

		//Horizontal and vertical neighbors
		if (left) {
			neighborList.Add(nodes[x-1, y]);
		}
		if (right) {
			neighborList.Add(nodes[x+1, y]);
		}
		if (down) {
			neighborList.Add(nodes[x, y-1]);
		}
		if (up) {
			neighborList.Add(nodes[x, y+1]);
		}

		//Diagonal neighbors
		if (allowDiagonals) {
			if (left) {
					if (down) {
						if (!nodes[x-1, y].isSolid && !nodes[x, y-1].isSolid) {
							neighborList.Add(nodes[x-1, y-1]);
						}
					}
					if (up) {
						if (!nodes[x-1, y].isSolid && !nodes[x, y+1].isSolid) {
							neighborList.Add(nodes[x-1, y+1]);
						}
					}
			}
			if (right) {
				if (down) {
					if (!nodes[x+1, y].isSolid && !nodes[x, y-1].isSolid) {
						neighborList.Add(nodes[x+1, y-1]);
					}
				}
				if (up) {
					if (!nodes[x+1, y].isSolid && !nodes[x, y+1].isSolid) {
						neighborList.Add(nodes[x+1, y+1]);
					}
				}
			}
		}

		return neighborList;
	}

	public Node NodeFromWorldPoint(Vector3 worldPos) {
		//Center on the world center
		Vector3 centered = worldPos - transform.position;

		//Get pos at % of total grid width, limit between 0-1, multiply by # of nodes - 1 for position in grid
		int x = Mathf.RoundToInt(Mathf.Clamp01(centered.x / gridWorldSize.x + 0.5f) * (gridX - 1));
		int y = Mathf.RoundToInt(Mathf.Clamp01(centered.y / gridWorldSize.y + 0.5f) * (gridY - 1));

		return nodes[x, y];
	}

	public Vector3 WorldPointFromNode(Node node) {
		//Center on grid position
		Vector3 pos = transform.position;

		pos.x = pos.x - (gridWorldSize.x / 2) + ((node.posX + 0.5f) * resolution);
		pos.y = pos.y - (gridWorldSize.y / 2) + ((node.posY + 0.5f) * resolution);

		return pos;
	}

	private void OnDrawGizmos() {
		//if debugging
		if (showGrid && nodes != null) {
			//color solid nodes yellow, moveable nodes white
			foreach (Node n in nodes) {
				if (n.isSolid) {
					Gizmos.color = Color.yellow;
				} else {
					Gizmos.color = Color.white;
				}
				Gizmos.DrawWireCube(WorldPointFromNode(n), new Vector3(1, 1, 1) * resolution);
			}
		}
	}
}
