using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetLocalPositionOnTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        other.gameObject.transform.localPosition = Vector3.zero;    
        other.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
