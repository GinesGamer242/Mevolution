using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Unity.VisualScripting;

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
        if (!context.started) return;

        var rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()));

        if (!rayHit.collider) return;
        if (!rayHit.collider.gameObject.CompareTag(mevoTag)) return;

        AddMevoToList(rayHit.collider.gameObject);

        Mevo selectedMevo = GetMevoByGameObject(rayHit.collider.gameObject);

        if (selectedMevo.isSelected)
        {
            selectedMevo.isSelected = false;
            ChangeMevoColor(selectedMevo, Color.white);
        }
        else
        {
            if (GetSelectedMevos().Count < mevoSelectionLimit)
            {
                selectedMevo.isSelected = true;
                ChangeMevoColor(selectedMevo, Color.green);
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

        for (int i = 0; i < selectedMevoList.Count; i++)
        {
            Vector3 mevoTargetPosition = CalculateMevoTargetPosition(i, mousePosition);

            selectedMevoList[i].gameObject.GetComponent<MevoController>().SetTargetPosition(mevoTargetPosition);
        }

        if (selectedMevoList.Count == 1 && rayHit.collider.gameObject.CompareTag(holdableTag))
        {
            selectedMevoList[0].gameObject.GetComponent<MevoController>().SetTargetObject(rayHit.collider.gameObject);
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
            if (mevoList[i].gameObject == mevoGameObject) isMevoInList = true;
        
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
}
