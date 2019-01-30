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
        print(modelsInList.Length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadCube()
    {
        //Mesh mesh = modelsInList[0].GetComponent<MeshFilter>().sharedMesh;
        //Mesh mesh2 = Instantiate(mesh);
        //modelToSee.GetComponent<MeshFilter>().sharedMesh = mesh2;

        //Instantiate(modelsInList[0]);
        //modelToSee = modelsInList[0];
        //modelToSee.GetComponent<MeshFilter>().mesh = modelsInList[0].GetComponent<MeshFilter>().mesh;

        modelToSee = modelsInList[0];

        modelToSee.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        print(modelToSee.transform.position);
    }

    public void LoadSphere()
    {
        modelToSee = modelsInList[1];
        //modelToSee.GetComponent<MeshFilter>().mesh = modelsInList[1].GetComponent<MeshFilter>().mesh;
        modelToSee.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    public void LoadCylinder()
    {
        modelToSee = modelsInList[2];
        //modelToSee.GetComponent<MeshFilter>().mesh = modelsInList[2].GetComponent<MeshFilter>().mesh;
        modelToSee.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }
}
