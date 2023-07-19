using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public bool isRun;
    public Transform A;
    public Transform B;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dot = Vector3.Dot(A.forward, B.forward);
        Debug.Log(dot);
    }

    private void FixedUpdate()
    {
        if (!isRun) return;

        GetComponent<Rigidbody>().velocity = Vector3.right * 5.0f;
    }
}
