﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemMap : MonoBehaviour {
    public GameObject SwordPrefab;
    public GameObject ShieldPrefab;
    public GameObject TorchPrefab;
    //public GameObject DaggerPrefab;
    //public GameObject MedievalSwordPrefab;

    private Dictionary<string, Transform> ItemTransforms;

    void Awake()
    {
        ItemTransforms = new Dictionary<string, Transform>();
        ItemTransforms.Add(SwordPrefab.name, SwordPrefab.transform);
        ItemTransforms.Add(ShieldPrefab.name, ShieldPrefab.transform);
        ItemTransforms.Add(TorchPrefab.name, TorchPrefab.transform);
        //ItemTransforms.Add(DaggerPrefab.name, DaggerPrefab.transform);
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
