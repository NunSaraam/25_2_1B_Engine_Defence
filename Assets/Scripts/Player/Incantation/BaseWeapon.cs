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
        weaponData = Instantiate(data); // 개별 인스턴스화 (강화/변경에 영향 안 줌)
    }

    protected bool CanFire()
    {
        return Time.time >= nextFireTime && !isAttacking;
    }

    public abstract void Attack();
}
