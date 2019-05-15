using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script creates and handles TrophyList prefabs in Unity.  
/// </summary>
public class TrophyListManager : MonoBehaviour
{
    private string prefabName = "TrophyItem";
    private List<GameObject> itemList = new List<GameObject>();

    /// <summary>
    /// Ran before the first frame.
    /// Creates prefabs objects from all trophies before adding them to a list. 
    /// </summary>
    void Start()
    {
        var trophylist = TrophyController.GetInstance().TrophyList;
        int length = trophylist.Count;

        for (int i = 0; i < length; i++)
        {
            itemList.Add(GetListItem(trophylist[i]));
        }
    }

    /// <summary>
    /// Creates the prefab and sets parent. 
    /// Checks if the trophy is collected or not.
    /// </summary>
    /// <param name="trophy"></param>
    /// <returns></returns>
    private GameObject GetListItem(Trophy trophy)
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("_Prefabs/" + prefabName));
        go.transform.SetParent(transform, false);
        go.GetComponent<TrophyListItem>().CurrentTrophy = trophy;
        SetCollectedTrophies(trophy, go);

        return go;
    }

    /// <summary>
    /// Check if Trophy is collected or not. Updates overlay and button.
    /// </summary>
    /// <param name="trophy">Trophy to check</param>
    /// <param name="go">Gameobject to update</param>
    private static void SetCollectedTrophies(Trophy trophy, GameObject go)
    {
        var trophies = TrophyController.GetInstance().CollectedTrophies;

        if (trophies == null)
            return;
        
        foreach (var collectedtrophy in trophies)
        {
            if (trophy.trophyname == collectedtrophy.trophyname)
            {
                go.GetComponent<Button>().interactable = true;
                go.GetComponent<TrophyListItem>().overlayPanel.SetActive(false);
                return;
            }
        }
    }

    /// <summary>
    /// Update Collected trophies. 
    /// </summary>
    /// <param name="trophyname">Name of trophy to add</param>
    public void UpdateTrophyList(Trophy collectedTrophy)
    {
        foreach (var item in itemList)
        {
            var trophy = item.GetComponent<TrophyListItem>().CurrentTrophy;
            if (trophy.trophyname == collectedTrophy.trophyname)
            {
                SetCollectedTrophies(collectedTrophy, item);
            }
        }
    }
}
