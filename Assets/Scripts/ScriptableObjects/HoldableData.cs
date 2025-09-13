using UnityEngine;

[CreateAssetMenu(fileName = "HoldableData", menuName = "Scriptable Objects/HoldableData")]
public class HoldableData : ScriptableObject
{
    public HoldableCategory holdableCategory;
    public FoodType foodType;
    public MaterialType materialType;
    public MevoData spawnMevoData;
    public Sprite sprite;
}
