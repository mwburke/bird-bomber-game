using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerupObj : MonoBehaviour
{

    public PowerupManager powerupManager;
    public UnityEvent myUnityEvent;
    private bool canMove;

    private float destroyLimit;

    private void Awake() {
        canMove = false;
    }

    private void Start() {
        Camera cam = Camera.main;
        destroyLimit = -cam.orthographicSize;
        
    }

    void Update() {
        Vector3 movement = new(0, -GameManager.scrollSpeed * Time.deltaTime, 0);

        if (canMove) {
            transform.Translate(movement);
        }

        if (transform.position.y < destroyLimit) {
            Destroy(this.gameObject);
        }
    }

    public void SetMovable() {
        canMove = true;
    }

    // Use colliders to detect when it runs into anything
    // If it's a vehicle, set it as the target
    private void OnTriggerEnter2D(Collider2D collision) {

        GameObject parentGameObj = collision.transform.gameObject;
        BirdAttacking birdAttacking = parentGameObj.GetComponent<BirdAttacking>();

        if (birdAttacking != null) {
            myUnityEvent.Invoke();
            Destroy(this.gameObject);
        }
    }
}
