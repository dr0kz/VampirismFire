using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUnlocker : Item
{
    [SerializeField] public GameObject resourceImage;
    [SerializeField] public GameObject buyButton;
    [SerializeField] public List<Item> itemsToUnlock;
    public override void LoadItemInfo(ItemData item)
    {
        base.LoadItemInfo(item);
        resourceImage.SetActive(item.isResourceVisible);
        buyButton.SetActive(item.isButtonVisible);
        costText.gameObject.SetActive(item.isPriceVisible);

    }
}
