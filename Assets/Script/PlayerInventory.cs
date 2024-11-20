using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private SurvivalStats survivalStats;

    //각각의 아이템 개수를 저장하는 변수
    public int crystalCount = 0;
    public int plantCount = 0;
    public int bushCount = 0;
    public int treeCount = 0;

    public void Start()
    {
        survivalStats = GetComponent<SurvivalStats>();
    }

    public void UseItem(ItemType itemType)
    {
        if(GetItemCount(itemType) <= 0)
        {
            return;
        }

        switch (itemType)     
        {
            case ItemType.VegetableStew:
                Removeitem(ItemType.VegetableStew, 1);
                survivalStats.EatFood(RecipeList.KitchenRecipes[0].hungerRestoreAmount);
                break;
            case ItemType.FruitSalad:
                Removeitem(ItemType.FruitSalad, 1);
                survivalStats.EatFood(RecipeList.KitchenRecipes[1].hungerRestoreAmount);
                break;
            case ItemType.RepairKit:
                Removeitem(ItemType.RepairKit, 1);
                survivalStats.EatFood(RecipeList.WorkbenchRecipes[0].repairAmount);
                break;


        }

    }


    public void AddItem(ItemType itemType, int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            AddItem(itemType);
        }
    }
    public void AddItem(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Crystal:
                crystalCount++;
                Debug.Log($"크리스탈 획득! 현재 개수 : {crystalCount}");
                break;
            case ItemType.Plant:
                plantCount++;
                Debug.Log($"식물 획득! 현재 개수 : {plantCount}");
                break;
            case ItemType.Bush:
                bushCount++;
                Debug.Log($"수풀 획득! 현재 개수 : {bushCount}");
                break;
            case ItemType.Tree:
                treeCount++;
                Debug.Log($"나무 획득! 현재 개수 : {treeCount}");
                break;
        }
    }

    public bool Removeitem (ItemType itemType, int amount = 1)
    {
        switch (itemType)
        {
            case ItemType.Crystal:
                if (crystalCount >= amount)
                {
                    crystalCount -= amount;
                    Debug.Log($"크리스탈 {amount} 사용! 현재 개수 :{crystalCount}");
                    return true;
                }
                break;
            case ItemType.Plant:
                if (plantCount >= amount)
                {
                    plantCount -= amount;
                    Debug.Log($"식물 {amount} 사용! 현재 개수 :{plantCount}");
                    return true;
                }
                break;
            case ItemType.Bush:
                if (bushCount >= amount)
                {
                   bushCount -= amount;
                   Debug.Log($"덤불 {amount} 사용! 현재 개수 :{bushCount}");
                   return true;
                }
                break;
            case ItemType.Tree:
                if (treeCount >= amount)
                {
                    treeCount -= amount;
                    Debug.Log($"나무 {amount} 사용! 현재 개수 :{treeCount}");
                    return true;
                }
                break;
        }

        Debug.Log($"{itemType} 아이템이 부족합니다. ");
        return false;
    }

    public int GetItemCount(ItemType itemType)
    {

        switch (itemType) 
        { 
            case ItemType.Crystal:
                return crystalCount;
            case ItemType.Plant:
                return plantCount;
            case ItemType.Bush:
                return bushCount;
            case ItemType.Tree:
                return treeCount;
            default:
                return 0;
        
        }



    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private void ShowInventory()
    {
        Debug.Log("====인벤토리====");
        Debug.Log($"크리스탈:{crystalCount}개");
        Debug.Log($"식물:{plantCount}개");
        Debug.Log($"수풀:{bushCount}개");
        Debug.Log($"나무:{treeCount}개");
        Debug.Log("================");
    }
}
