using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Vehicle : MonoBehaviour
{
    public GameObject poopSplat;
    private bool hasBeenHit;
    private float destroyLimit;
    public int scoreValue;

    public event OnVehicleHitDelegate OnVehicleHit;
    public delegate void OnVehicleHitDelegate(Vehicle vehicle);

    void Start()
    {
        hasBeenHit = false;
        Camera cam = Camera.main;
        destroyLimit = -cam.orthographicSize;
    }

    void Update()
    {
        transform.Translate(0, -GameManager.scrollSpeed * Time.deltaTime, 0);

        if (transform.position.y < destroyLimit) {
            Destroy(this.gameObject);
        }
    }

    public void GetHit() {
        if (!hasBeenHit) {
            hasBeenHit = true;
            poopSplat.GetComponent<SpriteRenderer>().enabled = true;
            OnVehicleHit?.Invoke(this);
        }
    }

}
