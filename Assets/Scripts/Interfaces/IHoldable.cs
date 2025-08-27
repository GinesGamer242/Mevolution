using System;
using Unity.VisualScripting;
using UnityEngine;

public enum HoldableCategory
{
    Food,
    Material
}

public enum FoodType
{
    None,
    Pineapple
}

public enum MaterialType
{
    None,
    Stone
}

public interface IHoldable
{
    public HoldableData GetHoldableData();
}
