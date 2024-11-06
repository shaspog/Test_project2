using UnityEngine;

public class ingamemenu : MonoBehaviour
{
    public Transform player; // Player's Transform
    public GameObject menuObject; // Menu object to display
    public float menuDistance = 2.5f; // Distance in front of the player where the menu should appear
    public LayerMask obstructingLayers; // Layers that could obstruct the menu's path (e.g., walls)

    private void OnEnable()
    {
        // Position the menu every time the script is enabled
        PositionMenu();
    }

    private void PositionMenu()
    {
        Vector3 menuPosition = GetMenuPosition();
        menuObject.transform.position = menuPosition;

        // Make the menu face the player, restricting rotation to only the Y-axis
        Vector3 lookDirection = player.position - menuObject.transform.position;
        lookDirection.y = 0; // Lock rotation on the x-axis to keep the menu upright
        menuObject.transform.rotation = Quaternion.LookRotation(lookDirection);
    }

    private Vector3 GetMenuPosition()
    {
        // Raycast from the player in the direction they are looking
        RaycastHit hit;
        Vector3 targetPosition = player.position + player.forward * menuDistance;

        // Check if there's an obstacle in the way
        if (Physics.Raycast(player.position, player.forward, out hit, menuDistance, obstructingLayers))
        {
            // If there is an obstruction, place the menu just before the obstacle to prevent clipping
            targetPosition = hit.point - player.forward * 0.5f; // Adjust distance from the obstacle
        }

        // Ensure the y-position stays fixed at a specified height
        targetPosition.y = 1f; // Adjust to your desired height

        return targetPosition;
    }
}
