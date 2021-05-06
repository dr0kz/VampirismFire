using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    public int cost { get; set; }
    public int index { get; set; }
    public int pageIndex { get; set; }
    public bool isVisible { get; set; }
    public int level { get; set; }
    public bool isPriceVisible { get; set; }
    public bool isButtonVisible { get; set; }
    public bool isResourceVisible { get; set; }
    public ItemData(int level, int cost, int index, int pageIndex, bool isVisible, bool isPriceVisible, bool isButtonVisible, bool isResourceVisible)
    {
        this.level = level;
        this.cost = cost;
        this.index = index;
        this.pageIndex = pageIndex;
        this.isVisible = isVisible;
        this.isPriceVisible = isPriceVisible;
        this.isButtonVisible = isButtonVisible;
        this.isResourceVisible = isResourceVisible;
    }
}

