using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class ArcaneVaultSpells : MonoBehaviour
{
    public static ArcaneVaultSpells ARCANE_VAULT_SPELLS;
    [SerializeField] private float backdoorProtectionCooldown;
    public float towerDoubleDamageMultiplier = 1.0f;
    public float wallDoubleArmorMultiplier = 1.0f;
    public float backdoorProtectionSeconds = 1.0f;
    public bool isBackdoorProtectionActivated = false;
    public bool[] isUsed;
    private void Awake()
    {
        ARCANE_VAULT_SPELLS = this;
        isUsed = new bool[3] { false, false, false };    
    }
    public bool IsUsed(MyButton button)
    {
        if (button.name.Equals("DoubleArmor")) return isUsed[0];
        else if (button.name.Equals("DoubleDamage")) return isUsed[1];
        else return isUsed[2];
    }
    public IEnumerator BackdoorProtection(GameObject backdoorEffect)
    {
        backdoorProtectionSeconds = SaveSystem.LoadData().arcaneVaultData.backdoorProtection;
        isBackdoorProtectionActivated = true;
        List<GameObject> effects = new List<GameObject>();
        BuildingManager.BUILDING_MANAGER.getBuildings().ForEach(building =>
        {
            GameObject effect = Instantiate(backdoorEffect, building.transform.position, Quaternion.identity);
            effects.Add(effect);
        });
        yield return new WaitForSeconds(backdoorProtectionSeconds);
        isBackdoorProtectionActivated = false;
        effects.ForEach(effect => Destroy(effect.gameObject));
    }
    public void IncreaseArmor()
    {
        wallDoubleArmorMultiplier = SaveSystem.LoadData().arcaneVaultData.armorMultiplier;
        BuildingManager.BUILDING_MANAGER.getBuildings()
            .Where(building => building.GetComponent<Wall>() != null)
            .Select(building => building as Wall)
            .ToList()
            .ForEach(wall => wall.UpdateArmor());
    }
    public void IncreaseDamage()
    {
        towerDoubleDamageMultiplier = SaveSystem.LoadData().arcaneVaultData.healthMultiplier;
        BuildingManager.BUILDING_MANAGER.getBuildings()
            .Where(building => building.GetComponent<DamageTower>() != null)
            .Select(building => building as DamageTower)
            .ToList()
            .ForEach(tower => tower.UpdateDamage());
    }

}
