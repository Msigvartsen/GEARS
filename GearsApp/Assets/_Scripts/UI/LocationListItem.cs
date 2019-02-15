using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationListItem : MonoBehaviour
{
    private GameObject parent;
    private Button listButton;
    public Location location;


    private void Start()
    {
        Init();
    }

    private void Init()
    {
        GetComponentInChildren<Text>().text = location.name;
        listButton = GetComponentInChildren<Button>();
        listButton.onClick.AddListener(OpenLocationTab);
    }

    private void OpenLocationTab()
    {
        Debug.Log("CLICK BUTTON");
        //GameObject go = Instantiate(Resources.Load<GameObject>("_Prefabs/LocationCanvas"));
        GameObject obj = GameObject.FindGameObjectWithTag("LocationCanvas");
        Canvas canvas = obj.GetComponent<Canvas>();

        //canvas.enabled = true;
        CanvasManager manager = GameObject.FindGameObjectWithTag("CanvasManager").GetComponent<CanvasManager>();
        if (manager != null)
            manager.ChangeCanvas(canvas.name);
        else
            Debug.Log("ERROR: NO CANVAS MANAGER FOUND");
    }
}
