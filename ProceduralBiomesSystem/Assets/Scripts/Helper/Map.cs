using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map<T>
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public T[,] Values { get; private set; }


    public Map(int width, int height)
    {
        Width = width;
        Height = height;
        Values = new T[width, height];
    }

    public Map(T[,] values)
    {
        Width = values.GetLength(0);
        Height = values.GetLength(1);
        Values = values;
    }

    public void Set(T[,] values)
    {
        Values = values;
    }

    public T this[int x, int y]
    {
        get => Values[x, y];
        set => Values[x, y] = value;
    }

    public bool Contains(int x, int y)
    {
        if (!IsInBounds(x, y))
            return false;

        return Values[x, y] != null;
    }

    bool IsInBounds(int x, int y)
    {
        return (x >= 0 && x < Width) && (y >= 0 && y < Height);
    }
}

