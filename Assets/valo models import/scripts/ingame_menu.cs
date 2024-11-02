using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class ingame_menu : MonoBehaviour
{
    public Transform player; // Player's Transform
    public GameObject menuObject; // Menu object to display
    public InputActionReference menuButtonAction; // Input action for menu toggle
    public bool isMenuActive = false;

    public void OnEnable()
    {
        menuButtonAction.action.performed += ToggleMenu;
    }

    public void OnDisable()
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
            Vector3 menuPosition = player.position + player.forward * 2;
            menuPosition.y = 1; // Fix y position to 1
            menuObject.transform.position = menuPosition;

            // Restrict rotation to only face the player on Y-axis
            Vector3 lookDirection = player.position - menuObject.transform.position;
            lookDirection.y = 0; // Lock rotation on x-axis
            menuObject.transform.rotation = Quaternion.LookRotation(lookDirection);
            Time.timeScale = 0f; // Pause game
        }
        else
        {
            Time.timeScale = 1f; // Resume game
        }
    }
}
