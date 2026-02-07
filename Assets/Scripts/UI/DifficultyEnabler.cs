using UnityEngine;
using UnityEngine.UI;

public class DifficultyEnabler : MonoBehaviour
{
    public Button hard, expert;

    public void UnlockAll()
    {
        RunManager.Instance.UnlockAll();

        SetButtonsEnabled();
    }

    public void Start()
    {
        SetButtonsEnabled();
    }

    void SetButtonsEnabled()
    {
        hard.interactable = RunManager.Instance.highestAchievedLevel > 3;
        expert.interactable = RunManager.Instance.highestAchievedLevel > 7;
    }
}
