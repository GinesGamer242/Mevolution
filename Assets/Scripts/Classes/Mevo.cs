using System.Diagnostics;
using UnityEngine;

public class Mevo
{
    public int id;
    public bool isSelected;
    public bool isHolding;
    public GameObject gameObject;

    public Mevo(int newId, bool newIsSelected, bool newIsHolding, GameObject newGameObject)
    {
        id = newId;
        isSelected = newIsSelected;
        isHolding = newIsHolding;
        gameObject = newGameObject;
    }

}
