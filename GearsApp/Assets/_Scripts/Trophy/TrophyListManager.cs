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
    }

    private GameObject GetListItem(int index, Trophy trophy)
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("_Prefabs/" + prefabName));
        go.transform.SetParent(transform, false);
        SetCollectedTrophies(index, trophy, go);
        go.GetComponent<TrophyListItem>().CurrentTrophy = trophy;
        
        return go;
    }

    private static void SetCollectedTrophies(int index, Trophy trophy, GameObject go)
    {
        foreach (var collectedtrophy in TrophyController.GetInstance().CollectedTrophies)
        {
            if (trophy.trophyname == collectedtrophy.trophyname)
            {
                go.GetComponent<Button>().interactable = true;
                go.GetComponent<TrophyListItem>().overlayPanel.SetActive(false);
                return;
            }
            else
            {
                go.GetComponent<Button>().interactable = false;
                go.GetComponent<TrophyListItem>().overlayPanel.SetActive(true);
            }
        }
    }
}
