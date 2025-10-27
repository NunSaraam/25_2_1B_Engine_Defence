using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChronoRepeater : BaseWeapon
{
    public Transform firePoint;
    public GameObject weaponModel;

    private void Update()
    {

        if (!isAttacking && weaponModel != null)
            weaponModel.transform.Rotate(Vector3.up * 180 * Time.deltaTime);

        if (Input.GetMouseButton(0))
            Attack();
    }

    public override void Attack()
    {
        if (!CanFire() || weaponData == null) return;

        StartCoroutine(FireSequence());
        nextFireTime = Time.time + weaponData.fireRate;
    }

    private IEnumerator FireSequence()
    {
        isAttacking = true;


        if (weaponModel != null) weaponModel.SetActive(false);


        GameObject proj = Instantiate(weaponData.incantationPrefab, firePoint.position, firePoint.rotation);
        if (proj.TryGetComponent(out Projectile projectile))
            projectile.SetDamage(weaponData.damage);

        yield return new WaitForSeconds(weaponData.fireRate * 0.7f);


        if (weaponModel != null) weaponModel.SetActive(true);
        isAttacking = false;
    }
}

