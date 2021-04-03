using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAtCamera : MonoBehaviour
{
    public Transform cameraTransform;
    // Start is called before the first frame update
    void Start()
    {
        if(cameraTransform==null) {
            cameraTransform = Camera.main.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
         transform.forward = cameraTransform.forward;

    }
}
