using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMovement : MonoBehaviour
{

    // Movement
    public float maxTurnAngle;
    public float turnSpeedModifier;
    private float horizontalSpeed;
    public float maxHorizontalSpeed;
    public float frictionSpeed;
    public float bonkTime;
    public float bonkTimer;

    // Movement edge handling
    public float edgeLimitFraction;
    private float edgeLimitLeft, edgeLimitRight;

    // Game activity
    public bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        isActive = true;
        horizontalSpeed = 0f;
        SetEdgeLimits();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive) {
            ProcessMovement();
            ProcessAngle();
        }
    }

    private void ProcessAngle() {
        float fraction = horizontalSpeed / maxHorizontalSpeed;
        transform.localRotation = Quaternion.Euler(0, 0, -fraction * maxTurnAngle);
    }

    private void ProcessMovement() {
        float horizontalInput = Input.GetAxis("Horizontal");

        if (transform.position.x <= edgeLimitLeft) {
            horizontalSpeed = Mathf.Min(horizontalSpeed + turnSpeedModifier, maxHorizontalSpeed);
            // Only do this if tilted back? need to add in more logic
        } else if (transform.position.x >= edgeLimitRight) {
            horizontalSpeed = Mathf.Max(horizontalSpeed - turnSpeedModifier, -maxHorizontalSpeed);
            // Only do this if tilted back? need to add in more logic
        } else if (horizontalInput != 0) {
            horizontalSpeed = Mathf.Clamp(horizontalSpeed + (horizontalInput * turnSpeedModifier), -maxHorizontalSpeed, maxHorizontalSpeed);
        } else {
            // Friction here
            if (horizontalSpeed < 0) {
                horizontalSpeed = Mathf.Min(0, horizontalSpeed + frictionSpeed);
            } else if (horizontalSpeed > 0) {
                horizontalSpeed = Mathf.Max(0f, horizontalSpeed - frictionSpeed);
            }
            
        }

        transform.position += new Vector3(horizontalSpeed * Time.deltaTime, 0, 0);
    }

    private void SetEdgeLimits() {
        // TODO: add listener for screen size change to update?

        Camera cam = Camera.main;// Camera component to get their size, if this change in runtime make sure to update values
        float camWidth = cam.orthographicSize * cam.aspect;
        float spriteSize = GetComponent<SpriteRenderer>().bounds.size.x * .5f;

        edgeLimitLeft = -camWidth + (camWidth * edgeLimitFraction) + spriteSize; // left bound
        edgeLimitRight = camWidth - (camWidth * edgeLimitFraction) - spriteSize; // right bound 
    }
}
