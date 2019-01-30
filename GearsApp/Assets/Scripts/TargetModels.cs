using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using UnityEngine.SceneManagement;

public class TargetModels : MonoBehaviour
{
    public Text overlay;
    public Camera ArCamera;
    public int sceneIndex;

    private float zoomSpeed = 0.01f;
    private GameObject[] models = null;
    private int numberOfModels = 0;
    private GameObject selectedObject;
    private float rotateSpeed = 0.2f;

    private Color originalColor;
    private Color selectedColor;

    // Start is called before the first frame update
    void Start()
    {
        models = GameObject.FindGameObjectsWithTag("Model");
        selectedColor = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (Input.touchCount == 1)
            {
                // Touch to select an object
                if (touch.phase == TouchPhase.Began)
                {
                    Ray ray = ArCamera.ScreenPointToRay(touch.position);
                    RaycastHit hit = new RaycastHit();

                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.gameObject.CompareTag("Model"))
                        {
                            selectedObject = hit.transform.gameObject;
                            if (selectedObject.GetComponent<Renderer>().material.color != selectedColor)
                                originalColor = selectedObject.GetComponent<Renderer>().material.color;

                            changeColorOnSelected();
                        }
                    }
                }

                // Rotate selected object
                if (touch.phase == TouchPhase.Moved)
                {
                    Vector2 touchPrevPos = touch.position - touch.deltaPosition;
                    float prevTouchDeltaMagnitude = (touchPrevPos).magnitude;
                    float touchDeltaMagnitude = (touch.position).magnitude;

                    float deltaMagnitudeDifference = (prevTouchDeltaMagnitude - touchDeltaMagnitude) * rotateSpeed;

                    Vector3 rotateVector = new Vector3(0, deltaMagnitudeDifference, 0);

                    if (selectedObject != null)
                    {
                        selectedObject.transform.Rotate(rotateVector);
                    }
                }
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

                if (selectedObject != null && selectedObject.GetComponent<Renderer>().enabled)
                {
                    if (selectedObject.transform.localScale.x <= 1.0f || selectedObject.transform.localScale.x >= 0.5f)
                        selectedObject.transform.localScale -= zoomVector * Time.deltaTime;

                    // Configure max and minimum scale size
                    if (selectedObject.transform.localScale.x > 1.0f)
                        selectedObject.transform.localScale = new Vector3(1, 1, 1);
                    if (selectedObject.transform.localScale.x < 0.5f)
                        selectedObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                }
            }

            if (Input.touchCount == 3)
            {
                LoadSceneByIndex(sceneIndex);
            }
        }

    }

    public void LoadSceneByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void changeColorOnSelected()
    {
        foreach (var obj in models)
        {
            if (obj.transform == selectedObject.transform)
                obj.GetComponent<Renderer>().material.color = selectedColor;
            else
                obj.GetComponent<Renderer>().material.color = originalColor;
        }

    }
}
