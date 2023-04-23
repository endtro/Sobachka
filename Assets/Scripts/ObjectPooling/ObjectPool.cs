using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    private Stack<T> _poolItems = new Stack<T>();

    public int Count => _poolItems.Count;

    public T Get()
    {
        return _poolItems.Pop();
    }

    public void Return(T poolObject)
    {
        _poolItems.Push(poolObject);
    }
}
