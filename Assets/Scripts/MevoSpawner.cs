using UnityEngine;

public class MevoSpawner : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField]
    Transform interactionPoint;
    [SerializeField]
    Transform spawnPoint;

    public Vector3 GetInteractionPosition()
    {
        return interactionPoint.position;
    }

    public bool IsSingularInteraction()
    {
        return false;
    }

    public void OnInteracted(GameObject interactionMevoGameObject)
    {
        MevoController interactionMevoController = null;

        if (interactionMevoGameObject.TryGetComponent<MevoController>(out MevoController outMevoController))
            interactionMevoController = outMevoController;
        else
        {
            Debug.LogError("MevoController wasn't found");
            return;
        }

        HoldableData interactionMevoHoldableData = interactionMevoController.GetCurrentHoldableData();

        switch(interactionMevoHoldableData.holdableCategory)
        {
            case HoldableCategory.Food:
                Instantiate(interactionMevoHoldableData.spawnMevoData.prefab, spawnPoint.position, Quaternion.Euler(Vector3.zero));
                interactionMevoController.RemoveCurrentHoldable();
                break;
        }
    }
}
