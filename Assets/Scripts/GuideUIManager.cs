using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GuideUIManager : MonoBehaviour
{
    [SerializeField] private Transform playerHead; // Reference to the player's head transform
    [SerializeField, Range(0f, 10f)] private float spawnDist = 1.5f; // Distance from the player's head to spawn the UI
    [SerializeField, Range(-2f, 2f)] private float uiPositionX = 0f; // X-axis offset for the UI position
    [SerializeField, Range(-2f, 2f)] private float uiPositionY = 1.5f; // Y-axis offset for the UI position
    [SerializeField, Range(-2f, 2f)] private float uiPositionZ = 2f; // Z-axis offset for the UI position

    [SerializeField] private GameObject guideUI; // Reference to the UI game object
    [SerializeField] private GameObject skipTaskUI;
    [SerializeField] private InputActionProperty showButton; // Reference to the input action for showing/hiding the UI

    private GuideManager guideManager;
    [SerializeField] private float uiUpdateTimer = 10f; // Timer for updating the UI position
    [SerializeField] private float skipTaskTimer = 40f; // Timer for displaying the skip task UI

    private bool isUpdateUITimerRunning = false;
    private bool isSkipTaskTimerRunning = false;

    private GameManager gameManager;

    void Start() {
        gameManager = GameManager.Instance; // Assign the GameManager instance
        guideManager = FindObjectOfType<GuideManager>(); // Find GuideManager in the scene
        skipTaskUI.SetActive(false); // Hide the skip task UI
    }

    void Update() {
        if (guideManager.needGuides) {
            if (!guideUI.activeSelf)
                guideUI.SetActive(true);
            if (!isUpdateUITimerRunning) { // update the position of ui every 10 second
                StartCoroutine(UpdateUIPositionTimer());
            }

            if (!isSkipTaskTimerRunning) {
                StartCoroutine(SkipTaskTimer()); // message pop up asking if player want to skip current task
            }
            
            if (showButton.action.WasPressedThisFrame()) // if the show/hide button was pressed
                UpdateUIPosition();

            guideUI.transform.LookAt(new Vector3(playerHead.position.x, guideUI.transform.position.y, playerHead.position.z));
            guideUI.transform.forward *= -1; // Invert the forward direction
        } 
        else {
            if (guideUI.activeSelf) { // if guideUI is not closed, turn it off 
                StopAllCoroutines();
                isUpdateUITimerRunning = false;
                isSkipTaskTimerRunning = false;
                guideUI.SetActive(false);
            }
        }
    }

    private void UpdateUIPosition() {
        // Set the UI's position in front of the player's head
        guideUI.transform.position = playerHead.position + new Vector3(
                playerHead.forward.x * spawnDist + uiPositionX, // X-axis position
                playerHead.forward.y * spawnDist + uiPositionY, // Y-axis position
                playerHead.forward.z * spawnDist + uiPositionZ  // Z-axis position
        );
    }

    private IEnumerator UpdateUIPositionTimer() {
        isUpdateUITimerRunning = true;

        while (true) {
            yield return new WaitForSeconds(uiUpdateTimer);
            isUpdateUITimerRunning = false; // set false to wait for next UpdateUIPosition()
            UpdateUIPosition();
        }
    }

    private IEnumerator SkipTaskTimer() {
        isSkipTaskTimerRunning = true;
        while (true) {
            yield return new WaitForSeconds(skipTaskTimer);
            skipTaskUI.SetActive(true);
        }
    }

    public void SkipCurrentTask() {
        isSkipTaskTimerRunning = false; // Stop the current skip task timer
        skipTaskUI.SetActive(false); // Hide the skip task UI
        gameManager.SkipCurrentTask();
    }
}