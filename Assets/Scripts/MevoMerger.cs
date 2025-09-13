using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class MevoMerger : MonoBehaviour, IInteractable
{
    [SerializeField]
    Transform interactionPosition;
    [SerializeField]
    Transform spawnPosition;
    [SerializeField]
    MevoMergerPlatform mevoMergerPlatformScript;

    public Vector3 GetInteractionPosition()
    {
        return interactionPosition.position;
    }

    public bool IsSingularInteraction()
    {
        return true;
    }

    public void OnInteracted(GameObject interactionMevo)
    {
        List<MevoData> mevoMergeDataList = new List<MevoData>();

        if (mevoMergerPlatformScript.GetMevoMergeList().Count == 0)
            return;

        foreach (GameObject mevo in mevoMergerPlatformScript.GetMevoMergeList())
            if (mevo.TryGetComponent<MevoController>(out MevoController mevoController))
                mevoMergeDataList.Add(mevoController.GetMevoData());
            else
                Debug.LogWarning("MevoController component couldn't be found in Mevo GameObject " + mevo.name);

        foreach (MevoData mevoData in GameManager.instance.mevoDataList)
            if (mevoMergeDataList.All(mevoData.mevoIngredients.Contains) &&
                mevoMergeDataList.Count == mevoData.mevoIngredients.Count)
            {
                foreach (GameObject mevo in mevoMergerPlatformScript.GetMevoMergeList())
                    DestroyImmediate(mevo);

                mevoMergerPlatformScript.ClearMevoMergeList();

                Instantiate(mevoData.prefab, spawnPosition.position, Quaternion.Euler(Vector3.zero));
            }
    }
}
