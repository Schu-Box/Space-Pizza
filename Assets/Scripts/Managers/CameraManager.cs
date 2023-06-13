using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform trackedObject;
    public float followSpeed = 2f;
    public float followThresholdRadius = 20f;

    private float _cameraZDistance = -10f;

    private void Start()
    {
        _cameraZDistance = transform.position.z;
    }

    void Update()
    {
        if (trackedObject == null) return;
        
        if (Vector2.Distance(transform.position, trackedObject.position) > followThresholdRadius)
        {
            Vector3 newTargetPosition = new Vector3(trackedObject.position.x, trackedObject.position.y, _cameraZDistance);
            transform.position = Vector3.Lerp(transform.position, newTargetPosition, followSpeed * Time.deltaTime);
        }
    }
}
