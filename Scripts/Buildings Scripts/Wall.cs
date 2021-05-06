using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class Wall : Building
{
    [SerializeField] private float armor;
    [SerializeField] private int index;
    protected override void Start()
    {
        CANVAS_MANAGER = CanvasManager.CANVAS_MANAGER;
        BUILDING_MANAGER = BuildingManager.BUILDING_MANAGER;
        RESOURCE_MANAGER = ResourceManager.RESOURCE_MANAGER;
        WORKER_MANAGER = WorkerManager.WORKER_MANAGER;
        ENEMY_MANAGER = EnemyManager.ENEMY_MANAGER;
        SOUND_MANAGER = SoundManager.SOUND_MANAGER;
        transform.position = new Vector3(standingPlane.transform.position.x
        , standingPlane.transform.position.y + standingPlane.GetPlaneHeight() / 2.0f + GetBuildingHeight() / 2.0f
        , standingPlane.transform.position.z);
        if (SOUND_MANAGER.GetSoundEffectsStatus()) audioSource.mute = true;
        standingPlane.GetComponent<BoxCollider>().enabled = false; // the plane's collider under the building is disabled

        if (maxHealth == 0)
        {
            maxHealth = SaveSystem.LoadData().wallData.wallsHealth[index];
            health = maxHealth;
        }
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(health);
        if (health < maxHealth)
        {
            healthBar.gameObject.SetActive(true);
        }
        else
        {
            healthBar.gameObject.SetActive(false);
        }
        BUILDING_MANAGER.addBuilding(this);
        if (gameObject.activeSelf)
        {
            BuildTimeProcess();
        }
        UpdateArmor();
    }
    public override bool IsUnlocked()
    {
        return SaveSystem.LoadData().wallData.biggestUnlockedWallIndex >= index;
    }
    public override void TakeDamage(float damage)
    {
        if (ArcaneVaultSpells.ARCANE_VAULT_SPELLS.isBackdoorProtectionActivated) return;
        health -= damage * (damage / (damage + armor));
        healthBar.gameObject.SetActive(true);
        healthBar.SetHealth(health);
        if (health <= 0.0f)
        {
            DestroyBuilding(OnBuildingDestroyParticleEffect);
        }
    }
    public override void Init(IInstantiatable _building)
    {
        Building building = (Building)_building;
        standingPlane = building.GetStandingPlane();
        maxHealth = SaveSystem.LoadData().wallData.wallsHealth[index];
        float percent = (building.GetHealth() * 100.0f) / building.GetMaxHealth();
        health = (maxHealth * percent) / 100.0f + ((100.0f - percent) * maxHealth) / 200.0f;
    }
    public void UpdateArmor()
    {
        armor *= ArcaneVaultSpells.ARCANE_VAULT_SPELLS.wallDoubleArmorMultiplier;
    }
}
