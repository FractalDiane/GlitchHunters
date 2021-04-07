using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPC : MonoBehaviour
{
	[System.Serializable]
	class DialoguePage
	{
		public string speakerName;
		public string dialogueText;
	}

	[System.Serializable]
	class DialogueSet
	{
		public DialoguePage[] dialogue;
	}

	[SerializeField]
	DialogueSet[] dialogueSets;

	[SerializeField]
	string glitchToUnlock = string.Empty;
	bool glitchUnlocked = false;

	[SerializeField]
	UnityEvent OnDialogueFinished;

	[Space(20)]
	[SerializeField]
	GameObject dialogueObject = null;

	int currentDialogueSet = 0;

	bool playerInArea = false;
	Player playerRef = null;
	public Player PlayerRef { set => playerRef = value; }
	
	[SerializeField]
	Sprite interactSprite;
	[SerializeField]
	Sprite taskSprite;
	[SerializeField]
	SpriteRenderer indicatorSprite;

	void Start()
	{
		if (!glitchUnlocked && glitchToUnlock.Length > 0)
		{
			indicatorSprite.sprite = taskSprite;
		}
		else
		{
			indicatorSprite.enabled = false;
		}
	}

	void Update()
	{
		if (playerInArea && !playerRef.LockMovement && Input.GetButtonDown("Interact"))
		{
			StartDialogue();
		}
	}

	public void StartDialogue()
	{
		indicatorSprite.enabled = false;
		playerRef.LockMovement = true;
		var dlg = Instantiate(dialogueObject, Vector3.zero, Quaternion.identity);
		
		int count = dialogueSets[currentDialogueSet].dialogue.Length;
		string[] speakers = new string[count];
		string[] texts = new string[count];
		for (int i = 0; i < count; i++)
		{
			speakers[i] = dialogueSets[currentDialogueSet].dialogue[i].speakerName;
			texts[i] = dialogueSets[currentDialogueSet].dialogue[i].dialogueText;
		}

		dlg.GetComponent<Dialogue>().StartDialogue(texts, speakers, this);
	}

	public void EndDialogue()
	{
		currentDialogueSet = Mathf.Min(++currentDialogueSet, dialogueSets.Length - 1);
		playerRef.LockMovement = false;
		if (!glitchUnlocked && glitchToUnlock.Length > 0)
		{
			GlitchProgress.Singleton.UnlockGlitch(glitchToUnlock, true);
			glitchUnlocked = true;
		}

		indicatorSprite.enabled = true;
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.tag == "Player")
		{
			playerInArea = true;
			playerRef = collider.gameObject.GetComponent<Player>();
			indicatorSprite.sprite = interactSprite;
			indicatorSprite.enabled = true;
		}
	}

	void OnTriggerExit(Collider collider)
	{
		if (collider.gameObject.tag == "Player")
		{
			playerInArea = false;
			playerRef = null;
			if (!glitchUnlocked && glitchToUnlock.Length > 0)
			{
				indicatorSprite.sprite = taskSprite;
			}
			else
			{
				indicatorSprite.enabled = false;
			}
		}
	}
}
