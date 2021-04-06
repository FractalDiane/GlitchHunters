using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
	[SerializeField]
	string npcName;

	[System.Serializable]
	class DialogueSet
	{
		public string[] dialogueText; 
	}

	[SerializeField]
	DialogueSet[] dialogueSets;

	[SerializeField]
	string glitchToUnlock = string.Empty;
	bool glitchUnlocked = false;
	[Space(20)]
	[SerializeField]
	GameObject dialogueObject = null;

	int currentDialogueSet = 0;

	bool playerInArea = false;
	Player playerRef = null;
	
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
			indicatorSprite.enabled = false;
			playerRef.LockMovement = true;
			var dlg = Instantiate(dialogueObject, Vector3.zero, Quaternion.identity);
			dlg.GetComponent<Dialogue>().StartDialogue(dialogueSets[currentDialogueSet].dialogueText, npcName, this);
		}
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
