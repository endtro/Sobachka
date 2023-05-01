using UnityEngine;

    // Использование OnDisable для возврата объектов в пул оказалось очень плохой идеей, так как некоторым объектам необходимо, перед
    // возвратом, сбрасывать "родителя". В override-е OnDisable Юнити этого делать не хочет.
public abstract class PoolObject : MonoBehaviour
{
    private int _poolKey;

    public int PoolKey => _poolKey;

    //public virtual void OnDisable()
    //{
    //    ObjectPooling.Return(this);
    //}

    public void SetPoolKey(int poolKey)
    {
        _poolKey = poolKey;
    }
}
