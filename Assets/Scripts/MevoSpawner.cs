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
        Debug.Log("uwu");

        MevoController interactionMevoController = null;

        if (interactionMevoGameObject.TryGetComponent<MevoController>(out MevoController outMevoController))
            interactionMevoController = outMevoController;
        else
        {
            Debug.LogWarning("MevoController component wasn't found in Mevo " + interactionMevoGameObject.name);
            return;
        }

        HoldableData interactionMevoHoldableData = interactionMevoController.GetCurrentHoldableData();

        if (interactionMevoHoldableData == null)
            return;

        switch(interactionMevoHoldableData.holdableCategory)
        {
            case HoldableCategory.Food:
                Instantiate(interactionMevoHoldableData.spawnMevoData.prefab, spawnPoint.position, Quaternion.Euler(Vector3.zero));
                interactionMevoController.RemoveCurrentHoldable();
                break;
        }
    }
}
