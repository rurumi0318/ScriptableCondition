using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

[CustomEditor(typeof(ScriptableCondition))]
public class ScriptableConditionEditor : Editor
{
	private SerializedProperty propFieldName;
	private SerializedProperty propVerifyMethod;
	private SerializedProperty propDefaultData;

	private SerializedProperty propBoolCondition;
	private SerializedProperty propFloatCondition;
	private SerializedProperty propIntCondition;
	private SerializedProperty propStringCondition;
	
	void OnEnable()
	{
		propFieldName = serializedObject.FindProperty("fieldName");
		propVerifyMethod = serializedObject.FindProperty("verifyMethod");
		propDefaultData = serializedObject.FindProperty("defaultData");

		propBoolCondition = serializedObject.FindProperty("boolCondition");
		propFloatCondition = serializedObject.FindProperty("floatCondition");
		propIntCondition = serializedObject.FindProperty("intCondition");
		propStringCondition = serializedObject.FindProperty("stringCondition");
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		if (DrawDefaultDataField() == false)
			return;

		System.Type fieldType;
		if (DrawFieldSelector(out fieldType) == false)
			return;

		if (DrawOperator(fieldType) == false)
			return;

		DrawConditionValue(fieldType);
	}

	private bool DrawDefaultDataField()
	{
		if (propDefaultData.objectReferenceValue == null)
		{
			EditorGUILayout.HelpBox("Must steup a default scriptable object", MessageType.Warning);

			var oldColor = GUI.backgroundColor;
			GUI.backgroundColor = new Color(0.8f, 0f, 0f);
			EditorGUILayout.PropertyField(propDefaultData, new GUIContent("Data template"));
			GUI.backgroundColor = oldColor;

			propFieldName.stringValue = null;
			serializedObject.ApplyModifiedProperties();
			return false;
		}
		else
		{
			EditorGUILayout.PropertyField(propDefaultData, new GUIContent("Data template"));
			serializedObject.ApplyModifiedProperties();
			return propDefaultData.objectReferenceValue != null;
		}
	}

	private bool DrawFieldSelector(out System.Type fieldType)
	{
		fieldType = default(System.Type);

		var defaultObject = propDefaultData.objectReferenceValue;
		var fieldInfoArray = defaultObject.GetType().GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

		if (fieldInfoArray.Length == 0)
		{
			propFieldName.stringValue = null;
			serializedObject.ApplyModifiedProperties();
			return false;
		}

		List<string> names = new List<string>();
		for (int i = 0; i < fieldInfoArray.Length; ++i)
		{
			names.Add(fieldInfoArray[i].Name);
		}

		string oldFieldName = propFieldName.stringValue;
		int selectedIndex = Math.Max(names.IndexOf(oldFieldName), 0);
		
		selectedIndex = EditorGUILayout.Popup("Field to verify", selectedIndex, names.ToArray());

		propFieldName.stringValue = names[selectedIndex];
		serializedObject.ApplyModifiedProperties();

		fieldType = fieldInfoArray[selectedIndex].FieldType;
		return true;
	}

	private bool DrawOperator(System.Type fieldType)
	{
		ScriptableCondition condition = (ScriptableCondition)target;

		if (condition.IsSupportedType(fieldType) == false)
		{
			EditorGUILayout.HelpBox($"The field type isn't supported: {fieldType}", MessageType.Warning);
			return false;
		}

		int selectedIndex = propVerifyMethod.enumValueIndex;
		List<string> operators = new List<string>();
		operators.Add("==");
		operators.Add("!=");

		if (condition.IsOnlySupportEquals(fieldType))
		{
			if (selectedIndex > 1)
				selectedIndex = 0;
		}
		else
		{
			operators.Add(">");
			operators.Add(">=");
			operators.Add("<");
			operators.Add("<=");
		}

		selectedIndex = EditorGUILayout.Popup("Operator", selectedIndex, operators.ToArray());
		propVerifyMethod.enumValueIndex = selectedIndex;

		serializedObject.ApplyModifiedProperties();
		return true;
	}

	private void DrawConditionValue(System.Type fieldType)
	{
		GUIContent label = new GUIContent("Compare to");

		if (fieldType == typeof(bool))
			EditorGUILayout.PropertyField(propBoolCondition, label);
		else if (fieldType == typeof(float))
			EditorGUILayout.PropertyField(propFloatCondition, label);
		else if (fieldType == typeof(int))
			EditorGUILayout.PropertyField(propIntCondition, label);
		else
			EditorGUILayout.PropertyField(propStringCondition, label);

		serializedObject.ApplyModifiedProperties();
	}
}
