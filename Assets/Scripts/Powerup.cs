using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Powerup {
    protected string name;
    protected float powerupTime;
    protected float powerupTimer;
    protected SpriteRenderer powerupVisual;

    protected Powerup() {
        powerupTimer = powerupTime;
    }

    public void ResetTimer() {
        powerupTimer = 0f;
    }

    public void AddTimeToTimer(float time) {
        powerupTimer += time;
    }

    public void ProcessUpdate(float time) {
        bool isCurrentlyActive = IsActive();
        AddTimeToTimer(time);
        
        if (isCurrentlyActive & (!IsActive())) {
            DeactivatePowerup();
        }
    }

    abstract public bool IsActive();
    abstract public void ActivatePowerup();
    abstract public void DeactivatePowerup();
}


public class ScoreMultiplierPowerup : Powerup {

    private GameManager gameManager;
    private float scoreMultiplier;
    
    public ScoreMultiplierPowerup(float powerupTime, float scoreMultiplier, GameManager gameManager, SpriteRenderer powerupVisual) : base() {
        name = "score multiplier";
        this.powerupTime = powerupTime;
        this.scoreMultiplier = scoreMultiplier;
        this.gameManager = gameManager;
        this.powerupVisual = powerupVisual;
        ResetTimer();
    }

    override public bool IsActive() {
        return powerupTimer < powerupTime;
    }

    override public void ActivatePowerup() {
        ResetTimer();
        powerupVisual.enabled = true;
        gameManager.SetScoreMultiplier(scoreMultiplier);
    }

    public override void DeactivatePowerup() {
        powerupVisual.enabled = false;
        gameManager.ResetScoreMultiplier();
    }
}

public class FastShootPowerup : Powerup {

    private BirdAttacking birdAttacking;
    private float shootDelay;
    public FastShootPowerup(float powerupTime, float shootDelay, BirdAttacking birdAttacking, SpriteRenderer powerupVisual) : base() {
        name = "multishot";
        this.powerupTime = powerupTime;
        this.shootDelay = shootDelay;
        this.birdAttacking = birdAttacking;
        this.powerupVisual = powerupVisual;

        ResetTimer();
    }

    override public bool IsActive() {
        return powerupTimer < powerupTime;
    }

    override public void ActivatePowerup() {
        powerupVisual.enabled = true;
        birdAttacking.SetBasicShootDelay(shootDelay);
        ResetTimer();
    }

    public override void DeactivatePowerup() {
        powerupVisual.enabled = false;
        birdAttacking.ResetBasicShootDelay();
    }
}

public class MultiShotPowerup : Powerup {

    private BirdAttacking birdAttacking;

    public MultiShotPowerup(float powerupTime, BirdAttacking birdAttacking, SpriteRenderer powerupVisual) : base() {
        name = "multishot";
        this.powerupTime = powerupTime;
        this.birdAttacking = birdAttacking;
        this.powerupVisual = powerupVisual;
    }

    override public bool IsActive() {
        return powerupTimer < powerupTime;
    }

    override public void ActivatePowerup() {
        powerupVisual.enabled = true;
        birdAttacking.ActivateMultiShot();
        ResetTimer();
    }

    public override void DeactivatePowerup() {
        powerupVisual.enabled = false;
        birdAttacking.DeactivateMultiShot();
    }
}