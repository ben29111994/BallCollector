using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class UpgradeSystem : MonoBehaviour
{
    [Header("Count System")]
    public GameObject handAnim;
    public TypeSystem typeSystem;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI nameText;
    public GameObject CountBlockImage;
    [SerializeField] private int countPrice;
    public int CountPrice
    {
        get
        {
            return countPrice;
        }
        set
        {
            countPrice = value;
            priceText.text = "$" + DataManager.CoinFixedText(countPrice);
        }
    }

    public enum TypeSystem
    {
        Timer,
        Size,
        Power
    }

    private void Awake()
    {
        UpdatePrice();
        switch ((int)typeSystem)
        {
            case 0:
                nameText.text = "TIMER LVL." + DataManager.Instance.TimerLevel;
                break;
            case 1:
                nameText.text = "SIZE LVL." + DataManager.Instance.SizeLevel;
                break;
            case 2:
                nameText.text = "POWER LVL." + DataManager.Instance.PowerLevel;
                break;
        }
        CountBlockImage = transform.GetChild(5).gameObject;
    }

    private void Update()
    {
        UpdateBlock();
    }

    private void UpdateBlock()
    {
        if (DataManager.Instance.Coin < CountPrice || IsMax())
        {
            CountBlockImage.gameObject.SetActive(true);
        }
        else
        {
            CountBlockImage.gameObject.SetActive(false);
        }
    }

    public void UpdatePrice()
    {
        if (IsMax())
        {
            priceText.text = "MAX";
            CountBlockImage.gameObject.SetActive(true);
        }
        else
        {
            CountPrice *= 2;
            priceText.text = CountPrice.ToString(); 
        }
    }

    private bool IsMax()
    {
        if(typeSystem == TypeSystem.Timer)
        {
            return false;
        }

        return Level() >= 7 ? true : false;
    }

    public void OnClink_Buy()
    {
        if (DataManager.Instance.Coin < CountPrice) return;
        DataManager.Instance.Coin -= CountPrice;
        UpgradeSucces();
    }

    public void UpgradeSucces()
    {
        switch ((int)typeSystem)
        {
            case 0:
                DataManager.Instance.TimerLevel++;
                OnCollected.Instance.UpgradeTimer(1);
                nameText.text = "TIMER LVL." + DataManager.Instance.TimerLevel;
                break;
            case 1:
                DataManager.Instance.SizeLevel++;
                OnCollected.Instance.UpgradeSize(1);
                nameText.text = "SIZE LVL." + DataManager.Instance.SizeLevel;
                break;
            case 2:
                DataManager.Instance.PowerLevel++;
                OnCollected.Instance.UpgradePower(1);
                nameText.text = "POWER LVL." + DataManager.Instance.PowerLevel;
                break;
        }

        HandAnim();
        UpdatePrice();
        transform.DOPunchScale(Vector3.one * 0.1f, 0.5f);
        //VibrationManager.Instance.Vibration();
    }

    private int BasePrice()
    {
        return (typeSystem == TypeSystem.Timer) ? 1000 : 200;
    }

    private int Level()
    {
        switch ((int)typeSystem)
        {
            case 0:
                return DataManager.Instance.TimerLevel;
            case 1:
                return DataManager.Instance.PowerLevel;
            case 2:
                return DataManager.Instance.SizeLevel;
        }

        return 0;
    }

    public void HandAnim()
    {
        if(C2_HandAnim != null)
        {
            StopCoroutine(C2_HandAnim);
            handAnim.SetActive(false);
        }
        C2_HandAnim = C_HandAnim();
        StartCoroutine(C2_HandAnim);
    }

    private IEnumerator C2_HandAnim;
    private IEnumerator C_HandAnim()
    {
        handAnim.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        handAnim.SetActive(false);
    }
}
