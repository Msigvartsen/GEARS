using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TogglePanelScreens : MonoBehaviour
{
    public string panelTag = "PanelUI";

    [SerializeField]
    private GameObject[] panels;
    private Transform[] tabButtons;

    private void Start()
    {
        tabButtons = GetAllChildren();
        panels = GetSiblingsByTag().ToArray();
    }

    private Transform[] GetAllChildren()
    {
        Transform parent = transform;
        Transform[] children = new Transform[parent.childCount];
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            children[i] = child;
        }
        return children;
    }

    private List<GameObject> GetSiblingsByTag()
    {
        Transform parent = transform;
        List<GameObject> siblings = new List<GameObject>();
        for (int i = 0; i < parent.parent.childCount; i++)
        {
            Transform sibling = parent.parent.GetChild(i);
            if (sibling.tag == panelTag)
            {
                siblings.Add(sibling.gameObject);
            }
        }
        return siblings;
    }

    public void ToggleUIPanel()
    {
        for(int i = 0; i < tabButtons.Length; i++)
        {
            if (tabButtons[i].GetComponent<Toggle>().isOn)
            {
                panels[i].SetActive(true);
            }
            else
            {
                panels[i].SetActive(false);
            }
        }
    }
}
