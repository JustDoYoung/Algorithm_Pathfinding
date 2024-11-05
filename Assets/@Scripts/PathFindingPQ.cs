using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PathFindingPQ : MonoBehaviour
{
    Grid grid;

    PriorityQueue<Node> openSet = new PriorityQueue<Node>();
    HashSet<Node> closedSet = new HashSet<Node>();

    private void Start()
    {
        grid = GetComponent<Grid>();

        StartCoroutine(Findpath());
    }

    IEnumerator Findpath()
    {
        openSet.Enqueue(grid.StartCell);

        while (openSet.Count > 0)
        {
            //openSet���� �ּҰ� ����
            Node now = openSet.Dequeue();

            //���� �������� ��� Ȯ���ϰ� ����
            if (now == grid.EndCell)
            {
                Retracepath();
                yield break;
            }

            closedSet.Add(now);

            yield return new WaitForSeconds(0.3f);

            //grid���� openSet ���� ǥ��
            foreach (var next in openSet.List)
                grid.PaintTile(next.gridX, next.gridY, grid.GetTile("Openset"));

            //���� ��� ǥ��
            RetracePath(now);

            //���� ������ �̿��� ���� �߿� ���� ����� ��� openSet�� ����
            foreach (var next in grid.GetNeighbors(now))
            {
                if (next.isWalkable == false) continue;
                if (closedSet.Contains(next)) continue;

                grid.PaintTile(next.gridX, next.gridY, grid.GetTile("Search"));
                yield return new WaitForSeconds(0.1f);

                int g = now.gCost + GetDistance(now, next); // g�� ����~���� ���� �Ÿ�
                int h = GetDistance(next, grid.EndCell); // h�� ���� ������ ���������� �Ÿ�
                int f = g + h;

                if (openSet.Contains(next))
                {
                    if (next.fCost > f)
                    {
                        next.parent = now;
                        next.gCost = g;
                        openSet.Reposition(next);
                    }
                }
                else
                {
                    next.gCost = g;
                    next.hCost = h;
                    next.parent = now;
                    openSet.Enqueue(next);
                }
            }
        }

        print("End");
    }

    void RetracePath(Node node)
    {
        foreach (Node n in closedSet)
        {
            if (n == grid.StartCell || n == grid.EndCell) continue;
            grid.PaintTile(n.gridX, n.gridY, grid.GetTile("Done"));
        }

        if (node != grid.StartCell && node != grid.EndCell)
            grid.PaintTile(node.gridX, node.gridY, grid.GetTile("Possesed"));
    }

    void Retracepath()
    {
        Node now = grid.EndCell;

        foreach (var next in openSet.List)
            grid.PaintTile(next.gridX, next.gridY, grid.GetTile("Openset"));

        while (now != grid.StartCell)
        {
            grid.PaintTile(now.gridX, now.gridY, grid.GetTile("Possesed"));
            now = now.parent;
        }
    }

    int GetDistance(Node from, Node to)
    {
        int dx = Mathf.Abs(from.gridX - to.gridX);
        int dy = Mathf.Abs(from.gridY - to.gridY);

        if (dx > dy)
        {
            return 14 * dy + 10 * (dx - dy);
        }

        return 14 * dx + 10 * (dy - dx);
    }

    protected void OnDrawGizmos()
    {
        grid?.DisplayCost(openSet.List);
    }
}
