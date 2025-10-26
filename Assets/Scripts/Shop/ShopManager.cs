using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    public int playerGold = 1000;

    public int barrierCost = 500;
    public int weaponUpgradeCost = 100;

    public Transform crystal;
    public GameObject barrierPrefab;
    public PlayerWeapon playerWeapon;
    public NavMeshSurface surface;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public bool TryPurchase(int cost)
    {
        if (playerGold >= cost)
        {
            playerGold -= cost;
            return true;
        }

        Debug.Log("∞ÒµÂ ∫Œ¡∑");
        return false;
    }

    public void BuyBarrier(int barrierCost)
    {
        if (TryPurchase(barrierCost))
        {
            SpawnBarrier();
            barrierCost += 500;
        }
    }

    public void UpgradeWeapon()
    {
        if (!TryPurchase(weaponUpgradeCost))
        {
            playerWeapon.UpgradeWeapon();
            weaponUpgradeCost += 100;
        }
    }

    void SpawnBarrier()
    {
        float radius = 6f;
        Vector3 randomPos = crystal.position + (Random.onUnitSphere * radius);
        randomPos.y = crystal.position.y;

        GameObject newBarrier = Instantiate(barrierPrefab, randomPos, Quaternion.identity);
        newBarrier.transform.parent = crystal;

        if (surface != null)
        {
            surface.BuildNavMesh();
        }
    }
}
