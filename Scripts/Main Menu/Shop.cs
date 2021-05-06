using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Shop : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resourceAmount;
    [SerializeField] private Image pageImage;
    [SerializeField] private List<Page> pages;
    [SerializeField] private AudioClip arrowsSoundEffect;
    [SerializeField] private AudioClip unlockSoundEffect;
    [SerializeField] private AudioClip notEnoughResources;
    private bool isResourceTextBlinking = false;
    private SoundManager SOUND_MANAGER;
    private int currentPage;
    private void Start()
    {
        SOUND_MANAGER = SoundManager.SOUND_MANAGER;
        resourceAmount.text = SaveSystem.LoadData().diamondsAmount.ToString();
        if (pages.Count > 0)
        {
            pages.ForEach(page => page.gameObject.SetActive(false));
            pages[0].gameObject.SetActive(true);
        }
        currentPage = 0;
        LoadData();

    }
    private void DisplayPageInfo(int page)
    {
        pages[page].gameObject.SetActive(true);
        pageImage.sprite = pages[page].displayPageImage;
    }
    public void HidePageInfo(int page)
    {
        pages[page].gameObject.SetActive(false);
    }
    public void MoveLeft()
    {
        SOUND_MANAGER.PlaySound(arrowsSoundEffect, 0.3f);
        if (currentPage == 0) return;
        HidePageInfo(currentPage);
        DisplayPageInfo(--currentPage);
    }
    public void MoveRight()
    {
        SOUND_MANAGER.PlaySound(arrowsSoundEffect, 0.3f);
        if (currentPage == pages.Count - 1) return;
        HidePageInfo(currentPage);
        DisplayPageInfo(++currentPage);
    }
    public void LoadData()
    {
        SavedData savedData;
        if ((savedData = SaveSystem.LoadData()) != null)
        {
            pages.SelectMany(page => page.items).ToList().ForEach(item =>
            {
                List<ItemData> items = savedData.items;
                ItemData getDataFromItem = items.Find(_item => _item.index == item.index && _item.pageIndex == item.pageIndex);
                item.LoadItemInfo(getDataFromItem);
            });
        }
    }
    private bool IsEnoughResourcesAvailable(int itemPrice)
    {
        return int.Parse(resourceAmount.text) >= itemPrice;
    }
    private IEnumerator AlertUserOnNotEnoughResources()
    {
        isResourceTextBlinking = true;
        resourceAmount.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        resourceAmount.color = Color.black;
        yield return new WaitForSeconds(0.3f);
        resourceAmount.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        resourceAmount.color = Color.black;
        yield return new WaitForSeconds(0.3f);
        resourceAmount.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        resourceAmount.color = Color.black;
        isResourceTextBlinking = false;
    }
    public void WorkerDataUpgrade([SerializeField] ItemUpgradeable item)
    {
        if (!IsEnoughResourcesAvailable(item.cost))
        {
            SOUND_MANAGER.PlaySound(notEnoughResources, 0.3f);
            if (!isResourceTextBlinking) StartCoroutine(AlertUserOnNotEnoughResources());
            return;
        }
        SOUND_MANAGER.PlaySound(unlockSoundEffect, 0.3f);
        SavedData savedData = SaveSystem.LoadData() as SavedData;
        WorkerData loadData = savedData.workerData;
        List<ItemData> items = savedData.items;
        savedData.diamondsAmount -= item.cost;
        resourceAmount.text = savedData.diamondsAmount.ToString();
        ItemData i = items.Find(_item => _item.index == item.index && _item.pageIndex == item.pageIndex);
        i.cost *= 2;
        i.level++;
        item.costText.text = i.cost.ToString();
        item.levelText.text = i.level.ToString();
        item.cost *= 2;

        #region Deciding Which Characteristic Is Being Upgraded
        if (item.abilityName.Equals("movementSpeed")) loadData.movementSpeed += item.increment;
        else if (item.abilityName.Equals("cuttingSpeed")) loadData.cuttingSpeed += item.increment;
        else if (item.abilityName.Equals("repairingSpeed")) loadData.repairingSpeed += item.increment;
        else if (item.abilityName.Equals("repairingPercentPerHit")) loadData.repairingPercentPerHit += item.increment;
        else if (item.abilityName.Equals("woodCapacity")) loadData.woodCapacity += (int)item.increment;
        else if (item.abilityName.Equals("woodPerHit")) loadData.woodPerHit += (int)item.increment;
        #endregion
        SaveSystem.SaveData(loadData, items, savedData.wallData, savedData.towerData, savedData.arcaneVaultData, savedData.diamondsAmount);
    }
    public void WallDataUpgrade([SerializeField] ItemUpgradeable item)
    {
        if (!IsEnoughResourcesAvailable(item.cost))
        {
            SOUND_MANAGER.PlaySound(notEnoughResources, 0.3f);
            if (!isResourceTextBlinking) StartCoroutine(AlertUserOnNotEnoughResources());
            return;
        }
        SOUND_MANAGER.PlaySound(unlockSoundEffect, 0.3f);
        SavedData savedData = SaveSystem.LoadData() as SavedData;
        WallData loadData = savedData.wallData;
        List<ItemData> items = savedData.items;
        savedData.diamondsAmount -= item.cost;
        resourceAmount.text = savedData.diamondsAmount.ToString();
        ItemData i = items.Find(_item => _item.index == item.index && _item.pageIndex == item.pageIndex);
        i.cost *= 2;
        i.level++;
        item.costText.text = i.cost.ToString();
        item.levelText.text = i.level.ToString();
        item.cost *= 2;
        #region Deciding Which Characteristic Is Being Upgraded
        if (item.abilityName.Equals("health")) loadData.wallsHealth[item.index] += item.increment;
        #endregion

        SaveSystem.SaveData(savedData.workerData, items, loadData, savedData.towerData, savedData.arcaneVaultData, savedData.diamondsAmount);
    }
    public void UnlockWall([SerializeField] ItemUnlocker item)
    {
        if (!IsEnoughResourcesAvailable(item.cost))
        {
            SOUND_MANAGER.PlaySound(notEnoughResources, 0.3f);
            if (!isResourceTextBlinking) StartCoroutine(AlertUserOnNotEnoughResources());
            return;
        }
        SOUND_MANAGER.PlaySound(unlockSoundEffect, 0.3f);
        SavedData savedData = SaveSystem.LoadData() as SavedData;
        WallData loadData = savedData.wallData;
        List<ItemData> items = savedData.items;
        savedData.diamondsAmount -= item.cost;
        resourceAmount.text = savedData.diamondsAmount.ToString();
        ItemData i = items.Find(_item => _item.index == item.index && _item.pageIndex == item.pageIndex);
        i.cost *= 2;
        i.level++;
        item.costText.text = i.cost.ToString();
        item.cost *= 2;
        loadData.biggestUnlockedWallIndex++;
        if (loadData.biggestUnlockedWallIndex >= item.itemsToUnlock.Count)
        {
            i.isVisible = false;
            item.gameObject.SetActive(false);
        }
        items.Find(_item => _item.index == pages[item.pageIndex].items[loadData.biggestUnlockedWallIndex].index &&
        _item.pageIndex == pages[item.pageIndex].items[loadData.biggestUnlockedWallIndex].pageIndex).isVisible = true;

        pages[item.pageIndex].items[loadData.biggestUnlockedWallIndex].gameObject.SetActive(true);

        SaveSystem.SaveData(savedData.workerData, items, loadData, savedData.towerData, savedData.arcaneVaultData, savedData.diamondsAmount);
    }
    public void TowerDataUpgrade([SerializeField] ItemUpgradeable item)
    {
        if (!IsEnoughResourcesAvailable(item.cost))
        {
            SOUND_MANAGER.PlaySound(notEnoughResources, 0.3f);
            if (!isResourceTextBlinking) StartCoroutine(AlertUserOnNotEnoughResources());
            return;
        }
        SOUND_MANAGER.PlaySound(unlockSoundEffect, 0.3f);
        SavedData savedData = SaveSystem.LoadData() as SavedData;
        TowerData loadData = savedData.towerData;
        List<ItemData> items = savedData.items;
        savedData.diamondsAmount -= item.cost;
        resourceAmount.text = savedData.diamondsAmount.ToString();
        ItemData i = items.Find(_item => _item.index == item.index && _item.pageIndex == item.pageIndex);
        i.cost *= 2;
        i.level++;
        item.costText.text = i.cost.ToString();
        item.cost *= 2;
        item.levelText.text = i.level.ToString();
        #region Deciding Which Characteristic Is Being Upgraded
        if (item.abilityName.Equals("damage")) loadData.towersDamage[item.index] += item.increment;
        #endregion

        SaveSystem.SaveData(savedData.workerData, items, savedData.wallData, loadData, savedData.arcaneVaultData, savedData.diamondsAmount);
    }
    public void UnlockTower([SerializeField] ItemUnlocker item)
    {
        if (!IsEnoughResourcesAvailable(item.cost))
        {
            SOUND_MANAGER.PlaySound(notEnoughResources, 0.3f);
            if (!isResourceTextBlinking) StartCoroutine(AlertUserOnNotEnoughResources());
            return;
        }
        SOUND_MANAGER.PlaySound(unlockSoundEffect, 0.3f);
        SavedData savedData = SaveSystem.LoadData() as SavedData;
        TowerData loadData = savedData.towerData;
        List<ItemData> items = savedData.items;
        savedData.diamondsAmount -= item.cost;
        resourceAmount.text = savedData.diamondsAmount.ToString();
        ItemData i = items.Find(_item => _item.index == item.index && _item.pageIndex == item.pageIndex);
        i.cost *= 2;
        i.level++;
        item.costText.text = i.cost.ToString();
        item.cost *= 2;
        loadData.biggestUnlockedTowerIndex++;
        if (loadData.biggestUnlockedTowerIndex >= item.itemsToUnlock.Count)
        {
            i.isVisible = false;
            item.gameObject.SetActive(false);
        }
        items.Find(_item => _item.index == pages[item.pageIndex].items[loadData.biggestUnlockedTowerIndex].index &&
        _item.pageIndex == pages[item.pageIndex].items[loadData.biggestUnlockedTowerIndex].pageIndex).isVisible = true;

        pages[item.pageIndex].items[loadData.biggestUnlockedTowerIndex].gameObject.SetActive(true);
        SaveSystem.SaveData(savedData.workerData, items, savedData.wallData, loadData, savedData.arcaneVaultData, savedData.diamondsAmount);
    }
    public void UnlockArcane([SerializeField] ItemUnlocker item)
    {
        if (!IsEnoughResourcesAvailable(item.cost))
        {
            SOUND_MANAGER.PlaySound(notEnoughResources, 0.3f);
            if (!isResourceTextBlinking) StartCoroutine(AlertUserOnNotEnoughResources());
            return;
        }
        SOUND_MANAGER.PlaySound(unlockSoundEffect, 0.3f);
        SavedData savedData = SaveSystem.LoadData() as SavedData;
        ArcaneVaultData loadData = savedData.arcaneVaultData;
        List<ItemData> items = savedData.items;
        savedData.diamondsAmount -= item.cost;
        resourceAmount.text = savedData.diamondsAmount.ToString();
        ItemData i = items.Find(_item => _item.index == item.index && _item.pageIndex == item.pageIndex);
        i.level++;
        i.isButtonVisible = false;
        i.isPriceVisible = false;
        i.isResourceVisible = false;
        loadData.isUnlockedArcaneVault = true;
        item.costText.gameObject.SetActive(false);
        item.buyButton.gameObject.SetActive(false);
        item.resourceImage.gameObject.SetActive(false);
        item.cost *= 2;
        item.itemsToUnlock.ForEach(_item =>
        {
            items.Find(item_ => item_.index == _item.index && item_.pageIndex == _item.pageIndex).isVisible = true;
            _item.gameObject.SetActive(true);

        });
        SaveSystem.SaveData(savedData.workerData, items, savedData.wallData, savedData.towerData, loadData, savedData.diamondsAmount);
    }
    public void UnlockSpell([SerializeField] ItemUpgradeable item)
    {
        if (!IsEnoughResourcesAvailable(item.cost))
        {
            SOUND_MANAGER.PlaySound(notEnoughResources, 0.3f);
            if (!isResourceTextBlinking) StartCoroutine(AlertUserOnNotEnoughResources());
            return;
        }
        SOUND_MANAGER.PlaySound(unlockSoundEffect, 0.3f);
        SavedData savedData = SaveSystem.LoadData() as SavedData;
        ArcaneVaultData loadData = savedData.arcaneVaultData;
        List<ItemData> items = savedData.items;
        savedData.diamondsAmount -= item.cost;
        resourceAmount.text = savedData.diamondsAmount.ToString();
        #region Deciding Which Characteristic Is Being Upgraded
        if (item.abilityName.Equals("armorMultiplier")) loadData.armorMultiplier += item.increment;
        else if (item.abilityName.Equals("healthMultiplier")) loadData.healthMultiplier += item.increment;
        else if (item.abilityName.Equals("backdoorProtection")) loadData.backdoorProtection += item.increment;
        #endregion
        ItemData i = items.Find(_item => _item.index == item.index && _item.pageIndex == item.pageIndex);
        i.cost *= 2;
        i.level++;
        item.costText.text = i.cost.ToString();
        item.levelText.text = i.level.ToString();
        item.cost *= 2;
        SaveSystem.SaveData(savedData.workerData, items, savedData.wallData, savedData.towerData, loadData, savedData.diamondsAmount);
    }
}
