using UnityEngine;

public class ChangeRole : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);

            if (hit != null)
            {
                var manager = hit.GetComponent<SpriteActivityManager>();
                if (manager != null)
                {
                    manager.SetActive(true);

                    var applyStateMethod = typeof(SpriteActivityManager).GetMethod("ApplyState", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    applyStateMethod.Invoke(manager, null);


                    var deactivateOthersMethod = typeof(SpriteActivityManager).GetMethod("DeactivateOthers", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    deactivateOthersMethod.Invoke(manager, null);
                }
            }
        }
    }
}
