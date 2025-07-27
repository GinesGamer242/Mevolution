using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class MevoController : MonoBehaviour
{
    public MevoState currentMevoState;
    public Vector3 targetPosition;
    public GameObject targetHoldable;
    public IHoldable currentHoldable;

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
                    if (currentHoldable == null)
                        transform.Translate(walkDirection * moveSpeed * dt);
                    else
                        transform.Translate(walkDirection * moveSpeed * (1 - holdSpeedPenalization) * dt);

                    if (targetHoldable != null)
                    {
                        if (Vector3.Distance(transform.position, targetPosition) <= distanceToHoldable)
                        {
                            if (targetHoldable.TryGetComponent<IHoldable>(out IHoldable holdable))
                                PickHoldableUp(holdable);

                            targetPosition = nullVectorValue;
                        }
                    }
                }
                else
                {
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

    private void PickHoldableUp(IHoldable holdable)
    {
        targetHoldable = null;
        currentHoldable = holdable;

        switch (currentHoldable.GetHoldableCategory())
        {
            case HoldableCategory.Food:
                foreach (Food foodItem in GameManager.instance.foodList)
                {
                    if (foodItem.GetFoodType().Equals(currentHoldable.GetFoodType()))
                    {
                        holdSpriteRenderer.sprite = foodItem.GetSprite();
                    }
                }
                break;

            case HoldableCategory.Object:
                foreach (Object objectItem in GameManager.instance.objectList)
                {
                    if (objectItem.GetObjectType().Equals(currentHoldable.GetObjectType()))
                    {
                        holdSpriteRenderer.sprite = objectItem.GetSprite();
                    }
                }
                break;
        }
    }

    private void PutHoldableDown()
    {
        currentHoldable = null;
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

    public void SetTargetObject(GameObject newTargetObject)
    {
        targetHoldable = newTargetObject;
        targetPosition = newTargetObject.transform.position;
    }
}
