using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipByParentVelocity : MonoBehaviour
{
   public Rigidbody rb;
    public float minVelocity;
    public bool flip;
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
        float horizontal = Input.GetAxisRaw("Horizontal");
        if(horizontal!=0) {
            transform.localScale = new Vector3(horizontal<0==flip?1:-1,1,1);
        }
        // if(Mathf.Abs(rb.velocity.x)>minVelocity || Mathf.Abs(rb.velocity.z)>minVelocity) {
            
        // }
        // lastDirection.y = rb.velocity.y;
        // transform.rotation = Quaternion.LookRotation(lastDirection);
    }
}
