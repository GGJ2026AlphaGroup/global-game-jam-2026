using UnityEngine;

public class TheLightingCrew : MonoSingleton<TheLightingCrew>
{
    public GameObject lighting;

    public static void On()
    {
        Instance.lighting.SetActive(true);
    }

    public static void Off()
    {
        Instance.lighting.SetActive(false);
    }
}
