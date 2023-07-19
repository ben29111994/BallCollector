using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPooling : MonoBehaviour
{
    public static BallPooling Instance => instance;
    private static BallPooling instance;

    public Transform parent;
    public int amount;
    public Ball objectPrefab;

    [HideInInspector]
    public List<Ball> listObject = new List<Ball>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(C_Start());
    }

    private IEnumerator C_Start()
    {
        for (int i = 0; i < amount; i++)
        {
            Ball objectClone = Instantiate(objectPrefab, parent);
            objectClone.gameObject.SetActive(false);
            listObject.Add(objectClone);
        }
        yield return null;

    }

    public Ball GetObject()
    {
        int childCount = listObject.Count;

        for (int i = 0; i < childCount; i++)
        {
            Ball childObject = listObject[i];
            if (childObject.gameObject.activeInHierarchy == false)
            {
                return childObject;
            }
        }

        Ball objectClone = Instantiate(objectPrefab, parent);
        objectClone.gameObject.SetActive(false);
        listObject.Add(objectClone);
        return objectClone;
    }


    public void Refresh()
    {
        for (int i = 0; i < listObject.Count; i++)
        {
            if (listObject[i].gameObject.activeSelf)
            {
                listObject[i].DisableBall();
                listObject[i].transform.SetParent(parent);
            }
        }
    }
}