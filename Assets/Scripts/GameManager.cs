using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using TMPro;


public class GameManager : MonoBehaviour
{
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
    private float scoreMultiplier;
    public TMPro.TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        ResetScore();
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
        GameObject newVehicle = Instantiate(newVehiclePrefab, null);

        newVehicle.transform.position = new Vector3(Random.Range(-1f, 1f) * maxSpawnRangeX, spawnPositionY, 0);
        newVehicle.GetComponent<Vehicle>().OnVehicleHit += GameManager_OnVehicleHit;

        UpdateNextVehicleSpawnTime();

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

    public void GameManager_OnVehicleHit(Vehicle vehicle) {
        Debug.Log("gm vehiclehit");
        AddVehicleScore(vehicle.scoreValue);
        UpdateScoreText();
    }
}
