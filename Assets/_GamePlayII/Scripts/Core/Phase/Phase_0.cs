using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Phase_0 : MonoBehaviour
{
    public int BallCount
    {
        get
        {
            return PlayerPrefs.GetInt("BallCount");
        }
        set
        {
            PlayerPrefs.SetInt("BallCount", value);
            ballCountText.text = value.ToString();
        }
    }

    [Header("Status")]
    public bool isSendingBall;

    [Header("References")]
    public TextMeshProUGUI ballCountText;
    public Transform startPoint;
    public GameObject handAnim;

    private void OnEnable()
    {
        BallCount += 0;
        isSendingBall = false;
        StartCoroutine(C_UpdateSendBall());
    }

    public void AddBall(int ballAmount)
    {
        BallCount += ballAmount;
    }

    public void CollectReward(Reward reward)
    {
        if (reward.rewardName == Reward.RewardName.Multiply_x2)
        {
            BallCount += 2;
        }
        else if (reward.rewardName == Reward.RewardName.Minus_1)
        {
            if (BallCount > 0)
                BallCount--;
        }
        else if (reward.rewardName == Reward.RewardName.Battle)
        {
            // Debug.Log("Spawn Human Battle");
            HumanController.Instance.SpawnHuman((eNameJob)0, eNameTeam.Blue);
        }
        else if (reward.rewardName == Reward.RewardName.Weapon)
        {
            // Debug.Log("Spawn Ball Phase 1");
            GamePlayII.Instance.phase_1.SpawnBall();
        }

        reward.Highlight();
    }

    public void SpawnBall()
    {
        BallController.Instance.SpawnBall(GetRandomStartPoint());
    }

    private Vector3 GetRandomStartPoint()
    {
        float fromPos = startPoint.GetChild(0).position.x;
        float toPos = startPoint.GetChild(1).position.x;
        Vector3 randomPos = startPoint.position;
        randomPos.x = Random.Range(fromPos,toPos);
        return randomPos;
    }

    private IEnumerator C_UpdateSendBall()
    {
        while (true)
        {
            if (isSendingBall && BallCount > 0)
            {
                BallCount--;
                SpawnBall();
                HandAnim();
                yield return new WaitForSeconds(0.08f);
            }
            else
            {
                yield return null;
            }
        }
    }

    public void OnClickDown_SendBall()
    {
        isSendingBall = true;
    }

    public void OnClickUp_SendBall()
    {
        isSendingBall = false;
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
}
