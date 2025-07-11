using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class MevoController : MonoBehaviour
{
    MevoState currentState;
    Vector3 targetPosition;
    [SerializeField]
    float moveSpeed;
    [SerializeField]
    float distanceToTarget;

    public enum MevoState
    {
        Idle,
        Walking,
        Reaching
    }

    private void Update()
    {
        OnStateBehaviour();
    }

    public void OnStateBehaviour()
    {
        switch (currentState)
        {
            case MevoState.Idle:
                break;
            case MevoState.Walking:
                if (Vector3.Distance(transform.position, targetPosition) <= distanceToTarget)
                {
                    Vector3 walkDirection = (targetPosition - transform.position).normalized;

                    transform.Translate(walkDirection * moveSpeed);
                }
                else
                {
                    TransitionToIdleState();
                }
                break;
        }
    }

    public void TransitionToIdleState()
    {
        targetPosition = Vector3.zero;
        SetCurrentState(MevoState.Idle);
    }

    public void TransitionToWalkingState(Vector3 newTargetPosition)
    {
        targetPosition = newTargetPosition;
        SetCurrentState(MevoState.Walking);
    }

    public MevoState GetCurrentState()
    {
        return currentState;
    }

    public void SetCurrentState(MevoState newCurrentState)
    {
        currentState = newCurrentState;
    }
}
