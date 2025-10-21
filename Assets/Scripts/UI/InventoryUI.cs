using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;

    private KeyCode inventoryKey = KeyCode.Tab;

    private void Update()
    {
        HandleInventory();
    }

    void HandleInventory()
    {
        if (Input.GetKey(inventoryKey))
        {
            inventoryPanel.SetActive(true);
        }
        else
        {
            inventoryPanel.SetActive(false);
        }
    }
}
