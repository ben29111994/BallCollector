using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    private static CameraController instance;
    public static CameraController Instance { get { return instance; } }

    public Transform camera;
    public RectTransform[] topUI;

    private StatusLocalCam statusLocalCam;
    private Vector3 startPosition = new Vector3(5.0f, 3.0f, 0.0f);
    private enum StatusLocalCam
    {
        None,
        Left,
        Right,
        Mid
    }

    private void Awake()
    {
        instance = (instance == null) ? this : instance;
    }

    private void Start()
    {
        SetCorrectCamera();
    }

    public void Refresh()
    {
        camera.position = startPosition;
    }

    private void SetCorrectCamera()
    {
        float ratio = Camera.main.aspect;

        if (ratio >= 0.74) // 3:4
        {
            Camera.main.fieldOfView = 60;
        }
        else if (ratio >= 0.56) // 9:16
        {
            Camera.main.fieldOfView = 60;
        }
        else if (ratio >= 0.45) // 9:19
        {
            Camera.main.fieldOfView = 70;

            foreach (RectTransform r in topUI)
            {
                Vector2 current = r.anchoredPosition;
                current.y -= 150.0f;
                r.anchoredPosition = current;
            }
        }
    }

    public void ZoomIn()
    {
        Camera.main.transform.localPosition = new Vector3(0.0f, 0.0f, -21.0f);
        statusLocalCam = StatusLocalCam.None;
    }

    public void ZoomOut()
    {
        Camera.main.transform.DOLocalMoveZ(-27.0f, 2.0f).SetEase(Ease.InOutSine);
        Camera.main.transform.DOLocalMoveX(2.0f,2.0f).SetEase(Ease.InOutSine);
    }

    public void LerpToTarget(Vector3 target)
    {
        camera.DOMove(target, 0.5f).SetEase(Ease.InOutSine);
    }

    public void FollowTarget(Vector3 target)
    {
        target.y = Mathf.Clamp(target.y, 3.0f, 24.0f);
        camera.position = target;

        //if (camera.position.x < PixelController.Instance.pivotPixels.x - 3.0f && statusLocalCam != StatusLocalCam.Left)
        //{
        //    statusLocalCam = StatusLocalCam.Left;
        //    StopAndRun_CameraMoveLocal(target);
        //}
        //else if (camera.position.x > PixelController.Instance.pivotPixels.x + 3.0f && statusLocalCam != StatusLocalCam.Right)
        //{
        //    statusLocalCam = StatusLocalCam.Right;
        //    StopAndRun_CameraMoveLocal(target);
        //}
        //else if(camera.position.x < PixelController.Instance.pivotPixels.x + 3.0f &&
        //    camera.position.x > PixelController.Instance.pivotPixels.x - 3.0f
        //    && statusLocalCam != StatusLocalCam.Mid)
        //{
        //    statusLocalCam = StatusLocalCam.Mid;
        //    StopAndRun_CameraMoveLocal(target);
        //}
    }

    private void StopAndRun_CameraMoveLocal(Vector3 target)
    {
        if (C2_MoveLocalCamToEdge != null)
        {
            StopCoroutine(C2_MoveLocalCamToEdge);
            DOTween.Kill(gameObject.GetInstanceID());
        }

        C2_MoveLocalCamToEdge = C_MoveLocalCamToEdge(target);
        StartCoroutine(C2_MoveLocalCamToEdge);
    }

    private IEnumerator C2_MoveLocalCamToEdge;
    private IEnumerator C_MoveLocalCamToEdge(Vector3 target)
    {
        Vector3 posLocalCam = Camera.main.transform.localPosition;
        if(statusLocalCam == StatusLocalCam.Left)
        {
            posLocalCam.x = 2.0f;
        }
        else if(statusLocalCam == StatusLocalCam.Right)
        {
            posLocalCam.x = 0.0f;
        }
        else if(statusLocalCam == StatusLocalCam.Mid)
        {
            posLocalCam.x = -2.0f;
        }

        Camera.main.transform.DOLocalMoveX(posLocalCam.x, 2.0f).SetEase(Ease.InOutSine).SetId(gameObject.GetInstanceID());
        Camera.main.transform.DOLocalMoveY(posLocalCam.y, 2.0f).SetEase(Ease.InOutSine).SetId(gameObject.GetInstanceID());
        yield return null;
    }
}
