using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {
	public bool isSolid;
	public int posX, posY;

	//gCost is the cost to travel from the starting node to this node
	//hCost is the heuristic estimating the cost from this node to the goal
	//In this case, hCost measures Manhattan Distance from this node to the goal
	public int gCost, hCost;
	public int fCost {get {return gCost + hCost;}}

	//parent is used to generate the path back from the goal to the starting node
	public Node parent;

	public Node(bool solid, int x, int y) {
		isSolid = solid;
		posX = x;
		posY = y;
	}
}
