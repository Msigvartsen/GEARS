using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using UnityEngine.SceneManagement;

public class TargetModels : MonoBehaviour
{
    public Camera ArCamera;

    private float zoomSpeed = 0.02f;
    private GameObject selectedObject;
    private float rotateSpeed = 0.3f;

    // Start is called before the first frame update
    void Start()
    {

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
                    SelectModel();

                // Rotate selected object
                if (touch.phase == TouchPhase.Moved)
                    RotateSelectedModel();
            }

            if ( Input.touchCount == 2)
            {
                ScaleSelectedModel();
            }

        }

    }

    public void SelectModel()
    {
        Touch touch = Input.GetTouch(0);
        Ray ray = ArCamera.ScreenPointToRay(touch.position);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.activeSelf)
            {
                selectedObject = hit.transform.gameObject;
            }
        }
    }

    public void RotateSelectedModel()
    {
        Touch touch = Input.GetTouch(0);
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

    public void ScaleSelectedModel()
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

        if (selectedObject != null)
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
}
