using UnityEngine;
using System.Collections;

public class ItemMap : MonoBehaviour {
    public GameObject swordPrefab;

    public Transform GetDefaultTransform(string itemName)
    {
        switch (itemName)
        {
            case "Block Sword":
                return swordPrefab.transform;
            default:
                throw new System.Exception("ItemMap.GetDefaultTransform() could not find the requested prefab");
        }
    }
}
