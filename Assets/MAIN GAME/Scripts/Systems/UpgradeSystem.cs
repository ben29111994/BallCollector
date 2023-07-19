using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeSystem : GameController
{
    [Header("Count System")]
    public GameObject handAnim;
    public TypeSystem typeSystem;
    private TextMeshProUGUI priceText;
    private GameObject CountBlockImage;
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
        Count,
        Fuel,
        Power,
        Size
    }

    private void Awake()
    {
        priceText = transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        CountBlockImage = transform.GetChild(5).gameObject;
    }

    private void Update()
    {
        //UpdateBlock();
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
            CountPrice = BasePrice() * (Level() + 1);
        }
    }

    private bool IsMax()
    {
        if(typeSystem == TypeSystem.Count)
        {
            return false;
        }

        return Level() >= 8 ? true : false;
    }

    public void OnClink_Buy()
    {
        if (DataManager.Instance.Coin < CountPrice) return;
        DataManager.Instance.Coin -= CountPrice;
        UpgradeSucces();
    }

    public void UpgradeSucces()
    {
        //switch ((int)typeSystem)
        //{
        //    case 0:
        //        DataManager.Instance.CountLevel++;
        //        SawController.Instance.SpawnSaw();
        //        break;
        //    case 1:
        //        DataManager.Instance.FuelLevel++;
        //        break;
        //    case 2:
        //        DataManager.Instance.PowerLevel++;
        //        SawController.Instance.UpdateSawPower();
        //        break;
        //    case 3:
        //        DataManager.Instance.SizeLevel++;
        //        SawController.Instance.UpdateSawSize();
        //        break;
        //}

        HandAnim();
        UpdatePrice();
        VibrationManager.Instance.Vibration();
    }

    private int BasePrice()
    {
        return (typeSystem == TypeSystem.Count) ? 1000 : 200;
    }

    private int Level()
    {
        switch ((int)typeSystem)
        {
            case 0:
                return DataManager.Instance.TimerLevel;
            //case 1:
            //    return DataManager.Instance.FuelLevel;
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
