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
            //openSet에서 최소값 추출
            Node now = openSet.Dequeue();

            //현재 목적지면 경로 확정하고 종료
            if (now == grid.EndCell)
            {
                Retracepath();
                yield break;
            }

            closedSet.Add(now);

            yield return new WaitForSeconds(0.3f);

            //grid에서 openSet 셀들 표시
            foreach (var next in openSet.List)
                grid.PaintTile(next.gridX, next.gridY, grid.GetTile("Openset"));

            //현재 경로 표시
            RetracePath(now);

            //현재 셀에서 이웃한 셀들 중에 가장 가까운 노드 openSet에 삽입
            foreach (var next in grid.GetNeighbors(now))
            {
                if (next.isWalkable == false) continue;
                if (closedSet.Contains(next)) continue;

                grid.PaintTile(next.gridX, next.gridY, grid.GetTile("Search"));
                yield return new WaitForSeconds(0.1f);

                int g = now.gCost + GetDistance(now, next); // g는 현재~다음 셀의 거리
                int h = GetDistance(next, grid.EndCell); // h는 다음 셀부터 목적지까지 거리
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
