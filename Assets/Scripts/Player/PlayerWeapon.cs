using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private IncantationSO currentWeaponData;

    public Transform firePoint;
    public float nextFireTime;

    Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    public void Initialize(IncantationSO weaponData)
    {
        currentWeaponData = Instantiate(weaponData);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            TryFire();
        }
    }

    public void Fire()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        Vector3 targetPoint;
        targetPoint = ray.GetPoint(50f);

        Vector3 direction = (targetPoint - firePoint.position).normalized;

        if (currentWeaponData.incantationPrefab != null && firePoint != null)
        {
            GameObject proj = Instantiate(currentWeaponData.incantationPrefab, firePoint.position, Quaternion.LookRotation(direction));

            Projectile projectile = proj.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.SetDamage(currentWeaponData.damage);
            }
        }
    }

    public void TryFire()
    {
        if (Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + currentWeaponData.fireRate;
        }
    }

    public void UpgradeWeapon()
    {
        currentWeaponData.level++;
        currentWeaponData.damage *= 1.2f;
        currentWeaponData.fireRate *= 0.9f;
    }
}
