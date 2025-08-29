using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MevoData", menuName = "Scriptable Objects/MevoData")]
public class MevoData : ScriptableObject
{
    new public string name;
    public Sprite sprite;
    public GameObject prefab;
    public float health;
    public float damage;
    public float attackSpeed;
    public float moveSpeed;
    public bool isBasicMevo;
    public FoodType foodIngredient;
    public List<MevoData> mevoIngredients;
}
