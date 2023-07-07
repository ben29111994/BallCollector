using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollected : MonoBehaviour
{
    public Transform spawnPos;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Pixel"))
        {
            other.GetComponent<Tile>().isCheck = false;
            other.GetComponent<Tile>().isMagnet = false;
            other.transform.localPosition = spawnPos.localPosition;
        }
    }
}
