using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Managers;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float followSpeed = 2f;
    public float followThresholdRadius = 20f;

    private float _cameraZDistance = -10f;

    private void Start()
    {
        _cameraZDistance = transform.position.z;
    }

    void LateUpdate()
    {
        Ship trackedShip = ShipManager.Current.PlayerShip;
        if (trackedShip == null) return;
        
        if (Vector2.Distance(transform.position, trackedShip.RootTransform.position) > followThresholdRadius)
        {
            Vector3 newTargetPosition = new Vector3(trackedShip.RootTransform.position.x, trackedShip.RootTransform.position.y, _cameraZDistance);
            transform.position = Vector3.Lerp(transform.position, newTargetPosition, followSpeed * Time.deltaTime);
        }
    }
}
