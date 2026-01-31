using UnityEngine;

public class PinnedClue: MonoBehaviour
{
    public Clue clue;

    public FillClueInfo clueInfo;

    public void Init()
    {
        clueInfo.clue = clue;
        clueInfo.pinnedClue = this;
    }

    public void UnpinClue()
    {
        clue.SetIsPinned(false);
        Destroy(gameObject);
    }
}
