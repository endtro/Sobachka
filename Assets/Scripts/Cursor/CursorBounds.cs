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
    public OutOfBoundsBehavior Behaviour;

    public CursorBounds(float minX, float maxX, float minY, float maxY, OutOfBoundsBehavior behaviour)
    {
        MinX = minX;
        MaxX = maxX;
        MinY = minY;
        MaxY = maxY;
        Behaviour = behaviour;
    }
}
