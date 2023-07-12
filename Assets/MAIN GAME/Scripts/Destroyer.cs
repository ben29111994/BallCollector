using DG.Tweening;
using GPUInstancer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pixel") && other.GetComponent<Tile>().isCheck)
        {
            AddRemoveInstances.instance.RemoveInstances(other.GetComponent<GPUInstancerPrefab>());
        }
    }
}
