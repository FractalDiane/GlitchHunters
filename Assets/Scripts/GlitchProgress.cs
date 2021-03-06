using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlitchProgress : MonoBehaviour
{
	static GlitchProgress singleton = null;
	public static GlitchProgress Singleton { get => singleton; }
	public bool awlaysRepeat = false;
	public UnityEvent OnMenuOpen;
	public UnityEvent OnMenuClose;
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

	GlitchList glitchListUI = null;

	int glitchesCompleted = 0;

	// =========================================================================

	[System.Serializable]
	struct GlitchData
	{
		public string identifier;
		public string text;
		public string description;
		public string[] prerequisites;
		public bool requiresAllPrerequisites;
	}

	public struct Glitch
	{
		public string longText;
		public string description;
		public bool completed;
		public bool available;
		public bool requiresAllPrerequisites;
		public Dictionary<string, bool> prerequisites;
		public Glitch(string longText, string description, string[] prerequisites, bool requiresAllPrerequisites)
		{
			this.longText = longText;
			this.description = description;
			this.requiresAllPrerequisites = requiresAllPrerequisites;
			available = prerequisites.Length == 0;
			completed = false;

			this.prerequisites = new Dictionary<string, bool>();
			foreach (string prereq in prerequisites)
			{
				this.prerequisites.Add(prereq, false);
			}
		}
	}

	[SerializeField]
	GlitchData[] startingGlitches;

	Dictionary<string, Glitch> glitches = new Dictionary<string, Glitch>();

	bool menuOpen = false;

	void Start()
	{
		foreach (GlitchData data in startingGlitches)
		{
			glitches.Add(data.identifier, new Glitch(data.text, data.description, data.prerequisites, data.requiresAllPrerequisites));
		}

		playerScript = FindObjectOfType<Player>();
		player = playerScript.gameObject;
	}

	void Update()
	{
		if (!playerScript.LockMovement && Input.GetButtonDown("Pause") && !menuOpen)
		{
			playerScript.LockMovement = true;
			playerScript.AddedVelocity = Vector3.zero;
			var menu = Instantiate(glitchListPrefab, Vector3.zero, Quaternion.identity);
			glitchListUI = menu.GetComponent<GlitchList>();
			glitchListUI.MenuClosed.AddListener(MenuClosed);
			glitchListUI.ShowList(glitches);
			OnMenuOpen.Invoke();
		}
	}

	public void CompleteGlitch(string identifier)
	{
		if (awlaysRepeat||!glitches[identifier].completed)
		{
			Glitch glitch = glitches[identifier];
			glitch.completed = true;
			glitches[identifier] = glitch;

			// THIS IS HORRIBLE
			bool anyUnlocked = false;
			string[] keys = glitches.Keys.ToArray();
			for (int i = 0; i < keys.Length; i++)
			{
				string thisKey = keys[i];	
				Glitch thisGlitch = glitches[thisKey];
				if (!thisGlitch.available && thisGlitch.prerequisites.ContainsKey(identifier))
				{
					thisGlitch.prerequisites[identifier] = true;
					glitches[thisKey] = thisGlitch;
					if (!thisGlitch.requiresAllPrerequisites || thisGlitch.prerequisites.Values.All(b => b))
					{
						UnlockGlitch(thisKey, false);
						anyUnlocked = true;
					}
				}
			}

			PlayerUI.Singleton.PlayGlitchCompletedAnimation(glitches[identifier].longText, anyUnlocked);
			
			if(glitchListUI != null) {
				glitchListUI.Refresh(glitches);
			}

			glitchesCompleted++;
			if (glitchesCompleted >= glitches.Count)
			{
				PlayerUI.Singleton.PlayGlitchMasterAnimation();
			}
		}
	}


	public void UnlockGlitch(string identifier, bool animation)
	{
		Glitch glitch = glitches[identifier];
		glitch.available = true;
		glitches[identifier] = glitch;

		if (animation)
		{
			PlayerUI.Singleton.PlayGlitchUnlockedAnimation();
		}
		if(glitchListUI != null) {
			glitchListUI.Refresh(glitches);
		}
	}

	public void ResetPlayerPosition() {
		if (player != null)
		{
			player.transform.localPosition = Vector3.zero;
		}
	}

	
	void MenuClosed()
	{
		playerScript.LockMovement = false;
		OnMenuClose.Invoke();
	}
}
