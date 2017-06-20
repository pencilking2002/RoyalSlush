using UnityEngine;
using System.Collections;

public class PlayerPrefsUtil
{
	private static readonly int INT_FALSE = 0;
	private static readonly int INT_TRUE = 1;

	private static bool isIntTrue(int truthValue)
	{
		return (truthValue != INT_FALSE);//non-zero--> true
	}
	private static bool isIntFalse(int truthValue)
	{
		return !isIntTrue(truthValue);
	}
	
	public static bool readBoolPref(string key, bool defaultValue)
	{
		int defaultV = defaultValue ? INT_TRUE : INT_FALSE;
		int retV = PlayerPrefs.GetInt(key, defaultV);
		return isIntTrue(retV);
	}
	public static void writeBoolPref(string key, bool value)
	{
		int writeV = value ? INT_TRUE : INT_FALSE;
		PlayerPrefs.SetInt(key, writeV);
	}
}
