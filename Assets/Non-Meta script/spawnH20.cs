using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnH20 : MonoBehaviour
{
    public GameObject itemToSpawn;   // The molecular item to spawn in front of the camera
    public GameObject notePrefab;    // The note prefab to spawn beside the molecular item
    public Vector3 spawnRotation = Vector3.zero; // Initial rotation to apply to the spawned item
    public float spawnDistance = 1.0f; // Distance from the camera to spawn the item (closer distance)
    public float noteOffset = 0.05f; // Offset distance for the note relative to the camera
    public float noteDistance = 0.05f; // Distance from the camera to spawn the note (closer distance)

    private GameObject spawnedItem;  // Reference to the spawned molecular item
    private GameObject spawnedNote;  // Reference to the spawned note
    private bool isLookingAtObject = false;  // Flag to check if the player is looking at the molecular object
    private bool isLookingAtNote = false;   // Flag to check if the player is looking at the note
    private float timeLookingAway = 0f;     // Time since the player last looked at the note
    private Camera mainCamera; // Cached camera reference

    void Start()
    {
        // Cache the main camera reference
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Check if the player is looking at this molecular object
        isLookingAtObject = CheckIfLookingAtObject(this.gameObject);

        // Check if the player is looking at the note
        if (spawnedNote != null)
        {
            isLookingAtNote = CheckIfLookingAtObject(spawnedNote);

            // Reset the timer if the player is looking at the note
            if (isLookingAtNote)
            {
                timeLookingAway = 0f;
            }
            else
            {
                // Increment the timer if the player is not looking at the note
                timeLookingAway += Time.deltaTime;

                // Destroy the note if the player has not looked at it for 10 seconds
                if (timeLookingAway >= 10f)
                {
                    Destroy(spawnedNote);
                }
            }
        }

        // If the player is looking at this molecular object and presses the "A" button, spawn or replace the item
        if (isLookingAtObject && OVRInput.GetDown(OVRInput.Button.One))
        {
            // Destroy the previously spawned molecular item and note if they exist
            if (spawnedItem != null)
            {
                Destroy(spawnedItem);
            }
            if (spawnedNote != null)
            {
                Destroy(spawnedNote);
            }

            // Get the position closer to the camera for the new molecular item
            Vector3 spawnPosition = mainCamera.transform.position + mainCamera.transform.forward * spawnDistance;

            // Instantiate the new molecular item with the specified rotation
            spawnedItem = Instantiate(itemToSpawn, spawnPosition, Quaternion.Euler(spawnRotation));

            // Instantiate the note at the right side of the camera and closer distance
            Vector3 notePosition = mainCamera.transform.position + mainCamera.transform.right * noteOffset + mainCamera.transform.forward * noteDistance;
            spawnedNote = Instantiate(notePrefab, notePosition, Quaternion.identity);

            // Make both the molecular item and the note face the camera
            LookAtCamera(spawnedItem);
            LookAtCamera(spawnedNote);

            // Reset the timer for looking away from the note
            timeLookingAway = 0f;
        }

        // Rotate the spawned molecular item 90 degrees to the left when the "B" button is pressed
        if (spawnedItem != null && OVRInput.GetDown(OVRInput.Button.Two))
        {
            // Rotate the spawned molecular item 90 degrees around the Y-axis (left rotation)
            spawnedItem.transform.Rotate(0, -90, 0);
        }
    }

    // Check if the player is looking at a specified object
    private bool CheckIfLookingAtObject(GameObject target)
    {
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Check if the object being looked at is the target object
            if (hit.collider.gameObject == target)
            {
                return true;
            }
        }

        return false;
    }

    // Make an object look at the camera
    private void LookAtCamera(GameObject obj)
    {
        if (obj != null)
        {
            // Make the object look at the camera
            obj.transform.LookAt(mainCamera.transform);

            // Optionally, you might want to adjust the rotation to keep the object's upright orientation
            // Adjust this based on your object's orientation and needs
            obj.transform.rotation = Quaternion.Euler(
                obj.transform.rotation.eulerAngles.x, 
                obj.transform.rotation.eulerAngles.y, 
                0f // Adjust this to ensure the object is upright
            );
        }
    }
}

