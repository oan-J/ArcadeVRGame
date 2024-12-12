using UnityEngine;

public class AIMalletScript : MonoBehaviour
{
    public float speed = 5.0f; // Speed at which the AI mallet moves

    private void OnEnable()
    {
        PuckScript.OnPuckPositionChange += MoveTowardsPuck;
    }

    private void OnDisable()
    {
        PuckScript.OnPuckPositionChange -= MoveTowardsPuck;
    }

    private void MoveTowardsPuck(Vector3 puckPosition)
    {
        // Move towards the puck position at a defined speed
        Vector3 targetPosition = new Vector3(puckPosition.x, transform.position.y, puckPosition.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }
}