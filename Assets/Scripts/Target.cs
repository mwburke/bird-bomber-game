using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject poopSplat;
    private bool hasBeenHit;
    private float destroyLimit;
    public int scoreValue;
    public float moveSpeed;
    private bool isBlocked;
    private Vector3 moveDirection;

    public event OnTargetHitDelegate OnTargetHit;
    public delegate void OnTargetHitDelegate(Target target);

    void Start() {
        hasBeenHit = false;
        isBlocked = false;
        Camera cam = Camera.main;
        destroyLimit = -cam.orthographicSize;
    }

    void Update() {

        Vector3 movement = new(0, -GameManager.scrollSpeed * Time.deltaTime, 0);

        if (!isBlocked) {
            movement += moveSpeed * Time.deltaTime * moveDirection;
        }

        transform.position += movement;

        if (transform.position.y < destroyLimit) {
            Destroy(this.gameObject);
        }
    }

    public void GetHit() {
        if (!hasBeenHit) {
            hasBeenHit = true;
            poopSplat.GetComponent<SpriteRenderer>().enabled = true;
            OnTargetHit?.Invoke(this);
        }
    }

    public void SetBlockedStatus(bool status) {
        isBlocked = status;
    }

    public void SetMoveDirection(Vector3 direction) {
        moveDirection = direction;
    }

    public void SetMoveSpeed(float speed) {
        moveSpeed = speed;
    }
}
