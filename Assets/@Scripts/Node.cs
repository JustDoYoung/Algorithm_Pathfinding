using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IComparable<Node>
{
    public int gridX;
    public int gridY;

    public bool isWalkable;

    public int gCost; //��ΰ� g(n)
    public int hCost; //������ h(n)
    public Node parent;

    public Node(int gridY, int gridX, bool isWalkable)
    {
        this.gridX = gridX;
        this.gridY = gridY;
        this.isWalkable = isWalkable;
    }

    //������
    public int fCost
    {
        get { return gCost + hCost; }
    }

    public int CompareTo(Node other)
    {
        if (fCost > other.fCost) return 1;
        else if(fCost == other.fCost && hCost > other.hCost) return 1;

        return -1;
    }
}
