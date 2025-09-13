using System.Collections.Generic;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

public class MevoDataEditor : Editor
{
    #region SerializedProperties
    new SerializedProperty name;
    SerializedProperty sprite;
    SerializedProperty prefab;

    SerializedProperty health;
    SerializedProperty damage;
    SerializedProperty attackSpeed;
    SerializedProperty moveSpeed;

    SerializedProperty isBasicMevo;
    SerializedProperty foodIngredient;
    SerializedProperty mevoIngredients;

    bool statsGroup = false;
    #endregion SerializedProperties

    private void OnEnable()
    {
        name = serializedObject.FindProperty("name");
        sprite = serializedObject.FindProperty("sprite");
        prefab = serializedObject.FindProperty("prefab");
        
        health = serializedObject.FindProperty("health");
        damage = serializedObject.FindProperty("damage");
        attackSpeed = serializedObject.FindProperty("attackSpeed");
        moveSpeed = serializedObject.FindProperty("moveSpeed");
        foodIngredient = serializedObject.FindProperty("foodIngredient");
    }

    public override void OnInspectorGUI()
    {
        MevoData thisMevoData = (MevoData)target;

        serializedObject.Update();

        EditorGUILayout.PropertyField(name);
        EditorGUILayout.PropertyField(sprite);
        EditorGUILayout.PropertyField(prefab);

        statsGroup = EditorGUILayout.BeginFoldoutHeaderGroup(statsGroup, "Stats");
        if (statsGroup)
        {
            EditorGUILayout.PropertyField(health);
            EditorGUILayout.PropertyField(damage);
            EditorGUILayout.PropertyField(attackSpeed);
            EditorGUILayout.PropertyField(moveSpeed);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        serializedObject.ApplyModifiedProperties();
    }
}
