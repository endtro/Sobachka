using UnityEngine;

public struct CursorBounds
{
    public enum OutOfBoundsBehavior
    {
        ClampCoordinates,
        DefaultPosition
    }

    public float MinX;
    public float MaxX;
    public float MinY;
    public float MaxY;
    public OutOfBoundsBehavior Behavior;

    public CursorBounds(float minX, float maxX, float minY, float maxY, OutOfBoundsBehavior behavior)
    {
        MinX = minX;
        MaxX = maxX;
        MinY = minY;
        MaxY = maxY;
        Behavior = behavior;
    }
}
