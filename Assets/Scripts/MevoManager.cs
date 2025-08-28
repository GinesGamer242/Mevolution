using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.InputSystem.Interactions;

public class MevoManager : MonoBehaviour
{
    [SerializeField]
    List<Mevo> mevoList = new List<Mevo>();
    [SerializeField]
    string mevoTag;
    [SerializeField]
    string holdableTag;

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
            foreach (var mevo in mevoList)
            {
                ChangeMevoSelection(mevo, false);
            }
            return;
        }

        if (rayHit.collider.gameObject.CompareTag(mevoTag))
        {
            AddMevoToList(rayHit.collider.gameObject);

            Mevo selectedMevo = GetMevoByGameObject(rayHit.collider.gameObject);

            if (selectedMevo.isSelected)
                ChangeMevoSelection(selectedMevo, false);
            else
            {
                if (GetSelectedMevos().Count < mevoSelectionLimit)
                {
                    ChangeMevoSelection(selectedMevo, true);
                }
            }
        }
    }

    public void OnMevoMove(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        var rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()));

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        List<Mevo> selectedMevoList = GetSelectedMevos();

        if (!rayHit || !rayHit.collider.gameObject.CompareTag(holdableTag))
        {
            for (int i = 0; i < selectedMevoList.Count; i++)
            {
                Vector3 mevoTargetPosition = CalculateMevoTargetPosition(i, mousePosition);

                selectedMevoList[i].gameObject.GetComponent<MevoController>().SetTargetPosition(mevoTargetPosition);
            }
            return;
        }

        if (selectedMevoList.Count == 1 && rayHit.collider.gameObject.CompareTag(holdableTag))
        {
            selectedMevoList[0].gameObject.GetComponent<MevoController>().SetTargetGameObject(rayHit.collider.gameObject);
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
            mevoList.Add(new Mevo(mevoList.Count, false, mevoGameObject));
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

    private Mevo GetMevoByGameObject(GameObject mevoGameObject)
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

    private void ChangeMevoSelection(Mevo mevo, bool selectedState)
    {
        mevo.isSelected = selectedState;

        if (selectedState == true)
            ChangeMevoColor(mevo, Color.green);
        else
            ChangeMevoColor(mevo, Color.white);
    }
}
