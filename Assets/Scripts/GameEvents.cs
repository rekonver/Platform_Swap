public static class GameEvents
{
    public static event System.Action OnLevelComplete;
    public static event System.Action OnLevelRestart;

    public static void TriggerLevelComplete()
    {
        OnLevelComplete?.Invoke();
    }

    public static void TriggerLevelRestart()
    {
        OnLevelRestart?.Invoke();
    }
}
