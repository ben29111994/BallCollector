using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rigid;

    public void ActiveBall(Vector3 _pos)
    {
        transform.position = _pos;
        gameObject.SetActive(true);
    }

    public void DisableBall()
    {
        GamePlayII.Instance.phase_0.BallCount++;
        HideBall();
    }

    private void HideBall()
    {
        rigid.velocity = Vector3.zero;
        transform.position = Vector3.zero;
        BallController.Instance.listBall.Remove(this);
        gameObject.SetActive(false);
    }

    private void Trigger_Reward(Reward reward)
    {
        GamePlayII.Instance.phase_0.CollectReward(reward);
        HideBall();
    }

    private void Trigger_Weapon(Reward reward)
    {
        string name = reward.transform.name;
        string[] nameArray = name.Split('_');
        int weaponIndex = nameArray.Length >= 2 ? int.Parse(nameArray[1]) : 0;
        // spawn human with weapon 
        // Debug.Log("Spawn Human Weapon " + weaponIndex);
        reward.Highlight();
        HumanController.Instance.SpawnHuman((eNameJob)weaponIndex, eNameTeam.Blue);
        HideBall();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Reward"))
        {
            Reward reward = other.transform.parent.GetComponent<Reward>();
            if (reward.rewardName != Reward.RewardName.Job)
            {
                // trigger reward (phase0)
                Trigger_Reward(reward);
            }
            else
            {
                // trigger pick weapon (phase1)
                Trigger_Weapon(reward);
            }
        }

    }

}
