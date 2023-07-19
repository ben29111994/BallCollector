using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Phase_1 : MonoBehaviour
{
    public int LevelTerrain
    {
        get
        {
            return PlayerPrefs.GetInt("LevelTerrain");
        }
        set
        {
            PlayerPrefs.SetInt("LevelTerrain", value);
        }
    }

    [Header("References")]
    public GameObject[] terrain_Array;
    private GameObject curTerrain;

    private void OnEnable()
    {
        LevelTerrain += 0;
    }

    public void Refresh()
    {

    }

    public void SpawnBall()
    {
        BallController.Instance.SpawnBall(GetStartPoint());
    }

    public void Update_Terrain()
    {
        int levelTerrain = LevelTerrain;
        for (int i = 0; i < terrain_Array.Length; i++) terrain_Array[i].SetActive(false);
        curTerrain = terrain_Array[levelTerrain];
        curTerrain.SetActive(true);
    }

    public void UpgradeJob()
    {
        LevelTerrain++;
        if (LevelTerrain >= terrain_Array.Length) LevelTerrain = terrain_Array.Length - 1;
        Update_Terrain();
        Debug.Log("LevelTerrain++");
    }

    public void ResetData()
    {
        LevelTerrain = 0;
    }

    public bool IsMaxJob()
    {
        return (LevelTerrain >= terrain_Array.Length - 1) ? true : false; 
    }

    private Vector3 GetStartPoint()
    {
        float fromPos = curTerrain.transform.GetChild(0).GetChild(0).position.x;
        float toPos = curTerrain.transform.GetChild(0).GetChild(1).position.x;
        Vector3 randomStartPoint = curTerrain.transform.GetChild(0).position;
        randomStartPoint.x = Random.Range(fromPos, toPos);
        return randomStartPoint;
    }
}
