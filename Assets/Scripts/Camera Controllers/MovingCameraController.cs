using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCameraController : MonoBehaviour
{
    // move camer randomly in xz plane

    private Vector2 Min = new Vector3(-26.2f, 13.6f);
    private Vector2 Max = new Vector3(-8.5f, 16.5f);
    private Vector3 NewPosition;
    private float LerpSpeed = 0.05f;

    void Start()
    {
        NewPosition = this.transform.position;
    }

    void Update()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, NewPosition, Time.deltaTime * LerpSpeed);
        if (Vector3.Distance(this.transform.position, NewPosition) < 1f) {
            GetNewPosition();
        }
    }

    void GetNewPosition()
    {
        float xPos = Random.Range(Min.x, Max.x);
        float yPos = Random.Range(Min.y, Max.y);
        NewPosition = new Vector3(xPos, yPos, this.transform.position.z);
    }
}
