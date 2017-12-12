using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingManager : MonoBehaviour {

	List<Tile> openSet;
	HashSet<Tile> closedSet;
	private bool canDraw = false;
	public List<Tile> FindPath(Transform origin, Transform target)
	{
		Tile startTile = ActionPhaseManager.Instance._MapGenerator.Tiles.Find(tile => tile.xCoord == origin.position.x && tile.yCoord == origin.position.z);
		Tile targetTile = ActionPhaseManager.Instance._MapGenerator.Tiles.Find(tile => tile.xCoord == target.position.x && tile.yCoord == target.position.z);

		openSet = new List<Tile>();
		closedSet = new HashSet<Tile>();

		openSet.Add(startTile);
		canDraw = true;

		while(openSet.Count > 0)
		{
			Tile currentTile = openSet[0];

			for(int i = 1; i < openSet.Count; i++)
			{
				if(openSet[i].fCost < currentTile.fCost || openSet[i].fCost == currentTile.fCost && openSet[i].hCost < currentTile.hCost)
				{
					currentTile = openSet[i];
				}
			}

			openSet.Remove(currentTile);
			closedSet.Add(currentTile);

			if(currentTile == targetTile)
			{
				return RetracePath(startTile, targetTile);
			}

			foreach(Tile neighbour in currentTile.GetNeighbours())
			{
				var neighbourPos = neighbour.transform.position;
				neighbourPos.y = 0;
				var targetPos = targetTile.transform.position;
				targetPos.y = 0;

				if(!neighbour.walkable && neighbourPos != targetPos || closedSet.Contains(neighbour))
				{
					continue;
				}

				int newMovementCostToNeighbour = currentTile.gCost + GetDistance(currentTile, neighbour);
				if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
				{
					neighbour.gCost = newMovementCostToNeighbour;
					neighbour.hCost = GetDistance(neighbour, targetTile);
					neighbour.parent = currentTile;

					if(!openSet.Contains(neighbour))
					{
						openSet.Add(neighbour);
					}
				}
			}
		}
		return null;
	}

	List<Tile> RetracePath(Tile start, Tile target)
	{
		List<Tile> path = new List<Tile>();
		
		Tile currentTile = target;

		while(currentTile != start)
		{
			path.Add(currentTile);
			currentTile = currentTile.parent;
		}

		path.RemoveAt(0);	
		return path;
	}
	int GetDistance(Tile tileA, Tile tileB)
	{
		int distanceX = Mathf.Abs(tileA.xCoord - tileB.xCoord);
		int distanceY = Mathf.Abs(tileA.yCoord - tileB.yCoord);

		if(distanceX > distanceY)
		{
			return 14*(distanceY) + 10*(distanceX - distanceY);
		}
		else
		{
			return 14*(distanceX) + 10*(distanceY - distanceX);
		}
	}

	/*
	void OnDrawGizmos() {
		
		var size = new Vector3(0.5f, 1, 0.5f);
		if(!canDraw)
			return;

		foreach(Tile t in openSet)
		{
			var pos = t.transform.position;
			pos.y = 1;
			Gizmos.DrawCube(pos, size);
		}
		Gizmos.color = Color.red;

		foreach(Tile t in closedSet)
		{
			var pos = t.transform.position;
			pos.y = 1;
			Gizmos.DrawCube(pos, size);			
		}


	}
	 */
}
