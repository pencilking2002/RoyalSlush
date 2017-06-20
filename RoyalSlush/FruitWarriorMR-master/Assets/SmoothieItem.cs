using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for identifying what kind of smoothieitem this is and also
/// adding itself to a smoothie upon collision with the blender
/// </summary>
public class SmoothieItem : MonoBehaviour {
	public SmoothieItemType type;

	public Color color = Color.white;

	public void OnCollisionEnter(Collision other){

		// Add a cool splat graphics

		if(other.gameObject.tag == "Blade"){

			// Check if it's on
			if(BlenderController.Instance.blendButton.blenderOn){

				// Splate sound effect

				GameObject go = Instantiate(Resources.Load("Splate"), gameObject.transform.position, Quaternion.identity) as GameObject;


				go.GetComponent<ParticleSystem>().startColor = color;
				Destroy(go,4f);

				Destroy(this.gameObject);
			}
		}

		/*Debug.Log("DIE!");
		BlenderController.Instance.TryToBlend(this);*/
	}
}
public enum SmoothieItemType {
	Apple,
	Cucumber,
	Watermelon,
    Broccolli,
    Banana,
    Carrot,
    Pear,
    Cabbage,
    Corn,
    Pumpkin,
    Skull,
    Hammer,
    Cheese
}
