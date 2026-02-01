using UnityEngine;

public class UIDisabler : MonoSingleton<UIDisabler>
{
    public GameObject holder;

    public void SetUIEnabled(bool isEnabled)
    {
        holder.SetActive(isEnabled);
    }
}
