using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    [Header("Player Economy")]
    public int playerGold = 1000;
    public TMP_Text goldText;

    [Header("Item Prices")]
    public int barrierCost = 500;
    public int weaponUpgradeCost = 200;

    [Header("References")]
    public Transform crystal;
    public GameObject barrierPrefab;
    public PlayerWeapon playerWeapon;
    public NavMeshSurface surface;

    public delegate void GoldChangedHandler(int newGold);
    public event GoldChangedHandler OnGoldChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        OnGoldChanged?.Invoke(playerGold);
        UpdateGoldUI();
    }

    public void AddGold(int amount)
    {
        playerGold += amount;
        UpdateGoldUI();
        OnGoldChanged?.Invoke(playerGold);
    }

    public bool TryPurchase(int cost)
    {
        if (playerGold >= cost)
        {
            playerGold -= cost;
            UpdateGoldUI();
            OnGoldChanged?.Invoke(playerGold);
            return true;
        }
        Debug.Log("골드 부족");
        return false;
    }

    void UpdateGoldUI()
    {
        if (goldText != null)
        {
            goldText.text = $"Gold: {playerGold}";
        }
    }

    public void BuyBarrier()
    {
        if (TryPurchase(barrierCost))
        {
            SpawnBarrier();
            barrierCost += 200; // 구매 시 가격 상승
            Debug.Log($" 배리어 구매! 다음 가격: {barrierCost}");
        }
    }

    //public void UpgradeWeapon()
    //{
    //    if (TryPurchase(weaponUpgradeCost))
    //    {
    //        playerWeapon.UpgradeWeapon();
    //        weaponUpgradeCost += 100; // 강화 시 가격 상승
    //        Debug.Log($" 무기 강화! 다음 가격: {weaponUpgradeCost}");
    //    }
    //}

    void SpawnBarrier()
    {
        if (barrierPrefab == null)
        {
            Debug.LogError("SpawnBarrier 실패: barrierPrefab이 할당되지 않음!");
            return;
        }
        if (crystal == null)
        {
            Debug.LogError("SpawnBarrier 실패: crystal(기준 오브젝트)가 할당되지 않음!");
            return;
        }

        //float radius = 6f;
        //Vector3 randomDir = Random.insideUnitSphere * radius;
        //randomDir.y = 0f;
        Vector3 candidate = crystal.position;// + randomDir;

        // NavMesh 위의 유효한 위치로 샘플링 (반경 5m)
        if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, 5f, NavMesh.AllAreas))
        {
            GameObject newBarrier = Instantiate(barrierPrefab, hit.position, Quaternion.identity);
            newBarrier.transform.SetParent(crystal, true);
            Debug.Log($"Barrier 생성됨 at {hit.position}");


        }
    }
}


