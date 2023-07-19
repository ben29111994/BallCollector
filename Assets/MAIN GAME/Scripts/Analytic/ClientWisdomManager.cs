using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using SupersonicWisdomSDK;

public class ClientWisdomManager : MonoBehaviour
{
    public static ClientWisdomManager Instance;
    public bool isInit;


    private void Awake()
    {
        Instance = this;
        //SupersonicWisdom.Api.Initialize();
        //SupersonicWisdom.Api.AddOnReadyListener(OnSupersonicReadyEvent);
    }

    private void OnSupersonicReadyEvent()
    {
        isInit = true;
    }

    public enum EventType
    {
        Start,
        Complete,
        Fail
    }

    public void CallEvent(EventType eventType)
    {
        StartCoroutine(C_CallEvent(eventType));
    }

    private IEnumerator C_CallEvent(EventType eventType)
    {
        while (isInit == false) yield return null;

        int lvl = DataManager.Instance.LevelGame + 1;

        //switch (eventType)
        //{
        //    case EventType.Start:
        //        SupersonicWisdom.Api.NotifyLevelStarted(lvl, null);
        //        break;
        //    case EventType.Complete:
        //        SupersonicWisdom.Api.NotifyLevelCompleted(lvl, null);
        //        break;
        //    case EventType.Fail:
        //        SupersonicWisdom.Api.NotifyLevelFailed(lvl, null);
        //        break;
        //}
    }
}
