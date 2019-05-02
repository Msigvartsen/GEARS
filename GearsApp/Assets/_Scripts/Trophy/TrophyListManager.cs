using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            itemList.Add(GetListItem(i, trophylist));
        }
    }

    private GameObject GetListItem(int index, Trophy[] trophies)
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("_Prefabs/" + prefabName));
        go.transform.SetParent(transform, false);
        //go.GetComponent<TrophyListItem>().trophy = trophylist[index];
        Debug.Log("Adding Trophy" + trophies[index].name);
        return go;
    }


}
