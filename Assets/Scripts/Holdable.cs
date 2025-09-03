using UnityEngine;

public class Holdable : MonoBehaviour, IHoldable
{
    [SerializeField]
    HoldableData holdableData;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = holdableData.sprite;
    }

    public HoldableData GetHoldableData()
    {
        return holdableData;
    }

    public Vector3 GetHoldablePosition()
    {
        return transform.position;
    }

    public GameObject GetHoldableGameObject()
    {
        return this.gameObject;
    }
}
