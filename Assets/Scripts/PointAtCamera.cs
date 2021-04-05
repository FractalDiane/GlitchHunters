using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAtCamera : MonoBehaviour
{
    public Transform cameraTransform;
    public float yModifier = 1;
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
         Vector3 forw = cameraTransform.forward;
         forw.y *= yModifier;
         transform.forward = forw;

    }
}
