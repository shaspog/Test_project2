using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class ingame_menu : MonoBehaviour
{
    public GameObject menuObject; // Menu object to display
    public InputActionReference menuButtonAction; // Input action for menu toggle
    public bool isMenuActive = false;

    private void OnEnable()
    {
        menuButtonAction.action.performed += ToggleMenu;
    }

    private void OnDisable()
    {
        menuButtonAction.action.performed -= ToggleMenu;
    }

    // Method to toggle the menu that can be called from the Inspector
    public void ToggleMenuFromButton()
    {
        ToggleMenu(new InputAction.CallbackContext());
    }

    private void ToggleMenu(InputAction.CallbackContext context)
    {
        isMenuActive = !isMenuActive;
        menuObject.SetActive(isMenuActive);

        if (isMenuActive)
        {
            Time.timeScale = 0f; // Pause the game when the menu is active
        }
        else
        {
            Time.timeScale = 1f; // Resume the game when the menu is inactive
        }
    }
}
