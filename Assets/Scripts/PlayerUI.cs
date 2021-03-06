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
	TextMeshProUGUI jumpHeightText2 = null;

	[SerializeField]
	GameObject dialogueAsyncPrefab = null;

	[SerializeField]
	bool showJumpHeight = true;

	struct AnimInfo
	{
		public string text;
		public bool newAvailable;
		public AnimInfo(string text, bool newAvailable) { this.text = text; this.newAvailable = newAvailable; }
	}

	Queue<AnimInfo> completedGlitches = new Queue<AnimInfo>();

	//[SerializeField]
	//GameObject glitchListPrefab = null;

	// =========================================================================

	void Start()
	{
		jumpHeightText.enabled = showJumpHeight;
		jumpHeightText2.enabled = showJumpHeight;
		playerScript = FindObjectOfType<Player>();
		player = playerScript.gameObject;
	}

	void Update()
	{
		if (Input.GetButtonDown("Reset"))
		{
			ResetPlayer();
		}
	}

	public void DisplayJumpHeight(float height) {
		jumpHeightText.text = Mathf.Floor(height) + "m";
		// jumpHeightText.text = height.ToString("F0") + "m";
	}

	public void PlayGlitchCompletedAnimation(string glitchText, bool newAvailable)
	{
		partialAnimation = false;
		GetComponent<AudioSource>().Play();
		bool queueWasEmpty = completedGlitches.Count == 0;
		completedGlitches.Enqueue(new AnimInfo(glitchText, newAvailable));
		if (queueWasEmpty)
		{
			PlayGlitchCompletedAnimationFromQueue();
		}
	}

	void PlayGlitchCompletedAnimationFromQueue()
	{
		partialAnimation = false;
		AnimInfo info = completedGlitches.Peek();
		newAvailableAnimation = info.newAvailable;
		completedGlitchText.text = info.text;
		GetComponent<Animator>().Play(info.newAvailable ? "GlitchFoundNewAvailable" : "GlitchFound");
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
		completedGlitches.Dequeue();
		if (completedGlitches.Count > 0)
		{
			Invoke(nameof(PlayGlitchCompletedAnimationFromQueue), 1f);
		}
	}

	public void DialogueAsync(string name, string[] dialogue)
	{
		var dlg = Instantiate(dialogueAsyncPrefab, Vector3.zero, Quaternion.identity);
		dlg.GetComponent<DialogueAsync>().StartDialogue(dialogue, name);
	}

	public void PlayGlitchMasterAnimation()
	{
		GetComponents<AudioSource>()[1].Play();
		GetComponent<Animator>().Play("GlitchMaster");
		Invoke(nameof(FinishGlitchMasterAnimation), 6f);
	}

	void FinishGlitchMasterAnimation()
	{
		GetComponent<Animator>().Play("GlitchMasterDisappear");
	}

	public void ResetPlayer()
	{
		player.transform.position = new Vector3(0, 3, 0);
	}
}
