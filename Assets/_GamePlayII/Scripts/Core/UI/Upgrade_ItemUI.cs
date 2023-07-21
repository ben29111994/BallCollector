using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Upgrade_ItemUI : MonoBehaviour
{
    //public GameObject handAnim;

    //public void OnClick()
    //{
    //    handAnim.SetActive(false);
    //    StopAllCoroutines();
    //    StartCoroutine(C_HandAnim());
    //}

    //private IEnumerator C_HandAnim()
    //{
    //    handAnim.SetActive(true);
    //    yield return new WaitForSeconds(0.33f);
    //    handAnim.SetActive(false);
    //}


    [Header("References")]
    public GameObject handAnim;
    public TypeSystem typeSystem;
    private TextMeshProUGUI priceText;
    private GameObject CountBlockImage;
    [SerializeField] private int price;
    public int Price
    {
        get
        {
            return price;
        }
        set
        {
            price = value;
            priceText.text = CoinFixedText(price) + "$";
            //  priceText.text = "$" + DataManager.CoinFixedText(countPrice);
        }
    }

    public enum TypeSystem
    {
        SendBall,
        UnlockWeapon,
        Power,
        Armor
    }


    private void OnEnable()
    {
        priceText = transform.GetChild(5).GetComponent<TextMeshProUGUI>();
        CountBlockImage = transform.GetChild(6).gameObject;
        UpdatePrice();
    }

    private IEnumerator C_OnEnalbe()
    {
        yield return null;

    }

    private void Update()
    {
        UpdateBlock();
    }

    private void UpdateBlock()
    {
        int myCoin = 99999;
        if (myCoin < Price || IsMax())
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
            Price = BasePrice() * (Level() + 1);
        }
    }

    private bool IsMax()
    {
        int levelLimit = (typeSystem == TypeSystem.UnlockWeapon) ? 4 : 6;
        return Level() >= levelLimit ? true : false;
    }

    public void OnClink_Buy()
    {
        //int myCoin = 9999;
        //if (myCoin < Price) return;
        //myCoin -= Price;
        if (DataManager.Instance.Coin < Price) return;
        DataManager.Instance.Coin -= Price;
        UpgradeSucces();
    }

    public void UpgradeSucces()
    {
        switch ((int)typeSystem)
        {
            case 0:
                break;
            case 1:
                GamePlayII.Instance.phase_1.UpgradeJob();
                break;
            case 2:
                GamePlayII.Instance.phase_2.LevelUpPower();
                break;
            case 3:
                GamePlayII.Instance.phase_2.LevelUpArmor();
                break;
        }

        HandAnim();
        UpdatePrice();
    }

    private int BasePrice()
    {
        switch ((int)typeSystem)
        {
            case 0:
            case 1:
                return 820;
            case 2:
                return 442;
            case 3:
                return 408;
        }

        return 400;
    }

    private int Level()
    {
        switch ((int)typeSystem)
        {
            case 0:
                return 0;
            case 1:
                return GamePlayII.Instance.phase_1.LevelTerrain;
            case 2:
                return GamePlayII.Instance.phase_2.LevelPower;
            case 3:
                return GamePlayII.Instance.phase_2.LevelArmor;
        }

        return 0;
    }

    public void HandAnim()
    {
        if (C2_HandAnim != null)
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
        yield return new WaitForSeconds(0.333f);
        handAnim.SetActive(false);
    }

    public string CoinFixedText(int number)
    {
        if (number < 1000)
        {
            return number.ToString();
        }
        else
        {
            int a = number / 1000;
            int b = number % 1000;
            int c = b / 10;

            if (c == 0)
            {
                return a + "K";
            }
            else
            {
                return a + "." + c + "K";
            }
        }
    }
}
