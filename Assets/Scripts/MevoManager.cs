using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.InputSystem.Interactions;

public class MevoManager : MonoBehaviour
{
    public static MevoManager instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //////////////////////////////////////////

    [SerializeField]
    List<Mevo> mevoList = new List<Mevo>();
    [SerializeField]
    string mevoTag;

    [Header("Group Settings")]
    [SerializeField]
    int mevoSelectionLimit;
    [SerializeField]
    float mevoSeparation;

    public void OnMevoSelect(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        var rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()));

        if (!rayHit || !rayHit.collider.gameObject.CompareTag(mevoTag))
        {
            foreach (Mevo mevo in mevoList)
            {
                ChangeMevoSelectionState(mevo, false);
            }
            return;
        }
        else
        {
            AddMevoToList(rayHit.collider.gameObject);

            Mevo selectedMevo = GetMevoByGameObject(rayHit.collider.gameObject);

            if (selectedMevo.isSelected)
                ChangeMevoSelectionState(selectedMevo, false);
            else
            {
                if (GetSelectedMevos().Count < mevoSelectionLimit)
                {
                    ChangeMevoSelectionState(selectedMevo, true);
                }
            }
        }
    }

    public void OnMevoMove(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        var rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()));

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        List<Mevo> selectedMevoList = GetSelectedMevos();

        if (!rayHit)
        {
            AssignMevosTargetPositions(selectedMevoList, mousePosition);
            return;
        }

        if (rayHit.collider.gameObject.TryGetComponent<IInteractable>(out IInteractable interactable))
        {
            if (interactable.IsSingularInteraction())
            {
                selectedMevoList[0].gameObject.GetComponent<MevoController>().SetTargetInteractable(interactable);
            }
            else
            {
                for (int i = 0; i < selectedMevoList.Count; i++)
                    selectedMevoList[i].gameObject.GetComponent<MevoController>().SetTargetInteractable(interactable);
            }
            AssignMevosTargetPositions(selectedMevoList, interactable.GetInteractionPosition());

            return;
        }
        else if (rayHit.collider.gameObject.TryGetComponent<IHoldable>(out IHoldable holdable))
        {
            Mevo firstFreeMevo = null;
            int firstFreeMevoIteration = 0;
            bool firstFreeMevoFound = false;

            for (int i = 0; i < selectedMevoList.Count; i++)
                if (!selectedMevoList[i].isHolding && !firstFreeMevoFound)
                {
                    firstFreeMevo = selectedMevoList[i];
                    firstFreeMevoIteration = i;
                    firstFreeMevoFound = true;
                }

            if (firstFreeMevo != null)
            {
                firstFreeMevo.gameObject.GetComponent<MevoController>().SetTargetHoldable(holdable);

                Mevo temp = selectedMevoList[0];
                selectedMevoList[0] = firstFreeMevo;
                selectedMevoList[firstFreeMevoIteration] = temp;

                AssignMevosTargetPositions(selectedMevoList, holdable.GetHoldablePosition());
            }
            else
                AssignMevosTargetPositions(selectedMevoList, mousePosition);

            return;
        }
        else
        {
            AssignMevosTargetPositions(selectedMevoList, mousePosition);
            return;
        }
        
    }

      ///////////////////////
     // UTILITY FUNCTIONS //
    ///////////////////////

    private void AddMevoToList(GameObject mevoGameObject)
    {
        //Check if the Mevo exists already in list
        bool isMevoInList = false;

        for (int i = 0; i < mevoList.Count; i++)
            if (mevoList[i].gameObject == mevoGameObject)
                isMevoInList = true;
        
        if (!isMevoInList)
            mevoList.Add(new Mevo(mevoList.Count, false, false, mevoGameObject));
    }

    private List<Mevo> GetSelectedMevos()
    {
        List<Mevo> selectedMevoList = new List<Mevo>();

        for (int i = 0; i < mevoList.Count; i++)
        {
            if (mevoList[i].isSelected == true)
                selectedMevoList.Add(mevoList[i]);
        }

        return selectedMevoList;
    }

    public Mevo GetMevoByGameObject(GameObject mevoGameObject)
    {
        Mevo mevoToReturn = null;
        for (int i = 0; i < mevoList.Count; i++)
        {
            if (mevoList[i].gameObject == mevoGameObject) return mevoList[i];
        }

        if (mevoToReturn == null) Debug.LogError("Mevo GameObject could not be found in Mevo List.");
        return mevoToReturn;
    }

    private void ChangeMevoColor(Mevo mevoToChange, Color newColor)
    {
        mevoToChange.gameObject.GetComponent<SpriteRenderer>().color = newColor;
    }


    private Vector3 CalculateMevoTargetPosition(int mevoNumber, Vector3 originalTargetPosition)
    {
        if (mevoNumber == 0)
            return originalTargetPosition;

        Vector3 newTargetPosition;

        float angleInRadians = ((2 * Mathf.PI) / (mevoSelectionLimit - 1)) * mevoNumber;
        
        float x = originalTargetPosition.x + mevoSeparation * Mathf.Cos(angleInRadians);
        float y = originalTargetPosition.y + mevoSeparation * Mathf.Sin(angleInRadians);

        newTargetPosition = new Vector3(x, y, 0);

        return newTargetPosition;
    }

    private void AssignMevosTargetPositions(List<Mevo> mevosToAssign, Vector3 originalTargetPosition)
    {
        for (int i = 0; i < mevosToAssign.Count; i++)
        {
            Vector3 mevoTargetPosition = CalculateMevoTargetPosition(i, originalTargetPosition);

            mevosToAssign[i].gameObject.GetComponent<MevoController>().SetTargetPosition(mevoTargetPosition);
        }
        return;
    }

    private void ChangeMevoSelectionState(Mevo mevo, bool selectionState)
    {
        mevo.isSelected = selectionState;

        if (selectionState == true)
            ChangeMevoColor(mevo, Color.green);
        else
            ChangeMevoColor(mevo, Color.white);
    }

    public void ChangeMevoHoldState(Mevo mevo, bool holdState)
    {
        mevo.isHolding = holdState;
    }
}
