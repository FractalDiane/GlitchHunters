using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingAnimation : MonoBehaviour
{
	[HideInInspector]
	[SerializeField]
	int visibleChars = 0;

	[SerializeField]
	TextMeshProUGUI text = null;
	[SerializeField]
	Transform spriteTransform = null;

	void Start()
	{
		Invoke(nameof(PlayAnimation), 1f);
		Invoke(nameof(Transition), 5f);
	}

	void Update()
	{
		text.maxVisibleCharacters = visibleChars;
		spriteTransform.Rotate(new Vector3(0, 0, 120f * Time.deltaTime), Space.World);
	}

	void PlayAnimation()
	{
		GetComponent<Animator>().Play("Type");
	}

	void Transition()
	{
		SceneManager.LoadScene("Loading");
	}
}
