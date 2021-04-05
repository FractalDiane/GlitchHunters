using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchCompletionTrigger : MonoBehaviour
{
	[SerializeField]
	string glitchToComplete;
	
	bool activated = false;

	void OnTriggerEnter(Collider collider)
	{
		if (!activated && collider.gameObject.tag == "Player")
		{
			GlitchProgress.Singleton.CompleteGlitch(glitchToComplete);
			activated = true;
		}
	}
}
