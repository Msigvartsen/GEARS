using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationListItem : MonoBehaviour
{
    private GameObject parent;
    private Button listButton;
    public Location location;
    public int index;

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
        //GameObject obj = GameObject.FindGameObjectWithTag("LocationCanvas");
        //Canvas canvas = obj.GetComponent<Canvas>();

        //CanvasManager manager = GameObject.FindGameObjectWithTag("CanvasManager").GetComponent<CanvasManager>();
        //if (manager != null)
        //    manager.ChangeCanvas(canvas.name);
        //else
        //    Debug.Log("ERROR: NO CANVAS MANAGER FOUND");
        LoadingScreen.LoadScene("ModelView");
    }
}
