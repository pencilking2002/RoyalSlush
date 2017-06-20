using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for identifying what kind of smoothieitem this is and also
/// adding itself to a smoothie upon collision with the blender
/// </summary>
public class SmoothieItem : MonoBehaviour {
	public SmoothieItemType type;

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
