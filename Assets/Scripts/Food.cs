using UnityEngine;

public class Food : IHoldable
{
    public enum FoodType
    {
        Apple
    }

    [SerializeField]
    private FoodType type;
}
