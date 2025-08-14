using UnityEngine;

public class StepSaw2D : MonoBehaviour
{
    [Header("Activity Manager")]
    public SpriteActivityManager activityManager;

    [Header("Movement Settings")]
    public float stepSize = 1f;
    public float baseSpeed = 2f;
    public float accelerationPerStep = 0.5f;

    [Header("Collision Detection")]
    public float rayDistance = 1f;
    public LayerMask collisionMask;
    public Transform rayOrigin;
    public float boxWidth = 0.5f;
    public float boxHeight = 0.5f;

    [Header("Spawn Settings")]
    public GameObject checkAreaPrefab;
    public bool spawnAtEnd = true;

    [Header("Rotation Angles")]
    public Quaternion rotationUp = Quaternion.Euler(0, 0, 0);
    public Quaternion rotationDown = Quaternion.Euler(0, 0, 180);
    public Quaternion rotationLeft = Quaternion.Euler(0, 0, 90);
    public Quaternion rotationRight = Quaternion.Euler(0, 0, -90);

    [Header("Destruction Radius")]
    public float destructionRadius = 1f;


    private Vector2 targetPosition;
    private bool isMoving = false;
    private Vector2 moveDirection;
    private int stepsSinceLastCollision = 0;
    private float currentStepProgress = 0f;
    private Vector2 lastPosition;
    private Vector2 lastBoxEnd;
    private bool lastHit;

    void Start()
    {
        targetPosition = transform.position;
        lastPosition = transform.position;

        if (rayOrigin == null)
        {
            var originObj = new GameObject("RayOrigin2D");
            rayOrigin = originObj.transform;
            rayOrigin.SetParent(transform);
            rayOrigin.localPosition = Vector3.zero;
        }
    }

    void Update()
    {
        CheckAndDestroyNearbyObjects();

        bool canInput = activityManager == null || activityManager.Active;

        // --- Обробка руху ---
        if (!isMoving)
        {
            if (!canInput)
                return;           // якщо не активний — не зчитувати клавіші
                                  // тільки тут читаємо Input
            if ((Vector2)transform.position != lastPosition)
                lastPosition = transform.position;

            if (Input.GetKeyDown(KeyCode.W)) TryStartMove(Vector2.up);
            else if (Input.GetKeyDown(KeyCode.S)) TryStartMove(Vector2.down);
            else if (Input.GetKeyDown(KeyCode.A)) TryStartMove(Vector2.left);
            else if (Input.GetKeyDown(KeyCode.D)) TryStartMove(Vector2.right);
        }
        else
        {
            // рух завжди продовжується, навіть якщо неактивний
            float speed = baseSpeed + accelerationPerStep * stepsSinceLastCollision;
            currentStepProgress += speed * Time.deltaTime;
            Vector2 newPos = (Vector2)transform.position + moveDirection * speed * Time.deltaTime;

            if (Vector2.Dot(moveDirection, targetPosition - newPos) <= 0f || currentStepProgress >= 1f)
                CompleteStep();
            else
                transform.position = newPos;
        }
    }

    bool CheckCollision(Vector2 direction)
    {
        Vector2 boxOrigin = (Vector2)rayOrigin.position + direction * 0.1f;
        Vector2 boxSize = new Vector2(boxWidth, boxHeight);
        lastBoxEnd = boxOrigin + direction * rayDistance;

        if (spawnAtEnd && checkAreaPrefab != null)
            Instantiate(checkAreaPrefab, lastBoxEnd, Quaternion.identity);

        bool shouldStop = false;
        lastHit = false;

        // Перевірка всіх об'єктів у зоні
        Collider2D[] hits = Physics2D.OverlapBoxAll(lastBoxEnd, boxSize, 0f, collisionMask);
        foreach (var col in hits)
        {
            var destructible = col.GetComponent<IDestructible>();
            if (destructible != null)
            {
                destructible.Destroy();
            }
            else
            {
                // Якщо не IDestructible — зупиняємось
                shouldStop = true;
            }
            lastHit = true;
        }

        Debug.DrawLine(boxOrigin, lastBoxEnd, shouldStop ? Color.red : Color.green, 0.1f);
        return shouldStop;
    }

    void CheckAndDestroyNearbyObjects()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, destructionRadius, collisionMask);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<IDestructible>(out var destructible))
            {
                destructible.Destroy();
            }
        }
    }


    void TryStartMove(Vector2 direction)
    {
        if (CheckCollision(direction))
        {
            StopMovement();
            return;
        }

        moveDirection = direction;
        currentStepProgress = 0f;
        targetPosition = (Vector2)transform.position + direction * stepSize;
        isMoving = true;

        if (direction == Vector2.up) transform.rotation = rotationUp;
        if (direction == Vector2.down) transform.rotation = rotationDown;
        if (direction == Vector2.left) transform.rotation = rotationLeft;
        if (direction == Vector2.right) transform.rotation = rotationRight;
    }

    void CompleteStep()
    {
        transform.position = targetPosition;
        currentStepProgress = 0f;
        targetPosition = (Vector2)transform.position + moveDirection * stepSize;

        if (CheckCollision(moveDirection))
            StopMovement();
        else
            stepsSinceLastCollision++;
    }

    void StopMovement()
    {
        isMoving = false;
        stepsSinceLastCollision = 0;
        currentStepProgress = 0f;
    }

    void OnDestroy()
    {
        if (rayOrigin) Destroy(rayOrigin.gameObject);
    }

    void OnDrawGizmos()
    {
        Vector3 center = new Vector3(lastBoxEnd.x, lastBoxEnd.y, 0f);
        var oldMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(center, transform.rotation, Vector3.one);
        Gizmos.color = lastHit ? Color.red : Color.green;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(boxWidth, boxHeight, 0.01f));
        Gizmos.matrix = oldMatrix;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, destructionRadius);
    }

}
