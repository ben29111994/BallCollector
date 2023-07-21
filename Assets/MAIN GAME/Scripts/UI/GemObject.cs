using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GemObject : MonoBehaviour
{
    public GameObject target;
    private List<GameObject> listGem = new List<GameObject>();
    private Vector3 startPos;
    private CompleteUI completeUI;

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++) listGem.Add(transform.GetChild(i).gameObject);
        startPos = listGem[0].transform.position;
    }

    private void OnEnable()
    {
        StartCoroutine(C_Animation());
    }

    private IEnumerator C_Animation()
    {
        //int coinEarn = GameController.coinEarn;
       
        for (int i = 0; i < listGem.Count; i++)
        {
            GameObject go = listGem[i];
            go.transform.position = startPos;
            go.transform.localScale = Vector3.one;
            float _time = 1.5f;
            float _jump = Random.Range(-4f, 4f);
            //go.transform.DOJump(target.transform.position, _jump, 1, _time).SetEase(Ease.InOutSine);
            go.transform.DOLocalMoveY(Random.Range(go.transform.localPosition.y + 100, go.transform.localPosition.y - 100), _time/2).SetEase(Ease.InOutSine);
            go.transform.DOLocalMoveX(Random.Range(-100.0f, 100.0f), _time/2).SetEase(Ease.InOutSine).OnComplete(() => go.transform.DOJump(target.transform.position, _jump, 1, _time/2).SetEase(Ease.InOutSine));
            go.transform.DOScale(Vector3.one * 0.6f, _time).SetEase(Ease.InOutSine);
            //completeUI.CurrentCoinEarn -= coinEarn;

            //if(i == listGem.Count - 1)
            //{
            //    completeUI.CurrentCoinEarn = 0;
            //    completeUI.coinEarnObject.SetActive(false);
            //}
            //yield return new WaitForSeconds(Random.Range(0.02f, 0.04f));
        }

        yield return new WaitForSeconds(0.6f);
        //completeUI.Show_ContinueButton();
    }

    private void OnCompelte(GameObject go,int coinEarn)
    {
        DataManager.Instance.Coin += coinEarn;
        go.transform.localScale = Vector3.zero;
    }
}
