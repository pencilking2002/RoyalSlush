using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlendButton : MonoBehaviour, IInputClickHandler{

	public void OnInputClicked(InputEventData eventData)
	{
		//SceneManager.LoadScene("Main");
		TurnOnBlender();
	}

	void OnBlend()
	{
		TurnOnBlender();
		//SceneManager.LoadScene("Main");
	}

	void TurnOnBlender(){


	}


	void TurnOffBlender(){


	}

}
