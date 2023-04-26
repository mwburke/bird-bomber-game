using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Powerup : MonoBehaviour
{

    // Update is called once per frame
    void Update() {
        Vector3 movement = new(0, -GameManager.scrollSpeed * Time.deltaTime, 0);
        transform.Translate(movement);
    }


    private void OnTriggerEnter2D(Collider2D collision) {

        GameObject parentGameObj = collision.transform.gameObject;
        BirdMovement collideTarget = parentGameObj.GetComponent<BirdMovement>();

        if (collideTarget != null) {
            collideTarget.ObstacleCollision();
        }

        // TODO: add animation, noise
        Destroy(this.gameObject);
    }

    // This is really vague because sometimes we want to modify the GameManager
    // and sometimes we want to modify the bird
    protected abstract void ApplyPowerup();
}
