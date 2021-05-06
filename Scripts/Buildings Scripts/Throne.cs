using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Throne : Building
{
    [SerializeField] private GameManager gameManager;
    protected override void Start()
    {
        CANVAS_MANAGER = CanvasManager.CANVAS_MANAGER;
        BUILDING_MANAGER = BuildingManager.BUILDING_MANAGER;
        WORKER_MANAGER = WorkerManager.WORKER_MANAGER;
        SOUND_MANAGER = SoundManager.SOUND_MANAGER;
        maxHealth = health;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(health);
        if (health < maxHealth) healthBar.gameObject.SetActive(true);
        else healthBar.gameObject.SetActive(false);
        buildingProgressBar.gameObject.SetActive(false);
        BUILDING_MANAGER.addBuilding(this);
    }
    public override void DestroyBuilding(GameObject particleEffect)
    {
        base.DestroyBuilding(particleEffect);
        gameManager.GameStatus(GameManager.GameState.lose);
    }
}
