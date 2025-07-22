using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class MevoController : MonoBehaviour
{
    public MevoState currentMevoState;
    public Vector3 targetPosition;
    public GameObject targetObject;
    public GameObject holdingObject;

    [Header ("References")]
    [SerializeField]
    Transform holdPosition;

    [Header ("Attributes")]
    [SerializeField]
    float moveSpeed;
    [SerializeField]
    float distanceToPosition;
    [SerializeField]
    float distanceToObject;
    [SerializeField]
    Vector3 nullVectorValue;

    public enum MevoState
    {
        Idle,
        Walking,
        Reaching
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        OnStateBehaviour(dt);
    }

    public void OnStateBehaviour(float dt)
    {
        if (targetObject != null)
        {
            targetObject.transform.position = holdPosition.position;
        }

        switch (currentMevoState)
        {
            case MevoState.Idle:
                break;

            case MevoState.Walking:
                if (targetObject != null)
                {
                    if (Vector3.Distance(transform.position, targetObject.transform.position) > distanceToObject)
                    {
                        Vector3 walkDirection = (targetObject.transform.position - transform.position).normalized;

                        transform.Translate(walkDirection * moveSpeed * dt);
                    }
                    else
                    {
                        targetObject.transform.position = holdPosition.position;
                        targetObject = null;
                        targetPosition = nullVectorValue;
                    }
                }
                else
                {
                    if (Vector3.Distance(transform.position, targetPosition) > distanceToPosition)
                    {
                        Vector3 walkDirection = (targetPosition - transform.position).normalized;

                        transform.Translate(walkDirection * moveSpeed * dt);
                    }
                    else
                    {
                        targetPosition = nullVectorValue;
                    }
                }
                break;

            case MevoState.Reaching:
                if (Vector3.Distance(transform.position, targetObject.transform.position) > distanceToObject)
                {
                    Vector3 walkDirection = (targetObject.transform.position - transform.position).normalized;

                    transform.Translate(walkDirection * moveSpeed * dt);
                }
                else
                {

                }
                break;
        }

        CheckTransitionConditions();
    }

    private void CheckTransitionConditions()
    {
        if (targetPosition != nullVectorValue || targetObject != null)
            TransitionToState(MevoState.Walking);
        else
            TransitionToState(MevoState.Idle);
    }

    private void TransitionToState(MevoState newState)
    {
        currentMevoState = newState;
    }

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
        targetObject = newTargetObject;
    }
}
