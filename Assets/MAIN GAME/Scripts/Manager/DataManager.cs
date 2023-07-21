using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class DataManager : MonoBehaviour
{
    private static DataManager instance;
    public static DataManager Instance { get { return instance; } }

    [Header("References")]
    public Text coinText;
    public Animation coinAnim;

    public int TutorialIndex
    {
        get
        {
            return PlayerPrefs.GetInt("TutorialIndex");
        }
        set
        {
            PlayerPrefs.SetInt("TutorialIndex", value);
        }
    }

    public int LevelGame
    {
        get
        {
            return PlayerPrefs.GetInt("LevelGame") == 0 ? 1 : PlayerPrefs.GetInt("LevelGame");
        }
        set
        {
            PlayerPrefs.SetInt("LevelGame",value);
        }
    }

    public int Coin
    {
        get
        {
            return PlayerPrefs.GetInt("Coin");
        }
        set
        {
            PlayerPrefs.SetInt("Coin", value);
            coinText.text = "" + CoinFixedText(value);
            //coinAnim.Play("CoinCollect",PlayMode.StopAll);
        }
    }

    public int TimerLevel
    {
        get
        {
            return PlayerPrefs.GetInt("TimerLevel") == 0 ? 1: PlayerPrefs.GetInt("TimerLevel");
        }
        set
        {
            PlayerPrefs.SetInt("TimerLevel", value);
        }
    }

    public int PowerLevel
    {
        get
        {
            return PlayerPrefs.GetInt("PowerLevel") == 0 ? 1 : PlayerPrefs.GetInt("PowerLevel");
        }
        set
        {
            PlayerPrefs.SetInt("PowerLevel", value);
        }
    }

    public int SizeLevel
    {
        get
        {
            return PlayerPrefs.GetInt("SizeLevel") == 0 ? 1 : PlayerPrefs.GetInt("SizeLevel");
        }
        set
        {
            PlayerPrefs.SetInt("SizeLevel", value);
        }
    }

    private void Awake()
    {
        instance = (instance == null) ? this : instance;
        LevelGame += 0;
        Coin += 0;
    }

    private void Start()
    {
        //if (GameManager.Instance.isRecord)
        //{
        //    Coin += 0;
        //}
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) Coin += 9999999;
        if (Input.GetKeyDown(KeyCode.R)) PlayerPrefs.DeleteAll();
    }

    public static string CoinFixedText(int number)
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
