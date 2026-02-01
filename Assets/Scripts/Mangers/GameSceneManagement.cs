using System;
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
    private SceneInformation_State currentSceneInformationState;
    private bool hasCurrentState = false;

    public void Initialise(GameSceneCollection collection)
    {
        this.collection = collection;
        StartCoroutine(C_LoadScenesOnStart());
    }

    IEnumerator C_LoadScenesOnStart()
    {
        yield return C_LoadOrActivate(SceneNames.Main, LoadSceneMode.Single);

        yield return C_SwitchCollection(collection.m_SceneCollectionsList[0]);
    }

    public void LoadCollection(SceneInformation_State info, Action OnComplete = null)
    {
        StartCoroutine(C_SwitchCollection(info, OnComplete));
    }

    IEnumerator C_SwitchCollection(SceneInformation_State newState, Action OnComplete = null)
    {
        if (hasCurrentState)
        {
            yield return C_UnloadCollection(currentSceneInformationState);
        }

        yield return C_LoadCollection(newState);

        OnComplete?.Invoke();

        currentSceneInformationState = newState;
        hasCurrentState = true;
    }

    IEnumerator C_LoadCollection(SceneInformation_State info)
    {
        foreach (string scene in info.sceneNameList)
        {
            yield return C_LoadOrActivate(scene, LoadSceneMode.Additive);
        }
    }

    IEnumerator C_UnloadCollection(SceneInformation_State info)
    {
        foreach (string sceneName in info.sceneNameList)
        {
            if (sceneName == SceneNames.Main)
                continue;

            Scene scene = SceneManager.GetSceneByName(sceneName);

            if (!scene.IsValid() || !scene.isLoaded)
                continue;

            AsyncOperation op = SceneManager.UnloadSceneAsync(scene);

            if (op == null)
            {
                Debug.LogError($"Failed to unload: {sceneName}");
                continue;
            }

            yield return op;
            Debug.Log($"Unloaded: {sceneName}");
        }
    }

    IEnumerator C_LoadOrActivate(string sceneName, LoadSceneMode mode)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);

        if (scene.IsValid() && scene.isLoaded)
        {
            if (sceneName == SceneNames.Main)
            {
                SceneManager.SetActiveScene(scene);
                Debug.Log($"Activated existing Main: {sceneName}");
            }
            yield break;
        }

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, mode);

        if (op == null)
        {
            Debug.LogError($"Failed to load: {sceneName}");
            yield break;
        }

        yield return op;

        Scene loaded = SceneManager.GetSceneByName(sceneName);

        if (!loaded.IsValid() || !loaded.isLoaded)
        {
            Debug.LogError($"Load failed: {sceneName}");
            yield break;
        }

        if (sceneName == SceneNames.Main)
        {
            SceneManager.SetActiveScene(loaded);
            Debug.Log($"Main activated: {sceneName}");
        }
    }
}
