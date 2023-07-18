using UnityEngine;

public class Node : IHeapItem<Node>
{
    //Both
    public bool walkable;
    public int posX;
    public int posZ;
    public Vector3 worldPoint;
    
    //A*
    public int gCost;
    public int hCost;
    public int fCost
    {
        get
        {
            if (influenceAmm != 0)
            {
                publicFCost = gCost + hCost + influenceAmm;
                return publicFCost;
            }
            // else if (influenceAmm < 0)
            // {
            //     publicFCost = gCost + hCost - influenceAmm;
            //     return publicFCost;
            // }
            else
            {
                publicFCost = gCost + hCost;
                return publicFCost;
            }
        }
    }
    public int publicFCost = 0;
    public Node prevNode;
    
    //FF
    public int cost;
    public ushort bestCost;
    public GridDirection bestDirection;
    
    //Influencers
    public influencerType influence;
    public int influenceAmm;

    private int heapIndex;
    
    public Node(bool _walkable, Vector3 _worldPoint, int _posX, int _posZ)
    {
        walkable = _walkable;
        worldPoint = _worldPoint;
        posX = _posX;
        posZ = _posZ;

        //FF
        cost = 1;
        if (!walkable) cost = 256;
        bestCost = ushort.MaxValue;
    }

    public void IncreaseCost(int _amm)
    {
        if (cost == 255) return;
        if (_amm + cost >= 255) cost = 255;
        else
        {
            cost += _amm;
        }
    }

    private int lastInfluenceCost = 0;
    public void InfluenceCost(int _amm)
    {
        lastInfluenceCost = _amm;
        
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Node _nodeToCompareTo)
    {
        int compare = fCost.CompareTo(_nodeToCompareTo.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(_nodeToCompareTo.hCost);
        }

        return compare;
    }
}