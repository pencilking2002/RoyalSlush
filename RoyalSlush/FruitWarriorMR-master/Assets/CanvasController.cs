using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CanvasController : MonoBehaviour {

	public static CanvasController Instance;
	public Text blackboaxrdText; 
	private Canvas canvas;

	private void Awake()
	{
		InitInstance ();
		canvas = GetComponent<Canvas>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void InitInstance()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy (gameObject);
	}

	public void ReplaceText(string text)
	{
		this.blackboaxrdText.text = text;	
	}
}
