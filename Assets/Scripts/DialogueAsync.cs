using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueAsync : MonoBehaviour
{	
	string[] dialogueText;

	int dialoguePage = 0;
	float visibleCharacters = 0;
	int flooredVisibleCharacters = 0;

	bool rollText = false;

	[SerializeField]
	GameObject textObject;
	TextMeshProUGUI text;
	[SerializeField]
	GameObject nameObject;
	new TextMeshProUGUI name;
	Image textbox;

	[SerializeField]
	AudioClip advanceSound;

	[SerializeField]
	AudioClip[] textSounds;

	AudioSource soundClose;

	bool pageEnded = false;

	float soundPitch = 1f;
	public float SoundPitch { set => soundPitch = value; }
	
	void Start()
	{
		text = textObject.GetComponent<TextMeshProUGUI>();
		name = nameObject.GetComponent<TextMeshProUGUI>();
		textbox = GetComponentInChildren<Image>();
		AudioSource[] sources = GetComponents<AudioSource>();
		soundClose = sources[1];
	}

	void Update()
	{
		if (rollText)
		{
			visibleCharacters = Mathf.Clamp(visibleCharacters + 25f * Time.deltaTime, 0, dialogueText[dialoguePage].Length);
			text.maxVisibleCharacters = Mathf.RoundToInt(visibleCharacters);
			flooredVisibleCharacters = Mathf.FloorToInt(visibleCharacters);
			//if (flooredVisibleCharacters % 2 == 0 && flooredVisibleCharacters < dialogueText[dialoguePage].Length)
			//{
				//Controller.Singleton.PlaySoundOneShot(textSounds[Random.Range(0, textSounds.Length)], soundPitch + Random.Range(-pitchVariance, pitchVariance), 0.05f);
			//}

			if (!pageEnded && visibleCharacters >= dialogueText[dialoguePage].Length)
			{
				rollText = false;
				pageEnded = true;
				if (dialoguePage < dialogueText.Length - 1)
				{
					Invoke(nameof(NextPage), 3f);
				}
				else
				{
					Invoke(nameof(EndAnimation), 3f);
				}
			}
		}
	}

	public void StartDialogue(string[] text, string npcName)
	{
		GetComponent<AudioSource>().Play();
		nameObject.GetComponent<TextMeshProUGUI>().text = npcName;
		var txt = GetComponentInChildren<TextMeshProUGUI>();
		txt.maxVisibleCharacters = 0;
		dialogueText = text;
		txt.text = dialogueText[0];
		GetComponent<Animator>().Play("Appear");
		Invoke(nameof(RollText), 0.5f);
	}

	void NextPage()
	{
		visibleCharacters = 0;
		text.maxVisibleCharacters = 0;
		dialoguePage++;
		text.text = dialogueText[dialoguePage];
		pageEnded = false;
		Invoke(nameof(RollText), 0.05f);
	}

	void RollText()
	{
		rollText = true;
	}

	void EndAnimation()
	{
		soundClose.Play();
		GetComponent<Animator>().Play("Disappear");
		Invoke(nameof(DestroySelf), 0.65f);
	}

	void DestroySelf()
	{
		Destroy(gameObject);
	}
}
