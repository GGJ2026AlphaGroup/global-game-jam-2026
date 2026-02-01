using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AudioLibrary
{
    private static List<AudioClip> m_AllAudio = new List<AudioClip>();

    [RuntimeInitializeOnLoadMethod]
    public static void InitialiseAudioOnLoad()
    {
        List<AudioClip> audio = Resources.LoadAll<AudioClip>("Audio").ToList();

        Debug.Assert(audio.Count > 0, $"Audio list is empty! {audio.Count}");

        m_AllAudio = audio;
    }

    public static AudioClip Get(string name) =>
        m_AllAudio.Find(a => a.name == name);
}
