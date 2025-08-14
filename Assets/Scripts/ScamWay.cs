using UnityEngine;
using System.Collections;

[ExecuteAlways]
public class ScamWay : MonoBehaviour
{
    public Transform player;
    public Transform triggerPoint;
    public Transform startPoint;
    public Transform triggeredPoint;
    public Transform mover;

    [Header("Settings")]
    [SerializeField] private float checkRadius = 1f;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotationSpeed = 360f;
    [SerializeField] private float delay = 1f;

    private bool isMoving = false;

    void Start()
    {
        if (mover == null)
            mover = transform;

        if (startPoint != null)
        {
            mover.position = startPoint.position;
            mover.rotation = startPoint.rotation;
        }
    }

    void Update()
    {
        if (isMoving || player == null || triggerPoint == null)
            return;

        if (Vector3.Distance(player.position, triggerPoint.position) <= checkRadius)
        {
            StartCoroutine(MoveOutAndBack());
        }
    }

    private IEnumerator MoveOutAndBack()
    {
        if (startPoint == null || triggeredPoint == null || mover == null)
            yield break;

        isMoving = true;

        Vector3 targetPos = triggeredPoint.position;
        Quaternion targetRot = triggeredPoint.rotation;

        while (Vector3.Distance(mover.position, targetPos) > 0.01f ||
               Quaternion.Angle(mover.rotation, targetRot) > 0.5f)
        {
            mover.position = Vector3.MoveTowards(mover.position, targetPos, moveSpeed * Time.deltaTime);
            mover.rotation = Quaternion.RotateTowards(mover.rotation, targetRot, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(delay);

        Vector3 returnPos = startPoint.position;
        Quaternion returnRot = startPoint.rotation;

        while (Vector3.Distance(mover.position, returnPos) > 0.01f ||
               Quaternion.Angle(mover.rotation, returnRot) > 0.5f)
        {
            mover.position = Vector3.MoveTowards(mover.position, returnPos, moveSpeed * Time.deltaTime);
            mover.rotation = Quaternion.RotateTowards(mover.rotation, returnRot, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        isMoving = false;
    }

    void OnDrawGizmos()
    {
        // радіус активації
        if (triggerPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(triggerPoint.position, checkRadius);
        }

        // стартова точка
        if (startPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(startPoint.position, 0.1f);
            Gizmos.DrawLine(startPoint.position, startPoint.position + startPoint.up * 0.5f);
        }

        // кінцева (triggered) точка
        if (triggeredPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(triggeredPoint.position, 0.1f);
            Gizmos.DrawLine(triggeredPoint.position, triggeredPoint.position + triggeredPoint.up * 0.5f);
        }

        // позначка самого mover'а
        if (mover != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(mover.position, Vector3.one * 0.2f);
        }
    }
}
