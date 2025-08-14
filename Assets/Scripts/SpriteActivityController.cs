// SpriteActivityController.cs
using UnityEngine;
using System.Collections.Generic;

public class SpriteActivityController : MonoBehaviour
{
    [Header("Тег об’єкта, що має бути активованим за замовчуванням")]
    public string defaultTag = "Player";

    private List<SpriteActivityManager> allManagers = new List<SpriteActivityManager>();

    void Awake()
    {
        FindAllManagers();

        ActivateByTag(defaultTag);
    }

    public void FindAllManagers()
    {
        allManagers.Clear();
        allManagers.AddRange(Object.FindObjectsByType<SpriteActivityManager>(FindObjectsSortMode.None));
    }

    public void Activate(SpriteActivityManager target)
    {
        foreach (var manager in allManagers)
        {
            manager.SetActive(manager == target);
        }
    }

    public void ActivateByTag(string tag)
    {
        foreach (var manager in allManagers)
        {
            if (manager.CompareTag(tag))
                manager.SetActive(true);
            else
                manager.SetActive(false);
        }
    }
}
