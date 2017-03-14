using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemMap : MonoBehaviour {
    public GameObject SwordPrefab;
    public GameObject ShieldPrefab;
    //public GameObject DaggerPrefab;
    public GameObject LancePrefab;
    //public GameObject MedievalSwordPrefab;

    private Dictionary<string, Transform> ItemTransforms;

    void Awake()
    {
        ItemTransforms = new Dictionary<string, Transform>();
        ItemTransforms.Add(SwordPrefab.name, SwordPrefab.transform);
        ItemTransforms.Add(ShieldPrefab.name, ShieldPrefab.transform);
        //ItemTransforms.Add(DaggerPrefab.name, DaggerPrefab.transform);
        ItemTransforms.Add(LancePrefab.name, LancePrefab.transform);
        //ItemTransforms.Add(MedievalSwordPrefab.name, MedievalSwordPrefab.transform);
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
