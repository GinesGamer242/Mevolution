using System;
using UnityEngine;

public enum ObjectType
{
    None,
    Stone
}

[Serializable]
public class Object : IHoldable
{
    HoldableCategory holdableCategory = HoldableCategory.Object;
    [SerializeField]
    ObjectType type;
    [SerializeField]
    Sprite sprite;

    public HoldableCategory GetHoldableCategory()
    {
        return HoldableCategory.Object;
    }

    public ObjectType GetObjectType()
    {
        return type;
    }

    public FoodType GetFoodType()
    {
        return FoodType.None;
    }

    public Sprite GetSprite()
    {
        return sprite;
    }
}
