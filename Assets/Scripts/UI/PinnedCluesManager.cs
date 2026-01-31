using System.Collections.Generic;
using UnityEngine;

public class PinnedCluesManager : MonoSingleton<PinnedCluesManager>
{
    public GameObject pinnedCluePrefab;

    List<PinnedClue> pinnedClues = new();

    public void PinClue(Clue clue)
    {
        PinnedClue newClue = Instantiate(pinnedCluePrefab, transform).GetComponent<PinnedClue>();

        newClue.clue = clue;
        newClue.Init();

        pinnedClues.Add(newClue);

        for (int i = 0; i < pinnedClues.Count; i++)
        {
            if (pinnedClues[i] == null)
            {
                pinnedClues.RemoveAt(i);
                i--;
            }
        }

        while (pinnedClues.Count > 5)
        {
            pinnedClues[0].UnpinClue();
            pinnedClues.RemoveAt(0);
        }
    }
}
