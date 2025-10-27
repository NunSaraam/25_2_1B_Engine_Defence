using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject shopPanel;
    //public TMP_Text goldText;
    public TMP_Text barrierPriceText;
    //public TMP_Text weaponPriceText;

    public Button barrierButton;
    //public Button weaponUpgradeButton;

    private bool isShopOpen = false;
    private KeyCode shopKey = KeyCode.B; // 상점 열기 키

    private void Start()
    {
        shopPanel.SetActive(false);

        ShopManager.Instance.OnGoldChanged += UpdateGoldUI;

        barrierButton.onClick.AddListener(OnBuyBarrier);
        //weaponUpgradeButton.onClick.AddListener(OnUpgradeWeapon);

        UpdateUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(shopKey))
        {
            ToggleShop();
        }
    }

    void ToggleShop()
    {
        isShopOpen = !isShopOpen;
        shopPanel.SetActive(isShopOpen);

        if (isShopOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }

        UpdateUI();
    }

    void OnBuyBarrier()
    {
        ShopManager.Instance.BuyBarrier();
        UpdateUI();
    }

    //void OnUpgradeWeapon()
    //{
    //    ShopManager.Instance.UpgradeWeapon();
    //    UpdateUI();
    //}

    void UpdateUI()
    {
        //goldText.text = $"Gold: {ShopManager.Instance.playerGold}";
        barrierPriceText.text = $"Barrier ({ShopManager.Instance.barrierCost}G)";
        //weaponPriceText.text = $"Upgrade ({ShopManager.Instance.weaponUpgradeCost}G)";
    }

    void UpdateGoldUI(int newGold)
    {
        //goldText.text = $"Gold: {newGold}";
    }
}

