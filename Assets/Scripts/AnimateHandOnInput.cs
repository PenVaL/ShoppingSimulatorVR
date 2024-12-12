using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
    
public class AnimateHandOnInput : MonoBehaviour {
    public Animator handAnimator;
    public InputActionProperty pinchAnimAction, gripAnimAction;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update()
    {
        float triggerValue = pinchAnimAction.action.ReadValue<float>();

        handAnimator.SetFloat("Trigger", triggerValue);

        float gripValue = gripAnimAction.action.ReadValue<float>();

        handAnimator.SetFloat("Grip", gripValue);
    }
}
