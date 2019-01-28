using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class TargetModels : MonoBehaviour
{
    public Text overlay;

    private float zoomSpeed = 0.01f;
    private GameObject[] models = null;
    private int numberOfModels = 0;

    // Start is called before the first frame update
    void Start()
    {
        models = GameObject.FindGameObjectsWithTag("Model");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.touchCount == 1)
            {

            }

            if ( Input.touchCount == 2)
            {
                // Handle scaling of models that are visible
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = (touchZero.position - touchZero.deltaPosition);
                Vector2 touchOnePrevPos = (touchOne.position - touchOne.deltaPosition);

                float prevTouchDeltaMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMagnitude = (touchZero.position - touchOne.position).magnitude;

                float deltaMagnitudeDifference = (prevTouchDeltaMagnitude - touchDeltaMagnitude) * zoomSpeed;

                Vector3 zoomVector = new Vector3(deltaMagnitudeDifference, deltaMagnitudeDifference, deltaMagnitudeDifference);

                overlay.text = deltaMagnitudeDifference.ToString();

                for (int i = 0; i < models.Length; i++)
                {
                    // Only scale visible models
                    if (models[i].GetComponent<Renderer>().enabled)
                    {
                        if (models[i].transform.localScale.x <= 1.0f || models[i].transform.localScale.x >= 0.5f)
                            models[i].transform.localScale -= zoomVector * Time.deltaTime;

                        // Configure max and minimum scale size
                        if (models[i].transform.localScale.x > 1.0f)
                            models[i].transform.localScale = new Vector3(1, 1, 1);
                        if (models[i].transform.localScale.x < 0.5f)
                            models[i].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    }
                }
            }
        }

    }
}
