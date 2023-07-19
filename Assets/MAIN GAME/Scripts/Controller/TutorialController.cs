using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    private static TutorialController instance;
    public static TutorialController Instance { get { return instance; } }

    public GameObject[] tutorialObject;

    private void Awake()
    {
        instance = (instance == null) ? this : instance;
    }

    public void ActiveTutorial()
    {
        if (DataManager.Instance.TutorialIndex >= tutorialObject.Length) return;
        tutorialObject[DataManager.Instance.TutorialIndex].SetActive(true);
    }

    public void DisableTutorial()
    {
        DataManager.Instance.TutorialIndex++;
        tutorialObject[0].SetActive(false);
        tutorialObject[1].SetActive(false);
    }
}
