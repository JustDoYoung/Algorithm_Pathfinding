using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Grid : MonoBehaviour
{
    [SerializeField]
    List<TileBase> tiles;

    Node[,] grid;
    Tilemap tilemap;
    int yMin, xMin, yMax, xMax;
    int ySize, xSize;

    public Node StartCell, EndCell;

    private void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        
        Init();

        //tilemap.SetTile(new Vector3Int(0, -10), GetTile("Search"));
    }

    void Init()
    {
        yMin = tilemap.cellBounds.yMin;
        xMin = tilemap.cellBounds.xMin;
        yMax = tilemap.cellBounds.yMax;
        xMax = tilemap.cellBounds.xMax;

        ySize = yMax - yMin + 1;
        xSize = xMax - xMin + 1;

        grid = new Node[ySize, xSize];

        for (int y = yMax; y >= yMin; y--)
        {
            for (int x = xMin; x <= xMax; x++)
            {
                TileBase tile = tilemap.GetTile(new Vector3Int(x, y));
                
                if (tile != null)
                {
                    grid[yMax - y, x + xMax] = new Node(y, x, CheckTileWalkable(tile));
                    string name = tile.name;
                    switch (name)
                    {
                        case "Start":
                            {
                                StartCell = grid[yMax - y, x + xMax];
                                break;
                            }
                        case "End":
                            {
                                EndCell = grid[yMax - y, x + xMax];
                                break;
                            }
                    }
                }
            }
        }
    }

    public TileBase GetTile(string name)
    {
        foreach(TileBase tile in tiles)
        {
            if (tile.name.Contains(name))
                return tile;
        }

        return null;
    }

    public List<Node> GetNeighbors(Node now)
    {
        List<Node> ret = new List<Node>();

        int nowX = now.gridX;
        int nowY = now.gridY;

        for(int j = -1; j <= 1; j++)
        {
            for(int i = -1; i <= 1; i++)
            {
                int nextX = nowX + i;
                int nextY = nowY + j;

                if (nextX < xMin || nextX > xMax|| nextY < yMin || nextY > yMax) continue;

                ret.Add(grid[yMax-nextY, nextX+xMax]);
            }
        }

        return ret;
    }

    public void PaintTile(int x, int y, TileBase tile)
    {
        tilemap.SetTile(new Vector3Int(x, y), tile);
    }

    bool CheckTileWalkable(TileBase tile)
    {
        if (tile.name == "Unwalkable")
            return false;
        else
            return true;
    }
}
