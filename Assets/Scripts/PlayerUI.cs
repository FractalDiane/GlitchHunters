using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
	static PlayerUI singleton = null;
	public static PlayerUI Singleton { get => singleton; }
	void Awake()
	{
		if (singleton == null)
		{
			singleton = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	GameObject player = null;
	Player playerScript = null;

	[SerializeField]
	TextMeshProUGUI completedGlitchText = null;

	//[SerializeField]
	//GameObject glitchListPrefab = null;

	// =========================================================================

	void Start()
	{
		
	}

	void Update()
	{
		
	}

	public void PlayGlitchCompletedAnimation(string glitchText, bool newAvailable)
	{
		completedGlitchText.text = glitchText;
		GetComponent<AudioSource>().Play();
		GetComponent<Animator>().Play(newAvailable ? "GlitchFoundNewAvailable" : "GlitchFound");
		Invoke(nameof(FinishGlitchCompletedAnimation), 4f);
	}

	void FinishGlitchCompletedAnimation()
	{
		GetComponent<Animator>().Play("GlitchFoundDisappear");
	}
}