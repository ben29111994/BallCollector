using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeController : MonoBehaviour
{
    private static UpgradeController instance;
    public static UpgradeController Instance { get { return instance; } }

    public UpgradeSystem[] upgradeSystem;

    private void Awake()
    {
        instance = (instance == null) ? this : instance;
    }

    public void UpdatePriceSystem()
    {
        for (int i = 0; i < upgradeSystem.Length; i++) upgradeSystem[i].UpdatePrice();
    }

    public void Active(bool isActive)
    {
        upgradeSystem[0].transform.parent.gameObject.SetActive(isActive);
    }

    public float GetSumPrice()
    {
        float sumPrice = 0.0f;
        for (int i = 0; i < upgradeSystem.Length; i++) sumPrice += upgradeSystem[i].CountPrice;
        return sumPrice;
    }
}
