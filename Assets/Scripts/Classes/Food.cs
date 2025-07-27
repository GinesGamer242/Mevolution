using System;
using UnityEngine;

public enum FoodType
{
    None,
    Apple
}

[Serializable]
public class Food : IHoldable
{
    HoldableCategory holdableCategory = HoldableCategory.Food;
    [SerializeField]
    FoodType type;
    [SerializeField]
    Sprite sprite;

    public HoldableCategory GetHoldableCategory()
    {
        return holdableCategory;
    }

    public FoodType GetFoodType()
    {
        return type;
    }

    public ObjectType GetObjectType()
    {
        return ObjectType.None;
    }

    public Sprite GetSprite()
    {
        return sprite;
    }
}
