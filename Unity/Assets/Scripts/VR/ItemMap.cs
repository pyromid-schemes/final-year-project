using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*
 * @author Japeth Gurr (jarg2)
 * Script for ItemMap prefab;
 * Stores default prefab transforms for specified GameObjects
*/
public class ItemMap : MonoBehaviour {
    public GameObject SwordPrefab;
    public GameObject ShieldPrefab;
    public GameObject TorchPrefab;

    private Dictionary<string, Transform> ItemTransforms;

    void Awake()
    {
        ItemTransforms = new Dictionary<string, Transform>();
        ItemTransforms.Add(SwordPrefab.name, SwordPrefab.transform);
        ItemTransforms.Add(ShieldPrefab.name, ShieldPrefab.transform);
        ItemTransforms.Add(TorchPrefab.name, TorchPrefab.transform);
    }

    public Transform GetDefaultTransform(string itemName)
    {
        Transform result;
        ItemTransforms.TryGetValue(itemName, out result);
        if (result != null)
        {
            return result;
        }
        else
        {
            throw new System.Exception("ItemMap.GetDefaultTransform() could not find the requested prefab");
        }
        
    }
}
