using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlendButton : MonoBehaviour, IInputClickHandler{

	Vector3 str;
	public bool blenderOn = false;
	public GameObject blades;

	public GameObject blenderGo;
	Vector3 blenderGoStr;


	public GameObject tableGo;
	Vector3 tableGoStr;


	void Start(){
		str = transform.position;
		blenderGoStr = blenderGo.transform.position;
		tableGoStr = tableGo.transform.position;
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

			LeanTween.delayedCall(5, () => {
				TurnOffBlender();
				blenderOn = false;

				foreach(SmoothieItem item in BlenderController.Instance.smoothiItems)
				{
					string itemType = item.type.ToString();
					//print(item.type.ToString());

					if (itemType == "Skull")
					{
						//Customer.Validate(RoyalSlushController.Instance.currentCustomer.order.modifier, item.type);
						print (RoyalSlushController.Instance.currentCustomer.order.modifier.ToString());
						print(item.type.ToString());
					}
					else if (itemType == "Hammer")
					{
						
					} 
					else if (itemType == "Cheese")
					{
						
					}

				}

			});
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

				blenderGo.transform.position = blenderGoStr + (Random.insideUnitSphere * 0.03f);

				//blenderGo.transform.rotation = Random.r

				shakeTimer = 0.011f;


				tableGo.transform.position = tableGoStr + (Random.insideUnitSphere * 0.01f);
			}

			shakeTimer -= Time.deltaTime;

		}else{
			blenderGo.transform.position = blenderGoStr;
			tableGo.transform.position = tableGoStr;
		}

		// SPIN Blades around
		if(blades){
			if(blenderOn){
				
				bladeSpeed = 2000f;
			}

			if(bladeSpeed > 0){
				bladeSpeed -= 10f;
			}

			blades.transform.Rotate(Vector3.up * bladeSpeed * Time.deltaTime);
		}
	}

	float bladeSpeed = 0;

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
