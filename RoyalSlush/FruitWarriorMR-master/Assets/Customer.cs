using System.Collections;
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

		LeanTween.delayedCall(3, PlaceOrder);
	}

	private void Start()
	{
		
	}


	public static Dictionary<SmoothieType,string> Types = new Dictionary<SmoothieType, string>()
	{
		{ SmoothieType.Healthy, "HEALTHY" },
		{ SmoothieType.Sweet, "SWEET" },
		{ SmoothieType.Sour, "SOUR" }
	};

	public static Dictionary<SmoothieModifier,string> Modifiers = new Dictionary<SmoothieModifier, string>()
	{
		{ SmoothieModifier.Death, "DEATH" },
		{ SmoothieModifier.Luck, "PUNCH in the FACE!!!" },
		{ SmoothieModifier.TheFunk, "FUNKY" }
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

		randNum = UnityEngine.Random.Range(0,4);
		order.modifier = Modifiers[ (SmoothieModifier) randNum];

		randNum = UnityEngine.Random.Range(0,4);

		var customerOrder = beginList[UnityEngine.Random.Range(0,6)] + " " + order.type + " smoothie " + hintList[randNum] + " " + order.modifier;
		CanvasController.Instance.ReplaceText("Smoothie Order: ", customerOrder);

		print("do order");

		switch(order.type)
		{
			case "HEALTHY":
				AudioController.getSingleton().PlaySFX("healthy");
				break;

			case "SWEET":
				AudioController.getSingleton().PlaySFX("sweet");
				break;

			case "SOUR":
				AudioController.getSingleton().PlaySFX("sour");
				break;
		}
	}

	public static bool Validate(string customerSmothieMod, SmoothieItemType type)
	{
		bool validated = false;

		if (customerSmothieMod == Modifiers[SmoothieModifier.Death])
		{
			if (type == SmoothieItemType.Skull)
			{
				validated = true;
				CanvasController.Instance.ReplaceText("Smoothie Operator", "This smoothie tastes good!");
			}
		}

//		else if (customerSmothieMod == Modifiers[SmoothieModifier.Luck])
//		{
//			//if (type == Smoothie
//		}

		else if (customerSmothieMod == Modifiers[SmoothieModifier.PunchInTheFace])
		{
			if (type == SmoothieItemType.Hammer)
			{
				validated = true;
				CanvasController.Instance.ReplaceText("Smoothie Operator", "This smoothie tastes good!");

			}
		}

		else if (customerSmothieMod == Modifiers[SmoothieModifier.TheFunk])
		{
			if (type == SmoothieItemType.Cheese)
			{
				validated = true;
				CanvasController.Instance.ReplaceText("Smoothie Operator", "This smoothie tastes good!");

			}
		}

		if (!validated)
			CanvasController.Instance.ReplaceText("Smoothie Operator", "Ugh this tastes terrible!");
		

		return validated;
	}

}


public class SmoothieOrder
{
	public string type;
	public string modifier;
}

