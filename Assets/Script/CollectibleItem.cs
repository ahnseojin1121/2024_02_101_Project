using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public ItemType itemType;
    public string itemName;
    public float respawnTime = 30.0f;
    public bool canCollect = true;

    //아이템을 수집하는 메서드 , playerInventory를 통해 추가

    public void CollectItem(PlayerInventory inventory)
    {

        if (!canCollect) return;

        inventory.AddItem(itemType);

        if(FloatingTextManager.Instance != null)
        {
            Vector3 textPosition = transform.position + Vector3.up * 0.5f;
            FloatingTextManager.Instance.Show($"+ {itemName}", textPosition);
        }

        Debug.Log($"{itemName} 수집 완료");

        StartCoroutine(RespawnRoutine());

    }

    private IEnumerator RespawnRoutine()
    {
        canCollect = false;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<MeshCollider>().enabled = false;

        yield return new WaitForSeconds(respawnTime);

        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<MeshCollider>().enabled = true;
        canCollect = true;

    }
}
