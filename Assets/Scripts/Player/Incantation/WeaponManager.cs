using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }

    public List<IncantationSO> allweapon;

    public PlayerWeapon currentWeapon;

    private int currentWeaponIndex = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (allweapon.Count >= 0)
        {
            EquipWeapon(0);
        }
    }

    public void EquipWeapon(int index)
    {
        if (index < 0 || index >= allweapon.Count)
            return;
        currentWeaponIndex = index;
        currentWeapon.Initialize(allweapon[index]);
    }

    public void EquipNextWeapon()
    {
        int next = (currentWeaponIndex + 1) % allweapon.Count;
        EquipWeapon(next);
    }

    public IncantationSO GetCurrentWeaponData()
    {
        return allweapon[currentWeaponIndex];
    }
}
