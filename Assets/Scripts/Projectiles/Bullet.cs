using System;
using System.Collections;
using UnityEngine;

public class Bullet : Projectile
{
    // ���������� ���� �� ���-�� �������������� ��� �������� � ������ FixedUpdate, �� � �� ������ ��� ������ ��� �������� ���������.
    // ����������, ��� ����� yield return WaitForSeconds �������� � Time.fixedDeltaTime, ��� ���-�� ���� ����. �� ���� ��� �������� � ����,
    // ��� �������� ���������� ����������� ���� FixedUpdate, ������, ��������� � ������� �� ���� ��� ����� ������ "��� �������", � �� ������,
    // ��� ��� ��� �� �� �������� � ���������� �������� �, �� ����, � �������� ��������� �����.
    public override IEnumerator Move()
    {
        while (Time.time < DeathTime)
        {
            transform.Translate(MoveSpeed * Time.deltaTime * Vector3.up, Space.Self);

            yield return null;
        }

        gameObject.SetActive(false);
    }
}
