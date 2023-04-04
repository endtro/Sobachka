using System;
using System.Collections;
using UnityEngine;

public class Bullet : Projectile
{
    // Правильной было бы как-то синхронизовать эту корутину с тиками FixedUpdate, но я не уверен как именно это делается правильно.
    // Предположу, что нужен yield return WaitForSeconds размером в Time.fixedDeltaTime, или что-то типа того. По идее это приведет к тому,
    // что корутина перестанет срабатывать чаще FixedUpdate, однако, поскольку я понятия не имею что Юнити делает "под капотом", я не уверен,
    // что это так же не приведет к отставанию корутины и, по сути, к пропуску некоторых тиков.
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
