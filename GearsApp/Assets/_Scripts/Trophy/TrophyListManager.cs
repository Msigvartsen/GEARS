using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrophyListManager : MonoBehaviour
{
    private string prefabName = "TrophyItem";
    private List<GameObject> itemList = new List<GameObject>();

    void Start()
    {
        var trophylist = TrophyController.GetInstance().TrophyList.ToArray();
        int length = trophylist.Length;
        for (int i = 0; i < length; i++)
        {
            itemList.Add(GetListItem(i, trophylist[i]));
        }
        //Trophy t = itemList[1].GetComponent<TrophyListItem>().CurrentTrophy;
        //Debug.Log("Test: Adding trophy to collected trophy list " + t.trophyname);
        //TrophyController.GetInstance().CallAddCollectedTrophy(t);
    }

    private GameObject GetListItem(int index, Trophy trophy)
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("_Prefabs/" + prefabName));
        go.transform.SetParent(transform, false);
        SetCollectedTrophies(trophy, go);
        go.GetComponent<TrophyListItem>().CurrentTrophy = trophy;

        return go;
    }

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

    public void UpdateTrophyList(string trophyname)
    {
        foreach (var item in itemList)
        {
            var trophy = item.GetComponent<TrophyListItem>().CurrentTrophy;
            if (trophy.trophyname == trophyname)
            {
                SetCollectedTrophies(trophy, item);
            }
        }
    }
}
