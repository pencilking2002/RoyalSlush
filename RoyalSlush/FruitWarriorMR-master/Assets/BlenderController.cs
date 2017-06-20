using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BlenderController : MonoBehaviour {

	public static BlenderController Instance;

	public Action onAddSmoothieItem;

	public BlendButton blendButton;

	/// <summary>
	/// Particles that get emitted when a food item is dropped into the blender
	/// </summary>
	public ParticleSystem dropParticles;

	private Rigidbody rb;
	//private List<string> foodTagList;

	// List that contain all the items in a smoothie
	public List<SmoothieItem> smoothiItems = new List<SmoothieItem> ();

	private void Awake()
	{
		InitInstance ();
		rb = GetComponent<Rigidbody> ();
	}


	/// <summary>
	/// Play a particle effect when a food item collides with the blender
	/// </summary>
	private void PlayDropParticles()
	{
		if (dropParticles != null) 
		{
			dropParticles.Play ();
		}
	}

	/// <summary>
	/// Adds an item to the blender
	/// </summary>
	/// <param name="item">Item.</param>
	public void AddSmoothieItem(SmoothieItem item)
	{
		if (!smoothiItems.Contains (item)) 
		{
			smoothiItems.Add (item);

			print ("Item added");

			if (onAddSmoothieItem != null)
				onAddSmoothieItem ();
		}
	}

	public void TryToBlend(SmoothieItem item){

		if (!smoothiItems.Contains (item)) 
		{
			smoothiItems.Remove(item);
			Destroy(item.gameObject);
		}
	}

	private void InitInstance()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy (gameObject);
	}

}