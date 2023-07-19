using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }

    public int coinEarn;

    [Header("References")]
    public GameObject MainMenuUI;
    public GameObject InGameUI;
    public GameObject CompleteUI;
    public GameObject FailUI;
    public GameObject SettingUI;
    public GameObject LoadingUI;

    private void Awake()
    {
        instance = (instance == null) ? this : instance;
    }

    private void Start()
    {
        Show_LoadingUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) Show_LoadingUI();
    }

    public void Show_LoadingUI()
    {
        StartCoroutine(C_LoadingUI());
    }

    private IEnumerator C_LoadingUI()
    {
        GameManager.Instance.isComplete = false;

        MainMenuUI.SetActive(false);
        InGameUI.SetActive(false);
        CompleteUI.SetActive(false);
        FailUI.SetActive(false);
        SettingUI.SetActive(false);
        LoadingUI.SetActive(true);
        yield return new WaitForSecondsRealtime(1.2f);
        LoadingUI.SetActive(false);
        Show_MainMenuUI();

        GameManager.Instance.OnStartGame();
    }

    public void Show_MainMenuUI()
    {
        MainMenuUI.SetActive(true);
        InGameUI.SetActive(true);
        CompleteUI.SetActive(false);
        FailUI.SetActive(false);
    }

    public void Show_InGameUI()
    {
        MainMenuUI.SetActive(false);
        InGameUI.SetActive(true);
        CompleteUI.SetActive(false);
        FailUI.SetActive(false);
    }

    public void Show_CompleteUI()
    {
        MainMenuUI.SetActive(false);
        InGameUI.SetActive(false);
        CompleteUI.SetActive(true);
        FailUI.SetActive(false);
    }

    public void Show_FailUI()
    {
        MainMenuUI.SetActive(false);
        InGameUI.SetActive(false);
        CompleteUI.SetActive(false);
        FailUI.SetActive(true);
    }

    public void OnClick_Continue()
    {
        Show_LoadingUI();
    }

    public void OnClick_TryAgain()
    {
        Show_LoadingUI();
    }

    public void OnClick_Next()
    {
        GameManager.Instance.LevelUp();
        Show_LoadingUI();
    }

    public void OnClick_Previous()
    {
        PlayerPrefs.DeleteAll();
        DataManager.Instance.LevelGame += 0;
        DataManager.Instance.Coin += 0;

        Show_LoadingUI();
    }
}
