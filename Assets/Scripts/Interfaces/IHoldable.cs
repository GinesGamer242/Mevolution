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
    Pineapple,
    Watermelon,
    Meat,
    Fish,
    Flower,
    Starfruit
}

public enum MaterialType
{
    None,
    Stone
}

public interface IHoldable
{
    public HoldableData GetHoldableData();

    public Vector3 GetHoldablePosition();

    public GameObject GetHoldableGameObject();
}
