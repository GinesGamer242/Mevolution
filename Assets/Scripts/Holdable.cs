using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Holdable : MonoBehaviour, IHoldable
{
    [SerializeField]
    HoldableData holdableData;

    private void Start()
    {
        if (TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
            spriteRenderer.sprite = holdableData.sprite;
        else
            Debug.LogWarning("Couldn't find a SpriteRenderer component in holdable GameObject");
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
