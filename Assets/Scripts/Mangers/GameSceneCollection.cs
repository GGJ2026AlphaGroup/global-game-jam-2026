using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Scene Collection", fileName = "SceneCollection_NAME")]
public class GameSceneCollection : ScriptableObject
{
    public List<SceneInformation_State> m_SceneCollectionsList = new List<SceneInformation_State>();
}
