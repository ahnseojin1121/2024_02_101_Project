using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ConstructibleBuilding : MonoBehaviour
{
    [Header("Building Seetings")]
    public BuildingType buildingType;
    public string buildingName;
    public int requiredTree; 
    public float constructionTime = 2.0f;

    public bool canBuild = true;
    public bool isConstructed = false;

    private Material buildingMaterial;

    // Start is called before the first frame update
    void Start()
    {
        buildingMaterial = GetComponent<MeshRenderer>().material;

        Color color = buildingMaterial.color;
        color.a = 0.5f;
        buildingMaterial.color = color;
    }

    private IEnumerator ConstructionRoutine()
    {
        canBuild = false;
        float timer = 0;
        Color color = buildingMaterial.color;

        while (timer < constructionTime)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0.5f, 1f , timer / constructionTime);
            buildingMaterial.color = color;
            yield return null;
        }
        isConstructed = true;

        if(FloatingTextManager.Instance != null)
        {
            FloatingTextManager.Instance.Show($"{buildingName} 건설 완료!", transform.position + Vector3.up);
        }
    }

    public void StartConstruction(PlayerInventory inventory)
    {
        if (!canBuild || isConstructed) return;

        if (inventory.treeCount >= requiredTree)
        {
            inventory.Removeitem(ItemType.Tree, requiredTree);
            if (FloatingTextManager.Instance != null)
            {
                FloatingTextManager.Instance.Show($"{buildingName} 건설 시작!" , transform.position + Vector3.up);
            }
            StartCoroutine(ConstructionRoutine());
        }
        else
        {
            if (FloatingTextManager.Instance != null)
            {
                FloatingTextManager.Instance.Show($"나무가 부족합니다! ({inventory.treeCount} / {requiredTree})" , transform.position + Vector3.up);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
