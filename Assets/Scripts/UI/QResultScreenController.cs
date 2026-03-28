using TMPro;
using UnityEngine;

public class QResultScreenController : MonoSingleton<QResultScreenController>
{
    public GameObject holder;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI clueText;

    public void OpenScreen(Character subject)
    {
        holder.SetActive(true);
        nameText.text = $"{subject.name} Says:";
        clueText.text = subject.questionClue.GetClueText(subject);
    }

    public void CloseScreen()
    {
        holder.SetActive(false);
    }
}
