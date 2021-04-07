using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogue : MonoBehaviour
{	
	string[] dialogueText;
	string[] dialogueNames;

	int dialoguePage = 0;
	float visibleCharacters = 0;
	int flooredVisibleCharacters = 0;

	bool rollText = false;

	NPC host = null;
	public NPC Host { set => host = value; }

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

	float soundPitch = 1f;
	public float SoundPitch { set => soundPitch = value; }

	float pitchVariance = 0.02f;
	
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

			if (Input.GetButtonDown("Interact"))
			{
				if (visibleCharacters < dialogueText[dialoguePage].Length)
				{
					visibleCharacters = dialogueText[dialoguePage].Length;
				}
				else
				{
					if (dialoguePage < dialogueText.Length - 1)
					{
						visibleCharacters = 0;
						text.maxVisibleCharacters = 0;
						dialoguePage++;
						text.text = dialogueText[dialoguePage];
						name.text = dialogueNames[dialoguePage];
						rollText = false;
						Invoke(nameof(RollText), 0.05f);
					}
					else
					{
						soundClose.Play();
						GetComponent<Animator>().Play("Disappear");
						Invoke(nameof(DestroySelf), 0.48f);
					}
				}
			}
		}
	}

	public void StartDialogue(string[] text, string[] speakerNames, NPC newHost)
	{
		GetComponent<AudioSource>().Play();
		host = newHost;
		nameObject.GetComponent<TextMeshProUGUI>().text = speakerNames[0];
		var txt = GetComponentInChildren<TextMeshProUGUI>();
		txt.maxVisibleCharacters = 0;
		dialogueText = text;
		dialogueNames = speakerNames;
		txt.text = dialogueText[0];
		GetComponent<Animator>().Play("Appear");
		Invoke(nameof(RollText), 0.5f);
	}

	void RollText()
	{
		rollText = true;
	}

	void DestroySelf()
	{
		host.EndDialogue();
		Destroy(gameObject);
	}
}
