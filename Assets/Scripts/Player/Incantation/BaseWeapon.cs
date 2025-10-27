using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    [HideInInspector] public IncantationSO weaponData;

    protected float nextFireTime;
    protected bool isAttacking;

    public virtual void Initialize(IncantationSO data)
    {
        weaponData = Instantiate(data); // ���� �ν��Ͻ�ȭ (��ȭ/���濡 ���� �� ��)
    }

    protected bool CanFire()
    {
        return Time.time >= nextFireTime && !isAttacking;
    }

    public abstract void Attack();
}
