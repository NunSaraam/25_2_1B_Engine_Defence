using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private IncantationSO currentWeaponData;
    public GameObject WeaponModel { get; set; }

    public void Initialize(IncantationSO weaponData)
    {
        currentWeaponData = Instantiate(weaponData);
    }

    public void UpgradeWeapon()
    {
        if (currentWeaponData == null) return;

        currentWeaponData.level++;
        currentWeaponData.damage *= 1.2f;
        currentWeaponData.fireRate *= 0.9f;
    }

    public IncantationSO GetCurrentData() => currentWeaponData;
}