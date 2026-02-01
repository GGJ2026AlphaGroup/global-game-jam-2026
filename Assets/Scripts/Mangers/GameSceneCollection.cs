using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Scene Collection", fileName = "SceneCollection_NAME")]
public class GameSceneCollection : ScriptableObject
{
    private static GameSceneCollection instance;
    public static GameSceneCollection Instance
    {
        get
        {
            if (instance != null) return instance;
            instance = Resources.Load<GameSceneCollection>("Scenes/SceneCollection");
            Debug.Assert(instance != null, "No Scene collection found.");
            return instance;
        }
    }

    public SceneInformation_State Get(string name) => m_SceneCollectionsList.Find(s => s.sceneName == name);

    public List<SceneInformation_State> m_SceneCollectionsList = new List<SceneInformation_State>();
}
