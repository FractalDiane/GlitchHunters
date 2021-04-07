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
	Dictionary<string, GlitchProgress.Glitch> currentGlitches;

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
			Close();
		}
	}

	void Close() {
		soundClose.Play();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		GetComponent<Animator>().Play("Disappear");
		Invoke(nameof(CloseMenu), 0.8f);
	}

	public void ShowList(Dictionary<string, GlitchProgress.Glitch> glitches)
	{
		Refresh(glitches);

		GetComponent<Animator>().Play("Appear");
		isMenuOpen = true;
	}
	public void Refresh(Dictionary<string, GlitchProgress.Glitch> glitches) {
		listText.text = string.Empty;
		foreach (var pair in glitches)
		{
			if (pair.Value.available||pair.Value.completed)
			{
				listText.text += $"• {(pair.Value.completed ? "<color=grey><s>" : "")}{pair.Value.longText}{(pair.Value.completed ? "</s></color>" : "")}\n";
				if (pair.Value.description != "")
				{
					listText.text += $"<size=20>{(pair.Value.completed ? "<color=grey><s>" : "<color=#888>")}{pair.Value.description}{(pair.Value.completed ? "</s></color>" : "</color>")}</size>\n";
				}
			}
			else
			{
				listText.text += "<color=grey>[???]</color>\n";
			}
		}
	}

	void CloseMenu()
	{
		menuClosed.Invoke();
		Destroy(gameObject);
	}

	public void NextPage()
	{
		listText.pageToDisplay++;
		if (listText.pageToDisplay > 10)
		{
			// GlitchProgress.Singleton.UnlockGlitch("page_error", true);
			GlitchProgress.Singleton.CompleteGlitch("page_error");
			listText.pageToDisplay = 1;
			Close();
		}
	}

	public void LastPage()
	{
		if (listText.pageToDisplay>1)
		{
			listText.pageToDisplay--;
		}
	}
}
