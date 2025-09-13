using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "MevoData", menuName = "Scriptable Objects/MevoData")]
public class MevoData : ScriptableObject
{
    new public string name;
    public Sprite sprite;
    public GameObject prefab;
    public List<MevoData> mevoIngredients;

    [Header ("Stats")]
    public float health;
    public float damage;
    public float attackSpeed;
    public float moveSpeed;
    public float attackRange;
}
