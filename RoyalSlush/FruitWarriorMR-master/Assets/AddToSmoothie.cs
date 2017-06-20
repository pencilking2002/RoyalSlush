using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToSmoothie : MonoBehaviour {

	private void OnCollisionEnter(Collision other)
	{
		var item = other.gameObject.GetComponent<SmoothieItem>();

		if (item != null)
		{
			if (!BlenderController.Instance.smoothiItems.Contains(item))
			{
				BlenderController.Instance.AddSmoothieItem(item);
			}
		}
	}
}
