﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class ShowHideMenu : MonoBehaviour {

    public GameObject HelpMenu;

    SteamVR_TrackedObject TrackedObj;
    SteamVR_Controller.Device Device;

    void Awake()
    {
        TrackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    void FixedUpdate()
    {
        Device = SteamVR_Controller.Input((int)TrackedObj.index);
        if (Device.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            HelpMenu.SetActive(!HelpMenu.activeSelf);
        }
    }
}
