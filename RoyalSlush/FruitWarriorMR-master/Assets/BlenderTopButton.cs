using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlenderTopButton : MonoBehaviour, IInputClickHandler{

	Vector3 str;
	bool blenderOn = false;

	public GameObject blenderTopGo;
	Vector3 blenderGoTopStr;

	public BlendButton blenderButton;

	void Start(){
		str = transform.position;
		//blenderGoStr = blenderGo.transform.position;
	}

	public void OnInputClicked(InputEventData eventData)
	{
		OpenBlenderTop();
	}

	void OnBlend()
	{
		if(blenderOn){
			CloseBlenderTop();

			blenderOn = false;
		}else{
			OpenBlenderTop();
			blenderOn = true;
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.T))
		{
			blenderButton.OnBlend();
		}
	}
		
	void OpenBlenderTop(){


		AudioController.getSingleton().PlaySFX("Sounds/Toggle-SoundBible.com-231290292");


		// Need sound effect
		LeanTween.rotateAroundLocal(blenderTopGo,Vector3.left, 170f,0.5f).setOnComplete (() => {

		
		AudioController.getSingleton().PlaySFX("Sounds/Sharp Punch-SoundBible.com-1947392621");

		});
	}


	void CloseBlenderTop(){

		AudioController.getSingleton().PlaySFX("Sounds/Toggle-SoundBible.com-231290292");


		LeanTween.rotateAroundLocal(blenderTopGo,Vector3.left, -170f,0.5f);
		/*LeanTween.rotateX(blenderTopGo,170,0.5f).setOnComplete (() => {


		});*/
	}

}
