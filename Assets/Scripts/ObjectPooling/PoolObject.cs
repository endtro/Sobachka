using UnityEngine;

public abstract class PoolObject : MonoBehaviour
{
    private int _poolKey;

    public int PoolKey => _poolKey;

    // ������������� OnDisable ��� �������� �������� � ��� ��������� ����� ������ �����, ��� ��� ��������� �������� ����������, �����
    // ���������, ���������� "��������". � override-� OnDisable ����� ����� ������ �� �����.
    // ��� ������������ - ����. ��������, � �����.
    public virtual void OnDisable()
    {
        ObjectPooling.Return(this);
    }

    public void SetPoolKey(int poolKey)
    {
        _poolKey = poolKey;
    }
}
