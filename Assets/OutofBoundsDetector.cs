using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutofBoundsDetector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Player") 
        {
            GlitchProgress.Singleton.CompleteGlitch("out_of_bounds");
            other.gameObject.transform.localPosition = Vector3.zero;    
            other.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

}
