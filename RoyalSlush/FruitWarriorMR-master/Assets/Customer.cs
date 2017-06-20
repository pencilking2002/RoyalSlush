﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum SmoothieType {
	Healthy,
	Sweet,
	Sour
}

public enum SmoothieModifier {
	Death,
	Luck,
	TheFunk,
	PunchInTheFace
}


public class Customer : MonoBehaviour {
	public static SmoothieOrder currentOrder;
	public SmoothieOrder order;

	private void Awake()
	{
		order = new SmoothieOrder();
		currentOrder = order;
	}


	public static Dictionary<SmoothieType,string> Types = new Dictionary<SmoothieType, string>()
	{
		{ SmoothieType.Healthy, "healthy" },
		{ SmoothieType.Sweet, "sweet" },
		{ SmoothieType.Sour, "sour" }
	};

	public static Dictionary<SmoothieModifier,string> Modifiers = new Dictionary<SmoothieModifier, string>()
	{
		{ SmoothieModifier.Death, "death" },
		{ SmoothieModifier.Luck, "luck" },
		{ SmoothieModifier.TheFunk, "the funk" },
		{ SmoothieModifier.PunchInTheFace, "punch in the face" }
	};

	public static List<string> beginList = new List<string>()
	{
		{ "Can I get a" },	
		{ "May I have a" },	
		{ "Yeah can I get a" },	
		{ "I REALLY want a" },
		{ "Yo! Drop me a " },
		{ "Sup dawg? I'm in the mood for a " }

	};

	public static List<string> hintList = new List<string>()
	{
		{ "with a hint of" },	
		{ "and a touch of" },	
		{ "and a little bit of" },	
		{ "and can you add some" }

	};

	private void Update ()
	{
		if (Input.GetKeyDown(KeyCode.G))
		{


			//print("Hello");
			PlaceOrder();
		}
	}

	private void PlaceOrder()
	{
		
		int randNum = UnityEngine.Random.Range(0,4);
		order.type = Types[ (SmoothieType) randNum];

		Debug.Log(order.type);

		randNum = UnityEngine.Random.Range(0,5);
		order.modifier = Modifiers[ (SmoothieModifier) randNum];

		randNum = UnityEngine.Random.Range(0,5);

		var customerOrder = beginList[UnityEngine.Random.Range(0,7)] + order.type + " smoothie " + hintList[randNum] + " " + order.modifier;
		CanvasController.Instance.ReplaceText("Smoothie Order: ", customerOrder);
	}
}


public class SmoothieOrder
{
	public string type;
	public string modifier;
}

