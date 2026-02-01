using UnityEngine;

public class RunManager : Singleton<RunManager>
{
    public void StartNewRun()
    {
        level = 1;
    }

    public void Victory()
    {
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
            TextDisplay.DisplayText("Fired!", 2f, () =>
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
