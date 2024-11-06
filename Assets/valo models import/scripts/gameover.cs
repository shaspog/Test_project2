using UnityEngine;

public class gameover : MonoBehaviour
{
    public Transform player; // Player's Transform
    public GameObject menuObject; // Menu object to display
    public bool isMenuActive = false;
    public float menuDistance = 2.5f; // Distance in front of the player where the menu should appear
    public LayerMask obstructingLayers; // Layers that could obstruct the menu's path (e.g., walls)

    private void OnEnable()
    {
        // Every time the script is enabled, toggle the menu
        ToggleMenu();
    }

    private void ToggleMenu()
    {
        isMenuActive = !isMenuActive;
        menuObject.SetActive(isMenuActive);

        if (isMenuActive)
        {
            Vector3 menuPosition = GetMenuPosition();
            menuObject.transform.position = menuPosition;

            // Restrict rotation to only face the player on Y-axis
            Vector3 lookDirection = player.position - menuObject.transform.position;
            lookDirection.y = 0; // Lock rotation on x-axis
            menuObject.transform.rotation = Quaternion.LookRotation(lookDirection);

            Time.timeScale = 0f; // Pause the game
        }
        else
        {
            Time.timeScale = 1f; // Resume the game
        }
    }

    private Vector3 GetMenuPosition()
    {
        // Raycast from the player in the direction they are looking
        RaycastHit hit;
        Vector3 targetPosition = player.position + player.forward * menuDistance;

        // Check if there's an obstacle in the way
        if (Physics.Raycast(player.position, player.forward, out hit, menuDistance, obstructingLayers))
        {
            // If there is an obstruction, set the menu in front of the obstacle, just before it
            targetPosition = hit.point - player.forward * 0.5f; // Adjust to prevent clipping with the obstacle
        }

        // Ensure the y-position stays fixed (if needed)
        targetPosition.y = 1f; // Adjust to the desired height

        return targetPosition;
    }

    // Public method to resume the game from another script
    public void ResumeGame()
    {
        Time.timeScale = 1f; // Resume the game
    }
}
