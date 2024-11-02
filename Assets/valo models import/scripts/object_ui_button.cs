using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections;

public class object_ui_button : XRBaseInteractable
{
    [SerializeField] private InputActionReference triggerPressAction; // Right trigger
    [SerializeField] private InputActionReference leftTriggerPressAction; // Left trigger
    [SerializeField] private Material hoverMaterial;
    [SerializeField] private Material activatedMaterial;
    private Material originalMaterial;

    // UnityEvents to assign actions through the Inspector
    [SerializeField] private UnityEvent onTriggerPressed;

    // Booleans to toggle the timers separately
    [SerializeField] private bool enableWaitTimeAfterActivation = false;
    [SerializeField] private bool enableHoverResetTime = false;

    // Time values editable in the Inspector
    [SerializeField] private float waitTimeAfterActivation = 0.25f;
    [SerializeField] private float hoverResetTime = 0.5f;

    private bool isHovering = false;
    private bool isActionInProgress = false;

    protected override void OnEnable()
    {
        base.OnEnable();
        triggerPressAction.action.started += OnTriggerPressed;
        leftTriggerPressAction.action.started += OnTriggerPressed;
        originalMaterial = GetComponent<Renderer>().material;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        triggerPressAction.action.started -= OnTriggerPressed;
        leftTriggerPressAction.action.started -= OnTriggerPressed;
    }

    private void OnTriggerPressed(InputAction.CallbackContext context)
    {
        if (isHovering && !isActionInProgress)
        {
            isActionInProgress = true; // Prevent multiple actions at the same time
            Debug.Log("Trigger pressed!");
            GetComponent<Renderer>().material = activatedMaterial;
            onTriggerPressed.Invoke();

            // Run the coroutine only if one of the toggles is enabled
            if (enableWaitTimeAfterActivation || enableHoverResetTime)
            {
                StartCoroutine(HandleHoverReset());
            }
            else
            {
                isActionInProgress = false; // If no countdowns, reset immediately
            }
        }
    }

    private IEnumerator HandleHoverReset()
    {
        if (enableWaitTimeAfterActivation)
        {
            yield return new WaitForSeconds(waitTimeAfterActivation);
        }

        // Simulate exiting the hover state only if hover reset is enabled
        if (enableHoverResetTime)
        {
            GetComponent<Renderer>().material = originalMaterial;
            isHovering = false;

            yield return new WaitForSeconds(hoverResetTime);

            // Check if the object is still being hovered over
            if (IsHoverInteractable() && !isHovering)
            {
                // Re-enter the hover state
                GetComponent<Renderer>().material = hoverMaterial;
                isHovering = true;
            }
        }

        isActionInProgress = false; // Allow new actions to be triggered
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        if (!isActionInProgress) // Only set hover if no action is in progress
        {
            GetComponent<Renderer>().material = hoverMaterial;
            isHovering = true;
        }
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);
        GetComponent<Renderer>().material = originalMaterial;
        isHovering = false;
    }

    // Helper function to check if the object is still being hovered
    private bool IsHoverInteractable()
    {
        return interactorsHovering.Count > 0;
    }
}
