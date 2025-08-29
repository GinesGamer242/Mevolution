using System.Collections.Generic;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

[CustomEditor(typeof(MevoData))]
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

    bool statsGroup, ingredientsGroup = false;
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

        isBasicMevo = serializedObject.FindProperty("isBasicMevo");
        foodIngredient = serializedObject.FindProperty("foodIngredient");
        mevoIngredients = serializedObject.FindProperty("mevoIngredients");
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

        ingredientsGroup = EditorGUILayout.BeginFoldoutHeaderGroup(ingredientsGroup, "Ingredients");
        if (ingredientsGroup)
        {
            EditorGUILayout.PropertyField(isBasicMevo);
            if (thisMevoData.isBasicMevo)
            {
                EditorGUILayout.PropertyField(foodIngredient);
            }
            else if (!thisMevoData.isBasicMevo)
            {
                EditorGUILayout.PropertyField(mevoIngredients);
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        serializedObject.ApplyModifiedProperties();
    }
}
