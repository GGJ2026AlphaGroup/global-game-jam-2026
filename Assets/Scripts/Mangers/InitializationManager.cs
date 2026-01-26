using UnityEngine;

public static class InitializationManager
{
    [RuntimeInitializeOnLoadMethod]
    public static void OnLoad()
    {
        GameSceneCollection sceneManagement = Resources.Load<GameSceneCollection>("Scenes/SceneCollection");
        Debug.Assert(sceneManagement != null, "SceneCollection resource couldn't be found!");
        GameObject obj = new GameObject("Game Scene Management");
        obj.AddComponent<GameSceneManagement>().Initialise(sceneManagement);
    }
}
