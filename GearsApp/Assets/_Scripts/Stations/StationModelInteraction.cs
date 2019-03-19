using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationModelInteraction : MonoBehaviour
{
    private GameObject model;
    private Camera currentCamera;

    private GameObject selectedObject;
    private int selectModelThreshold = 250;
    private float moveSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        currentCamera = Camera.current;
        
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
        Vector2 beginTouchPos = new Vector2(0, 0);
        Vector2 endTouchPos = new Vector2(0, 0);

        if (inTouch.phase == TouchPhase.Began)
        {
            beginTouchPos = inTouch.position;
        }

        if (inTouch.phase == TouchPhase.Moved)
        {
            if (selectedObject != null)
            {
                Vector2 touchPrevPos = inTouch.position - inTouch.deltaPosition;
                float prevTouchDeltaMagnitude = (touchPrevPos).magnitude;
                float touchDeltaMagnitude = (inTouch.position).magnitude;

                float deltaMagnitudeDifference = (prevTouchDeltaMagnitude - touchDeltaMagnitude) * moveSpeed;

                Vector3 right = currentCamera.transform.right;
                selectedObject.transform.localPosition += right * deltaMagnitudeDifference * Time.deltaTime;
            }
        }

        if (inTouch.phase == TouchPhase.Ended)
        {
            endTouchPos = inTouch.position;
            float distance = (endTouchPos - beginTouchPos).magnitude;

            // Try to select a model if the users finger has moved relatively little
            if (distance < selectModelThreshold)
            {
                SelectModel(inTouch);
            }
        }
    }
}
