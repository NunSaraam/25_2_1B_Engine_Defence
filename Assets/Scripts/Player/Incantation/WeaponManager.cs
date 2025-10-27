using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }

    public List<IncantationSO> allWeapons;
    public Transform weaponHolder;
    public PlayerWeapon playerWeapon;

    private BaseWeapon currentWeaponInstance;
    private int currentWeaponIndex = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (allWeapons.Count > 0)
        {
            EquipWeapon(0);
        }
    }

    public void EquipWeapon(int index)
    {
        if (index < 0 || index >= allWeapons.Count) return;

        currentWeaponIndex = index;
        IncantationSO weaponData = allWeapons[index];

        playerWeapon.Initialize(weaponData);

        if (currentWeaponInstance != null)
        {
            Destroy(currentWeaponInstance.gameObject);
        }

        GameObject weaponObj = Instantiate(weaponData.weaponPrefab, weaponHolder);
        weaponObj.SetActive(true);
        currentWeaponInstance = weaponObj.GetComponent<BaseWeapon>();
        currentWeaponInstance.Initialize(weaponData);

        playerWeapon.WeaponModel = weaponObj;
    }

    public BaseWeapon GetCurrentWeapon() => currentWeaponInstance;
    public IncantationSO GetCurrentWeaponData() => allWeapons[currentWeaponIndex];
    public int GetCurrentWeaponIndex()
    {
        return currentWeaponIndex;
    }
}