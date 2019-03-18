using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationModel : MonoBehaviour
{
    [Header("Resources")]
    public GameObject model;
    public Material material;

    private GameObject child;

    // Start is called before the first frame update
    void Start()
    {
        if (model != null)
        {
            if (transform.childCount > 0)
            {
                child = transform.GetChild(0).gameObject;
                Destroy(child);
            }

            GameObject go = Instantiate(model, transform);

            if (material != null)
            {
                var matComponents = go.GetComponentsInChildren<Renderer>();
                foreach (var item in matComponents)
                {
                    item.material = material;
                }
            }

        }
    }
}
