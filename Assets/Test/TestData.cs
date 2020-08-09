using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TestData", menuName = "ScriptableCondition/TestData")]
public class TestData : ScriptableObject
{
	public bool boolField;
	public string stringField;
	public int intField;
	public float floatField;
}
