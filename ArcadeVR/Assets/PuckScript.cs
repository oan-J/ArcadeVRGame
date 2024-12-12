using UnityEngine;

public class PuckScript : MonoBehaviour
{
    // Event to signal the puck's position update
    public delegate void PuckPositionHandler(Vector3 position);
    public static event PuckPositionHandler OnPuckPositionChange;

    private void Update()
    {
        // Check if the z position of the puck is greater than 0.4
        if (transform.position.z > 0.4f)
        {
            // Invoke the event with the current position
            OnPuckPositionChange?.Invoke(transform.position);
        }
    }
}