using Unity.VisualScripting;
using UnityEngine;

public class HomeScreenManager : MonoBehaviour
{
    public Canvas m_Home, m_Credits;

    public void Play()
    {
        FadeScreenManager.Instance.FadeOut(3.0f, () =>
        {
            GameSceneManagement.Instance.LoadCollection(GameSceneCollection.Instance.Get("Level"), () =>
            {
                FadeScreenManager.Instance.FadeIn(3.0f);
            });
        });
    }

    public void Credits()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }
}
