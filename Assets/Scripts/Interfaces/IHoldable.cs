using System;
using Unity.VisualScripting;
using UnityEngine;

public enum HoldableCategory
{
    Food,
    Object
}

public interface IHoldable
{
    public HoldableCategory GetHoldableCategory();

    public FoodType GetFoodType();

    public ObjectType GetObjectType();

    public Sprite GetSprite();
}
