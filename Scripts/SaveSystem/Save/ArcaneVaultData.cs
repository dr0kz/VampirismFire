using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArcaneVaultData
{
    public bool isUnlockedArcaneVault { get; set; }
    public float armorMultiplier { get; set; }
    public float healthMultiplier { get; set; }
    public float backdoorProtection { get; set; }
    public ArcaneVaultData(bool isUnlockedArcaneVault, float armorMultiplier, float healthMultiplier, float backdoorProtection)
    {
        this.isUnlockedArcaneVault = isUnlockedArcaneVault;
        this.armorMultiplier = armorMultiplier;
        this.healthMultiplier = healthMultiplier;
        this.backdoorProtection = backdoorProtection;
    }
}