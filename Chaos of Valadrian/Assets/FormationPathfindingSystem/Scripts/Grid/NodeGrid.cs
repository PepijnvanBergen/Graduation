using System.Collections.Generic;
using UnityEngine;

public class NodeGrid : MonoBehaviour
{
    public bool showGrid;
    public Vector2 gridWorldSize;

    // [TableMatrix(DrawElementMethod = "DrawElement")]
    //
    public Node[,] nodeGrid = new Node[21, 21];
    
    // static Node DrawElement(Rect rect, Node value)
    // {
    //     rect.size = new Vector2(5,5);
    //     
    //     value.fCost
    //     return value;
    // }
    
    public float nodeRadius;
    private float nodeDiameter;
    
    public LayerMask obstacles;

    public Transform gridPos;
    
    public bool aStar;

    public Node targetNode;
    private Vector2Int gridSize;
    private void Start()
    {
        //showGrid = true;
        nodeDiameter = nodeRadius * 2;
        gridSize.x = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSize.y = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        //CreateGrid();
    }

    //private void CreateGrid(int _andere, int _optioneel = 2, params int[] _intarray)
    private void CreateGrid()
    {
        nodeGrid = new Node[gridSize.x, gridSize.y];
        Vector3 worldBottomLeft =
            gridPos.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int z = 0; z < gridSize.y; z++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (z * nodeDiameter + nodeRadius) ;
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, obstacles));
                nodeGrid[x, z] = new Node(walkable, worldPoint,x ,z);
            }
        }
    }
    public List<Node> GetNeighbours(Node _node)
    {
        List<Node> neighbours = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                if (x == 0 && z == 0)
                {
                    continue;
                }
                int checkX = _node.posX + x;
                int checkZ = _node.posZ + z;
                if (checkX >= 0 && checkX < gridSize.x && checkZ >= 0 && checkZ < gridSize.y)
                {
                    neighbours.Add(nodeGrid[checkX, checkZ]);
                }
            }
        }
        return neighbours;
    }
    public Node NodeFromWorldPoint(Vector3 _worldPosition)
    {
        //     (a + b/2) / b = (a/b) + (b/(2*b)) = a/b + 1/2   [ = a/b + 0.5 ]
        // And calculating "a/b + 0.5" is easier for the PC than "(a + b/2) / b
        
        float percentX =  (_worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x; // _worldPosition.x / (gridWorldSize.x * nodeDiameter); // 
        float percentZ =  (_worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y; // _worldPosition.z / (gridWorldSize.y * nodeDiameter); // 
        percentX = Mathf.Clamp01(percentX);
        percentZ = Mathf.Clamp01(percentZ);
        
        int x = Mathf.RoundToInt((gridSize.x -1) * percentX);
        int z = Mathf.RoundToInt((gridSize.y -1) * percentZ);
        return nodeGrid[x, z];
    }

    public void CreateIntegrationField(Node _targetNode)
    {
        targetNode = _targetNode;
        targetNode.cost = 0;
        targetNode.bestCost = 0;
        Queue<Node> nodesToCheck = new Queue<Node>();
        nodesToCheck.Enqueue(targetNode);

        while (nodesToCheck.Count > 0)
        {
            Node currentNode = nodesToCheck.Dequeue();
            List<Node> currentNeighbours = GetNeighbours(currentNode);
            foreach (Node currentNeigbour in currentNeighbours)
            {
                if (currentNeigbour.cost == 255) continue;
                if (currentNeigbour.cost + currentNode.bestCost < currentNeigbour.bestCost)
                {
                    currentNeigbour.bestCost = (ushort) (currentNeigbour.cost + currentNode.bestCost);
                    nodesToCheck.Enqueue(currentNeigbour);
                }
            }
        }
    }
    public void CreateCostField()
    {
        Vector3 cellHalfExtents = Vector3.one * nodeRadius;
        int terrainMask = obstacles; //Add more to layermask check dat
        foreach (Node curNode in nodeGrid)
        {
            Collider[] obstacles = Physics.OverlapBox(curNode.worldPoint, cellHalfExtents, Quaternion.identity, terrainMask);
            bool hasIncreasedCost = false;
            foreach (Collider col in obstacles)
            {
                if (col.gameObject.layer == 6)
                {
                    curNode.IncreaseCost(255);
                    continue;
                }
                // else if (!hasIncreasedCost && col.gameObject.layer == 9)
                // {
                //     curNode.IncreaseCost(3);
                //     hasIncreasedCost = true;
                // }
            }
        }
    }
    public void CreateFlowField()
    {
        foreach(Node curNode in nodeGrid)
        {
            List<Node> curNeighbors = GetNeighbours(curNode);

            int bestCost = curNode.bestCost;

            foreach(Node curNeighbor in curNeighbors)
            {
                if(curNeighbor.bestCost < bestCost)
                {
                    bestCost = curNeighbor.bestCost;
                    curNode.bestDirection = GridDirection.GetDirectionFromV2I(new Vector2Int(curNeighbor.posX, curNeighbor.posZ) - new Vector2Int(curNode.posX, curNode.posZ));
                }
            }
        }
    }

    public List<Node> path;
    private void OnDrawGizmos()
    {
        if (showGrid)
        {
            float rgbFloat = 0;
            Gizmos.DrawWireCube(gridPos.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
            if (nodeGrid != null)
            {
                foreach (Node n in nodeGrid)
                {
                    Gizmos.color = (n.walkable)?Color.white:Color.red;
                    if (aStar)
                    {
                        if (path != null)
                        {
                            if (path.Contains(n))
                            {
                                Gizmos.color = Color.black;
                            }
                        }
                    }
                    else
                    {
                        //Debug.Log(n.bestCost);  
                        if (n.bestCost == 0)
                        {
                            Gizmos.color = Color.HSVToRGB(0.6f, 1, 1); // Ik snap dit niet het moeten wel float zijn domme kut
                            Gizmos.DrawCube(n.worldPoint, Vector3.one * (nodeDiameter-.1f));
                            continue;
                        }
                        rgbFloat = 1f / (float)n.bestCost;
                        Gizmos.color = Color.HSVToRGB(rgbFloat, 1, 1); //255, 68, 51   yellow (255,255,0), red 	(255,0,0)

                        if (n.influenceAmm != 0)
                        {
                            Gizmos.color = Color.HSVToRGB(0.85f, 1, 1);
                        }
                    }
                    Gizmos.DrawCube(n.worldPoint, Vector3.one * (nodeDiameter-.1f));
                }
            }   
        }
    }
}
