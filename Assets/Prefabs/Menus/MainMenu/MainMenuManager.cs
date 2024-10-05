using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

// Specifically used for managing the Toggle objects' states
// that are in the MainMenu.
// Toggle 1: View Chemical List
// Toggle 2: VR to MR
// Toggle 3: Exit app
public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject chemList;
    [SerializeField] private Toggle chemListToggle;
    [SerializeField] private GameObject labPrefab;
    [SerializeField] private Toggle VrMrToggle;
    [SerializeField] private Toggle exitToggle;
    [SerializeField] private TextMeshProUGUI VrMrToggleText;
    [SerializeField] private TextMeshProUGUI chemListToggleText;

    // Start is called before the first frame update
    void Start()
    {
        // This is to differentiate values in the inspector
        chemListToggle.gameObject.name = "chemListToggle";
        VrMrToggle.gameObject.name = "VrMrToggle";
        VrMrToggleText.gameObject.name = "VrMrToggleText";
        chemListToggleText.gameObject.name = "ChemListToggleText";
        exitToggle.gameObject.name = "ExitToggle";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ViewChemList() {
        if(chemListToggle.isOn) {
            chemList.SetActive(true);
            chemListToggleText.text = "Hide ChemList";
        }
        else {
            chemList.SetActive(false);
            chemListToggleText.text = "Show ChemList";
        }
    }

    // Make sure this is called on the 'VR to MR' 
    // toggle's On Value Changed
    public void VirtualToMixedReality() {
        if (VrMrToggle.isOn) {
            labPrefab.SetActive(false);
            VrMrToggleText.text = "MR to VR";
        }
        else {
            labPrefab.SetActive(true);
            VrMrToggleText.text = "VR to MR";
        }
    }

    public void Exit() {
        if (exitToggle.isOn) {
            SceneManager.LoadScene(0);
        }
    }
}
