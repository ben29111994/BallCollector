using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase_2 : MonoBehaviour
{
    public bool isComplete;

    public int LevelPower
    {
        get
        {
            return PlayerPrefs.GetInt("PowerLevel");
        }
        set
        {
            PlayerPrefs.SetInt("PowerLevel", value);
        }
    }

    public int LevelArmor
    {
        get
        {
            return PlayerPrefs.GetInt("ArmorLevel");
        }
        set
        {
            PlayerPrefs.SetInt("ArmorLevel", value);
        }
    }

    private void Update()
    {
        UpdateCheckWinLose();
    }

    public void LevelUpPower()
    {
        LevelPower++;
    }

    public void LevelUpArmor()
    {
        LevelArmor++;
    }

    public void ResetData()
    {
        LevelPower = 0;
        LevelArmor = 0;
    }

    public void Win()
    {
        if (isComplete) return;
        isComplete = true;

        Debug.Log("GamePlayII => Win");
        GamePlayII.Instance.Complete(true);
    }

    public void Lose()
    {
        if (isComplete) return;
        isComplete = true;

        Debug.Log("GamePlayII => Lose");
        GamePlayII.Instance.Complete(false);
    }

    public void Refresh()
    {
        isComplete = false;
        HumanPooling.Instance.RefreshAll();
        HumanController.Instance.Refresh();
    }

    private void UpdateCheckWinLose()
    {
        if (GamePlayII.Instance.phase_0.BallCount == 0 && BallController.Instance.listBall.Count == 0
            && HumanController.Instance.IsBlueTeamDieAll() && HumanController.Instance.IsRedTeamDieAll())
        {
            Lose();
            return;
        }

        if (HumanController.Instance.IsBlueTeamAttackingRedCastle())
        {
            Win();
        }
        else if (HumanController.Instance.IsRedTeamAttackingBlueCastle())
        {
            Lose();
        }
    }
}
