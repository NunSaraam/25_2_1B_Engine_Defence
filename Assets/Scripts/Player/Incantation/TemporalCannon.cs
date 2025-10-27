using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporalCannon : BaseWeapon
{
    [Header("Setup")]
    public Transform firePoint;     // 발사 위치
    public Transform muzzle;        // 반동할 Transform

    [Header("Settings")]
    public float recoilAmount = 0.3f;
    private Vector3 originalPos;

    private void Start()
    {
        if (muzzle != null)
            originalPos = muzzle.localPosition;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
            Attack();
    }

    public override void Attack()
    {
        if (!CanFire() || weaponData == null) return;

        nextFireTime = Time.time + weaponData.fireRate;
        FireProjectile();
        StartCoroutine(RecoilEffect());
    }

    void FireProjectile()
    {
        GameObject proj = Instantiate(weaponData.incantationPrefab, firePoint.position, firePoint.rotation);


        if (proj.TryGetComponent(out Rigidbody rb))
            rb.AddForce(firePoint.forward * weaponData.projectileForce, ForceMode.Impulse);


        if (proj.TryGetComponent(out ExplosionProjectile explosion))
        {
            explosion.damage = weaponData.damage;
            explosion.explosionRadius = weaponData.explosionRadius;
        }

        Collider projCol = proj.GetComponent<Collider>();
        Collider ownerCol = GetComponentInParent<Collider>(); // Player나 무기 본체 Collider

        if (projCol != null && ownerCol != null)
            Physics.IgnoreCollision(projCol, ownerCol, true);
    }

    IEnumerator RecoilEffect()
    {
        if (muzzle == null) yield break;

        muzzle.localPosition -= Vector3.forward * recoilAmount;
        yield return new WaitForSeconds(0.05f);
        muzzle.localPosition = originalPos;
    }
}
