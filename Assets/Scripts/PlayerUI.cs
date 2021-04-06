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

	bool partialAnimation = false;
	bool newAvailableAnimation = false;

	[SerializeField]
	TextMeshProUGUI completedGlitchText = null;
	[SerializeField]
	TextMeshProUGUI jumpHeightText = null;

	[SerializeField]
	GameObject dialogueAsyncPrefab = null;

	//[SerializeField]
	//GameObject glitchListPrefab = null;

	// =========================================================================

	public void DisplayJumpHeight(float height) {
		jumpHeightText.text = Mathf.Floor(height) + "m";
		// jumpHeightText.text = height.ToString("F0") + "m";
	}

	public void PlayGlitchCompletedAnimation(string glitchText, bool newAvailable)
	{
		partialAnimation = false;
		newAvailableAnimation = newAvailable;
		completedGlitchText.text = glitchText;
		GetComponent<AudioSource>().Play();
		GetComponent<Animator>().Play(newAvailable ? "GlitchFoundNewAvailable" : "GlitchFound");
		Invoke(nameof(FinishGlitchCompletedAnimation), 4f);
	}

	public void PlayGlitchUnlockedAnimation()
	{
		partialAnimation = true;
		GetComponent<AudioSource>().Play();
		GetComponent<Animator>().Play("GlitchUnlocked");
		Invoke(nameof(FinishGlitchCompletedAnimation), 4f);
	}

	void FinishGlitchCompletedAnimation()
	{
		GetComponent<Animator>().Play(partialAnimation ? "GlitchFoundDisappear2" : newAvailableAnimation ? "GlitchFoundDisappear" : "GlitchFoundDisappear3");
	}

	public void DialogueAsync(string name, string[] dialogue)
	{
		var dlg = Instantiate(dialogueAsyncPrefab, Vector3.zero, Quaternion.identity);
		dlg.GetComponent<DialogueAsync>().StartDialogue(dialogue, name);
	}
}
