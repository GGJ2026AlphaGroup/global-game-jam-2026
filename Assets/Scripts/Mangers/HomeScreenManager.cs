using Unity.VisualScripting;
using UnityEngine;

public class HomeScreenManager : MonoBehaviour
{
    public Canvas m_Home, m_Credits, m_Difficulty;

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
        m_Difficulty.enabled = false;
    }

    public void LoadHome()
    {
        m_Home.enabled = true;
        m_Credits.enabled = false;
        m_Difficulty.enabled = false;
    }

    public void LoadDifficulty()
    {
        m_Home.enabled = false;
        m_Credits.enabled = false;
        m_Difficulty.enabled = true;
    }


    public void Play(int difficulty)
    {
        RunManager.Instance.StartNewRun(difficulty);

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
