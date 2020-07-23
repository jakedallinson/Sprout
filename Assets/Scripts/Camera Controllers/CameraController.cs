using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera MainCamera;
    public Camera OverheadCamera;

    public Vector3 MainCameraStartPosition;
    public Vector3 OverheadCameraStartPosition;
    public Transform target;
    public Vector3 offset;

    public float heightOffset = 6f;
    public float smoothSpeed = 0.125f;
    public float zoom = 10f;

    void Start()
    {
        MainCamera.transform.position = MainCameraStartPosition;
        OverheadCamera.transform.position = OverheadCameraStartPosition;
        MainCamera.transform.LookAt(target.position);
        ShowMain();

        // event triggers
        GameEvents.current.OnPowerUpStarted += ShowOverhead;
        GameEvents.current.OnPowerUpFinished += ShowMain;
    }

    void LateUpdate()
    {
        // MainCamera.transform.LookAt(target.position);
        if (SpawnController.IsGameOver()) {
            // game over, set camera to start position
            Vector3 desiredPos = MainCameraStartPosition;
            Vector3 smoothedPos = Vector3.Lerp(MainCamera.transform.position, desiredPos, smoothSpeed);

            // set position of camera to now smoothed position
            MainCamera.transform.position = smoothedPos;
        } else {
            // height offset allows for HUD at top of screen
            Vector3 desiredPos = target.position - offset * zoom + Vector3.up * heightOffset;
            Vector3 smoothedPos = Vector3.Lerp(MainCamera.transform.position, desiredPos, smoothSpeed);

            // set position of camera to now smoothed position
            MainCamera.transform.position = smoothedPos;
        }
    }

    void ShowOverhead()
    {
        MainCamera.enabled = false;
        OverheadCamera.enabled = true;
    }
    
    void ShowMain()
    {
        MainCamera.enabled = true;
        OverheadCamera.enabled = false;
    }

    int getNumCarrots()
    {
        GameObject[] carrotArr = GameObject.FindGameObjectsWithTag("Carrot");
        return carrotArr.Length;
    }
}
