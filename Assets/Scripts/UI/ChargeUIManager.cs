using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeUIManager : MonoBehaviour
{
    public static ChargeUIManager Instance { get; private set; }

    [Header("UI Elements")]
    public GameObject chargePanel;   // ��ü �г�
    public Image chargeBarFill;      // ä������ ��

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetActive(bool active)
    {
        chargePanel.SetActive(active);
        if (!active)
            chargeBarFill.fillAmount = 0f;
    }

    public void UpdateChargeProgress(float progress)
    {
        chargeBarFill.fillAmount = Mathf.Clamp01(progress);
    }
}
