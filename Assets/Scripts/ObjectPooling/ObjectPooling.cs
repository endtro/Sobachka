using System.Collections.Generic;
using UnityEngine;

// Хотелось бы это как-то все переписать под generic type, чтобы словарь хранил записи типа <int, T>, и можно было сразу принимать и отдавать
// объекты ссылками на нужные компоненты, тем самым избежав использования GetComponent. Сомневаюсь, что Dictionary позволяет такое делать.
public static class ObjectPooling
{
    private static Dictionary<int, Stack<PoolObject>> _pools = new Dictionary<int, Stack<PoolObject>>();

    public static PoolObject Get(PoolObject prefab)
    {
        int poolKey = prefab.GetInstanceID();

        PoolObject poolObject;

        if (_pools.ContainsKey(poolKey) == false)
        {
            _pools.Add(poolKey, new Stack<PoolObject>());
        }
        else
        {
            if (_pools[poolKey].Count > 0)
            {
                poolObject = _pools[poolKey].Pop();
                poolObject.gameObject.SetActive(true);

                return poolObject;
            }
        }

        poolObject = GameObject.Instantiate(prefab);
        poolObject.SetPoolKey(poolKey);

        return poolObject;
    }

    public static void Return(PoolObject poolObject)
    {
        _pools[poolObject.PoolKey].Push(poolObject);
    }
}
