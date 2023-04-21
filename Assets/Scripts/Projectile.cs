using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float shootTime;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float moveTime;

    private Vehicle targetVehicle = null;

    // Start is called before the first frame update
    void Start()
    {
        moveTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        moveTime += Time.deltaTime;

        if (moveTime < shootTime) {
            transform.position = Vector3.Lerp(startPosition, targetPosition, moveTime / shootTime);
        } else {
            // Has reached target position
            if (targetVehicle != null) {
                targetVehicle.GetHit();
            }
            Destroy(this.gameObject);
        }
    }

    public void SetStartPosition(Vector3 startPosition) {
        this.startPosition = startPosition;
    }

    public void SetTargetPosition(Vector3 targetPosition) {
        this.targetPosition = targetPosition;
    }

    // Use colliders to detect when it runs into anything
    // If it's a vehicle, set it as the target
    private void OnTriggerEnter2D(Collider2D collision) {

        GameObject parentGameObj = collision.transform.gameObject;
        Vehicle vehicle = parentGameObj.GetComponent<Vehicle>();

        if (vehicle != null) {
            targetVehicle = vehicle;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {

        GameObject parentGameObj = collision.transform.gameObject;
        Vehicle vehicle = parentGameObj.GetComponent<Vehicle>();

        // If the thing we left was a vehicle, then set our target vehicle to null
        if (vehicle != null) {
            targetVehicle = null;
        }   
    }
}
