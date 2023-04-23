using System;
using System.Collections;
using UnityEngine;

public class Bullet : Projectile
{
    private WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();
    public override IEnumerator Move()
    {
        while (Time.time < DeathTime)
        {
            transform.Translate(MoveSpeed * Time.deltaTime * Vector3.up, Space.Self);

            yield return _waitForFixedUpdate;
        }

        // Проблема
        ObjectPooling<Projectile>.Return(this);
    }
}
