using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class GlitchList : MonoBehaviour
{
	UnityEvent menuClosed;
	public UnityEvent MenuClosed { get => menuClosed; }

	bool isMenuOpen = false;

	AudioSource soundOpen;
	AudioSource soundClose;

	[SerializeField]
	TextMeshProUGUI listText;

	void Awake()
	{
		menuClosed = new UnityEvent();
	}

	void Start()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		AudioSource[] sources = GetComponents<AudioSource>();
		soundOpen = sources[0];
		soundClose = sources[1];
		soundOpen.Play();
	}

	void Update()
	{
		if (Input.GetButtonDown("Pause") && isMenuOpen)
		{
			soundClose.Play();
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			GetComponent<Animator>().Play("Disappear");
			Invoke(nameof(CloseMenu), 0.8f);
		}
	}

	public void ShowList(Dictionary<string, GlitchProgress.Glitch> glitches)
	{
		listText.text = string.Empty;
		foreach (var pair in glitches)
		{
			if (pair.Value.available)
			{
				listText.text += $"• {(pair.Value.completed ? "<color=grey><s>" : "")}{pair.Value.longText}{(pair.Value.completed ? "</s></color>" : "")}\n";
				//listText.text += $"\t{(pair.Value.completed ? "<color=grey><s>" : "")}{pair.Value.description}{(pair.Value.completed ? "</s></color>" : "")}\n";
			}
			else
			{
				listText.text += "    <color=grey>[???]</color>\n";
			}
		}

		GetComponent<Animator>().Play("Appear");
		isMenuOpen = true;
	}

	void CloseMenu()
	{
		menuClosed.Invoke();
		Destroy(gameObject);
	}

	public void NextPage()
	{
		listText.pageToDisplay++;
	}

	public void LastPage()
	{
		listText.pageToDisplay--;
	}
}
