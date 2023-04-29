using System.Collections;
using System.Collections.Generic;
using Utils;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PowerupManager : MonoBehaviour {

    public GameManager gameManager;
    public GameObject bird;
    private BirdAttacking birdAttacking;
    private BirdMovement birdMovement;

    public SpriteRenderer multiShotSprite;
    public float multiShotTime;

    public SpriteRenderer fastShotSprite;
    public float fastShotTime;
    public float fastShotDelay;

    private Powerup[] powerups = new Powerup[2];

    // Start is called before the first frame update
    void Start() {
        birdAttacking = bird.GetComponent<BirdAttacking>();
        birdMovement = bird.GetComponent<BirdMovement>();

        InitializePowerups();
    }

    // Update is called once per frame
    void Update() {
        ProcessPowerups();
    }

    private void InitializePowerups() {
        powerups[0] = new MultiShotPowerup(multiShotTime, birdAttacking, multiShotSprite);
        powerups[1] = new FastShootPowerup(fastShotTime, fastShotDelay, birdAttacking, fastShotSprite);
    }

    public void ProcessPowerups() {
        foreach (Powerup powerup in powerups) {
            powerup.ProcessUpdate(Time.deltaTime);
        }
    }

    public void ActivateMultiShot() {
        powerups[0].ActivatePowerup();
    }

    public void ActivateFastShoot() {
        powerups[1].ActivatePowerup();
    }


    public void ActivateRandomPowerup() {
        Powerup[] unusedPowerups = GetUnusedPowerups();
        if (unusedPowerups.Length > 0) {
            unusedPowerups.GetRandom().ActivatePowerup();
        } else {
            powerups.GetRandom().ActivatePowerup();
        }
    }

    private Powerup[] GetUnusedPowerups() {
        List<Powerup> unusedPowerups = new();
        for (int i = 0; i < powerups.Length; i++) {
            if (!powerups[i].IsActive()) {
                unusedPowerups.Add(powerups[i]);
            }
        }

        return unusedPowerups.ToArray();
    }

}