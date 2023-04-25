using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VehicleCollision : MonoBehaviour
{
    public Target parentTarget;

    private void OnTriggerEnter2D(Collider2D collision) {

        GameObject parentGameObj = collision.transform.gameObject;
        Target collideTarget = parentGameObj.GetComponent<Target>();

        if (collideTarget != null) {
            parentTarget.SetBlockedStatus(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {

        GameObject parentGameObj = collision.transform.gameObject;
        Target collideTarget = parentGameObj.GetComponent<Target>();

        if (collideTarget != null) {
            parentTarget.SetBlockedStatus(false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        GameObject parentGameObj = collision.transform.gameObject;
        Target collideTarget = parentGameObj.GetComponent<Target>();

        if (collideTarget != null) {
            parentTarget.SetBlockedStatus(true);
        }
    }


}
