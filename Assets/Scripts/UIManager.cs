using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour {
    [SerializeField] private Transform playerHead; // Reference to the player's head transform
    [SerializeField, Range(0f, 10f)] private float spawnDist = 1.5f; // Distance from the player's head to spawn the UI
    [SerializeField, Range(-2f, 2f)] private float uiPositionX = 0f; // X-axis offset for the UI position
    [SerializeField, Range(-2f, 2f)] private float uiPositionY = 1.5f; // Y-axis offset for the UI position
    [SerializeField, Range(-2f, 2f)] private float uiPositionZ = 2f; // Z-axis offset for the UI position
    
    [SerializeField] private GameObject ui; // Reference to the UI game object
    [SerializeField] private InputActionProperty showButton; // Reference to the input action for showing/hiding the UI

    [SerializeField] private bool lookAtPlayerHeadY; // Variable to control the LookAt behavior

    void Update() {
        if (showButton.action.WasPressedThisFrame()) { // if the show/hide button was pressed
            // Toggle the UI's active state
            ui.SetActive(!ui.activeSelf);

            UpdateUIPosition();
        }

        // If the UI is active
        if (ui.activeSelf) {
            // Rotate the UI to face the player's head
            if (lookAtPlayerHeadY)
                ui.transform.LookAt(new Vector3(playerHead.position.x, playerHead.position.y, playerHead.position.z));
            else
                ui.transform.LookAt(new Vector3(playerHead.position.x, ui.transform.position.y, playerHead.position.z));
            
            ui.transform.forward *= -1; // Invert the forward direction
        }
    }

    private void UpdateUIPosition() {
        // Set the UI's position in front of the player's head
        ui.transform.position = playerHead.position + new Vector3(
            playerHead.forward.x * spawnDist + uiPositionX, // X-axis position
            playerHead.forward.y * spawnDist + uiPositionY, // Y-axis position
            playerHead.forward.z * spawnDist + uiPositionZ  // Z-axis position
        );
    }
}