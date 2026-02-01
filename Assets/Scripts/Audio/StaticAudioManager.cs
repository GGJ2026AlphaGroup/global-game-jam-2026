using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class StaticAudioManager : MonoSingleton<StaticAudioManager>
{
    #region Play Audio

    public CustomAudio PlayAudio(string name, bool isLooping = false) => PlayAudio(AudioLibrary.Get(name), isLooping);
    public CustomAudio PlayAudio(AudioClip clip, bool isLooping = false) {
        CustomAudio audio = new GameObject(clip.name).AddComponent<CustomAudio>();
        audio.transform.SetParent(transform);
        audio.source.clip = clip;
        audio.source.loop = isLooping;
        audio.source.Play();
        return audio;
    }

    #endregion

    #region Stop Audio

    public void StopAllAudio(bool isFade = false, float fadeTime = .5f)
    {
        CustomAudio[] audios = GetComponentsInChildren<CustomAudio>();

        if (isFade) foreach (CustomAudio a in audios) a.Fade(fadeTime);
        else foreach(CustomAudio a in audios) a.Stop();
    }

    public void StopAudio(AudioClip clip, bool isFade = false, float fadeTime = .5f) => StopAudio(clip.name, isFade, fadeTime);
    public void StopAudio(string name, bool isFade = false, float fadeTime = .5f)
    {
        CustomAudio[] audios = GetComponentsInChildren<CustomAudio>();
        foreach (CustomAudio a in audios)
        {
            if (a.name == name)
            {
                if (isFade) a.Fade(fadeTime);
                else a.Stop();
            }
        }
    }

    #endregion
}
