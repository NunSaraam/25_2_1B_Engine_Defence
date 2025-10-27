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
        InitializeInventory();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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

            if (isInventoryOpen)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0f;
                RefreshInventory();
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1f;
            }
        }
    }

    void InitializeInventory()
    {
        foreach (Transform child in slotContainer)
        {
            Destroy(child.gameObject);
        }

        List<IncantationSO> weapons = WeaponManager.Instance.allWeapons; //  ¼öÁ¤µÊ

        for (int i = 0; i < weapons.Count; i++)
        {
            GameObject slotInstance = Instantiate(weaponSlotPrefab, slotContainer);

            TMP_Text slotText = slotInstance.GetComponentInChildren<TMP_Text>();
            if (slotText != null)
            {
                slotText.text = $"{weapons[i].incantationName} (Lv.{weapons[i].level})";
            }

            Button slotButton = slotInstance.GetComponent<Button>();
            if (slotButton != null)
            {
                int weaponIndex = i;
                slotButton.onClick.AddListener(() =>
                {
                    WeaponManager.Instance.EquipWeapon(weaponIndex);
                    HighlightCurrentWeapon(weaponIndex);
                });
            }
        }
    }

    public void RefreshInventory()
    {
        foreach (Transform child in slotContainer)
        {
            Destroy(child.gameObject);
        }

        List<IncantationSO> weapons = WeaponManager.Instance.allWeapons; //  ¼öÁ¤µÊ

        for (int i = 0; i < weapons.Count; i++)
        {
            GameObject slotInstance = Instantiate(weaponSlotPrefab, slotContainer);

            TMP_Text slotText = slotInstance.GetComponentInChildren<TMP_Text>();
            if (slotText != null)
            {
                slotText.text = $"{weapons[i].incantationName} (Lv.{weapons[i].level})";
            }

            Button slotButton = slotInstance.GetComponent<Button>();
            if (slotButton != null)
            {
                int weaponIndex = i;
                slotButton.onClick.AddListener(() =>
                {
                    WeaponManager.Instance.EquipWeapon(weaponIndex);
                    HighlightCurrentWeapon(weaponIndex);
                });
            }

            if (i == WeaponManager.Instance.GetCurrentWeaponIndex())
            {
                HighlightCurrentWeapon(i);
            }
        }
    }

    private void HighlightCurrentWeapon(int index)
    {
        for (int i = 0; i < slotContainer.childCount; i++)
        {
            Image img = slotContainer.GetChild(i).GetComponent<Image>();
            if (img != null)
            {
                img.color = (i == index) ? Color.yellow : Color.white;
            }
        }
    }
}