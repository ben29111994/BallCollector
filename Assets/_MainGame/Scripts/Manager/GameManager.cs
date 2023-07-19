using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    [Header("Status Game")]
    public bool isRecord;
    public bool isComplete;

    private void Awake()
    {
        instance = (instance == null) ? this : instance;
        InitPlugin();
    }
    
    private void Start()
    {
    }

    public void OnStartGame()
    {
        //GP_Main.Instance.StartMainGamePlay();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene(0);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            ShakeManager.Instance.ShakeCamera(1.0f);
        }
    }

    public void InitPlugin()
    {
        Application.targetFrameRate = 60;


#if UNITY_IOS

#else

#endif
    }

    public void LevelUp()
    {
        DataManager.Instance.LevelGame++;
        DataManager.Instance.BitmapLevel++;

        if (!isRecord) DataManager.Instance.Coin = 0;
        DataManager.Instance.CountLevel = 0;
        DataManager.Instance.FuelLevel = 0;
        DataManager.Instance.PowerLevel = 0;
        DataManager.Instance.SizeLevel = 0;
    }

    public void Complete()
    {
        if (isComplete) return;

        isComplete = true;
        StartCoroutine(C_Complete());
    }

    private IEnumerator C_Complete()
    {
        yield return new WaitForSeconds(1.4f);
        //if (PixelController.Instance.listPixel.Count <= 0)
        //{
        //    ClientWisdomManager.Instance.CallEvent(ClientWisdomManager.EventType.Complete);
        //    LevelUp();
        //}
        //else
        //{
        //    ClientWisdomManager.Instance.CallEvent(ClientWisdomManager.EventType.Fail);
        //}
        UIManager.Instance.Show_CompleteUI();
    }

    public void Fail()
    {
        if (isComplete) return;

        isComplete = true;
        StartCoroutine(C_Fail());
    }

    private IEnumerator C_Fail()
    {
        yield return null;
    }


}
