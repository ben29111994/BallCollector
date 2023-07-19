using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReferenceManager : MonoBehaviour
{
    private static ReferenceManager instance;
    public static ReferenceManager Instance { get { return instance; } }

    public Image fuelBarImg;
    public Transform cameraMain;
    public Transform player;

    private void Awake()
    {
        instance = (instance == null) ? this : instance;
    }
}
