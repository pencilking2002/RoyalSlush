using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlendButton : MonoBehaviour, IInputClickHandler{

	Vector3 str;
	bool blenderOn = false;

	public GameObject blenderGo;

	Vector3 blenderGoStr;

	void Start(){
		str = transform.position;
		blenderGoStr = blenderGo.transform.position;
	}

	public void OnInputClicked(InputEventData eventData)
	{
		//SceneManager.LoadScene("Main");
		TurnOnBlender();
	}

	public void OnBlend()
	{
		if(blenderOn){
			TurnOffBlender();
			blenderOn = false;
		}else{
			blenderOn = true;
			TurnOnBlender();
		}
			
		//SceneManager.LoadScene("Main");
	}

	float shakeTimer = 0;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.B))
		{
			OnBlend();
		}

		// Shake blender
		if(blenderOn){

			if(shakeTimer < 0){

				blenderGo.transform.position = blenderGoStr + (Random.insideUnitSphere * 0.04f);

				//blenderGo.transform.rotation = Random.r

				shakeTimer = 0.01f;
			}

			shakeTimer -= Time.deltaTime;

		}else{
			blenderGo.transform.position = blenderGoStr;
		}
	}

	void TurnOnBlender(){
			
		AudioController.getSingleton().PlaySFX("Sounds/Blender on Blend Sound Effect");
		AudioController.getSingleton().PlaySFX("Sounds/Toggle-SoundBible.com-231290292");


	
		// Need sound effect
		LeanTween.moveLocalY(gameObject,(str.y - 0.6f), 0.3f).setOnComplete (() => {


		});

	}


	void TurnOffBlender(){

		AudioController.getSingleton().PlaySFX("Sounds/Toggle-SoundBible.com-231290292");

		LeanTween.moveLocalY(gameObject,(str.y),0.3f);
	}

}
