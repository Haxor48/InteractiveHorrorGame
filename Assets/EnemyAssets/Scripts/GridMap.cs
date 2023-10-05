using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap
{
    private int width;
    private int length;
    private float cellSize;
    private float height;
    private GameObject plane;
    private int[,] gridArray;
    private TextMesh[,] debugTextArray;

    public GridMap(int width, int length, float cellSize, GameObject plane)
    {
        this.width = width;
        this.length = length;
        this.cellSize = cellSize;
        this.plane = plane;
        height = plane.transform.position.y;
        gridArray = new int[width, length];
        debugTextArray = new TextMesh[width, length];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                //debugTextArray[x, y] = CreateText(gridArray[x, y].ToString(), GetWorldPosition(x, y) + new Vector3(cellSize, 0, cellSize) / 2);

                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100);
            }
            Debug.DrawLine(GetWorldPosition(0, length), GetWorldPosition(width, length), Color.white, 100);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, length), Color.white, 100);
        }

        SetValue(2, 1, 69);
    }

    public void SetValue(int x, int y, int value)
    {
        if (x >= 0 && y >= 0 && x < width && y < length)
        {
            gridArray[x, y] = value;
            //debugTextArray[x, y].text = gridArray[x, y].ToString();
        }
    }

    private Vector2Int GetXY(Vector3 WorldPosition)
    {
        return new Vector2Int(Mathf.FloorToInt(WorldPosition.x / cellSize), Mathf.FloorToInt(WorldPosition.y / cellSize));
    }

    public void SetValue(Vector3 WorldPosition, int value)
    {
        SetValue(GetXY(WorldPosition).x, GetXY(WorldPosition).y, value);
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, height / 10, y) * cellSize;
    }

    public int GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < length)
        {
            return gridArray[x, y];
        }
        return 0;
    }

    public int GetValue(Vector3 WorldPosition)
    {
        return GetValue(GetXY(WorldPosition).x, GetXY(WorldPosition).y);
    }

    public static TextMesh CreateText(string text, Vector3 pos)
    {
        GameObject g = new GameObject("World_Text", typeof(TextMesh));
        Transform t = g.transform;
        t.localPosition = pos;
        TextMesh tm = g.GetComponent<TextMesh>();
        tm.text = text;
        tm.fontSize = 20;
        tm.color = Color.white;
        return tm;
    }
}
