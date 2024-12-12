using UnityEngine;

public class MalletMover : MonoBehaviour
{
    private static MalletMover instance;

    public float normalSpeed = 5f;
    public float quickSpeed = 50f; // Increased for very quick movement
    public float range = 2f;
    public GameObject puck;
    private Rigidbody2D puckRb;

    private bool forceApplied = false;
    private float forceStrength = -500f; // Modify this value if needed
    private float maxMalletZ = 0.719f;
    private float maxPuckZ = 0.7f;

    private float minX;
    private float maxX;
    private float startX;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    void Start()
    {
        startX = transform.position.x;
        minX = startX - range;
        maxX = startX + range;

        if (puck != null)
            puckRb = puck.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (puck == null) return;

        // Ensure puck's z position stays below maximum
        if (puck.transform.position.z > maxPuckZ)
        {
            Vector3 puckPos = puck.transform.position;
            puckPos.z = maxPuckZ;
            puck.transform.position = puckPos;
        }

        if (puck.transform.position.z > 0.4f)
        {
            Vector3 targetPosition = new Vector3(
                Mathf.Clamp(puck.transform.position.x, minX, maxX),
                transform.position.y,
                Mathf.Min(puck.transform.position.z, maxMalletZ));

            // Calculate the speed needed to reach the target in 0.1s
            float distance = Vector3.Distance(transform.position, targetPosition);
            float requiredSpeed = distance / 0.1f;  // Speed = Distance / Time

            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * requiredSpeed * Time.deltaTime;

            // Apply a large force if not already applied
            if (!forceApplied)
            {
                puckRb.AddForce(new Vector3(0, 0, forceStrength), ForceMode2D.Impulse);
                forceApplied = true;
            }
        }
        else
        {
            // Reset force application
            forceApplied = false;
            DefendingBehavior();
        }
    }

    void DefendingBehavior()
    {
        // Predict and move to the puck's position along the x-axis only
        Vector2 predictedPosition = PredictPuckPosition();
        MoveToPosition(predictedPosition.x, normalSpeed);
    }

    void MoveToPosition(float targetX, float speed)
    {
        float currentX = transform.position.x;
        float direction = Mathf.Sign(targetX - currentX);
        float newX = currentX + direction * speed * Time.deltaTime;

        transform.position = new Vector3(
            Mathf.Clamp(newX, minX, maxX),
            transform.position.y,
            transform.position.z);
    }

    Vector2 PredictPuckPosition()
    {
        if (puckRb == null) return puck.transform.position;

        Vector2 futurePosition = (Vector2)puck.transform.position + puckRb.velocity;
        float tableWidth = 8f;
        futurePosition.x = Mathf.Clamp(futurePosition.x, -tableWidth / 2, tableWidth / 2);

        return futurePosition;
    }
}

