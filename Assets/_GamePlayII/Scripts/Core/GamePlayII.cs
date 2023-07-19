using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GamePlayII : MonoBehaviour
{
    public static GamePlayII Instance => instance;
    private static GamePlayII instance;

    [Header("References")]
    public Phase_0 phase_0;
    public Phase_1 phase_1;
    public Phase_2 phase_2;
    public GameObject mainObject;
    public TextMeshProUGUI completeHeaderText;
    public GameObject completeUI;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Hide_GamePlayII();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) phase_0.SpawnBall();
        else if (Input.GetKeyDown(KeyCode.A)) Active_GamePlayII();
        //else if (Input.GetKeyDown(KeyCode.H)) Hide_GamePlayII();
        //else if (Input.GetKeyDown(KeyCode.B)) phase_0.AddBall(999);
        //else if (Input.GetKeyDown(KeyCode.D)) PlayerPrefs.DeleteAll();
    }

    public void Active_GamePlayII()
    {
        phase_0.AddBall(GetBallAmount());
        phase_1.Update_Terrain();
        HumanController.Instance.SpawnStartRedTeam();
        mainObject.SetActive(true);
    }

    public void Hide_GamePlayII()
    {
        Refresh();
        mainObject.SetActive(false);
        completeUI.SetActive(false);
    }

    private void Refresh()
    {
        PoolingII.Instance.RefreshAll();
        BallPooling.Instance.Refresh();
        BallController.Instance.Refresh();
        phase_2.Refresh();
    }

    private int GetBallAmount()
    {
        int baseBall = phase_0.BallCount >= 20 ? 5 : 20;
        //   int extraBall = (int)((DataManager.Instance.LevelGame + 1) * 2.0f);
        int extraBall = Random.Range(1, 6);
        if (extraBall > 25) extraBall = 25;
        return baseBall + extraBall;
    }

    public void Complete(bool _isWin)
    {
        if (_isWin)
        {
            phase_1.ResetData();
            phase_2.ResetData();
        }

        string headText = _isWin ? "VICTORY" : "DEFEAT";
        completeHeaderText.text = headText;
        completeUI.SetActive(true);
    }
}