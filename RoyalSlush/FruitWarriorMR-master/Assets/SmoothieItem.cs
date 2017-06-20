using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for identifying what kind of smoothieitem this is and also
/// adding itself to a smoothie upon collision with the blender
/// </summary>
public class SmoothieItem : MonoBehaviour {
	public SmoothieItemType type;

	/// <summary>
	/// Add the smoothie item to the blender when it collides
	/// </summary>
	/// <param name="other">Other.</param>
	private void OnCollisionEnter(Collision other)
	{
		if (other.collider.CompareTag ("Blender")) 
		{
			BlenderController.Instance.AddSmoothieItem (this);
		}	
	}
}

public enum SmoothieItemType {
	Apple,
	Cucumber,
}
