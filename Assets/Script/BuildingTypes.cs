using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType
{
    CraftingTable,
    Furance,
    Kitchen,
    Storage
}

[System.Serializable]

public class CraftingRecipe
{
    public string itemName;
    public ItemType resultItem;
    public int resultAmount = 1;
    public ItemType[] requireditems;
    public int[] requiredAmounts;
}