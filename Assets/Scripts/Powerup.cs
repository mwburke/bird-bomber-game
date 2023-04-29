using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Powerup {
    protected string name;
    protected float powerupTime;
    protected float powerupTimer;
    protected SpriteRenderer powerupVisual;
    protected float lowerOpacity = 0.2f;

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

public class FastShootPowerup : Powerup {

    private BirdAttacking birdAttacking;
    private float shootDelay;
    public FastShootPowerup(float powerupTime, float shootDelay, BirdAttacking birdAttacking, SpriteRenderer powerupVisual) : base() {
        name = "multishot";
        this.powerupTime = powerupTime;
        this.shootDelay = shootDelay;
        this.birdAttacking = birdAttacking;
        this.powerupVisual = powerupVisual;
    }

    override public bool IsActive() {
        return powerupTimer < powerupTime;
    }

    override public void ActivatePowerup() {
        powerupVisual.color = new Color(1f, 1f, 1f, 1f);
        birdAttacking.SetBasicShootDelay(shootDelay);
        ResetTimer();
    }

    public override void DeactivatePowerup() {
        powerupVisual.color = new Color(1f, 1f, 1f, lowerOpacity);
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
        powerupVisual.color = new Color(1f, 1f, 1f, 1f);
        birdAttacking.ActivateMultiShot();
        ResetTimer();
    }

    public override void DeactivatePowerup() {
        powerupVisual.color = new Color(1f, 1f, 1f, lowerOpacity);
        birdAttacking.DeactivateMultiShot();
    }
}