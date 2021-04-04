using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GlitchList : MonoBehaviour
{
	void Start()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	void Update()
	{
		
	}

	public void ShowList(Dictionary<string, bool> glitches)
	{
		TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();
		text.text = string.Empty;
		foreach (var pair in glitches)
		{
			text.text += $"{(pair.Value ? "<color=blue>✓</color>" : "<color=red>✗</color>")} {pair.Key}\n";
		}

		GetComponent<Animator>().Play("Show");
	}
}
