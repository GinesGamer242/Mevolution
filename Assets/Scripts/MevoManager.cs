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

    private void Start()
    {
        
    }

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
            selectedMevo.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            selectedMevo.isSelected = true;
            selectedMevo.gameObject.transform.localScale = new Vector3(2f, 2f, 2f);
        }
    }

    public void OnMevoMove(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        var rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()));

        if (!rayHit.collider)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;

            List<Mevo> selectedMevoList = GetSelectedMevos();

            for (int i = 0; i < selectedMevoList.Count; i++)
                selectedMevoList[i].gameObject.GetComponent<MevoController>().TransitionToWalkingState(mousePosition);
        }

        
    }

      ///////////////////////
     // UTILITY FUNCTIONS //
    ///////////////////////

    private void AddMevoToList(GameObject mevoGameObject)
    {
        if (mevoGameObject.CompareTag(mevoTag)) Debug.Log("The GameObject clicked wasn't a Mevo!");

        //Check if the Mevo exists already in list
        bool isMevoInList = false;
        for (int i = 0; i < mevoList.Count; i++)
        {
            if (mevoList[i].gameObject == mevoGameObject) isMevoInList = true;
        }
        
        if (!isMevoInList)
        {
            mevoList.Add(new Mevo(mevoList.Count, false, mevoGameObject));
        }
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
}
