using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableCondition : MonoBehaviour
{
	public enum Operator
	{
		Equals,
		NotEqual,
		Greater,
		GreaterOrEqual,
		Less,
		LessOrEqual
	}

	[SerializeField]
	private string fieldName;

	[SerializeField]
	private Operator verifyMethod;

	[SerializeField]
	private ScriptableObject defaultData;

	[SerializeField]
	private bool boolCondition;
	[SerializeField]
	private float floatCondition;
	[SerializeField]
	private int intCondition;
	[SerializeField]
	private string stringCondition;

	/*
	 * Compare value of the data to the value of condition
	 * [data] [operator] [condition]
	 */
	public bool Verify(ScriptableObject data)
	{
		if (data == null)
			return false;

		if (string.IsNullOrEmpty(fieldName))
			return false;

		var fieldInfo = data.GetType().GetField(fieldName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

		if (fieldInfo == null)
		{
			Debug.LogError($"Data doesn't have this field: {fieldName}");
			return false;
		}

		if (fieldInfo.FieldType == typeof(bool))
		{
			bool v = (bool)fieldInfo.GetValue(data);
			return VerifyBool(v, verifyMethod, (bool)boolCondition);
		}
		else if (fieldInfo.FieldType == typeof(float))
		{
			float v = (float)fieldInfo.GetValue(data);
			return VerifyFloat(v, verifyMethod, (float)floatCondition);
		}
		else if (fieldInfo.FieldType == typeof(int))
		{
			int v = (int)fieldInfo.GetValue(data);
			return VerifyInt(v, verifyMethod, (int)intCondition);
		}
		else
		{
			string v = (string)fieldInfo.GetValue(data);
			return VerifyString(v, verifyMethod, (string)stringCondition);
		}

	}

	public bool IsSupportedType(System.Type type)
	{
		return type == typeof(bool)
			|| type == typeof(float)
			|| type == typeof(int)
			|| type == typeof(string);
	}

	public bool IsOnlySupportEquals(System.Type type)
	{
		return type == typeof(bool)
			|| type == typeof(string);
	}

	#region comparison functions
	private bool VerifyBool(bool value, Operator verifyMethod, bool target)
	{
		switch (verifyMethod)
		{
			case Operator.NotEqual:
				return value != target;
			case Operator.Equals:
			default:
				return value == target;
		}
	}

	private bool VerifyFloat(float value, Operator verifyMethod, float target)
	{
		switch (verifyMethod)
		{
			case Operator.NotEqual:
				return value != target;
			case Operator.Greater:
				return value > target;
			case Operator.GreaterOrEqual:
				return value >= target;
			case Operator.Less:
				return value < target;
			case Operator.LessOrEqual:
				return value <= target;
			case Operator.Equals:
			default:
				return Mathf.Abs(value - target) <= float.Epsilon;
		}
	}

	private bool VerifyInt(int value, Operator verifyMethod, int target)
	{
		switch (verifyMethod)
		{
			case Operator.NotEqual:
				return value != target;
			case Operator.Greater:
				return value > target;
			case Operator.GreaterOrEqual:
				return value >= target;
			case Operator.Less:
				return value < target;
			case Operator.LessOrEqual:
				return value <= target;
			case Operator.Equals:
			default:
				return value == target;
		}
	}

	private bool VerifyString(string value, Operator verifyMethod, string target)
	{
		switch (verifyMethod)
		{
			case Operator.NotEqual:
				return value != target;
			case Operator.Equals:
			default:
				return value == target;
		}
	}
	#endregion
}
