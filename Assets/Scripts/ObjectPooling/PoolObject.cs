using UnityEngine;

    // ������������� OnDisable ��� �������� �������� � ��� ��������� ����� ������ �����, ��� ��� ��������� �������� ����������, �����
    // ���������, ���������� "��������". � override-� OnDisable ����� ����� ������ �� �����.
public abstract class PoolObject<T> : MonoBehaviour where T : PoolObject<T>
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
