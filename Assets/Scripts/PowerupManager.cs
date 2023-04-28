using System.Collections;
using System.Collections.Generic;
using Utils;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PowerupManager : MonoBehaviour
{

    public GameManager gameManager;
    public GameObject bird;
    private BirdAttacking birdAttacking;
    private BirdMovement birdMovement;

    public SpriteRenderer scoreMultiplierSprite;
    public float scoreMultiplierTime;
    private readonly float scoreMultiplier = 2f;

    public SpriteRenderer multiShotSprite;
    public float multiShotTime;

    public SpriteRenderer fastShotSprite;
    public float fastShotTime;
    public float fastShotDelay;
    
    
    public int consecutiveHitsToPowerup;
    private int numConsecutiveHits;
    public Image powerupBarImage;

    private Powerup[] powerups = new Powerup[3];

    // Start is called before the first frame update
    void Start()
    {
        birdAttacking = bird.GetComponent<BirdAttacking>();
        birdMovement = bird.GetComponent<BirdMovement>();

        numConsecutiveHits = 0;

        InitializePowerups();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessPowerups();
    }

    private void InitializePowerups() {
        powerups[0] = new ScoreMultiplierPowerup(scoreMultiplierTime, scoreMultiplier, gameManager, scoreMultiplierSprite);
        powerups[1] = new MultiShotPowerup(multiShotTime, birdAttacking, multiShotSprite); 
        powerups[2] = new FastShootPowerup(fastShotTime, fastShotDelay, birdAttacking, fastShotSprite);
    }

    public void ProcessPowerups() {
        foreach (Powerup powerup in powerups) {
            powerup.ProcessUpdate(Time.deltaTime);
        }
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

    public void PowerupManager_OnProjectileLand(Target target) {
        if ((target != null) & (!target.HasBeenHit())) {
            numConsecutiveHits += 1;
        } else {
            numConsecutiveHits = 0;
        }
        UpdateConsecutiveHits();
    }

    private void UpdateConsecutiveHits() {
        // Figure out how to make it fill up, and then reset
        if (numConsecutiveHits == consecutiveHitsToPowerup) {
            numConsecutiveHits = 0;
            ActivateRandomPowerup();
        }
        UpdatePowerupBarVisual();
    }

    private void UpdatePowerupBarVisual() {
        powerupBarImage.fillAmount = (float)numConsecutiveHits / (consecutiveHitsToPowerup - 1);
    }
}
