using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using  UnityEngine.UI;

public class PrimaryMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private Transform centerEyeAnchor;
    private float distanceInFront = .45f;

    // Start is called before the first frame update
    void Start()
    {
         // Find Camera Rig to know where to spawn objects
        var ovrCameraRig = FindObjectOfType<OVRCameraRig>();
        if (ovrCameraRig != null)
        {
            centerEyeAnchor = ovrCameraRig.centerEyeAnchor;
        }
    }

    void Update() {
        if (centerEyeAnchor != null)
        {
            // Grab eye position of the user.
            Vector3 positionInFront = centerEyeAnchor.position + centerEyeAnchor.forward * distanceInFront;
            // Set the position of the game object
    
            menu.transform.position = positionInFront;
            menu.transform.rotation = Quaternion.LookRotation(centerEyeAnchor.forward);

        }
    }

    // singleUserToggleCase
    public void ToggleSingle() {
            SceneManager.LoadScene(1);
    }

    // multiUserToggle case
    // needs to be implemented
    public void ToggleMulti() {
        // SceneManager.LoadScene(2)
    }

    public void ToggleExit() {
        // if (exitAppToggle.isOn) {
        Application.Quit();
        // }
    }
}