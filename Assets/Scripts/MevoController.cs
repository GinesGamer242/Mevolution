using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class MevoController : MonoBehaviour
{
    public MevoState currentMevoState;
    public Vector3 targetPosition;
    public GameObject targetHoldableGameObject;
    public HoldableData currentHoldableData;

    [Header ("References")]
    [SerializeField]
    SpriteRenderer holdSpriteRenderer;

    [Header ("Attributes")]
    [SerializeField]
    float moveSpeed;
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
                        transform.Translate(walkDirection * moveSpeed * dt);
                    else
                        transform.Translate(walkDirection * moveSpeed * (1 - holdSpeedPenalization) * dt);

                    if (targetHoldableGameObject != null && currentHoldableData == null)
                    {
                        if (Vector3.Distance(transform.position, targetPosition) <= distanceToHoldable)
                        {
                            Debug.Log("Distance to pick up: " + distanceToHoldable);
                            if (targetHoldableGameObject.TryGetComponent<IHoldable>(out IHoldable holdable))
                                PickHoldableUp(holdable.GetHoldableData());

                            targetPosition = nullVectorValue;
                        }
                    }
                }
                else
                {
                    if (currentHoldableData != null)
                    {
                        PutHoldableDown();
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

    private void PickHoldableUp(HoldableData holdable)
    {
        Debug.Log("Picking up");
        Destroy(targetHoldableGameObject);
        targetHoldableGameObject = null;
        currentHoldableData = holdable;

        holdSpriteRenderer.sprite = currentHoldableData.sprite;
    }

    private void PutHoldableDown()
    {
        Debug.Log("Putting down");
        Instantiate(currentHoldableData.prefab, transform.position, Quaternion.Euler(Vector3.zero));
        currentHoldableData = null;

        holdSpriteRenderer.sprite = null;
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
        targetHoldableGameObject = null;    
    }

    public void SetTargetGameObject(GameObject newTargetGameObject)
    {
        targetHoldableGameObject = newTargetGameObject;
        targetPosition = newTargetGameObject.transform.position;
    }
}
