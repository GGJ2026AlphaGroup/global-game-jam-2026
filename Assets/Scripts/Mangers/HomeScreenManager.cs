using Unity.VisualScripting;
using UnityEngine;

public class HomeScreenManager : MonoBehaviour
{
    public Canvas m_Home, m_Credits;

    CustomAudio audio = null;
    [SerializeField] private int clickRange = 3;

    private void Start()
    {
        audio = AudioManager.Instance.PlayAudio("audio_theme", true);
    }

    public void PlayClickSound() =>
        AudioManager.Instance.PlayAudio($"audio_click_{Random.Range(1, clickRange)}");


    public void LoadCredits()
    {
        m_Home.enabled = false;
        m_Credits.enabled = true;
    }

    public void LoadHome()
    {
        m_Home.enabled = true;
        m_Credits.enabled = false;
    }

    public void Play()
    {
        RunManager.Instance.StartNewRun();

        if (audio != null)
        {
            audio.Fade(.2f);
            AudioManager.Instance.PlayAudio("audio_play");
        }

        FadeScreenManager.Instance.FadeOut(3.0f, () =>
        {
            GameSceneManagement.Instance.LoadCollection(GameSceneCollection.Instance.Get("Level"), () =>
            {
                FadeScreenManager.Instance.FadeIn(3.0f);
            });
        });
    }

    public void Quit()
    {
        Application.Quit();
    }
}
