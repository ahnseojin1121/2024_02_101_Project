using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

//GridCell 클래스는 각 그리드 셀의 상태와 데이터를 관리
public class GridCell
{
    public Vector3Int Position;
    public bool IsOccupied;
    public GameObject Building;

    public GridCell(Vector3Int position)
    {
        Position = position;
        IsOccupied = false;
        Building = null;
    }

}
public class GridBuildingSystem : MonoBehaviour
{
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [SerializeField] private float cellsize = 1;

    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private GameObject buildingPrefabs;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float maxBuildDistance = 5f;


    [SerializeField] private Grid grid;
    private GridCell[,] cells;
    private Camera firstPersonCamera;



    // Start is called before the first frame update
    void Start()
    {
        firstPersonCamera = playerController.firstPersonCamera;
        CreateGrid();
    }

    void Update()
    {
        Vector3 lookPosition = GetLookposition();
        if(lookPosition == Vector3.zero)
        {
            Vector3Int gridPosition = grid.WorldToCell(lookPosition);
            if(isValidGridPosition(gridPosition))
            {
                HighlightCell(gridPosition);
                if(Input.GetMouseButtonDown(0))
                {
                    PlaceBuilding(gridPosition);
                }
                if(Input.GetMouseButtonDown(1))
                {
                    RemoveBuilding(gridPosition); 
                }

            }
        }
    }

    private void PlaceBuilding(Vector3Int gridPosition)
    {
        GridCell cell = cells[gridPosition.x, gridPosition.z];

        if(!cell.IsOccupied)
        {
            Vector3 worldPosition = grid.GetCellCenterWorld(gridPosition);
            GameObject building = Instantiate(buildingPrefabs, worldPosition, Quaternion.identity);
            cell.IsOccupied = true;
            cell.Building = building;
        }
    }

    private void RemoveBuilding(Vector3Int gridPosition)
    {
        GridCell cell = cells[gridPosition.x, gridPosition.z];

        if (!cell.IsOccupied)
        {
            Destroy(cell.Building);
            cell.IsOccupied = true;
            cell.Building = null;
        }
    }   
        
    private void CreateGrid()
    {
        grid.cellSize = new Vector3(cellsize, cellsize, cellsize);
        cells = new GridCell[width, height];
        Vector3 gridCenter = playerController.transform.position;
        gridCenter.y = 0;
        transform.position = gridCenter - new Vector3(width * cellsize / 2.0f, 0, height * cellsize / 2);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3Int cellposition = new Vector3Int(x, 0, z);
                Vector3 worldPosition = grid.GetCellCenterWorld(cellposition);
                GameObject cellObject = Instantiate(cellPrefab, worldPosition, cellPrefab.transform.rotation);
                cellObject.transform.SetParent(transform);

                cells[x, z] = new GridCell(cellposition);
            }
        }
    }

    //선택된 셀을 하이라이트하는 함수
    private void HighlightCell(Vector3Int gridPosition)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GameObject cellObjecct = 
                    cells[x, z].Building!=null?cells[x,z].Building : transform.GetChild(x * height + z).gameObject;
                cellObjecct.GetComponent<Renderer>().material.color = Color. white;
                   
            }
        }

        GridCell cell = cells[gridPosition.x, gridPosition.z];
        GameObject highlightObject =
            cell.Building!=null ? cell.Building:transform.GetChild(gridPosition.x*height + gridPosition.z).gameObject;
        highlightObject.GetComponent<Renderer>().material.color = cell.IsOccupied? Color.red : Color.green;
    }
    //
    private bool isValidGridPosition(Vector3Int gridPosition)
    {
        return gridPosition.x >= 0 && gridPosition.x < width &&
            gridPosition.z >= 0 && gridPosition.z < height;
    }

    private Vector3 GetLookposition()
    {
        if (playerController.isFirstPerson)
        {
            Ray ray = new Ray(firstPersonCamera.transform.position, firstPersonCamera.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, maxBuildDistance))
            {
                Debug.DrawRay(ray.origin, ray.direction * hitInfo.distance, Color.red);
                return hitInfo.point;
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * hitInfo.distance, Color.white);
            }
        }

        else
        {
            Vector3 characterPosition = playerController.transform.position;
            Vector3 characterForward = playerController.transform.forward;
            Vector3 rayOrigin = characterPosition + Vector3.up * 1.5f + characterForward * 0.5f;
            Vector3 rayDirection = (characterForward - Vector3.up).normalized;

            Ray ray = new Ray(firstPersonCamera.transform.position, firstPersonCamera.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, maxBuildDistance))
            {
                Debug.DrawRay(ray.origin, ray.direction * hitInfo.distance, Color.red);
                return hitInfo.point;
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * hitInfo.distance, Color.white);
            }
        }

        return Vector3.zero;
    }

    private void OnDrawGizmos()   //Gizmo를 표시해주는 함수
        {
            Gizmos.color = Color.blue;
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    // cellCenter = grid.GetCellCenterWorld(new Vector3Int(x, 0, z));
                    //Gizmos.DrawWireCube(cellCenter, new Vector3(cellsize, 0.1f, cellsize));
                }
            }
        }
    } 
    
    
