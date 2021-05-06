using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class StartState : MonoBehaviour, IButtonState
{
    private ResourceManager RESOURCE_MANAGER;
    public StartState()
    {
        RESOURCE_MANAGER = ResourceManager.RESOURCE_MANAGER;
    }
    public bool Buy()
    {
        return false;
    }
    public void executeAction(MyButton button)
    {
        if (!button.IsUnlocked()) button.SetState(button.GetNotUnlockedState());
        else if (button.IsSpell())
        {
            if (!ArcaneVaultSpells.ARCANE_VAULT_SPELLS.isUsed[int.Parse(button.transform.tag)])
            {
                if (CanBeInstantiated(button)) button.SetState(button.GetReadyToUseSpellState());
                else button.SetState(button.GetInsufficientResourcesState());
            }
            else button.SetState(button.GetNotUsableState());
        }
        else if (CanBeInstantiated(button)) button.SetState(button.GetReadyToUseState());
        else button.SetState(button.GetInsufficientResourcesState());
    }
    private bool CanBeInstantiated(MyButton button)
    {
        return button.GetResource().Equals("wood") ?
            RESOURCE_MANAGER.GetWoodAmount() >= button.GetPrice() :
            RESOURCE_MANAGER.GetGoldAmount() >= button.GetPrice();
    }
}
