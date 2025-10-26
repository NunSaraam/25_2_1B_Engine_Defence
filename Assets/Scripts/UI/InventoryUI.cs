using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject weaponSlotPrefab;
    public Transform slotContainer;
    private KeyCode inventoryKey = KeyCode.Tab;

    private bool isInventoryOpen = false;

    private void Start()
    {
        inventoryPanel.SetActive(false);
        InitializedInventory();
    }

    private void Update()
    {
        HandleInventoryToggle();
    }

    void HandleInventoryToggle()
    {
        if (Input.GetKeyDown(inventoryKey))
        {
            isInventoryOpen = !isInventoryOpen;
            inventoryPanel.SetActive(isInventoryOpen);
        }
    }

    void InitializedInventory()
    {
        foreach (Transform child in slotContainer)
        {
            Destroy(child.gameObject);
        }

        List<IncantationSO> weapons = WeaponManager.Instance.allweapon;

        for (int i = 0; i < weapons.Count; i++)
        {
            GameObject slotInstance = Instantiate(weaponSlotPrefab, slotContainer);

            TMP_Text slotText = slotInstance.GetComponentInChildren<TMP_Text>();
            if (slotText != null)
            {
                slotText.text = weapons[i].incantationName;
            }

            Button slotButton = slotInstance.GetComponent<Button>();
            if (slotButton != null)
            {
                int weaponIndex = i;
                slotButton.onClick.AddListener(() =>
                {
                    WeaponManager.Instance.EquipWeapon(weaponIndex);
                });
            }
        }
    }
}
