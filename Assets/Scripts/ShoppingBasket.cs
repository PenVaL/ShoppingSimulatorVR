using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShoppingBasket : MonoBehaviour
{
    [SerializeField] private Transform leftHandle, rightHandle;
    [SerializeField] private Quaternion initRot_L, initRot_R, holdRot_L, holdRot_R;

    [SerializeField] private GameObject basketObject, basketSpawnPoint, basketResetPosNearPlayer;

    [SerializeField] private GameObject checkMark;
    [SerializeField] private AudioClip successSFX, failureSFX;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private InputActionProperty resetPosButton; // Reference to the input action for reseting shopping basket pos

    void Update() {
        if (resetPosButton.action.WasPressedThisFrame()) { //reset shopping basket to player pos
            basketObject.transform.position = basketResetPosNearPlayer.transform.position;
            basketObject.transform.rotation = basketResetPosNearPlayer.transform.rotation;
        }
    }

    void OnTriggerEnter(Collider other) {
        // Check if the collided object has the Goods component
        Goods goods = other.GetComponent<Goods>();
        if (goods != null) {
            // Decrement the number needed for this goods type
            bool isDecremented = GameManager.Instance.DecrementShoppingListItem(goods);

            // Feedback if the item was successfully added to the basket
            ProvideFeedback(isDecremented);
        }
    }

    public void ProvideFeedback(bool isDecremented) {
        if (isDecremented) {
            audioSource.PlayOneShot(successSFX); // SFX
            StartCoroutine(SuccessVFX(1.2f)); // VFX
        } else {
            audioSource.PlayOneShot(failureSFX); // SFX
        }
    }

    private IEnumerator SuccessVFX(float waitTime) {
        checkMark.SetActive(true); // VFX
        yield return new WaitForSeconds(waitTime);
        checkMark.SetActive(false); // Hide the checkMark
    }

    public void ToggleHandleRotation(bool isHolding) {
        if (isHolding) {
            // Set the handles to the holding rotation
            leftHandle.localRotation = holdRot_L;
            rightHandle.localRotation = holdRot_R;
        }
        else
        {
            // Set the handles back to the initial rotation
            leftHandle.localRotation = initRot_L;
            rightHandle.localRotation = initRot_R;
        }
    }

    public void ResetShoppingBasketPosition() {
        basketObject.transform.position = basketSpawnPoint.transform.position;
        basketObject.transform.rotation = basketSpawnPoint.transform.rotation;
    }
}