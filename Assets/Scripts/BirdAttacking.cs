using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdAttacking : MonoBehaviour
{
    public GameManager gameManager;
    public PowerupManager powerupManager;
    public GameObject projectilePrefab;
    public GameObject target;
    public GameObject shootTimerBar;


    // Shooting
    public float shootDistance;
    public float defaultBasicShootDelay;
    private float basicShootDelay;
    private float basicShootTimer;
    private bool activeMultiShot;
    public float multishotAngle;

    private readonly float maxTimerBarXScale = 30;
    private readonly float shootTimerBarScale = 8.5f;

    // Start is called before the first frame update
    void Start()
    {
        activeMultiShot = false;
        basicShootDelay = defaultBasicShootDelay;
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

    public void SetBasicShootDelay(float shootDelay) {
        basicShootDelay = shootDelay;
    }

    public void ResetBasicShootDelay() {
        basicShootDelay = defaultBasicShootDelay;
    }

    public void ActivateMultiShot() {
        activeMultiShot = true;
    }

    public void DeactivateMultiShot() {
        activeMultiShot = false;
    }

    private void UpdateTimerBarScale() {
        float scale = Mathf.Max(0f, 1 - basicShootTimer / basicShootDelay);
        shootTimerBar.transform.localScale = new Vector3(scale * maxTimerBarXScale, shootTimerBarScale, shootTimerBarScale);
    }

    private void ShootBasicProjectile() {
        GameObject projectileObj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Projectile projectile = projectileObj.GetComponent<Projectile>();

        Vector3 targetPosition = GetTargetPosition(target.transform.position);
        float yDiff = targetPosition.y - transform.position.y;

        projectile.SetStartPosition(transform.position);
        projectile.SetTargetPosition(GetTargetPosition(target.transform.position));

        projectile.OnProjectileLand += gameManager.GameManager_OnProjectileLand;
        projectile.OnProjectileLand += powerupManager.PowerupManager_OnProjectileLand;

        if (activeMultiShot) {
            GameObject projectileObjLeft = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Projectile projectileLeft = projectileObjLeft.GetComponent<Projectile>();

            projectileLeft.SetStartPosition(transform.position);
            projectileLeft.SetTargetPosition(targetPosition + new Vector3(-yDiff * Mathf.Cos(multishotAngle), 0, 0));

            projectileLeft.OnProjectileLand += gameManager.GameManager_OnProjectileLand;
            projectileLeft.OnProjectileLand += powerupManager.PowerupManager_OnProjectileLand;

            GameObject projectileObjRight = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Projectile projectileRight = projectileObjRight.GetComponent<Projectile>();

            projectileRight.SetStartPosition(transform.position);
            projectileRight.SetTargetPosition(targetPosition + new Vector3(yDiff * Mathf.Cos(multishotAngle), 0, 0));

            projectileRight.OnProjectileLand += gameManager.GameManager_OnProjectileLand;
            projectileRight.OnProjectileLand += powerupManager.PowerupManager_OnProjectileLand;
        }

    }

    private Vector3 GetTargetPosition(Vector3 originalTargetPosition) {
        // Get projectile shoot time and use it instead
        float basicShootTime = projectilePrefab.GetComponent<Projectile>().shootTime;

        // return originalTargetPosition - new Vector3(0, basicShootTime * GameManager.scrollSpeed, originalTargetPosition.z);
        return originalTargetPosition - new Vector3(0, basicShootTime * GameManager.scrollSpeed, 0);
    }
}
