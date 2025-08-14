using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DestructibleStepMover : MonoBehaviour, IDestructible
{
    [Header("Movement Settings (on Destroy)")]
    public float stepSize = 1f;
    public float baseSpeed = 2f;
    public float accelerationPerStep = 0.5f;

    private bool isMoving = false;
    private Vector2 moveDirection = Vector2.down;
    private Vector2 startPosition;
    private Vector2 targetPosition;
    private float currentProgress = 0f;
    private int stepsCompleted = 0;

    public void Destroy()
    {
        if (isMoving)
            return;

        startPosition = transform.position;
        moveDirection = Vector2.down;
        targetPosition = startPosition + moveDirection * stepSize;
        currentProgress = 0f;
        isMoving = true;
    }

    void Update()
    {
        if (!isMoving)
            return;

        float speed = baseSpeed + accelerationPerStep * stepsCompleted;
        currentProgress += speed * Time.deltaTime;

        Vector2 newPos = Vector2.Lerp(startPosition, targetPosition, Mathf.Clamp01(currentProgress));
        transform.position = newPos;

        if (currentProgress >= 1f)
        {
            transform.position = targetPosition;
            stepsCompleted++;
            isMoving = false;
        }
    }
}
