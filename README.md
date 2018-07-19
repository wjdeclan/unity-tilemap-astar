## AStar for Unity Tilemaps

This is an A* Pathfinder for Unity 2D that uses navigates Tilemaps.

###Usage
To build an AStarGrid, first create an empty gameObject, and attach the AStarGrid to that object.

Position the object in the center of the area you want to path in, and then customize the settings of your AStarGrid.

* Collidable Map: The TileMap containing only tiles that you want to disallow pathing through.
* Grid World Size: The size of the pathing area in world units
* Resolution: How large each A* node should be in world units. For example, if your TileMap tiles are 1:1 with world units, and your resolution is 0.5, you will have 4 A* nodes per tile.
* Allow Diagonals: Allowing pathing diagonally, if unchecked, paths will only be horizontal/vertical.
* Show Grid: For debugging purposes, draws Gizmos onto the scene representing the AStarGrid, yellow indicates a blocked node, white indicates a passable node.

To use the AStarPathfinding script, simply drop it into an empty gameObject, then attach a reference to the AStarGrid you want to pathfind in. Draw Path can be used similarly to AStarGrid's Show Grid, and will highlight the last calculated path in red.

Any object can use the pathfinder with a reference to the AStarPathfinding script, via the function pathfinder.FindPath(Vector3 start, Vector3 end), the returned list is a list of Node objects, with the last object being the target node. The starting node is not included in this list.

To convert a node to a Vector3 representing a world point, you can use the function pathfinding.WorldPointFromNode(node) which gives the position of the center of the node.