using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelInteraction : MonoBehaviour
{
    private Camera currentCamera;
    private GameObject selectedObject;

    private int selectModelThreshold = 25;
    private float moveSpeed = 0.1f;
    private float rotateSpeed = 0.3f;
    private float scaleSpeed = 0.02f;

    Vector2 beginTouchPos = new Vector2(0, 0);
    Vector2 endTouchPos = new Vector2(0, 0);
    float startTime;

    [SerializeField]
    Toggle toggleStationSearch;

    private GameObject groundPlane;

    // Start is called before the first frame update
    void Start()
    {
        currentCamera = Camera.main;
        groundPlane = GameObject.FindGameObjectWithTag("GroundPlane");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).position.y < Screen.height * 0.7)
        {
            Touch touch = Input.GetTouch(0);

            if (Input.touchCount == 1)
            {
                DetermineTouchCountOneAction(touch);
            }

            if (Input.touchCount == 2)
            {
                DetermineTouchCountTwoAction();
            }
        }
    }

    void SelectModel(Touch inTouch)
    {
        Ray ray = currentCamera.ScreenPointToRay(inTouch.position);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.activeSelf)
            {
                selectedObject = hit.transform.gameObject;
            }
        }
        else
        {
            selectedObject = null;
        }
    }

    void DetermineTouchCountOneAction(Touch inTouch)
    {
        if (groundPlane.transform.childCount > 0)
        {
            selectedObject = groundPlane.transform.GetChild(0).gameObject;

            if (inTouch.phase == TouchPhase.Began)
            {
                // Get position and time of first touch
                beginTouchPos = inTouch.position;
                startTime = Time.time;
            }

            if (inTouch.phase == TouchPhase.Moved)
            {
                if (selectedObject != null)
                {
                    // If the user interacts with a station model
                    if (toggleStationSearch.isOn)
                    {
                        MoveModel(inTouch);
                    }
                    // If the user interacts with model on the 3D model panel
                    else
                    {
                        RotateSelectedObject(inTouch);
                    }
                }
            }

            if (inTouch.phase == TouchPhase.Ended)
            {
                // Check how far the user has moved their finger since the beginning of the touchphase
                endTouchPos = inTouch.position;
                float distance = (endTouchPos - beginTouchPos).magnitude;

                // Try to select a model if the users finger has moved relatively 
                // little and make sure the user is not holding down
                if (distance < selectModelThreshold && (Time.time - startTime) < 1)
                {
                    // SelectModel(inTouch);
                }
            }
        }
    }

    void DetermineTouchCountTwoAction()
    {
        if (!toggleStationSearch.isOn)
        {
            ScaleSelectedObject(Input.GetTouch(0));
        }
    }

    void MoveModel(Touch inTouch)
    {
        // Get previous touch positions
        Vector2 touchPrevPos = inTouch.position - inTouch.deltaPosition;
        Vector2 touchPrevPosRight = new Vector2(inTouch.position.x - inTouch.deltaPosition.x, 0);
        Vector2 touchPrevPosUp = new Vector2(0, inTouch.position.y - inTouch.deltaPosition.y);

        // Get magnitudes for previous touch
        float prevTouchRightDeltaMagnitude = touchPrevPosRight.magnitude;
        float prevTouchUpDeltaMagnitude = touchPrevPosUp.magnitude;

        // Get current touch positions
        Vector2 currentPos = inTouch.position;
        Vector2 currentPosRight = new Vector2(inTouch.position.x, 0);
        Vector2 currentPosUp = new Vector2(0, inTouch.position.y);

        // Get magnitudes for current touch
        float touchRightDeltaMagnitude = currentPosRight.magnitude;
        float touchUpDeltaMagnitude = currentPosUp.magnitude;

        // Get magnitude differences between previous and current touch
        float deltaRightMagnitudeDifference = (prevTouchRightDeltaMagnitude - touchRightDeltaMagnitude) * moveSpeed;
        float deltaUpMagnitudeDifference = (prevTouchUpDeltaMagnitude - touchUpDeltaMagnitude) * moveSpeed;

        // Transform the position of selected object based on magnitude differences
        Vector3 right = currentCamera.transform.right;
        Vector3 up = currentCamera.transform.forward;
        selectedObject.transform.localPosition -= right * deltaRightMagnitudeDifference * Time.deltaTime;
        selectedObject.transform.localPosition -= up * deltaUpMagnitudeDifference * Time.deltaTime;
    }

    void RotateSelectedObject(Touch inTouch)
    {
        selectedObject = groundPlane.transform.GetChild(0).gameObject;

        // Get previous touch positions and magnitudes
        Vector2 touchPrevPos = inTouch.position - inTouch.deltaPosition;
        float prevTouchDeltaMagnitude = (touchPrevPos).magnitude;
        float touchDeltaMagnitude = (inTouch.position).magnitude;

        // Get magnitude differences between previous and current touch to determine rotation amount
        float deltaMagnitudeDifference = (prevTouchDeltaMagnitude - touchDeltaMagnitude) * rotateSpeed;

        Vector3 rotateVector = new Vector3(0, deltaMagnitudeDifference, 0);
        selectedObject.transform.Rotate(rotateVector);
    }

    void ScaleSelectedObject(Touch inTouch)
    {
        if (groundPlane.transform.childCount > 0)
        {
            selectedObject = groundPlane.transform.GetChild(0).gameObject;

            // Get previous touch positions and deltapositions
            Touch touchZero = inTouch;
            Touch touchOne = Input.GetTouch(1);
            Vector2 touchZeroPrevPos = (touchZero.position - touchZero.deltaPosition);
            Vector2 touchOnePrevPos = (touchOne.position - touchOne.deltaPosition);

            // Get magnitudes
            float prevTouchDeltaMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMagnitude = (touchZero.position - touchOne.position).magnitude;

            // Use magnitude differences between previous and current touch to determine scale amount
            float deltaMagnitudeDifference = (prevTouchDeltaMagnitude - touchDeltaMagnitude) * scaleSpeed;

            Vector3 zoomVector = new Vector3(deltaMagnitudeDifference, deltaMagnitudeDifference, deltaMagnitudeDifference);

            if (selectedObject.transform.localScale.x <= 1.5f || selectedObject.transform.localScale.x >= 0.5f)
            {
                selectedObject.transform.localScale -= zoomVector * Time.deltaTime;
            }

            // Configure max and minimum scale size
            if (selectedObject.transform.localScale.x > 1.0f)
                selectedObject.transform.localScale = new Vector3(1, 1, 1);
            if (selectedObject.transform.localScale.x < 0.5f)
                selectedObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
    }
}
