using UnityEngine;
using System.Collections;

public class ItemMap : MonoBehaviour {
    public GameObject swordPrefab;
    public GameObject lancePrefab;

    public Transform GetDefaultTransform(string itemName)
    {
        switch (itemName)
        {
            case "PlayerSword":
                return swordPrefab.transform;
            case "PlayerLance":
                return lancePrefab.transform;
            default:
                throw new System.Exception("ItemMap.GetDefaultTransform() could not find the requested prefab");
        }
    }
}
