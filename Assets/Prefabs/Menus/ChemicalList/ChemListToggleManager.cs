using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ChemListToggleManager : MonoBehaviour
{
    [SerializeField] Toggle toggle;
    [SerializeField] Transform toggleTransform;
    [SerializeField] TextMeshProUGUI toggleText;
    [SerializeField] private Transform centerEyeAnchor;
    [SerializeField] private GameObject spawned;
    [SerializeField] private GameObject spawnedNote;

    void Start()
    {
        // Find Camera Rig to know where to spawn objects
        var ovrCameraRig = FindObjectOfType<OVRCameraRig>();
        if (ovrCameraRig != null)
        {
            centerEyeAnchor = ovrCameraRig.centerEyeAnchor;
        }
    }

    void Update()
    {  
        Debug.Log("Toggle text: " + toggleText.text);
        if (centerEyeAnchor != null)
        {
            // Grab eye position of the user.
            Vector3 eyePosition = centerEyeAnchor.position;
        }
    }

    public void SpawnObject() {
        // For some reason, when I try to delete objects, they simply do not delete.

        if (toggle.isOn) {
        // Spawn object at user location (can be shifted forward so it's not directly on top of the person)
            spawned = (GameObject) Instantiate(Resources.Load(toggleText.text));
            
            // change location of this to in front of the face
            spawned.transform.position = centerEyeAnchor.position;
            string spawnedNoteName = toggleText.text + "Note";
            spawnedNote = (GameObject) Instantiate(Resources.Load(spawnedNoteName));
            if (spawnedNote != null) {
                spawnedNote.transform.position = centerEyeAnchor.position;
            }
            //H20Note
            // toggleText.text + "Note"
        }
        else {
            DestroyImmediate(spawned);
            if (spawnedNote != null) DestroyImmediate(spawnedNote);
        }
    }
}
