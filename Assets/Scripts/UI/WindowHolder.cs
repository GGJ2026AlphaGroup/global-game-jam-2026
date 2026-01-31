using System.Collections.Generic;
using UnityEngine;

public class WindowHolder : MonoSingleton<WindowHolder>
{
    public GameObject suspectScreenPrefab;
    public GameObject identityScreenPrefab;

    List<FillSuspectInfo> suspectScreens = new();
    List<FillIdentityInfo> identityScreens = new();

    public void SpawnSuspectScreen(Character character, Vector2 position)
    {
        for (int i = 0; i < suspectScreens.Count; i++)
        {
            if (suspectScreens[i] == null)
            {
                suspectScreens.RemoveAt(i);
                i--;
                continue;
            }

            if (suspectScreens[i].character == character)
            {
                suspectScreens[i].transform.SetAsLastSibling();
                return;
            }
        }

        FillSuspectInfo suspectScreen = Instantiate(suspectScreenPrefab, transform).GetComponent<FillSuspectInfo>();
        suspectScreen.character = character;
        suspectScreen.transform.position = position;

        suspectScreens.Add(suspectScreen);
    }

    public void SpawnIdentityScreen(Character character, Vector2 position)
    {
        for (int i = 0; i < identityScreens.Count; i++)
        {
            if (identityScreens[i] == null)
            {
                identityScreens.RemoveAt(i);
                i--;
                continue;
            }

            if (identityScreens[i].character == character)
            {
                identityScreens[i].transform.SetAsLastSibling();
                return;
            }
        }

        FillIdentityInfo identityScreen = Instantiate(identityScreenPrefab, transform).GetComponent<FillIdentityInfo>();
        identityScreen.character = character;
        identityScreen.transform.position = position;

        identityScreens.Add(identityScreen);
    }
}
