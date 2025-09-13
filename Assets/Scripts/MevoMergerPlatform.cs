using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class MevoMergerPlatform : MonoBehaviour
{
    List<GameObject> mevoMergeList = new List<GameObject>();

    [SerializeField]
    string mevoTag;

    public List<GameObject> GetMevoMergeList()
    {
        return mevoMergeList;
    }

    public void ClearMevoMergeList()
    {
        mevoMergeList.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(mevoTag))
            mevoMergeList.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (mevoMergeList.Contains(collision.gameObject))
            mevoMergeList.Remove(collision.gameObject);
    }
}
