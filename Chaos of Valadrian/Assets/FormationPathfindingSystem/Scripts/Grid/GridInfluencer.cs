using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;


public class GridInfluencer : MonoBehaviour
{

    public influencerType influence;
    public int influenceAmm;
    public Vector3 currentPos;
    public List<Node> influencedNodes = new List<Node>();
    public Node currentNode;
    public NodeGrid myGrid;

    public void Initialize(NodeGrid _assignedGrid)
    {
        currentPos = Vector3.zero;
        myGrid = _assignedGrid;
        UpdateInfluence();
    }

    private void Update()
    {
        Vector3 checkPos = transform.position;
        if (currentPos != checkPos)
        {
            UpdateInfluence();
        }
    }
    public void UpdateInfluence()
    {
        //Update the current position and node
        currentPos = transform.position;
        currentNode = myGrid.NodeFromWorldPoint(currentPos);
        
        //Clear old influenced Nodes
        if (influencedNodes.Count > 0)
        {
            foreach (Node influencedNode in influencedNodes)
            {
                influencedNode.influence = influencerType.clear;
                influencedNode.influenceAmm = 0;
            }
            influencedNodes.Clear();
        }
        
        //Add neighbours and currentcel to influenced cells
        influencedNodes = myGrid.GetNeighbours(currentNode);
        influencedNodes.Add(currentNode);
        
        //Influence the cells
        foreach (Node influencedNode in influencedNodes)
        {
            influencedNode.influence = influence;
            influencedNode.influenceAmm = influenceAmm;
        }
    }
}
