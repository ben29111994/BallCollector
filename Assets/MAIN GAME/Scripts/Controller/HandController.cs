using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    private static HandController instance;
    public static HandController Instance { get { return instance; } }

    public GameObject handObject;

    private void Awake()
    {
        instance = (instance == null) ? this : instance;
    }

    public void SetHandPosition(Vector3 pos,bool isActive)
    {
        handObject.transform.position = pos;
        handObject.SetActive(isActive);
    }
}
