using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StationModelInteraction : MonoBehaviour
{
    private GameObject model;
    private Camera currentCamera;

    private GameObject selectedObject;
    private int selectModelThreshold = 25;
    private float moveSpeed = 0.1f;

    Vector2 beginTouchPos = new Vector2(0, 0);
    Vector2 endTouchPos = new Vector2(0, 0);
    float startTime;

    // Start is called before the first frame update
    void Start()
    {
        currentCamera = Camera.main;

        if (transform.childCount > 0)
        {
            model = transform.GetChild(0).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (Input.touchCount == 1)
            {
                DetermineTouchCountOneAction(touch);
            }

            if (Input.touchCount == 2)
            {
                ScaleSelectedObject(touch);
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

    void ScaleSelectedObject(Touch inTouch)
    {

    }

    void DetermineTouchCountOneAction(Touch inTouch)
    {
        // Determine how much the users finger has moved

        if (inTouch.phase == TouchPhase.Began)
        {
            beginTouchPos = inTouch.position;
            startTime = Time.time;
        }

        if (inTouch.phase == TouchPhase.Moved)
        {
            if (selectedObject != null)
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
                Vector2 currentPosRight = new Vector2 (inTouch.position.x , 0);
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
                SelectModel(inTouch);
            }
        }
    }
}
