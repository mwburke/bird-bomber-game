using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public static float scrollSpeed = 1.0f;
    public float maxVehicleSpawnTime;
    public float minVehicleSpawnTime;
    private float nextVehicleSpawnTime;
    private float vehicleSpawnTimer;
    public GameObject[] spawnableVehicles;
    private Camera cam;
    private float maxSpawnRangeX;
    private float spawnPositionY;
    private int score;
    public TextMeshProUGUI scoreText;
    public GameObject powerupManagerObj;
    private PowerupManager powerupManager;
    private float defaultScoreMultiplier = 1f;
    private float scoreMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        ResetScore();

        powerupManager = powerupManagerObj.GetComponent<PowerupManager>();

        scoreMultiplier = 1f;

        cam = Camera.main;

        maxSpawnRangeX = cam.orthographicSize * cam.aspect;
        spawnPositionY = cam.orthographicSize * 1.1f;

        SpawnVehicle();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (vehicleSpawnTimer > nextVehicleSpawnTime) {
            SpawnVehicle();
        }

        vehicleSpawnTimer += Time.deltaTime;
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

    private void AddVehicleScore(float vehicleScore) {
        score += (int)(vehicleScore * scoreMultiplier);
    }

    private void UpdateScoreText() {
        scoreText.SetText("Score: " + score.ToString());
    }

    public void GameManager_OnProjectileLand(Target target) {
        if (target != null) {
            AddVehicleScore(target.scoreValue);
            UpdateScoreText();
        }
    }

    public void ResetScoreMultiplier() {
        scoreMultiplier = defaultScoreMultiplier;
    }

    public void SetScoreMultiplier(float multiplier) {
        scoreMultiplier = multiplier;
    }
}
