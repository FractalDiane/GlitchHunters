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
	GameObject dialogueObject = null;

	int currentDialogueSet = 0;

	bool playerInArea = false;
	Player playerRef = null;

	void Start()
	{
		
	}

	void Update()
	{
		if (playerInArea && !playerRef.LockMovement && Input.GetButtonDown("Interact"))
		{
			playerRef.LockMovement = true;
			var dlg = Instantiate(dialogueObject, Vector3.zero, Quaternion.identity);
			dlg.GetComponent<Dialogue>().StartDialogue(dialogueSets[currentDialogueSet].dialogueText, npcName, this);
		}
	}

	public void EndDialogue()
	{
		currentDialogueSet = Mathf.Min(++currentDialogueSet, dialogueSets.Length - 1);
		playerRef.LockMovement = false;
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.tag == "Player")
		{
			playerInArea = true;
			playerRef = collider.gameObject.GetComponent<Player>();
		}
	}

	void OnTriggerExit(Collider collider)
	{
		if (collider.gameObject.tag == "Player")
		{
			playerInArea = false;
			playerRef = null;
		}
	}
}
