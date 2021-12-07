using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof (ARRaycastManager))]
public class SpawnObject : MonoBehaviour
{
    private ARRaycastManager raycastManager;
    private GameObject spawnedObject;
    private List<GameObject> placedPrefabList = new List<GameObject>();

    [SerializeField]
    private int maxPrefabSpawnCount=0;
    private int placedPrefabCount;
    [SerializeField]
    public GameObject placeablePrefab;


    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    private void Awake()
    {
        raycastManager=GetComponent<ARRaycastManager>();

    }
    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if(Input.GetTouch(0).phase == TouchPhase.Began)
        {
            touchPosition=Input.GetTouch(0).position;
            return true;
        }
        touchPosition =default;
        return false;
    }

    private void Update()
    {
        if(!TryGetTouchPosition(out Vector2 touchPosition ))
        {
            return;
        }
        if (raycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPos = s_Hits[0].pose;
            if (placedPrefabCount< maxPrefabSpawnCount)
            {
                SpawnPrefab(hitPos);
            }
            
        }
    }


    public void SetPrefabType(GameObject prefabType)
    {
        placeablePrefab=prefabType;
    }

    private void SpawnPrefab(Pose hitPos)
    {
        spawnedObject=Instantiate(placeablePrefab,hitPos.position, hitPos.rotation);
        placedPrefabList.Add(spawnedObject);
        placedPrefabCount++;

    }
}
