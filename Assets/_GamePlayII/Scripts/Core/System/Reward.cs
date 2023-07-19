using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward : MonoBehaviour
{
    public RewardName rewardName;

    public enum RewardName
    {
        Multiply_x2,
        Minus_1,
        Battle,
        Weapon,
        Job
    }

    public Animator anim;

    public void Highlight()
    {
        anim.SetTrigger("Highlight");
    }
}
