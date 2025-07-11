using UnityEngine;

public class Mevo
{
    public int id;
    public bool isSelected;
    public GameObject gameObject;

    public Mevo(int newId, bool newIsSelected, GameObject newGameObject)
    {
        id = newId;
        isSelected = newIsSelected;
        gameObject = newGameObject;
    }

}
