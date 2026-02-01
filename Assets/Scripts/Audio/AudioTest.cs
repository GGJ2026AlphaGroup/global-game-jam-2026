using UnityEngine;

public class AudioTest : MonoBehaviour
{
    CustomAudio audio;
    [SerializeField] private float m_Time = 10.0f;

    [ContextMenu("Play")]
    public void Audio()
    {
        if (audio == null)
            audio = StaticAudioManager.Instance.PlayAudio("audio_theme");
    }

    [ContextMenu("Stop")]
    public void StopAudio()
    {
        if(audio != null) audio.Stop();
    }

    [ContextMenu("Fade")]
    public void FadeAudio()
    {
        if (audio != null) audio.Fade(m_Time);
    }
}