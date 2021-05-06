using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArcaneVault : Building
{
    [SerializeField] private GameObject backdoorProtectionEffect;
    [SerializeField] private AudioClip researchComplete;
    public override bool IsUnlocked()
    {
        return SaveSystem.LoadData().arcaneVaultData.isUnlockedArcaneVault;
    }
    public override void ButtonTask(MyButton button)
    {
        if (!button.transform.tag.Equals("Destroy"))
        {
            if (currentState.Execute(button))
            {
                Upgrade(button);
            }
        }
        else
        {
            DestroyBuilding(OnBuildingDestroyParticleEffect);
            Unselect();
        }
    }
    protected override void Upgrade(MyButton button)
    {
        audioSource.PlayOneShot(researchComplete);
        if (button.name.Contains("BackdoorProtection"))
        {
            IEnumerator backdoor = ArcaneVaultSpells.ARCANE_VAULT_SPELLS.BackdoorProtection(backdoorProtectionEffect);
            StartCoroutine(backdoor);
        }
        else if (button.name.Contains("DoubleArmor"))
        {
           ArcaneVaultSpells.ARCANE_VAULT_SPELLS.IncreaseArmor();
        }
        else if (button.name.Contains("DoubleDamage"))
        {
            ArcaneVaultSpells.ARCANE_VAULT_SPELLS.IncreaseDamage();
        }
        ArcaneVaultSpells.ARCANE_VAULT_SPELLS.isUsed[int.Parse(button.transform.tag)] = true;
        CanvasManager.CANVAS_MANAGER.UpdateButtons();
    }
}
