using DG.Tweening;
using GPUInstancer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollected : MonoBehaviour
{
    public Transform spawnPos;
    public List<Rigidbody> listMaintainBalls = new List<Rigidbody>();
    public int limit = 100;
    bool isUpgrading = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pixel") && other.GetComponent<Tile>().isCheck)
        {
            other.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.GetComponent<Tile>().isCheck = false;
            other.GetComponent<Tile>().isMagnet = false;
            other.transform.parent = transform.parent;
            other.transform.DOLocalMove(spawnPos.localPosition, 0.1f);
            other.GetComponent<SphereCollider>().isTrigger = false;
            other.GetComponent<Rigidbody>().drag = 10;
            other.transform.localScale /= 2;
            //other.transform.DOScale(10f, 0.2f);
            listMaintainBalls.Add(other.GetComponent<Rigidbody>());
            if (listMaintainBalls.Count >= limit)
            {
                if (!isUpgrading)
                {
                    isUpgrading = true;
                    Upgrade();
                }
            }
        }
    }

    private void Update()
    {
        if (listMaintainBalls.Count >= limit)
        {
            //if (!isUpgrading)
            //{
            //    isUpgrading = true;
            //    Upgrade();
            //}
            for (int i = 0; i < listMaintainBalls.Count - limit; i++)
            {
                try
                {
                    AddRemoveInstances.instance.RemoveInstances(listMaintainBalls[i].GetComponent<GPUInstancerPrefab>());
                }
                catch { }
            }
            listMaintainBalls.RemoveAll(item => item == null);
        }
    }

    void Upgrade()
    {
        var scaleValue = transform.parent.transform.parent.transform.localScale;
        transform.parent.transform.parent.transform.DOScale(new Vector3(scaleValue.x + 0.2f, scaleValue.y + 0.2f, scaleValue.z + 0.2f), 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            limit += 200;
            isUpgrading = false;
        }); ;
        //DOTween.To(() => Camera.main.fieldOfView, x => Camera.main.fieldOfView = x, Camera.main.fieldOfView * 1.05f, 1f);
        DOTween.To(() => GameController.instance.CameraOffsetY, x => GameController.instance.CameraOffsetY = x, GameController.instance.CameraOffsetY * 1.05f, 0.5f);
        DOTween.To(() => GameController.instance.CameraOffsetZ, x => GameController.instance.CameraOffsetZ = x, GameController.instance.CameraOffsetZ * 1.05f, 0.5f);
        //GameController.instance.CameraOffsetY *= 1.1f;
        //GameController.instance.CameraOffsetZ *= 1.1f;
        GameController.instance.forceFactor += 5000;
    }
}
