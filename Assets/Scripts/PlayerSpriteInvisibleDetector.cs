using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteInvisibleDetector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnBecameInvisible()
    {
		GlitchProgress.Singleton.CompleteGlitch("out_of_frame");
    }
}
