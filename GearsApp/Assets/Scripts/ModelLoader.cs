using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class ModelLoader : MonoBehaviour
{
    public GameObject objectList;
    public GameObject modelToSee;

    private GameObject[] modelsInList;

    // Start is called before the first frame update
    void Start()
    {
        modelsInList = new GameObject[3];

        modelsInList[0] = GameObject.CreatePrimitive(PrimitiveType.Cube);
        modelsInList[1] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        modelsInList[2] = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadCube()
    {
        modelToSee.GetComponent<MeshFilter>().mesh = modelsInList[0].GetComponent<MeshFilter>().mesh;
        modelToSee.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    public void LoadSphere()
    {
        modelToSee.GetComponent<MeshFilter>().mesh = modelsInList[1].GetComponent<MeshFilter>().mesh;
        modelToSee.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    public void LoadCylinder()
    {
        modelToSee.GetComponent<MeshFilter>().mesh = modelsInList[2].GetComponent<MeshFilter>().mesh;
        modelToSee.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }
}
