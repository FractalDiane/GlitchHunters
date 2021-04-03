using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnByParentVelocity : MonoBehaviour
{
    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        if(rb==null) {
            rb = GetComponentInParent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(rb.velocity != Vector3.zero) {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }
}
