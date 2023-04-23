using System.Collections.Generic;
using UnityEngine;

public static class ObjectPooling<T> where T : PoolObject<T>
{
    private static Dictionary<int, ObjectPool<T>> _pools = new Dictionary<int, ObjectPool<T>>();

    public static T Get(T prefab)
    {
        int poolKey = prefab.GetInstanceID();

        T poolObject;

        if (_pools.ContainsKey(poolKey) == false)
        {
            _pools.Add(poolKey, new ObjectPool<T>());
        }
        else
        {
            if (_pools[poolKey].Count > 0)
            {
                poolObject = _pools[poolKey].Get();
                poolObject.gameObject.SetActive(true);

                return poolObject;
            }
        }


        poolObject = GameObject.Instantiate(prefab).GetComponent<T>();
        poolObject.SetPoolKey(poolKey);

        return poolObject;
    }

    public static void Return(T poolObject)
    {
        poolObject.gameObject.SetActive(false);

        _pools[poolObject.PoolKey].Return(poolObject);
    }
}
