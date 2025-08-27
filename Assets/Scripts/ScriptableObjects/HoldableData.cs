using UnityEngine;

[CreateAssetMenu(fileName = "HoldableData", menuName = "Scriptable Materials/HoldableData")]
public class HoldableData : ScriptableObject
{
    public HoldableCategory holdableCategory;
    public FoodType foodType;
    public MaterialType materialType;
    public Sprite sprite;
    public GameObject prefab;
}
