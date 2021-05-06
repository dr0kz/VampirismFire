using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Item : MonoBehaviour
{
    [SerializeField] public bool isVisible;
    [SerializeField] public int index;
    [SerializeField] public int pageIndex;
    [SerializeField] public TextMeshProUGUI descriptionObject;
    [SerializeField] public string description;
    [SerializeField] public TextMeshProUGUI costText;
    [SerializeField] public int cost;
    [SerializeField] public TextMeshProUGUI levelText;
    [SerializeField] public int level;
    public virtual void LoadItemInfo(ItemData item)
    {
        isVisible = item.isVisible;
        gameObject.SetActive(isVisible);
        level = item.level;
        cost = item.cost;
        descriptionObject.text = description;
        costText.text = cost.ToString();
        levelText.text = level.ToString();
    }
}


