using UnityEngine;
using UnityEngine.Events;

public interface IInteractable
{
    public void OnInteracted(GameObject interactionMevo);

    public Vector3 GetInteractionPosition();

    public bool IsSingularInteraction();
}
