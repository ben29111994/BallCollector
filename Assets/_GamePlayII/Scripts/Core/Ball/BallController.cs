using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public static BallController Instance => instance;
    private static BallController instance;

    public List<Ball> listBall = new List<Ball>();

    private void Awake()
    {
        instance = this;
    }

    public void SpawnBall(Vector3 _pos)
    {
        Ball _ball = BallPooling.Instance.GetObject();
        _ball.ActiveBall(_pos);
        listBall.Add(_ball);
    }

    public void Refresh()
    {
        listBall.Clear();
    }
}