using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Pathfinding : MonoBehaviour
{
    public Transform seeker, target;
    private NodeGrid grid;
    public List<Node> openSet;
    public HashSet<Node> closedSet;
    public GridDebug gridDebug;

    public List<GridInfluencer> influencers;

    public influencerType pathfindingSort;
    private void Start()
    {
        grid = GetComponent<NodeGrid>();
        foreach (GridInfluencer gi in influencers)
        {
            gi.Initialize(grid);
        }
    }
    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            grid.CreateCostField();
            grid.CreateIntegrationField(grid.NodeFromWorldPoint(target.position));
            grid.CreateFlowField();
            gridDebug.DrawFlowField();
            grid.aStar = false;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            FindPath(seeker.position, target.position, pathfindingSort);
            grid.aStar = true;
        }
    }

    void FindPath(Vector3 _startPos, Vector3 _endPos, influencerType _sort)
    {
        Node startNode = grid.NodeFromWorldPoint(_startPos);
        Node targetNode = grid.NodeFromWorldPoint(_endPos);
        
        openSet = new List<Node>();
        closedSet = new HashSet<Node>();
        
        openSet.Add(startNode);
        
        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                //Check the nodeCosts.
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
            
            if (currentNode == targetNode) //Found the path
            {
                RetracePath(startNode,targetNode);
                return;
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    //RetracePath(startNode, targetNode);
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.prevNode = currentNode;
                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }
    void RetracePath(Node _startNode, Node _targetNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = _targetNode;
        
        while (currentNode != _startNode)
        {
            path.Add(currentNode);
            grid.path = path;
            currentNode = currentNode.prevNode;
        }
        path.Reverse();
        grid.path = path;
    }
    int GetDistance(Node _nodeA, Node _nodeB)
    {
        int distanceX = Mathf.Abs(_nodeA.posX - _nodeB.posX);
        int distanceZ = Mathf.Abs(_nodeA.posZ - _nodeB.posZ);
        if (distanceX > distanceZ)
        {
            return 14 * distanceZ + 10 * (distanceX - distanceZ);
        }
        return 14 * distanceX + 10 * (distanceZ - distanceX);
    }
}
