using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
	AudioSource hoverSound;
	AudioSource clickSound;

	bool creditsOpen = false;
	bool buttonsDisabled = false;

	[SerializeField]
	GameObject credits = null;

	void Start()
	{
		AudioSource[] sources = GetComponents<AudioSource>();
		hoverSound = sources[0];
		clickSound = sources[1];
	}

	void Update()
	{
		if (creditsOpen && (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump") || Input.GetButtonDown("Pause") || Input.GetButtonDown("Cancel")))
		{
			creditsOpen = false;
			credits.SetActive(false);
			EnableAllButtons(true);
		}
	}

	public void PlayHoverSound()
	{
		if (!buttonsDisabled)
		{
			hoverSound.Play();
		}
	}

	public void ClickStart()
	{
		clickSound.Play();
		EnableAllButtons(false);
		GetComponent<Animator>().Play("Fadeout");
		Invoke(nameof(StartGame), 1f);
	}

	public void ClickCredits()
	{
		clickSound.Play();
		EnableAllButtons(false);
		credits.SetActive(true);
		creditsOpen = true;
	}

	public void ClickExit()
	{
		clickSound.Play();
		EnableAllButtons(false);
		GetComponent<Animator>().Play("Fadeout");
		Invoke(nameof(ExitGame), 1f);
	}

	void EnableAllButtons(bool enable)
	{
		buttonsDisabled = !enable;
		foreach (Button but in GetComponentsInChildren<Button>())
		{
			but.enabled = enable;
		}
	}
	
	void StartGame()
	{
		SceneManager.LoadScene("LoadingPre");
	}

	void ExitGame()
	{
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
			Application.Quit();
		#endif
	}
}
