using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ItemType
{
    Crystal,
    Plant,
    Bush,
    Tree,
    VegetableStew,
    FruitSalad,
    RepairKit
}

public class ItemDetector : MonoBehaviour
{
    public float checkRadius = 3.0f;
    public Vector3 lastPosition;
    private float moveThreshold = 0.1f;
    public CollectibleItem currentNearbyItem;



    void Start()
    {

        lastPosition = transform.position;
        CheckForItems();
        
    }



    void Update()
    {

        //플레이어가 일정 거리 이상 이동했는지 체크
        if (Vector3.Distance(lastPosition, transform.position) > moveThreshold) 
        {

            CheckForItems();
            lastPosition = transform.position;

        }

        if (currentNearbyItem != null && Input.GetKeyDown(KeyCode.E)) 
        { 

         currentNearbyItem.CollectItem(GetComponent<PlayerInventory>());

        }
    }


    private void CheckForItems()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, checkRadius);

        float closestDistance = float.MaxValue;
        CollectibleItem closestItem = null;

        //각 콜라이더를 검사하여 수집 가능한 아이템을 찾음
        foreach (Collider collider in hitColliders)
        {
            CollectibleItem item = collider.GetComponent<CollectibleItem>();
            if (item != null && item.canCollect)
            {
                float distance = Vector3.Distance(transform.position, item.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestItem = item;
                }
            }



        }

        if (closestItem != currentNearbyItem)
        {
            currentNearbyItem = closestItem;
            if (currentNearbyItem != null)
            {
                
                Debug.Log($"[E] 키를 눌러 {currentNearbyItem.itemName} 수집 ");

            }


        }
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
