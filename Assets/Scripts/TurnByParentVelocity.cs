using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnByParentVelocity : MonoBehaviour
{
    public Rigidbody rb;
    public float minVelocity;
    public Vector3 lastDirection;
    public float yModifier = 1;
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
        if(Mathf.Abs(rb.velocity.x)>minVelocity || Mathf.Abs(rb.velocity.z)>minVelocity) {
            lastDirection.x = rb.velocity.x;
            lastDirection.z = rb.velocity.z;
        }
        lastDirection.y = rb.velocity.y*yModifier;
        transform.rotation = Quaternion.LookRotation(lastDirection);
    }
}
