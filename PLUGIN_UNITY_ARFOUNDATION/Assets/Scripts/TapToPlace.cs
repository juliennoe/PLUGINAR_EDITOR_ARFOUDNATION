using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


[RequireComponent(typeof(ARRaycastManager))]
public class TapToPlace : MonoBehaviour
{
    public GameObject _objectToPlace;

    private GameObject _spawObject;
    private ARRaycastManager _arRaycastManager;
    private Vector2 _touchPosition;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    // Start is called before the first frame update
    private void Awake()
    {
        _arRaycastManager = GetComponent<ARRaycastManager>();
    }


    bool TryGetTouchPosition(out Vector2 _touchPosition)
    {
      
        if (Input.touchCount > 0)
        {
            _touchPosition = Input.GetTouch(0).position;
            return true;
        }

        _touchPosition = default;
        return false;
    }

    // Update is called once per frame
    void Update()
    {   

        
        if (!TryGetTouchPosition(out Vector2 _touchPosition))
            return;

        if(_arRaycastManager.Raycast(_touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;

            if(_spawObject == null)
            {
                _spawObject = Instantiate(_objectToPlace, hitPose.position, hitPose.rotation);
            }
            else
            {
                _spawObject.transform.position = hitPose.position;
            }
        }
    }

}
