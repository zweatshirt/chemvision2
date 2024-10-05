using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    /* Zachery Linscott for ChemVision 8/2024
    * Attach this script to a ChemicalListMenu asset.
    *
    * In order to use this script, add in-scence prefabs 
    * you want spawned to the prefab list in the inspector.
    *
    * This script takes the names of the prefabs put into the prefabList, 
    * populates the list with the names, and on click of one of the list elements 
    * containing the prefab name, it spanws the asset.
    */

    [SerializeField] private List<GameObject> prefabList;  // List of prefabs
    [SerializeField] private List<String> prefabNames;
    [SerializeField] private Toggle menuItemPrefab;  // Reference to the menu item prefab
    [SerializeField] private GameObject menuContainer;    // Reference to the container where items will be added
    void Start()
    {
        PopulateNamesList();
        PopulateMenu();
    }

    // Look into canvas force update
    void PopulateMenu() {
        // Redundant, will change later
        GameObject parentContainer = menuContainer;

        for (int i = 0; i < prefabNames.Count; i++) {
            Toggle newItem = Instantiate(menuItemPrefab, parentContainer.transform);
            if (newItem != null)
            {
                Debug.Log($"Instantiated new item: {prefabNames[i]}");
            }

            else
            {
                Debug.LogError("Failed to instantiate new item.");
            }

            TextMeshProUGUI itemText = newItem.GetComponentInChildren<TextMeshProUGUI>();
            if (itemText != null)
            {
                // change to itemText.text = prefabList[i].name
                itemText.text = prefabNames[i];
            }
        }
    }


    // delete this
    void PopulateNamesList() {
        for (int i = 0; i < prefabList.Count; i++)
        {
            Debug.Log("Prefab Name: " + prefabList[i].name);
            prefabNames.Add(prefabList[i].name);
        }
        prefabNames.Sort();
    }

}
