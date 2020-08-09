using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScriptableCondition : MonoBehaviour
{
	public ScriptableObject testData;

    void Start()
	{
		var condition = GetComponent<ScriptableCondition>();
		Debug.Log(condition.Verify(testData));
	}
}
