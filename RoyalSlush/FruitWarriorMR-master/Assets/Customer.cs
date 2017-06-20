using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour {
	public OrderType orderType;

	private void PlaceOrder(OrderType orderType)
	{
		this.orderType = orderType;
	}
}

public enum OrderType {
	Sweet,
	Juicy,
	Sour,
	Healthy,
	Energetic
}
