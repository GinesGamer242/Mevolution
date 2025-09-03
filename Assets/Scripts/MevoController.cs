using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.LowLevel;

public class MevoController : MonoBehaviour
{
    public MevoState currentMevoState;
    Vector3 targetPosition;
    public IHoldable targetHoldable;
    IInteractable targetInteractable;
    HoldableData currentHoldableData;

    [SerializeField]
    MevoData mevoData;

    [Header ("References")]
    [SerializeField]
    SpriteRenderer holdSpriteRenderer;

    [Header ("Attributes")]
    [SerializeField]
    [Range(0.0f, 1.0f)]
    float holdSpeedPenalization;
    [SerializeField]
    float distanceToPosition;
    [SerializeField]
    float distanceToHoldable;
    [SerializeField]
    Vector3 nullVectorValue;

    public enum MevoState
    {
        Idle,
        Walking
    }

    private void Start()
    {
        targetPosition = nullVectorValue;
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        OnStateBehaviour(dt);
    }

    public void OnStateBehaviour(float dt)
    {
        switch (currentMevoState)
        {
            case MevoState.Idle:
                break;

            case MevoState.Walking:
                if (Vector3.Distance(transform.position, targetPosition) > distanceToPosition)
                {
                    Vector3 walkDirection = (targetPosition - transform.position).normalized;
                    if (currentHoldableData == null)
                        transform.Translate(walkDirection * mevoData.moveSpeed * dt);
                    else
                        transform.Translate(walkDirection * mevoData.moveSpeed * (1 - holdSpeedPenalization) * dt);
                }
                else
                {
                    if (targetInteractable != null)
                    {
                        targetInteractable.OnInteracted(this.gameObject);
                        targetInteractable = null;
                    }
                    else if (targetHoldable != null)
                    {
                        PickHoldableUp(targetHoldable);
                    }

                    targetPosition = nullVectorValue;
                }
                break;
        }

        CheckTransitionConditions();
    }

    private void CheckTransitionConditions()
    {
        if (targetPosition != nullVectorValue)
            TransitionToState(MevoState.Walking);
        else
            TransitionToState(MevoState.Idle);
    }

    private void TransitionToState(MevoState newState)
    {
        currentMevoState = newState;
    }

    public void RemoveCurrentHoldable()
    {
        currentHoldableData = null;
        MevoManager.instance.ChangeMevoHoldState(MevoManager.instance.GetMevoByGameObject(this.gameObject), false);
        holdSpriteRenderer.sprite = null;
    }

    private void PickHoldableUp(IHoldable holdable)
    {
        Debug.Log("Picking up");
        Destroy(holdable.GetHoldableGameObject());
        targetHoldable = null;
        currentHoldableData = holdable.GetHoldableData();

        MevoManager.instance.ChangeMevoHoldState(MevoManager.instance.GetMevoByGameObject(this.gameObject), true);

        holdSpriteRenderer.sprite = currentHoldableData.sprite;
    }

    private void PutHoldableDown()
    {
        Debug.Log("Putting down");
        Instantiate(currentHoldableData.prefab, transform.position, Quaternion.Euler(Vector3.zero));
        currentHoldableData = null;

        MevoManager.instance.ChangeMevoHoldState(MevoManager.instance.GetMevoByGameObject(this.gameObject), false);

        holdSpriteRenderer.sprite = null;
    }

    public void ResetTargets()
    {
        targetInteractable = null;
        targetHoldable = null;
    }

      /////////////////////////
     /// GETTERS Y SETTERS ///
    /////////////////////////
    
    private MevoState GetCurrentState()
    {
        return currentMevoState;
    }

    public void SetTargetPosition(Vector3 newTargetPosition)
    {
        targetPosition = newTargetPosition;
    }

    public void SetTargetHoldable(IHoldable newTargetHoldable)
    {
        targetHoldable = newTargetHoldable;
        targetInteractable = null;
    }

    public void SetTargetInteractable(IInteractable newTargetInteractable)
    {
        targetInteractable = newTargetInteractable;
        targetHoldable = null;
    }

    public HoldableData GetCurrentHoldableData()
    {
        return currentHoldableData;
    }
}
