using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFixed : MonoBehaviour
{
    public RectTransform[] topUI;
    void Start()
    {
        Camera camera = GetComponent<Camera>();
        float ratio = camera.aspect;

        if (ratio >= 0.74) // 3:4
        {
            camera.fieldOfView = 60;
        }
        else if (ratio >= 0.56) // 9:16
        {
            camera.fieldOfView = 60;
        }
        else if (ratio >= 0.45) // 9:19
        {
            camera.fieldOfView = 70;

            foreach (RectTransform r in topUI)
            {
                Vector2 current = r.anchoredPosition;
                current.y -= 150.0f;
                r.anchoredPosition = current;
            }
        }


    }

}
