using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchCompletionTrigger : MonoBehaviour
{
	[SerializeField]
	string glitchToComplete;
	
	bool activated = false;
	public float timeRequirement = 0;
	bool insideOf = false;
	float activationTimer = 0f;

	private void Update() {
		if(activationTimer>0&&insideOf) {
			activationTimer -= Time.deltaTime;
			if(activationTimer<=0) {
				CompleteGlitch();
			}
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		if (!activated && collider.gameObject.tag == "Player")
		{
			insideOf = true;
			if(timeRequirement == 0)
			{
				CompleteGlitch(); //make sure that previous functionality still works exactly the same
			}
			else
			{
				activationTimer = timeRequirement;
			}
		}
	}
	private void OnTriggerExit(Collider other) 
	{
		if(insideOf && other.gameObject.tag == "Player")
		{
			insideOf = false;
		}
	}
	void CompleteGlitch() {
		GlitchProgress.Singleton.CompleteGlitch(glitchToComplete);
		activated = true;
	}
}
