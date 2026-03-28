using TMPro;
using UnityEngine;

public class QTargetScreenController : MonoSingleton<QTargetScreenController>
{
    public GameObject holder;
    public Character subject;
    public TextMeshProUGUI questionText;
    public QTargetScreenPopulator populator;

    public void OpenScreen(Character subject)
    {
        this.subject = subject;
        holder.SetActive(true);
        questionText.text = subject.personality switch
        {
            Personality.Perceptive => $"{subject.name} will tell you what mask this suspect is wearing.",
            Personality.Fashionable => $"{subject.name} will tell you what colour clothes this suspect is wearing.",
            Personality.Socialite => $"{subject.name} will tell you what activity this suspect is doing.",
            Personality.Astute => $"{subject.name} will tell you if this suspect is lying.",
            _ => "???"
        };
        populator.Populate();
    }

    public void CloseScreen()
    {
        holder.SetActive(false);
    }
}
