using UnityEngine;

public static class InitializationManager
{
    [RuntimeInitializeOnLoadMethod]
    public static void OnLoad()
    {
        GameObject obj = new GameObject("Game Scene Management");
        obj.AddComponent<GameSceneManagement>().Initialise(GameSceneCollection.Instance);
    }
}
