using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoading : MonoBehaviour
{
	[SerializeField]
	Transform loadingWheelTransform = null;

	[SerializeField]
	GameObject canvas = null;

	[SerializeField]
	GameObject blockToHide = null;
	[SerializeField]
	GameObject sceneTransition = null;

	bool loaded = false;

	void Start()
	{
		Invoke(nameof(ShowDialogue), 3.5f);
		Invoke(nameof(OpenDoor), 22f);
	}

	void Update()
	{
		loadingWheelTransform.Rotate(new Vector3(0, 0, 200f * Time.deltaTime), Space.World);
	}

	void ShowDialogue()
	{
		string[] dlg = new string[] {"Oh, hey. Welcome back, Mux.", "These loading times take a while sometimes, yeah?", "Maybe you could practice your skills in the meantime?"};
		PlayerUI.Singleton.DialogueAsync("Cheesegrater", dlg);
	}

	void OpenDoor()
	{
		GetComponent<AudioSource>().Play();
		canvas.SetActive(false);
		blockToHide.SetActive(false);
		loaded = true;
	}

	void OnTriggerEnter(Collider collider)
	{
		if (loaded && collider.gameObject.tag == "Player")
		{
			SceneManager.LoadScene(3);
		}
	}
}
