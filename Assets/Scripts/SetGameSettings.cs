using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

public class SetGameSettings : MonoBehaviour
{
    [SerializeField] private ActionBasedSnapTurnProvider snapTurn;
    [SerializeField] private ActionBasedContinuousTurnProvider continTurn;
    [SerializeField] private ActionBasedContinuousMoveProvider movingHand;
    [SerializeField] private XRDirectInteractor leftInteractor, rightInteractor;
    [SerializeField] private GuideManager guideManager;

    public void SetGuideFromIndex(int index){
        if (index == 0) { // 0 = enable
            guideManager.needGuides = true;
        } else if (index == 1) { // 1 = disable
            guideManager.needGuides = false;
        }
    }

    public void SetTurnTypeFromIndex(int index) {
        if (index == 0) { // 0 = continuous turn
            snapTurn.enabled = false;
            continTurn.enabled = true;
        } else if (index == 1) { // 1 = snap turn
            snapTurn.enabled = true;
            continTurn.enabled = false;
        }
    }

    public void SetMoveHandFromIndex(int index) {
        if (index == 0) { // 0 = both hand move in movingHand
            movingHand.leftHandMoveAction.action.Enable();
            movingHand.rightHandMoveAction.action.Enable();
        } else if (index == 1) { // 1 = left hand move in movingHand, disable right hand
            movingHand.leftHandMoveAction.action.Enable();
            movingHand.rightHandMoveAction.action.Disable();
        } else if (index == 2) {// 2 = right hand move in movingHand, disable left hand
            movingHand.leftHandMoveAction.action.Disable();
            movingHand.rightHandMoveAction.action.Enable();
        }
    }

    public void SetTurnHandFromIndex(int index) {
        if (index == 0) { // 0 = both hand turn in continTurn and snapTurn
            snapTurn.leftHandSnapTurnAction.action.Enable();
            snapTurn.rightHandSnapTurnAction.action.Enable();
            continTurn.leftHandTurnAction.action.Enable();
            continTurn.rightHandTurnAction.action.Enable();
        } else if (index == 1) { // 1 = left hand turn in continTurn and snapTurn, disable right hand
            snapTurn.leftHandSnapTurnAction.action.Enable();
            continTurn.leftHandTurnAction.action.Enable();
            snapTurn.rightHandSnapTurnAction.action.Disable();
            continTurn.rightHandTurnAction.action.Disable();
        } else if (index == 2) { // 2 = right hand turn in continTurn and snapTurn , disable left hand
            snapTurn.rightHandSnapTurnAction.action.Enable();
            continTurn.rightHandTurnAction.action.Enable();
            snapTurn.leftHandSnapTurnAction.action.Disable();
            continTurn.leftHandTurnAction.action.Disable();
        }
    }

    public void SetHapticFromIndex(int index)
    {
        if (index == 0)
        { // 0 = enable haptic feedback in leftInteractor and rightInteractor
            leftInteractor.playHapticsOnSelectEntered = true;
            rightInteractor.playHapticsOnSelectEntered = true;
        }
        else if (index == 1)
        { // 1 = disable haptic feedback in leftInteractor and rightInteractor
            leftInteractor.playHapticsOnSelectEntered = false;
            rightInteractor.playHapticsOnSelectEntered = false;
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
