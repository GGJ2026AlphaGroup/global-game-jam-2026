using TMPro;
using UnityEngine;

public class QuestionScreenHolder : MonoSingleton<QuestionScreenHolder>
{
    public GameObject holder;
    public GameObject questionButton;
    public TextMeshProUGUI questionsRemaining;
    public QuestionScreenPopulator populator;

    private void Start()
    {
        UpdateQuestionButtonVisibility();
    }

    public void OpenScreen()
    {
        holder.SetActive(true);
        questionsRemaining.text = $"Questions remaining: {PuzzleManager.Instance.questionsRemaining}/{PuzzleManager.Instance.startQuestions}";

        populator.Populate();
    }

    public void CloseScreen()
    {
        UpdateQuestionButtonVisibility();
        holder.SetActive(false);
    }

    private void UpdateQuestionButtonVisibility()
    {
        questionButton.SetActive(PuzzleManager.Instance.questionsRemaining > 0);
    }
}
