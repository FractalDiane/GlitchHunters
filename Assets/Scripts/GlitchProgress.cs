using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchProgress : MonoBehaviour
{
	static GlitchProgress singleton = null;
	public static GlitchProgress Singleton { get => singleton; }
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

	[SerializeField]
	GameObject glitchListPrefab = null;

	// =========================================================================

	[System.Serializable]
	struct GlitchData
	{
		public string identifier;
		public string longText;
	}

	public struct Glitch
	{
		public string longText;
		public bool completed;
		public Glitch(string longText) { this.longText = longText; completed = false; }
	}

	[SerializeField]
	GlitchData[] startingGlitches;

	Dictionary<string, Glitch> glitches = new Dictionary<string, Glitch>();

	bool menuOpen = false;

	void Start()
	{
		foreach (GlitchData data in startingGlitches)
		{
			glitches.Add(data.identifier, new Glitch(data.longText));
		}

		playerScript = FindObjectOfType<Player>();
		player = playerScript.gameObject;
	}

	void Update()
	{
		if (!playerScript.LockMovement && Input.GetButtonDown("Pause") && !menuOpen)
		{
			playerScript.LockMovement = true;
			var menu = Instantiate(glitchListPrefab, Vector3.zero, Quaternion.identity);
			GlitchList script = menu.GetComponent<GlitchList>();
			script.MenuClosed.AddListener(MenuClosed);
			script.ShowList(glitches);
		}
	}

	public void CompleteGlitch(string identifier)
	{
		Glitch glitch = glitches[identifier];
		glitch.completed = true;
		glitches[identifier] = glitch;
	}

	
	void MenuClosed()
	{
		playerScript.LockMovement = false;
	}
}
