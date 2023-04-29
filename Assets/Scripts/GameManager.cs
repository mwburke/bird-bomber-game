using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public static float scrollSpeed = 1.3f;
    public float maxVehicleSpawnTime;
    public float minVehicleSpawnTime;
    private float nextVehicleSpawnTime;
    private float vehicleSpawnTimer;
    public GameObject[] spawnableVehicles;

    public float maxPowerupSpawnTime;
    public float minPowerupSpawnTime;
    private float nextPowerupSpawnTime;
    private float powerupSpawnTimer;
    public GameObject powerupContainer;
    public GameObject[] spawnablePowerupObjs;

    private Camera cam;
    private float maxSpawnRangeX;
    private float spawnPositionY;
    private int score;
    public TextMeshProUGUI scoreText;
    public GameObject powerupManagerObj;
    private PowerupManager powerupManager;
    private float defaultScoreMultiplier = 1f;
    private float scoreMultiplier;

    public int consecutiveHitsToPowerup;
    private int numConsecutiveHits;
    public Image scoreMultiplierBar;
    public TextMeshProUGUI scoreMultiplierText;


    // Start is called before the first frame update
    void Start()
    {
        ResetScore();

        numConsecutiveHits = 0;

        powerupManager = powerupManagerObj.GetComponent<PowerupManager>();

        scoreMultiplier = defaultScoreMultiplier;

        cam = Camera.main;

        maxSpawnRangeX = cam.orthographicSize * cam.aspect;
        spawnPositionY = cam.orthographicSize * 1.1f;

        SpawnVehicle();
        UpdateNextPowerupSpawnTime();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (vehicleSpawnTimer > nextVehicleSpawnTime) {
            SpawnVehicle();
        }

        vehicleSpawnTimer += Time.deltaTime;

        if (powerupSpawnTimer > nextPowerupSpawnTime) {
            SpawnPowerup();
        }

        powerupSpawnTimer += Time.deltaTime;
    }

    private void UpdateNextPowerupSpawnTime() {
        nextPowerupSpawnTime = Random.Range(minPowerupSpawnTime, maxPowerupSpawnTime);
        powerupSpawnTimer = 0f;
    }

    public void UpdateNextVehicleSpawnTime() {
        nextVehicleSpawnTime = Random.Range(minVehicleSpawnTime, maxVehicleSpawnTime);
        vehicleSpawnTimer = 0f;
    }

    private void SpawnVehicle() {
        GameObject newVehiclePrefab = spawnableVehicles.GetRandom();

        Vector3 moveDirection = RandomCardinalDirection();

        GameObject newVehicle = Instantiate(newVehiclePrefab,
            new Vector3(Random.Range(-1f, 1f) * maxSpawnRangeX, spawnPositionY, 0),
            QuarternionFromDirection(moveDirection)
        );

        Target target = newVehicle.GetComponent<Target>();
        target.SetMoveDirection(moveDirection);

        UpdateNextVehicleSpawnTime();
    }

    private void SpawnPowerup() {
        GameObject newPowerupObj = spawnablePowerupObjs.GetRandom();

        GameObject newPowerup = Instantiate(newPowerupObj);
        newPowerup.transform.position = new Vector3(Random.Range(-1f, 1f) * maxSpawnRangeX, spawnPositionY, 0);

        PowerupObj powerup = newPowerup.GetComponent<PowerupObj>();
        powerup.SetMovable();

        UpdateNextPowerupSpawnTime();
    }

    private Vector3 RandomCardinalDirection() {
        float value = Random.Range(0, 1f);
        if (value < 0.25f) {
            return Vector3.left;
        } else if (value < 0.5f) {
            return Vector3.up;
        } else if (value < 0.75f) {
            return Vector3.right;
        } else {
            return Vector3.down;
        }
    }

    private Quaternion QuarternionFromDirection(Vector3 direction) {
        if (direction == Vector3.left) {
            return Quaternion.Euler(0, 0, 180);
        } else if (direction == Vector3.up) {
            return Quaternion.Euler(0, 0, 90);
        } else if (direction == Vector3.right) {
            return Quaternion.Euler(0, 0, 0);
        } else if (direction == Vector3.down) {
            return Quaternion.Euler(0, 0, 270);
        } else {
            // Shouldn't be used
            return Quaternion.Euler(0, 0, 0);
        }
    }

    private void ResetScore() {
        score = 0;
    }

    private void AddTargetScore(float targetScore) {
        score += (int)(targetScore * scoreMultiplier);
    }

    private void UpdateScoreText() {
        scoreText.SetText("Score: " + score.ToString());
    }

    public void GameManager_OnProjectileLand(Target target, bool wasAlreadyHit, bool excludeFromConsecutiveHits) {
        if ((target != null) & (!wasAlreadyHit)) {
            AddTargetScore(target.scoreValue);
            UpdateScoreText();
            numConsecutiveHits += 1;
        } else {
            if (!excludeFromConsecutiveHits) {
                numConsecutiveHits = 0;
                ResetScoreMultiplier();
            }
        }

        UpdateConsecutiveHits();
        UpdateScoreMultiplierText();
    }

    public void ResetScoreMultiplier() {
        scoreMultiplier = defaultScoreMultiplier;
    }

    public void SetScoreMultiplier(float multiplier) {
        scoreMultiplier = multiplier;
    }

    private void UpdateConsecutiveHits() {
        // Figure out how to make it fill up, and then reset
        if (numConsecutiveHits == consecutiveHitsToPowerup) {
            numConsecutiveHits = 0;

            scoreMultiplier += 1;
        }
        UpdateScoreMultiplierBarVisual();
        UpdateScoreMultiplierText();
    }


    private void UpdateScoreMultiplierText() {
        scoreMultiplierText.text = "MULTIPLIER: X" + ((int)scoreMultiplier).ToString();
    }

    private void UpdateScoreMultiplierBarVisual() {
        scoreMultiplierBar.fillAmount = (float)numConsecutiveHits / (consecutiveHitsToPowerup - 1);
    }
}
