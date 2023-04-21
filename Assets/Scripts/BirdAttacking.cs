using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdAttacking : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject target;
    public GameObject shootTimerBar;


    // Shooting
    public float shootDistance;
    public float basicShootDelay;
    private float basicShootTimer;

    private float maxTimerBarXScale = 30;
    private float shootTimerBarScale = 8.5f;

    // Start is called before the first frame update
    void Start()
    {
        basicShootTimer = 0f;
        target.transform.localPosition = new Vector3(0, shootDistance, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space)) {
            if (basicShootTimer >= basicShootDelay) {
                ShootBasicProjectile();
                basicShootTimer = 0f;
            }
        }

        basicShootTimer += Time.deltaTime;

        UpdateTimerBarScale();
    }

    private void UpdateTimerBarScale() {
        float scale = Mathf.Max(0f, 1 - basicShootTimer / basicShootDelay);
        shootTimerBar.transform.localScale = new Vector3(scale * maxTimerBarXScale, shootTimerBarScale, shootTimerBarScale);
    }

    private void ShootBasicProjectile() {
        GameObject projectileObj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Projectile projectile = projectileObj.GetComponent<Projectile>();

        projectile.SetStartPosition(transform.position);
        projectile.SetTargetPosition(GetTargetPosition(target.transform.position));
    }

    private Vector3 GetTargetPosition(Vector3 originalTargetPosition) {
        // Get projectile shoot time and use it instead
        float basicShootTime = projectilePrefab.GetComponent<Projectile>().shootTime;

        // return originalTargetPosition - new Vector3(0, basicShootTime * GameManager.scrollSpeed, originalTargetPosition.z);
        return originalTargetPosition - new Vector3(0, basicShootTime * GameManager.scrollSpeed, 0);
    }
}
