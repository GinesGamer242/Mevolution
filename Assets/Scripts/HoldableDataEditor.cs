using System.Collections.Generic;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

public class HoldableDataEditor : Editor
{
    #region SerializedProperties
    SerializedProperty holdableCategory;
    SerializedProperty foodType;
    SerializedProperty materialType;
    SerializedProperty spawnMevoData;
    SerializedProperty sprite;
    SerializedProperty prefab;
    #endregion SerializedProperties

    private void OnEnable()
    {
        holdableCategory = serializedObject.FindProperty("holdableCategory");
        foodType = serializedObject.FindProperty("foodType");
        materialType = serializedObject.FindProperty("materialType");
        spawnMevoData = serializedObject.FindProperty("spawnMevoData");
        sprite = serializedObject.FindProperty("sprite");
        prefab = serializedObject.FindProperty("prefab");
    }

    public override void OnInspectorGUI()
    {
        HoldableData thisHoldableData = (HoldableData)target;

        serializedObject.Update();

        EditorGUILayout.PropertyField(sprite);
        EditorGUILayout.PropertyField(prefab);
        EditorGUILayout.PropertyField(holdableCategory);

        switch (thisHoldableData.holdableCategory)
        {
            case HoldableCategory.Food:
                EditorGUILayout.PropertyField(foodType);
                EditorGUILayout.PropertyField(spawnMevoData);
                break;

            case HoldableCategory.Material:
                EditorGUILayout.PropertyField(materialType);
                break;

            default:
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}