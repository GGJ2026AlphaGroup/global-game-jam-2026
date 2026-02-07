using UnityEngine;

public class RunManager : Singleton<RunManager>
{
    public static int difficulty = 0;

    public int highestAchievedLevel = 0;

    protected override void Awake()
    {
        base.Awake();

        highestAchievedLevel = PlayerPrefs.GetInt("highestAchievedLevel", 0);
    }

    public void UnlockAll()
    {
        highestAchievedLevel = 20;
        PlayerPrefs.GetInt("highestAchievedLevel", highestAchievedLevel);
    }   

    public void StartNewRun(int difficulty)
    {
        RunManager.difficulty = difficulty;
        switch (difficulty)
        {
            case 0:
                level = 1;
                break;
            case 1:
                level = 4;
                break;
            case 2:
                level = 8;
                break;
            default:
                level = 1;
                break;
        }
    }

    public void Victory()
    {
        if (level > highestAchievedLevel)
        {
            PlayerPrefs.SetInt("highestAchievedLevel", highestAchievedLevel);
        }

        level++;

        FadeScreenManager.Instance.FadeOut(2.0f, () =>
        {
            TextDisplay.DisplayText("A New Case...", 2f, () =>
            {
                GameSceneManagement.Instance.LoadCollection(GameSceneCollection.Instance.Get("Level"), () =>
                {
                    FadeScreenManager.Instance.FadeIn(1.0f);
                });
            });
        });
    }

    public void Defeat()
    {
        FadeScreenManager.Instance.FadeOut(2.0f, () =>
        {
            TextDisplay.DisplayText("The killer escaped!", 2f, () =>
            {
                GameSceneManagement.Instance.LoadCollection(GameSceneCollection.Instance.Get("Menu"), () =>
                {
                    FadeScreenManager.Instance.FadeIn(1.0f);
                });
            });
        });
    }

    public int level = 0;
}
