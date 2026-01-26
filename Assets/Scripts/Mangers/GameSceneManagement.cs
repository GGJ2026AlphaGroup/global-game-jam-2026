using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct SceneInformation_State
{
    public string sceneName;
    [SceneDropdown] public List<string> sceneNameList;
}

public class GameSceneManagement : Singleton<GameSceneManagement>
{
    private GameSceneCollection collection;

    public void Initialise(GameSceneCollection collection)
    {
        this.collection = collection;
        StartCoroutine(C_LoadScenesOnStart());
    }

    IEnumerator C_LoadScenesOnStart()
    {
        // Ensure Main first
        yield return LoadOrActivate(SceneNames.Main, LoadSceneMode.Single);

        // Load rest
        yield return LoadCollection(collection.m_SceneCollectionsList[0]);
    }

    IEnumerator LoadCollection(SceneInformation_State info)
    {
        foreach (string scene in info.sceneNameList)
        {
            yield return LoadOrActivate(scene, LoadSceneMode.Additive);
        }
    }

    IEnumerator LoadOrActivate(string sceneName, LoadSceneMode mode)
    {
        // Check existing
        Scene scene = SceneManager.GetSceneByName(sceneName);

        // Already loaded
        if (scene.IsValid() && scene.isLoaded)
        {
            // Ensure Main is active
            if (sceneName == SceneNames.Main)
            {
                SceneManager.SetActiveScene(scene);
                Debug.Log($"Activated existing Main: {sceneName}");
            }

            yield break;
        }

        // Load
        AsyncOperation op =
            SceneManager.LoadSceneAsync(sceneName, mode);

        if (op == null)
        {
            Debug.LogError($"Failed to load: {sceneName}");
            yield break;
        }

        yield return op;

        // Re-fetch after load
        Scene loaded = SceneManager.GetSceneByName(sceneName);

        if (!loaded.IsValid() || !loaded.isLoaded)
        {
            Debug.LogError($"Load failed: {sceneName}");
            yield break;
        }

        // Activate if main
        if (sceneName == SceneNames.Main)
        {
            SceneManager.SetActiveScene(loaded);
            Debug.Log($"Main activated: {sceneName}");
        }
    }
}
